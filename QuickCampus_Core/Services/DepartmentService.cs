using QuickCampus_Core.Common;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Services
{
    internal class DepartmentService : IDepartmentRepo
    {
        private readonly QuikCampusDevContext _context;

        public DepartmentService(QuikCampusDevContext context)
        {
            _context = context;
        }


        public async Task<List<DepartmentVM>> GetAllDepartments(int clientid, bool isSuperAdmin)
        {
            List<DepartmentVM> response = new List<DepartmentVM>();

            if (isSuperAdmin)
            {
                _context.TblDepartments.Where(w => w.IsDeleted != true && (clientid == 0 ? true : w.ClientId == clientid)).Select(s =>
                new DepartmentResponseVM()
                {
                    Id = s.Id,
                    DepartmentName = s.DepartmentName,
                    Description = s.Description,
                    IsActive = s.IsActive
                }).ToList();
            }
            else
            {
                _context.TblDepartments.Where(w => w.IsDeleted != true && w.ClientId == clientid).Select(s =>
                new DepartmentResponseVM()
                {
                    Id = s.Id,
                    DepartmentName = s.DepartmentName,
                    Description = s.Description,
                    IsActive = s.IsActive
                }).ToList();
            }
            return response;
        }

        public async Task<IGeneralResult<DepartmentVM>> AddDepartments(int clientId, int userId, DepartmentVM vm)
        {
            IGeneralResult<DepartmentVM> result = new GeneralResult<DepartmentVM>();

            bool isExist = _context.TblDepartments.Any(s => s.DepartmentName == vm.DepartmentName);
            if (isExist)
            {
                result.IsSuccess = false;
                result.Message = "Department Already Exist";
                result.Data = vm;
                return result;
            }

            TblDepartment request = new TblDepartment()
            {
                ClientId = clientId,
                DepartmentName = vm.DepartmentName,
                Description = vm.DepartmentName,
                CreatedOn = DateTime.Now,
                ModefiedBy = null,
                ModefiedOn = null,
                IsDeleted = false,
                IsActive = true,
                CreatedBy = userId
            };

            _context.TblDepartments.Add(request);
            int save = _context.SaveChanges();
            if (save > 1)
            {
                result.IsSuccess = true;
                result.Message = "Department Added Exist";
                result.Data = vm;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = vm;
            }
            return result;
        }
        public async Task<IGeneralResult<DepartmentVM>> UpdateDepartments(int clientId, int userId, DepartmentVM vm)
        {
            IGeneralResult<DepartmentVM> result = new GeneralResult<DepartmentVM>();

            var  rec = _context.TblDepartments.Where(s => s.DepartmentName == vm.DepartmentName && s.Id!= vm.Id).FirstOrDefault();
            if (rec==null)
            {
                result.IsSuccess = false;
                result.Message = "Department Already Exist";
                result.Data = vm;
                return result;
            }

            rec.DepartmentName = vm.DepartmentName;
            rec.Description = vm.Description;
            rec.ModefiedBy = userId;
            rec.ModefiedOn = DateTime.Now;


            _context.TblDepartments.Update(rec);
            int save = _context.SaveChanges();
            if (save > 1)
            {
                result.IsSuccess = true;
                result.Message = "Department update successfully";
                result.Data = vm;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = vm;
            }
            return result;
        }
        public async Task<IGeneralResult<string>> ActiveInactiveDepartments(int clientId, bool isSuperAdmin, int id, bool status)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblDepartment dept = new TblDepartment();
            if (isSuperAdmin)
            {
                dept = _context.TblDepartments.Where(w => w.Id == id && w.IsDeleted != true && (clientId == 0 ? true : w.ClientId == clientId)).FirstOrDefault();
            }
            else
            {
                dept = _context.TblDepartments.Where(w => w.Id == id && w.IsDeleted != true && w.ClientId == clientId).FirstOrDefault();
            }

            if (dept == null)
            {
                result.IsSuccess = false;
                result.Message = "Department Not Found";
                result.Data = null;
                return result;
            }

            dept.IsActive = status;
            _context.TblDepartments.Update(dept);
            int update = _context.SaveChanges();

            if (update > 1)
            {
                result.IsSuccess = true;
                result.Message = "Department Update successfully";
                result.Data = null;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = null;
            }
            return result;
        }

        public async Task<IGeneralResult<string>> ActiveInactiveDepartments(int clientId, bool isSuperAdmin, int id)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            TblDepartment dept = new TblDepartment();
            if (isSuperAdmin)
            {
                dept = _context.TblDepartments.Where(w => w.Id == id && w.IsDeleted != true && (clientId == 0 ? true : w.ClientId == clientId)).FirstOrDefault();
            }
            else
            {
                dept = _context.TblDepartments.Where(w => w.Id == id && w.IsDeleted != true && w.ClientId == clientId).FirstOrDefault();
            }

            if (dept == null)
            {
                result.IsSuccess = false;
                result.Message = "Department Not Found";
                result.Data = null;
                return result;
            }

            dept.IsActive = dept.IsActive == false ? true : true;
            _context.TblDepartments.Update(dept);
            int update = _context.SaveChanges();

            if (update > 1)
            {
                result.IsSuccess = true;
                result.Message = "Department delete Successfully";
                result.Data = null;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Something Went Wrong";
                result.Data = null;
            }
            return result;
        }

        public async Task<IGeneralResult<string>> GetDepartmentsById(int clientId, bool isSuperAdmin, int id)
        {
            IGeneralResult<string> response = new GeneralResult<string>();
            return response;
        }
       
    }
}
