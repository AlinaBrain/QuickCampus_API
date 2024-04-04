using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class GroupVm
    {
        public static explicit operator GroupVm(MstGroupdl items)
        {
            return new GroupVm()
            {
                GroupId= items.GroupId,
                GroupName= items.GroupName,
            };
        }
        public int GroupId { get; set; }

        public string? GroupName { get; set; }

    }
}
