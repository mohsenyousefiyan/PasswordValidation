using Microsoft.EntityFrameworkCore;
using PasswordValidation.Dtos;
using PasswordValidation.Entites;
using PasswordValidation.InfraStructers.DAL.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers
{
    public interface IUserPasswordChangeHistoryService
    {
        Task AddHistory(string userid, string oldpassword);

        Task<List<UserPasswordChangeHistoryDto>> GetHistoryByDate(string userid, DateTime startdate, DateTime enddate);
    }

    public class UserPasswordChangeHistoryService : IUserPasswordChangeHistoryService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly DbSet<UserPasswordChangeHistory> UserPasswordChangeHistory;
        public UserPasswordChangeHistoryService(IUnitOfWork _unitOfWork)
        {
            this.UnitOfWork = _unitOfWork;
            UserPasswordChangeHistory = UnitOfWork.Set<UserPasswordChangeHistory>();
        }

        public async Task AddHistory(string userid, string oldpassword)
        {

            UserPasswordChangeHistory.Add(new UserPasswordChangeHistory()
            {
                UserId = userid,
                ChangeDate = DateTime.Now,
                OldPassword = oldpassword
            });

            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserPasswordChangeHistoryDto>> GetHistoryByDate(string userid, DateTime startdate, DateTime enddate)
        {
            var lst = await UserPasswordChangeHistory.Where(x => x.UserId == userid && x.ChangeDate.Date >= startdate.Date && x.ChangeDate.Date <= enddate.Date)
                .Select(x => new UserPasswordChangeHistoryDto()
                {
                    ChangeDate = x.ChangeDate,
                    OldPassword = x.OldPassword
                }).ToListAsync();

            return lst;
        }
    }
}
