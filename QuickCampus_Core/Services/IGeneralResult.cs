using QuickCampus_Core.ViewModel;
using System.Collections;

namespace QuickCampus_Core.Services
{
    internal class IGeneralResult : IEnumerable<CountryVM>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<CountryModel> Value { get; set; }

        public IEnumerator<CountryVM> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}