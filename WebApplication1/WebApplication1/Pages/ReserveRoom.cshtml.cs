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
                ErrorMessage = "La date de fin doit �tre apr�s la date de d�but.";
                return Page();
            }

            // Calculer le nombre de jours
            var nombreDeJours = (DateFin - DateDebut).Days;

            // R�cup�rer le prix de la chambre (exemple fixe)
            float prixParNuit = 100.0f; // Remplacez par la logique pour r�cup�rer le prix
            PrixTotal = prixParNuit * nombreDeJours;

            return Page();
        }

        public IActionResult OnPostReserve()
        {
            // V�rifier que les dates sont valides
            if (DateDebut >= DateFin)
            {
                ErrorMessage = "La date de fin doit �tre apr�s la date de d�but.";
                return Page();
            }

            // Enregistrer la r�servation dans la base de donn�es
            string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ins�rer la r�servation
                string insertQuery = @"
                    INSERT INTO Reservation (DateDebut, DateFin, Clientid)
                    VALUES (@DateDebut, @DateFin, @Clientid)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DateDebut", DateDebut);
                    command.Parameters.AddWithValue("@DateFin", DateFin);
                    command.Parameters.AddWithValue("@Clientid", 1); // Remplacez par l'ID du client connect�

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