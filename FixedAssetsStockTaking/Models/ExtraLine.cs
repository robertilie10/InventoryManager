using FixedAssetsStockTaking.Migrations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FixedAssetsStockTaking.Models
{
    public class ExtraLine
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Inventar")]
        public int InventarID { get; set; }
        public Inventar? Inventar { get; set; }

        [Key]
        [MaxLength(3)]
        public string MFLine { get; set; }       
    }
}
