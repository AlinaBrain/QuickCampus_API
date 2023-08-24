using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class GoalRequestVM
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public string targetYear { get; set; }
    }
    public class GoalResponseVM
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public string targetYear { get; set; }
        public bool? IsActive { get; set; }
    }
}
