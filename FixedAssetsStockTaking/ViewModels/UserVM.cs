using FixedAssetsStockTaking.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FixedAssetsStockTaking.ViewModels
{
    public class UserVM
    {
        public class User
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public int EmployeeID { get; set; }
            public int InventarID { get; set; }
            public string RoleID { get; set; }
        }

    }
}
