using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class SectionVm
    {
        public static explicit operator SectionVm(Section items)
        {
            return new SectionVm()
            {
                SectionId=items.SectionId,
                Section1=items.Section1,
                SortOrder=items.SortOrder,
                ClentId=items.ClentId,
            };
        }
        public int SectionId { get; set; }

        public string? Section1 { get; set; }

        public int? SortOrder { get; set; }

        public int? ClentId { get; set; }

        public Section ToSectionDbModel()
        {
            return new Section
            {
                Section1=Section1,
                SortOrder = SortOrder,
                ClentId=ClentId,
            };
        }

    }
}
