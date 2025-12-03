namespace EyeCareHub.API.Errors
{
    public class ApiExcptionResponse :ApiResponse
    {
        public string Details { get; set; }
        public ApiExcptionResponse(int StatusCode, string Message = null, string Details = null) : base(StatusCode, Message) // = null علي شان ينفع نسبهم فاضيين
        {
            this.Details = Details;
        }
    }
}
