using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PasswordValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers
{
    public class CustomUserManager<TUser> : UserManager<TUser> where TUser : class
    {        
        private readonly IUserStore<TUser> store;
        private readonly IOptions<IdentityOptions> optionsAccessor;
        private readonly IPasswordHasher<TUser> passwordHasher;
        private readonly IEnumerable<IUserValidator<TUser>> userValidators;
        private readonly IEnumerable<IPasswordValidator<TUser>> passwordValidators;
        private readonly ILookupNormalizer keyNormalizer;
        private readonly IdentityErrorDescriber errors;
        private readonly IServiceProvider services;
        private readonly ILogger<UserManager<TUser>> logger;

        public CustomUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {            
            this.store = store;
            this.optionsAccessor = optionsAccessor;
            this.passwordHasher = passwordHasher;
            this.userValidators = userValidators;
            this.passwordValidators = passwordValidators;
            this.keyNormalizer = keyNormalizer;
            this.errors = errors;
            this.services = services;
            this.logger = logger;
        }

        public override  Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            return  base.CreateAsync(user, password);            
        }
        public override Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
