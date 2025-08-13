using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Reservation
{
    public partial class ModifierRESERV : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ModifierRESERV()
        {
            InitializeComponent();
        }

        private void btnModifierReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateDebut = dpDateDebut.SelectedDate ?? DateTime.MinValue;
                DateTime dateFin = dpDateFin.SelectedDate ?? DateTime.MinValue;
                int clientId = int.Parse(txtClientId.Text);
                int chambreId = int.Parse(txtChambreId.Text);
                int employeId = int.Parse(txtEmployeId.Text);

                DataRowView selectedRow = (DataRowView)dgvReservationModif.SelectedItem;
                int reservationId = (int)selectedRow["Id"];

                string query = @"
                    UPDATE Reservation
                    SET DateDebut = @DateDebut, DateFin = @DateFin, Clientid = @ClientId, 
                        Chambreld = @ChambreId, Employerld = @EmployeId
                    WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DateDebut", dateDebut);
                    command.Parameters.AddWithValue("@DateFin", dateFin);
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@ChambreId", chambreId);
                    command.Parameters.AddWithValue("@EmployeId", employeId);
                    command.Parameters.AddWithValue("@Id", reservationId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ChargerReservations();
                MessageBox.Show("Réservation modifiée avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la modification : " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerReservations();
        }

        private void ChargerReservations()
        {
            try
            {
                string query = @"SELECT Id, DateDebut, DateFin, Clientid, Chambreld, Employerld FROM Reservation";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvReservationModif.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors du chargement des réservations : " + ex.Message);
            }
        }

        private void dgvReservationModif_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgvReservationModif.SelectedItem != null)
            {
                try
                {
                    DataRowView selectedRow = (DataRowView)dgvReservationModif.SelectedItem;
                    dpDateDebut.SelectedDate = (DateTime)selectedRow["DateDebut"];
                    dpDateFin.SelectedDate = (DateTime)selectedRow["DateFin"];
                    txtClientId.Text = selectedRow["Clientid"].ToString();
                    txtChambreId.Text = selectedRow["Chambreld"].ToString();
                    txtEmployeId.Text = selectedRow["Employerld"].ToString();
                    btnModifierReservation.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la sélection de la réservation : " + ex.Message);
                }
            }
        }
    }
}