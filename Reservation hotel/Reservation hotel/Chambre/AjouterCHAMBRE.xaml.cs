using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Chambre
{
    public partial class AjouterCHAMBRE : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public AjouterCHAMBRE()
        {
            InitializeComponent(); // Cette méthode est générée automatiquement
        }

        // Gestionnaire d'événements pour le bouton Ajouter
        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string numeroChambre = txtNumeroChambre.Text;
            string disponibilite = txtDisponibilite.Text;
            string typeChambreId = txtTypeChambreId.Text;

            // Vérifiez que tous les champs sont remplis
            if (string.IsNullOrEmpty(numeroChambre) || string.IsNullOrEmpty(disponibilite) || string.IsNullOrEmpty(typeChambreId))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Chambres (NumeroChambre, Disponibilite, TypeChambreId) VALUES (@NumeroChambre, @Disponibilite, @TypeChambreId)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@NumeroChambre", numeroChambre);
                    cmd.Parameters.AddWithValue("@Disponibilite", disponibilite);
                    cmd.Parameters.AddWithValue("@TypeChambreId", typeChambreId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Chambre ajoutée avec succès!");
                        txtNumeroChambre.Text = string.Empty;
                        txtDisponibilite.Text = string.Empty;
                        txtTypeChambreId.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Échec de l'ajout de la chambre.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite : " + ex.Message);
                }
            }
        }
    }
}