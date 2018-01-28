namespace SavingForASunnyDay.Api
{
    public class ItemResult
    {
        public string errorType { get; set; }
        public bool isError { get; set; }
        public string errorMessage { get; set; }
    }

    public class ItemResult<T> : ItemResult
    {
        public T item { get; set; }
    }

    public class ListResult<T> : ItemResult
    {
        public System.Collections.Generic.List<T> items { get; set; }
    }
}
