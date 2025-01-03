using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public class Transactions
    {
        public int Id { get; set; }
        public decimal Debit { get; set; } // Cash out
        public decimal Credit { get; set; } // Cash in
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }

}
