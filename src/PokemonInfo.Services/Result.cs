using System.Net;

namespace PokemonInfo.Services
{
    public abstract class Result
    {
        public ErrorResultContent ErrorResult { get; private set; }

        public HttpStatusCode StatusCode => ErrorResult.StatusCode;
        public string ErrorMessage => ErrorResult.ErrorMessage;

        public bool Succeeded => ErrorResult is null;
        public bool Failed => ErrorResult != null;

        protected Result()
        {
            ErrorResult = null;
        }

        protected Result(ErrorResultContent errorResult)
        {
            ErrorResult = errorResult;
        }

        private class Success : Result { }

        private class Error : Result
        {
            public Error(ErrorResultContent errorResult) : base(errorResult) { }
        }

        public class ErrorResultContent
        {
            public ErrorResultContent(HttpStatusCode statusCode, string message)
            {
                StatusCode = statusCode;
                ErrorMessage = message;
            }

            public HttpStatusCode StatusCode { get; private set; }
            public string ErrorMessage { get; private set; }
        }

        public static implicit operator Result(ErrorResultContent errorResult) => new Error(errorResult);
    }

    public class Result<T> : Result
    {
        public Result(T value)
        {
            Value = value;
        }

        public Result(ErrorResultContent errorResult) : base(errorResult) { }

        public T Value { get; private set; }

        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(ErrorResultContent errorResult) =>
            new Result<T>(errorResult);

    }
}
