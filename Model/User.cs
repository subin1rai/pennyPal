using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public class User
    {
        public int Userid { get; set; } // Unique identifier for the user
        public required string Username { get; set; } // Name of the user
        public required string Email { get; set; } // Email address
        public required string Password { get; set; } // Contact number
        public string Address { get; set; } // Address (optional)
        public DateTime CreatedOn { get; set; } = DateTime.Now; // Date the user was added
        public List<Debts> Debts { get; set; } = new();
        public List<Transactions> Transactions { get; set; } = new();
    }

}
