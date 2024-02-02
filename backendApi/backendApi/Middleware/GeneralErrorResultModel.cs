namespace backendApi.Middleware
{
    public class GeneralErrorResultModel
    {
        public string Message { get; }
        public ErrorCodes ErrorCode { get; }
        public GeneralErrorResultModel(ErrorCodes errorCode, string message)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
