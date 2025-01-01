using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public class User
    {
        public required string Username { get; set; }
        public required string Password { get; set; } // This will store the hashed password
        public required string Email { get; set; }
    }

}
