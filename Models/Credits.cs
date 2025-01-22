using ProoiectVladSipos.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;

public class Credits
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public decimal LoanedAmount { get; set; }
    public decimal AnualInterest { get; set; }
    public int LoanMonths { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    // Relația cu User
    [ForeignKey(typeof(User))]
    public int UserID { get; set; }
    [ManyToOne]
    public User User { get; set; }

    // Relația cu LoanType
    [ForeignKey(typeof(LoanType))]
    public int LoanTypeID { get; set; }
    [ManyToOne]
    public LoanType LoanType { get; set; }
}
