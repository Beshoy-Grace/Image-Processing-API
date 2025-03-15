namespace Image.API.DTOs
{
    public class BaseCommandResponse<T>
    {
        public bool IsSuccess { get; set; }
        public double Version { get; set; }
        public string Message { get; set; }
        public T ResponseData { get; set; }
        public List<Errors> Errors { get; set; }
        public int StatusCode { get; set; }

        public BaseCommandResponse()
        {

        }
        public BaseCommandResponse(List<Errors> errors, T responseData = default)
        {
            Errors = errors;
            IsSuccess = false;
            Message = "Validation Error";
            ResponseData = default;
            Version = 1.0;
        }
        public BaseCommandResponse(string message, T responseData = default)
        {
            Errors = new List<Errors>();
            IsSuccess = false;
            Message = message;
            ResponseData = default;
            Version = 1.0;
        }
        public BaseCommandResponse(bool isSuccess, T responseData, double version, List<Errors> errors)
        {
            IsSuccess = isSuccess;
            ResponseData = responseData;
            Version = version;
            Errors = errors;
        }

        public BaseCommandResponse(T Data)
        {
            ResponseData = Data;
            Version = 1.0;
            IsSuccess = true;
            Errors = new List<Errors>();
        }

    }
    public class GetMessage
    {
        public string Message { get; set; }
        public GetMessage(string message)
        {
            Message = message;
        }
    }
    public class Errors
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
