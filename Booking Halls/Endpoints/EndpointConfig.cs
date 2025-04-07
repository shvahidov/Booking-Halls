namespace Booking_Halls.Endpoints
{
    using Microsoft.AspNetCore.Routing;

    public static class EndpointConfig
    {
        public static void RegisterRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "login",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            endpoints.MapControllerRoute(
                name: "register",
                pattern: "{controller=Register}/{action=Register}/{id?}");

            endpoints.MapControllerRoute(
                name: "admin",
                pattern: "{controller=Admin}/{action=Index}/{id?}");
        }
    }

}
