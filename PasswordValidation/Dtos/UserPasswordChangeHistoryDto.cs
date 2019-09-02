using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.Dtos
{
    public class UserPasswordChangeHistoryDto
    {
        public DateTime ChangeDate { get; set; }
        public string OldPassword { get; set; }
    }
}
