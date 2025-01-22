using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace ProoiectVladSipos.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(100), NotNull]
        public string Name { get; set; }

        [MaxLength(100), NotNull]
        public string Email { get; set; }

        // Colecție de credite asociate
        [OneToMany]
        public List<Credits> Credits { get; set; }
    }
}
