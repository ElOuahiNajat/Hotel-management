using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Supprimer les informations de session
            HttpContext.Session.Remove("ClientId");
            HttpContext.Session.Remove("ClientName");

            // Rediriger vers la page de connexion
            return RedirectToPage("/Login");
        }
    }
}