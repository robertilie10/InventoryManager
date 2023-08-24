using System.ComponentModel.DataAnnotations;

namespace FixedAssetsStockTaking.Models
{
    public class Role
    {
        [Key]
        public string Name { get; set; }
    }
}
