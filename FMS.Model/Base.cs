namespace FMS.Model
{
    public class Base
    {
        public string Message { get; set; }
        public string ErrorMsg { get; set; }
        public string SuccessMsg { get; set; }
        public Exception Exception { get; set; }
        public int? ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string Data { get; set; }
    }
}
