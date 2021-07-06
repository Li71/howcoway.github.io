using System.Web.Mvc;

namespace goshopping.Areas.Member
{
    public class MemberAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Member_orders",
                "Member/{controller}/{action}/{id}/{code}",
                new { controller = "Orders" , action = "Index", id = UrlParameter.Optional , code = UrlParameter.Optional }
            );

            context.MapRoute(
                "Member_default",
                "Member/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}