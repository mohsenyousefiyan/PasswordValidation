using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordValidation.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers.DAL.EF.Configs
{
    public class UserPasswordChangeHistoryConfiguration : IEntityTypeConfiguration<UserPasswordChangeHistory>
    {
        
        public void Configure(EntityTypeBuilder<UserPasswordChangeHistory> builder)
        {
            
            #region PropertyConfig

            builder.Property(x => x.Id)
                .HasColumnName("ID");

            builder.Property(x => x.UserId)
                .HasColumnName("LinkUserID");

            builder.Property(x => x.OldPassword)
                .HasMaxLength(200)
                .IsRequired()
                .IsUnicode();

            #endregion

            #region RelationConfig

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
