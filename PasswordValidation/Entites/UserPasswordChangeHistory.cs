using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidation.Entites
{
    public class UserPasswordChangeHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime ChangeDate { get; set; }
        public string OldPassword { get; set; }
    }
}
