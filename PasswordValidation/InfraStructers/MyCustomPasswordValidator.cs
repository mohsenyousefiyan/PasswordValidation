using Microsoft.AspNetCore.Identity;
using PasswordValidation.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers
{
    public class MyCustomPasswordValidator:PasswordValidator<AppUser>
    {
        private readonly IValidationPassword validationPassword;

        public MyCustomPasswordValidator(IValidationPassword validationPassword)
        {
            this.validationPassword = validationPassword;
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> Errors = new List<IdentityError>();
            var result = await base.ValidateAsync(manager, user, password);

            if (!result.Succeeded)
                foreach (var item in result.Errors)
                    Errors.Add(item);

            var customresult =await validationPassword.ValidatePassword(user.Id,password);
            if (customresult.Count > 0)
                foreach (var item in customresult)
                    Errors.Add(new IdentityError() { Code = item.Key, Description = item.Value });

            return Errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(Errors.ToArray());
        }
    }
}
