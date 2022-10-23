namespace AuthArmor.Sdk.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class AuthArmorException : Exception, ISerializable
    {
        public AuthArmorException()
        { }

        public AuthArmorException(string message)
            : base(message)
        { }

        public AuthArmorException(string message, Exception ex)
            : base(message, ex)
        { }

        protected AuthArmorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
