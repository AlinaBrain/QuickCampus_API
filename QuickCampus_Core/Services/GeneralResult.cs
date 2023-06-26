using QuickCampus_Core.ViewModel;

namespace QuickCampus_Core.Services
{
    internal class GeneralResult : ApplicantViewModel.ApplicantGridViewModel
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public int Value { get; set; }
        public bool IsSuccess { get; internal set; }
    }
}