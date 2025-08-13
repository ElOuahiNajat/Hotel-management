using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        // Ajoutez ces propri�t�s pour afficher le message de bienvenue
        public string WelcomeMessage { get; set; }
        public string FullName { get; set; }

        public IActionResult OnPost()
        {
            // V�rifiez si les champs sont vides
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Please fill in all fields.";
                return Page();
            }

            // Cha�ne de connexion � la base de donn�es
            string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Requ�te SQL pour v�rifier l'existence du client
                string query = "SELECT Id, Nom, Prenom FROM Client WHERE Email = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Si le client existe
                        {
                            // Stockez les variables de session
                            HttpContext.Session.SetInt32("ClientId", reader.GetInt32(0));
                            HttpContext.Session.SetString("ClientName", reader.GetString(1) + " " + reader.GetString(2));

                            // D�finissez le message de bienvenue et le nom complet
                            WelcomeMessage = "Welcome";
                            FullName = reader.GetString(1) + " " + reader.GetString(2);

                            // Restez sur la m�me page pour afficher le message de bienvenue
                            return Page();
                        }
                        else
                        {
                            ErrorMessage = "Invalid Email or Password.";
                        }
                    }
                }
            }

            return Page();
        }
    }
}