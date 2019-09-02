using Microsoft.Extensions.Options;
using PasswordValidation.InfraStructers.DAL.EF.Context;
using PasswordValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers
{
    public interface IValidationPassword
    {
        Task<Dictionary<String, String>> ValidatePassword(string userid,string password);
    }

        
    public class ValidationPassword_InMemory : IValidationPassword
    {
        private List<string> BlackListPassword = new List<string>();

        public ValidationPassword_InMemory()
        {
            SeedData();
        }

        public Task<Dictionary<String, String>> ValidatePassword(string userid,string password)
        {
            Dictionary<String, String> Errors = new Dictionary<String, String>();

            if (BlackListPassword.Contains(password))
                Errors.Add("Black","Black List Password");

            return Task.FromResult(Errors);
        }

        private void SeedData()
        {
            if(BlackListPassword.Count==0)
            {
                BlackListPassword.Add("1");
                BlackListPassword.Add("123");
                BlackListPassword.Add("12345");
                BlackListPassword.Add("test");
                BlackListPassword.Add("password");
                BlackListPassword.Add("***");
            }
        }       
    }

    public class ValidationPassword_EFCore : IValidationPassword
    {
        private readonly IOptions<PasswordValidationConfig> config;
        private readonly IUserPasswordChangeHistoryService userPasswordChangeHistory;
        private readonly IUserPasswordBlackListService userPasswordBlackList;

        public ValidationPassword_EFCore(IOptions<PasswordValidationConfig> config, IUserPasswordChangeHistoryService userPasswordChangeHistory, IUserPasswordBlackListService userPasswordBlackList)
        {
            this.config = config;
            this.userPasswordChangeHistory = userPasswordChangeHistory;
            this.userPasswordBlackList = userPasswordBlackList;
        }

        public async Task<Dictionary<string, string>> ValidatePassword(string userid, string password)
        {
            Dictionary<String, String> Errors = new Dictionary<String, String>();

            if (await userPasswordBlackList.PasswordIsExistsAsync(password))
                Errors.Add("BlackListPassword", $"Password Is Weak");


            int MonthCount = config.Value.MonthCount;
            int TopPasswordCount = config.Value.PasswordCount;
            MonthCount = MonthCount > 0 ? MonthCount * (-1) : MonthCount;

            var History = await userPasswordChangeHistory.GetHistoryByDate(userid, DateTime.Now.AddMonths(MonthCount), DateTime.Now);
            History = History.Take(TopPasswordCount).
                OrderByDescending(x => x.ChangeDate)
                .ToList();

            if (History.Where(x => x.OldPassword == password).Count() > 0)
                Errors.Add("DuplicatePassword", $"Password Is Duplicate In Last {Math.Abs(MonthCount)} Months");

            return Errors;
        }
    }
}
