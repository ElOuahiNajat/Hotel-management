using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.TypeChambre
{
    public partial class AjouterTypeChambre : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public AjouterTypeChambre()
        {
            InitializeComponent();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string type = txtType.Text;
            string prix = txtPrix.Text;
            string typeChambre = txtTypeChambre.Text;

            // Vérifiez que tous les champs sont remplis
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(prix) || string.IsNullOrEmpty(typeChambre))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Type_Chambres (Type, Prix, typeChambre) VALUES (@Type, @Prix, @typeChambre)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Prix", prix);
                    cmd.Parameters.AddWithValue("@typeChambre", typeChambre);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Type de chambre ajouté avec succès!");
                        txtType.Text = string.Empty;
                        txtPrix.Text = string.Empty;
                        txtTypeChambre.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Échec de l'ajout du type de chambre.");
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