using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FixedAssetsStockTaking.Models
{
    public class CostCenter
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("Inventar")]
        public int InventarID { get; set; }
        public Inventar Inventar { get; set; }
    }
}
