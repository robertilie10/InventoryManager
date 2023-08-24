using FixedAssetsStockTaking.Data;
using Microsoft.AspNetCore.Http;

namespace FixedAssetsStockTaking.Middleware
{
    public class CustomAuth
    {
        public static bool IsAllowed(FixedAssetsStockTakingContext context, HttpContext httpContext, int StockTakeID)
        {
            var User = httpContext.User.Identity.Name;

            if (User != null)
            {
                User = User.Replace("MMRMAKITA\\", "");
            }
            else
            {
                return false;
            }


            return context.SuperAdmin.Any(admin => admin.Name == User); ;
        }


        public static string GetUserName(HttpContext httpContext)
        {
            var User = httpContext.User.Identity.Name;

            if (User != null)
            {
                return User.Replace("MMRMAKITA\\", "");
            }
            else
            {
                return "";
            }
        }
    }
}
