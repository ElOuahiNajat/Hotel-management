using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Reservation
{
    public partial class AjouterRESERVATION : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public AjouterRESERVATION()
        {
            InitializeComponent();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des champs
            DateTime dateDebut = dpDateDebut.SelectedDate ?? DateTime.Now;
            DateTime dateFin = dpDateFin.SelectedDate ?? DateTime.Now;
            string clientID = txtClientID.Text;
            string chambreID = txtChambreID.Text;
            string employeID = txtEmployeID.Text;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    // Requête SQL avec les noms de colonnes exacts
                    string query = "INSERT INTO Reservation (DateDebut, DateFin, Clientid, Chambreld, Employerld) " +
                                  "VALUES (@DateDebut, @DateFin, @Clientid, @Chambreld, @Employerld)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);
                    cmd.Parameters.AddWithValue("@Clientid", clientID);
                    cmd.Parameters.AddWithValue("@Chambreld", chambreID);
                    cmd.Parameters.AddWithValue("@Employerld", employeID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Réservation ajoutée avec succès !");
                        // Réinitialiser les champs
                        dpDateDebut.SelectedDate = null;
                        dpDateFin.SelectedDate = null;
                        txtClientID.Text = string.Empty;
                        txtChambreID.Text = string.Empty;
                        txtEmployeID.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Échec de l'ajout de la réservation.");
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