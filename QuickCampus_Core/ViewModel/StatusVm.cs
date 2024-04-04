﻿using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class StatusVm
    {
        public static explicit operator StatusVm(MstApplicantStatus items)
        {
            return new StatusVm
            {
                StatusId = items.StatusId,
                StatusName = items.StatusName,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
                
            };

        }
        public int StatusId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        
        [Required(ErrorMessage = "MstCity_State Name is required.")]
        [MaxLength(50, ErrorMessage = "Name must be at most 20 characters long.")]
        public string? StatusName { get; set; }
        public MstApplicantStatus ToDbModel()
        {
            return new MstApplicantStatus
            {
                StatusName = StatusName,
                IsActive = true,
                IsDeleted=false,
                CreatedDate = DateTime.Now,
                ModifiedDate= StatusId> 0? DateTime.Now:null
            };
        }
    }
}
