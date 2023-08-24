using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FixedAssetsStockTaking.Models
{
    public class SuperAdmin
    {

            [Key]
            public int ID { get; set; }
            [MaxLength(100)]
            public string Name { get; set; }

            //[MaxLength(100),NotMapped]
            //public string Email { get; set; }

            //[NotMapped]
            //public int EmployeeID { get; set; }

    }
}
