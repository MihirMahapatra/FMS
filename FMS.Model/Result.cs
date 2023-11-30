namespace FMS.Model
{
    public class Result<T>
    {
        public T SingleObjData { get; set; }
        public List<T> CollectionObjData { get; set; }
        public Exception Exception { get; set; }
        public int Count { get; set; }
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Response { get; set; }
        public string WarningMessage { get; set; }
    }
}
