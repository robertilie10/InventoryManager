using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace FixedAssetsStockTaking.Models
{
    public class Inventar
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100), DisplayName("Created by")]
        public string CreatedBy { get; set; }

        [MaxLength(100),DisplayName("Closed by")]
         public string ClosedBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}"),DisplayName("Create date")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}"), DisplayName("Closed date")]
        public DateTime ClosedDate { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }

        [MaxLength(100)]
        [NotMapped]
        public string Comment { get; set; }
        public ICollection<MijlocFix> MijlocFixes { get; set; }

    }
}
