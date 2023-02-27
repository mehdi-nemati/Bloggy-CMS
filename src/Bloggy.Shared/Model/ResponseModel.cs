namespace Bloggy.Shared.Model
{
    public class ResponseModel
    {
        public ResponseModel(bool isSuccess = false, string message = "", int code = 2)
        {
            Code = isSuccess ? 1 : code;
            Message = message;
            IsSuccess = isSuccess;
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ResponseModel<T>
    {
        public ResponseModel(bool isSuccess = false, T model = default, string message = "", int code = 2)
        {
            Code = isSuccess ? 1 : code;
            Message = message;
            IsSuccess = isSuccess;
            Model = model;
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T Model { get; set; }
    }
}