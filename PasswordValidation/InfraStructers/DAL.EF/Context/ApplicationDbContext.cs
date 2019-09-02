using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasswordValidation.Entites;
using PasswordValidation.InfraStructers.DAL.EF.Configs;

namespace PasswordValidation.InfraStructers.DAL.EF.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IUnitOfWork
    {
        public DbSet<PasswordBlackList> BlackLists { get; set; }
        public DbSet<UserPasswordChangeHistory> UserPasswordChangeHistories { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
         
            builder.ApplyConfiguration(new PasswordBlackListConfiguration());
            builder.ApplyConfiguration(new UserPasswordChangeHistoryConfiguration());
        }
    }
}
