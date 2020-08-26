using System;

namespace ErrorHandler
{
    public class BusinessException : Exception
    {
        public ErrorCodes Code { get; }

        public BusinessException(ErrorCodes code)
        {
            Code = code;
        }

        public BusinessException(ErrorCodes code, string message) : base(message)
        {
            Code = code;
        }

        public BusinessException(ErrorCodes code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
