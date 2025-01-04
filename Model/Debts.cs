using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public enum DebtStatus
    {
        Pending,
        Paid,
        Overdue,
        Clear // Added Clear status
    }


    public class Debts
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } // Total debt amount
        public decimal PaidAmount { get; set; } // Amount paid towards debt
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DebtStatus Status { get; set; } // Enum to represent the status
    }

}
