﻿using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class SectionVm
    {
        public static explicit operator SectionVm(MstSection items)
        {
            return new SectionVm()
            {
                SectionId=items.SectionId,
                Section=items.Section,
                SortOrder=items.SortOrder,
                ClentId=items.ClentId,
            };
        }
        public int SectionId { get; set; }

        public string? Section { get; set; }

        public int? SortOrder { get; set; }

        public int? ClentId { get; set; }

        public MstSection ToSectionDbModel()
        {
            return new MstSection
            {
                Section=Section,
                SortOrder = SortOrder,
                ClentId=ClentId,
            };
        }

    }
}
