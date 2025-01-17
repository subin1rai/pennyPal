using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public enum DebtStatus
    {
        Pending = 0,
        Paid = 1,
        Overdue = 2,
        Clear = 3
    }
    public class Debts
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; } 
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DebtStatus Status { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
}
