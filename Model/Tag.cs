using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PennyPal.Model
{
    public class Tag
    {
        public int Id { get; set; }
        public required string tagName { get; set; } 
    }
}
