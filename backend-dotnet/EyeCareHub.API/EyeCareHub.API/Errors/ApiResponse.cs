
namespace EyeCareHub.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ApiResponse(int StatusCode, string Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? GetDefualtMessageStatusCode(StatusCode);
        }

        public string GetDefualtMessageStatusCode(int StatusCode)
            => StatusCode switch
            {
                400 => "Bad Request, You Have Model?",
                401 => "Authorized , You Have Not",
                404 => "Resourse Was Not Found",
                500 => "Error Server",
                _ => null,
            };
    }
}

