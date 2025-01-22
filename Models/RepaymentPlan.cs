using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace ProoiectVladSipos.Models
{
    public class RepaymentPlan
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(Credits))]
        public int CreditID { get; set; }

        // Numărul plății (1, 2, 3, ... n)
        public int InstallmentNumber { get; set; }

        // Suma totală de plată în luna respectivă
        public decimal PaymentAmount { get; set; }

        // Partea din PaymentAmount care acoperă principalul (soldul)
        public decimal Principal { get; set; }

        // Partea din PaymentAmount care acoperă dobânda
        public decimal Interest { get; set; }

        // Soldul rămas după plată
        public decimal RemainingBalance { get; set; }

        // Data scadentă a acestei plăți
        public DateTime PaymentDate { get; set; }
    }
}
