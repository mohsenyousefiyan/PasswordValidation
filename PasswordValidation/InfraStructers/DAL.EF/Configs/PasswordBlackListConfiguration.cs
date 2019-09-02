using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordValidation.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.InfraStructers.DAL.EF.Configs
{
    public class PasswordBlackListConfiguration : IEntityTypeConfiguration<PasswordBlackList>
    {       
        public void Configure(EntityTypeBuilder<PasswordBlackList> builder)
        {
            #region PropertyConfig

            builder.HasKey(x => x.Password);

            builder.Property(x => x.Password)
                .HasMaxLength(200)
                .IsRequired()
                .IsUnicode();

            #endregion

            #region SeedData

            builder.HasData(new PasswordBlackList { Password = "123" },
                new PasswordBlackList { Password = "12345" },
                new PasswordBlackList { Password = "aaa" },
                new PasswordBlackList { Password = "password" }
                );

            #endregion
        }
    }
}
