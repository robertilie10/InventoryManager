using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FixedAssetsStockTaking.Models
{
    public class MijlocFix
    {

        [Key]
        public int ID { get; set; }

        [MaxLength(100)]
        public string COD { get; set; }

        [MaxLength(100)]
        public string Line { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string AlocatedCenter { get; set; }

        [MaxLength(100)]
        public string FoundLine { get; set; }

        [MaxLength(100)]
        public string FoundBy { get; set; }

        [MaxLength(100)]
        public string FoundAt { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        [MaxLength(100)]
        public string VerifiedBy { get; set; }

        [MaxLength(100)]
        public string VerifiedAt { get; set; }

        [MaxLength(3)]
        public string Locked { get; set; }

        [Required]
        public int WrittenQ { get; set; }

        [Required]
        public int FoundQ { get; set; }

        [MaxLength(100)]
        public string? Observations { get; set; }

        [ForeignKey("Inventar")]

        public int InventarID { get; set; }

        public Inventar Inventar { get; set; }

        [NotMapped]
        public string DisplayComment { get; set; }

        [NotMapped]
        public int OrderRank { get; set; }
    }
}
