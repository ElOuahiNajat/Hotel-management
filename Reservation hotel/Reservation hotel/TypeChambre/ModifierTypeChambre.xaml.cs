using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.TypeChambre
{
    public partial class ModifierTypeChambre : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ModifierTypeChambre()
        {
            InitializeComponent();
        }

        private void btnModifierTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = txtType.Text;
                decimal prix = decimal.Parse(txtPrix.Text);

                DataRowView selectedRow = (DataRowView)dgvTypeChambreModif.SelectedItem;
                int typeChambreId = (int)selectedRow["Id"];

                string query = @"
                    UPDATE Type_Chambres
                    SET Type = @Type, Prix = @Prix
                    WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Type", type);
                    command.Parameters.AddWithValue("@Prix", prix);
                    command.Parameters.AddWithValue("@Id", typeChambreId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ChargerTypeChambres();
                MessageBox.Show("Type de chambre modifié avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la modification : " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerTypeChambres();
        }

        private void ChargerTypeChambres()
        {
            try
            {
                string query = @"SELECT Id, Type, Prix FROM Type_Chambres";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvTypeChambreModif.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors du chargement des types de chambres : " + ex.Message);
            }
        }

        private void dgvTypeChambreModif_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgvTypeChambreModif.SelectedItem != null)
            {
                try
                {
                    DataRowView selectedRow = (DataRowView)dgvTypeChambreModif.SelectedItem;
                    txtType.Text = selectedRow["Type"].ToString();
                    txtPrix.Text = selectedRow["Prix"].ToString();
                    btnModifierTypeChambre.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la sélection du type de chambre : " + ex.Message);
                }
            }
        }
    }
}