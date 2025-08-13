using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Pages
{
    public class RoomsModel : PageModel
    {
        // Liste pour stocker les informations des chambres
        public List<Chambre> Chambres { get; set; } = new List<Chambre>();

        // Propri�t� pour stocker le crit�re de tri
        public string SortBy { get; set; }

        // Propri�t�s pour la pagination
        public int PageIndex { get; set; } = 1; // Page actuelle (par d�faut 1)
        public int PageSize { get; set; } = 3; // Nombre de chambres par page
        public int TotalPages { get; set; } // Nombre total de pages

        public void OnGet(string sortBy = null, int pageIndex = 1)
        {
            SortBy = sortBy; // R�cup�rer le crit�re de tri depuis l'URL
            PageIndex = pageIndex; // R�cup�rer l'index de la page depuis l'URL

            // R�cup�rer les chambres disponibles depuis la base de donn�es
            string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        c.NumeroChambre, 
                        c.Disponibilite, 
                        t.Type, 
                        t.Prix
                    FROM 
                        Chambres c
                    INNER JOIN 
                        Type_Chambres t ON c.TypeChambreId = t.Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Ajouter chaque chambre � la liste
                            Chambres.Add(new Chambre
                            {
                                NumeroChambre = reader.GetInt32(0), // NumeroChambre (int)
                                Disponibilite = reader.GetBoolean(1), // Disponibilite (bit)
                                TypeChambre = reader.GetString(2), // Type de chambre (string)
                                Prix = (float)reader.GetDouble(3) // Prix (float)
                            });
                        }
                    }
                }
            }

            // Trier les chambres en fonction du crit�re de tri
            switch (SortBy)
            {
                case "numero":
                    Chambres = Chambres.OrderBy(c => c.NumeroChambre).ToList();
                    break;
                case "disponibilite":
                    Chambres = Chambres.OrderByDescending(c => c.Disponibilite).ToList();
                    break;
                case "prix":
                    Chambres = Chambres.OrderBy(c => c.Prix).ToList();
                    break;
                default:
                    // Par d�faut, trier par num�ro de chambre
                    Chambres = Chambres.OrderBy(c => c.NumeroChambre).ToList();
                    break;
            }

            // Calculer le nombre total de pages
            TotalPages = (int)Math.Ceiling(Chambres.Count / (double)PageSize);

            // Appliquer la pagination
            Chambres = Chambres
                .Skip((PageIndex - 1) * PageSize) // Ignorer les chambres des pages pr�c�dentes
                .Take(PageSize) // Prendre les chambres de la page actuelle
                .ToList();
        }
    }

    // Classe pour repr�senter une chambre
    public class Chambre
    {
        public int NumeroChambre { get; set; } // Num�ro de la chambre (int)
        public bool Disponibilite { get; set; } // Disponibilit� de la chambre (bit)
        public string TypeChambre { get; set; } // Type de chambre (string)
        public float Prix { get; set; } // Prix de la chambre (float)
    }
}