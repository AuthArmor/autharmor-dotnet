namespace AuthArmor.Sdk.Utils
{
    using AuthArmor.Sdk.Exceptions;
    using AuthArmor.Sdk.Models.Auth;
    using AuthArmor.Sdk.Models.Auth.MessageValidation;
    using Fido2NetLib;
    using Fido2NetLib.Objects;
    using Newtonsoft.Json;
    using System;

    public static class FidoHelper
    {
        /// <summary>
        /// Validate an Auth Signature Message from Auth Armor
        /// </summary>
        /// <param name="authResponseDetail">AuthResponseDetail from AuthResponse Call</param>
        /// <param name="localAuthArmorProfileDeviceCounter">Local Counter on file for the Auth Armor Profile Device</param>
        /// <param name="localFidoU2FCounter">Local Counter on file for U2F signatures</param>
        /// <returns>Boolean value determining if the signature was valid or not</returns>
        public static AuthValidationResult ValidateWebAuthNSignature(AuthResponseDetails authResponseDetail, uint localCounterOnFile)
        {
            try
            {
                AuthValidationResult response = new AuthValidationResult();
                if (authResponseDetail is null)
                {
                    throw new ArgumentNullException("authResponseDetail can not be null");
                }
                // get signed data
                var signedDataObject = SecuredSignedDetails.DecodeSignedData<SignedAuthResponse>(authResponseDetail.SecuredSignedMessageDetails.SignedDataString);
                if (signedDataObject is null)
                {
                    throw new AuthArmorException("Signed data object was invalid");
                }

                var webAuthNValidationDetails = System.Text.Json.JsonSerializer.Deserialize<WebAuthnValidationDetails>(authResponseDetail.SecuredSignedMessageDetails.SignatureValidationDetails.ToString());
                //var webAuthNValidationDetails = (WebAuthNValidationDetails)authResponseDetail.SecuredSignedMessageDetails.SignatureValidationDetails.ToString();

                var fido2Response = System.Text.Json.JsonSerializer.Deserialize<Fido2AuthenticatorResponse>(authResponseDetail.SecuredSignedMessageDetails.SignatureDataDetail.RawSignatureData);
                
                var assertionOptions = AssertionOptions.FromJson(webAuthNValidationDetails.Fido2OptionsJson);

                AuthenticatorAssertionRawResponse assertionRawResponse = new AuthenticatorAssertionRawResponse();
                assertionRawResponse.Response = new AuthenticatorAssertionRawResponse.AssertionResponse();
                assertionRawResponse.Id = Convert.FromBase64String(WebsafeBase64Converter.ConvertWebSafeBase64ToNormalBase64(fido2Response.id));
                assertionRawResponse.RawId = Convert.FromBase64String(WebsafeBase64Converter.ConvertWebSafeBase64ToNormalBase64(fido2Response.id));
                assertionRawResponse.Response.AuthenticatorData = Convert.FromBase64String(WebsafeBase64Converter.ConvertWebSafeBase64ToNormalBase64(fido2Response.attestation_object));
                assertionRawResponse.Response.ClientDataJson = Convert.FromBase64String(WebsafeBase64Converter.ConvertWebSafeBase64ToNormalBase64(fido2Response.client_data));
                assertionRawResponse.Response.Signature = Convert.FromBase64String(WebsafeBase64Converter.ConvertWebSafeBase64ToNormalBase64(fido2Response.signature));
                assertionRawResponse.Response.UserHandle = null;
                assertionRawResponse.Type = PublicKeyCredentialType.PublicKey;

                //setup callback for the fido2 lib - this just returns true
                IsUserHandleOwnerOfCredentialIdAsync callback = async (args) =>
                {
                    return true;
                };

                Fido2Configuration config = new Fido2Configuration();
                //config.Origin = "android:apk-key-hash:4j0ZPDSD2rRZiCWBjGNuhTA43BrfrWJ-b-Qsa6hfdhk"; //dev android - get this from app instance table
                config.Origin = webAuthNValidationDetails.ExpectedOrigin;
                config.ServerName = "nothing"; //this could maybe be the securred entity name
                config.ServerDomain = webAuthNValidationDetails.RpId; //this would either be the custom value from the secured entity or the default, depending on the flag in the secured entity
                config.TimestampDriftTolerance = 300000; //hardocde, but should come from config appsettings
                config.Timeout = 60000; //hardocde, but should come from config appsettings

                Fido2 fido2 = new Fido2(config);

                if (!string.IsNullOrWhiteSpace(fido2Response.client_extension_data))
                {
                    assertionRawResponse.Extensions = JsonConvert.DeserializeObject<Fido2NetLib.Objects.AuthenticationExtensionsClientOutputs>(fido2Response.client_extension_data);
                }

                var parsedResponse = AuthenticatorAssertionResponse.Parse(assertionRawResponse);
                var authData = new AuthenticatorData(assertionRawResponse.Response.AuthenticatorData);
                var currentCount = authData.SignCount;



                var result = fido2.MakeAssertionAsync(assertionRawResponse, AssertionOptions.FromJson(webAuthNValidationDetails.Fido2OptionsJson), Convert.FromBase64String(webAuthNValidationDetails.PublicKeyBase64), localCounterOnFile, callback).Result;
                if (result.Status != "ok")
                {
                    response.Message = result.ErrorMessage;
                    response.isValid = false;
                    response.CounterValue = 0;
                    return response;
                    //error
                    //return false;

                }

                response.Message = result.Status;
                response.isValid = true;
                response.CounterValue = result.Counter;
                return response;

                //return true;

            }
            catch (AuthArmorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AuthArmorException(ex.Message, ex);
            }
        }
    }
}
