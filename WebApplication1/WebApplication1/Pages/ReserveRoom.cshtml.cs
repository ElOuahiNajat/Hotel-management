using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace WebApplication1.Pages
{
    public class ReserveRoomModel : PageModel
    {
        [BindProperty]
        public DateTime DateDebut { get; set; }

        [BindProperty]
        public DateTime DateFin { get; set; }

        public float PrixTotal { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (DateDebut >= DateFin)
            {
                ErrorMessage = "La date de fin doit être après la date de début.";
                return Page();
            }

            // Calculer le nombre de jours
            var nombreDeJours = (DateFin - DateDebut).Days;

            // Récupérer le prix de la chambre (exemple fixe)
            float prixParNuit = 100.0f; // Remplacez par la logique pour récupérer le prix
            PrixTotal = prixParNuit * nombreDeJours;

            return Page();
        }

        public IActionResult OnPostReserve()
        {
            // Vérifier que les dates sont valides
            if (DateDebut >= DateFin)
            {
                ErrorMessage = "La date de fin doit être après la date de début.";
                return Page();
            }

            // Enregistrer la réservation dans la base de données
            string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insérer la réservation
                string insertQuery = @"
                    INSERT INTO Reservation (DateDebut, DateFin, Clientid)
                    VALUES (@DateDebut, @DateFin, @Clientid)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DateDebut", DateDebut);
                    command.Parameters.AddWithValue("@DateFin", DateFin);
                    command.Parameters.AddWithValue("@Clientid", 1); // Remplacez par l'ID du client connecté

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ErrorMessage = "Erreur SQL : " + ex.Message;
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Une erreur s'est produite : " + ex.Message;
                        return Page();
                    }
                }
            }

            return RedirectToPage("/ReservationConfirmation");
        }
    }
}