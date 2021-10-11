using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Securite.DAL.Entities
{
    public class MemberCredential
    {
        public string HashPassword { get; set; }
        public string Salt { get; set; }
    }
}
