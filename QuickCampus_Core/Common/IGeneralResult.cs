
namespace QuickCampus_Core.Common
{
    public class GeneralResult<T> : IGeneralResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public long TotalRecordCount { get; set; }
    }
    public interface IGeneralResult<T>
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        T Data { get; set; }
        long TotalRecordCount { get; set; }
    }
}
