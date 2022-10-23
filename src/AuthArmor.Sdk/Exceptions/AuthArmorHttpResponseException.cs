namespace AuthArmor.Sdk.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    public class AuthArmorHttpResponseException : Exception, ISerializable
    {
        public HttpStatusCode StatusCode { get; private set; }

        public AuthArmorHttpResponseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        protected AuthArmorHttpResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
