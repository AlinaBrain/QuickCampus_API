using Microsoft.EntityFrameworkCore;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuickCampus_Core.Services
{
    public class ClientServices : IClientRepo
    {
        private readonly QuikCampusDevContext _context;
        public ClientServices(QuikCampusDevContext context)
        {
            _context= context;
        }

        public async Task<List<ClientVM>> GetAllClient()
        {
            var rec = await _context.TblClients.Select(x => new ClientVM()
            {
                Name = x.Name,
                CraetedBy = x.CraetedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy= x.ModifiedBy,
                ModofiedDate = x.ModofiedDate

            }).ToListAsync();
            if (rec.Any())
            {
                return rec.ToList();
            }
            else
            {
                return new List<ClientVM>();
            }
        }

        public async Task<ClientVM> Add(ClientVM clientVM)
        {
            using (var context = new QuikCampusDevContext())
            {

                TblClient client = new TblClient()
                {
                    Name = clientVM.Name,
                    CraetedBy = clientVM.CraetedBy,
                    CreatedDate = clientVM.CreatedDate,
                    ModifiedBy = clientVM.ModifiedBy,
                    ModofiedDate = clientVM.ModofiedDate

                };
                var result = context.TblClients.Add(client);
                context.SaveChanges();

              
                
            }
            return clientVM;
        }

        public IQueryable<TblClient> GetAllQuerable()
        {
            throw new NotImplementedException();
        }

        public Task<List<TblClient>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<TblClient>> GetAll(Expression<Func<TblClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TblClient> FirstOrDefaultAsync(Expression<Func<TblClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TblClient FirstOrDefault(Expression<Func<TblClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<TblClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<TblClient, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TblClient> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<TblClient> Add(TblClient entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TblClient entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(TblClient entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task AddApplicantAsync(ApplicantViewModel model)
        {
            throw new NotImplementedException();
        }

        public ApplicantViewModel.ApplicantGridViewModel UpdateApplicant(ApplicantViewModel.ApplicantGridViewModel model)
        {
            throw new NotImplementedException();
        }
    }


    
}
