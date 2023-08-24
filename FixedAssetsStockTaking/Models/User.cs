using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FixedAssetsStockTaking.Models
{
    public class User
    {
        [Key]
        [Column(Order = 0)]
        public string Username { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Inventar")]
        public int InventarID { get; set; }
        public Inventar? Inventar { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [ForeignKey("Role")]
        public string RoleID { get; set; }
        public Role? Role { get; set; }

        [NotMapped]
        public int OrderRank { get; set; }
    }
}
