using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Chambre
{
    public partial class ModifierCHAMBRE : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ModifierCHAMBRE()
        {
            InitializeComponent();
        }

        private void btnModifierChambre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string disponibilite = txtDisponibilite.Text;
                int typeChambreId = int.Parse(txtTypeChambreId.Text);

                DataRowView selectedRow = (DataRowView)dgvChambreModif.SelectedItem;
                int numeroChambre = (int)selectedRow["NumeroChambre"];

                string query = @"
                    UPDATE Chambres
                    SET Disponibilite = @Disponibilite, TypeChambreId = @TypeChambreId
                    WHERE NumeroChambre = @NumeroChambre";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Disponibilite", disponibilite);
                    command.Parameters.AddWithValue("@TypeChambreId", typeChambreId);
                    command.Parameters.AddWithValue("@NumeroChambre", numeroChambre);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ChargerChambres();
                MessageBox.Show("Chambre modifiée avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la modification : " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerChambres();
        }

        private void ChargerChambres()
        {
            try
            {
                string query = @"SELECT NumeroChambre, Disponibilite, TypeChambreId FROM Chambres";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvChambreModif.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors du chargement des chambres : " + ex.Message);
            }
        }

        private void dgvChambreModif_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgvChambreModif.SelectedItem != null)
            {
                try
                {
                    DataRowView selectedRow = (DataRowView)dgvChambreModif.SelectedItem;
                    txtNumeroChambre.Text = selectedRow["NumeroChambre"].ToString();
                    txtDisponibilite.Text = selectedRow["Disponibilite"].ToString();
                    txtTypeChambreId.Text = selectedRow["TypeChambreId"].ToString();
                    btnModifierChambre.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la sélection de la chambre : " + ex.Message);
                }
            }
        }
    }
}