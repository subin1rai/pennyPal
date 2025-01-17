using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public class User
    {
        public int Userid { get; set; } 
        public required string Username { get; set; } 
        public required string Email { get; set; }
        public required string Password { get; set; } 
        public string Address { get; set; } 
        public DateTime CreatedOn { get; set; } = DateTime.Now; 
        public List<Debts> Debts { get; set; } = new();
        public List<Transactions> Transactions { get; set; } = new();
        public string PreferredCurrency { get; set; } = "USD"; 
    }

}
