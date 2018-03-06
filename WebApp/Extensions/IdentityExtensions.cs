using System.Security.Claims;
using System.Security.Principal;
namespace WebApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetCulture(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Language");
            
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "en-GB";
        }

        public static string GetShutdownInMin(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ShutdownInMin");

            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "100";
        }
        public static string GetTimeZone(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("TimeZone");

            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "Egypt Standard Time";
        }

        public static string GetMessages(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Messages");

            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "en-GB";
        }

        public static int GetDefaultCompany(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DefaultCompany");

            // Test for null to avoid issues during local testing
            return (claim != null) ? int.Parse(claim.Value) : 0;
        }

        public static int GetEmpId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("EmpId");

            // Test for null to avoid issues during local testing
            return (claim != null) ? int.Parse(claim.Value) : -1;
        }



        public static bool RTL(this IIdentity identity)
        {
            string[] array = { "ps", "ar", "ur", "ku", "fa" };
            string lang = GetCulture(identity).Split('-')[0];
            return System.Array.IndexOf(array, lang) >= 0;
        }

        public static string GetLanguage(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Language");

            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "en-GB";
        }

        public static bool GetAllowInsertCode(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AllowInsertCode");
            return (claim != null) ? bool.Parse(claim.Value) : false;
           
        }

        public static bool LogTooltip(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LogTooltip");
            return (claim != null) ? bool.Parse(claim.Value) : false;
        }
        public static bool IsAvailable(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("IsAvailable");
            return (claim != null) ? bool.Parse(claim.Value) : false;
        }
        public static bool CanCustomize(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CanCustomize");
            return (claim != null) ? bool.Parse(claim.Value) : false;
        }
        public static bool Developer(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Developer");
            return (claim != null) ? bool.Parse(claim.Value) : false;
        }
    }
}