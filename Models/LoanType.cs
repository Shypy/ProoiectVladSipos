using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ProoiectVladSipos.Models
{
    public class LoanType
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }               
        public decimal DefaultInterest { get; set; }    
        public int DefaultDurationMonths { get; set; }  
    }
}
