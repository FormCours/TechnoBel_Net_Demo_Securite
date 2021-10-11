using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Securite.Models
{
    public class MemberRegister
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [MinLength(2)]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}