using System;
using System.Net;

namespace MusicKG.HondaPlugins.Services.Helpers
{
    public class ErrorHelper
    {
        public const string StatusCodeKey = "statusCode";

        public static void ThrowException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, params string[] args)
        {
            var exception = new ErrorMessageException(string.Format(message, args));
            exception.Data.Add(StatusCodeKey, statusCode);
            throw exception;
        }

        public class ErrorMessageException : Exception
        {
            public ErrorMessageException(string message) : base(message)
            {
            }
        }
    }
}
