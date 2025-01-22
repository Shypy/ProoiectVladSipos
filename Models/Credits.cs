using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProoiectVladSipos.Models
{
    public class Credits
    {
        [PrimaryKey, AutoIncrement] 
        public int ID { get; set; }
        public decimal LoanedAmount { get; set; }
        public decimal AnualInterest {  get; set; }
        public int LoanMonths { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey(typeof(User))]
        public int UserID { get; set; }
        [ManyToOne]
        public User User { get; set; }
    }
}
