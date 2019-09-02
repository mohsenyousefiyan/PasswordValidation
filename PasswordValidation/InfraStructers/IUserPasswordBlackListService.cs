using Microsoft.EntityFrameworkCore;
using PasswordValidation.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers
{
    public interface IUserPasswordBlackListService
    {        
        Task<Boolean> PasswordIsExistsAsync(string password);        
    }

    public class UserPasswordBlackListService : IUserPasswordBlackListService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly DbSet<PasswordBlackList> PasswordBlackLists;


        public UserPasswordBlackListService(IUnitOfWork _unitOfWork)
        {
            this.UnitOfWork = _unitOfWork;
            PasswordBlackLists = UnitOfWork.Set<PasswordBlackList>();
        }
              
        public async Task<Boolean> PasswordIsExistsAsync(string password)
        {            
            var FindObject = await PasswordBlackLists.FirstOrDefaultAsync(x => x.Password == password);
            return FindObject != null ? true : false;
        }
    }
}
