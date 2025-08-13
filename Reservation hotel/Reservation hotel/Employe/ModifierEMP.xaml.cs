using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Employe
{
    public partial class ModifierEMP : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public ModifierEMP()
        {
            InitializeComponent();
        }

        private void btnModifierEMP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nom = txtnom.Text;
                string prenom = txtprenom.Text;
                string cin = txtcin.Text;
                string telephone = txttel.Text;
                decimal salaire = decimal.Parse(txtsalaire.Text);
                string codeAdmin = txtcode.Text;
                DataRowView selectedRow = (DataRowView)dgvEMPmodif.SelectedItem;
                int employeId = (int)selectedRow["Id"];
                string query = @"
                    UPDATE Employer
                    SET Nom = @Nom, Prenom = @Prenom, Cin = @Cin, Telephone = @Telephone, 
                        CodeAdmin = @CodeAdmin, Salaire = @Salaire
                    WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nom", nom);
                    command.Parameters.AddWithValue("@Prenom", prenom);
                    command.Parameters.AddWithValue("@Cin", cin);
                    command.Parameters.AddWithValue("@Telephone", telephone);
                    command.Parameters.AddWithValue("@Salaire", salaire);
                    command.Parameters.AddWithValue("@CodeAdmin", codeAdmin);
                    command.Parameters.AddWithValue("@Id", employeId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                ChargerEmployes();
                MessageBox.Show("Employé modifié avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la modification : " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerEmployes();
        }
        private void ChargerEmployes()
        {
            try
            {
                string query = @"SELECT Id, Nom, Prenom, Cin, Telephone, CodeAdmin, Salaire FROM Employer";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvEMPmodif.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors du chargement des employés : " + ex.Message);
            }
        }

        private void dgvEMPmodif_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgvEMPmodif.SelectedItem != null)
            {
                try
                {
                    DataRowView selectedRow = (DataRowView)dgvEMPmodif.SelectedItem;
                    txtnom.Text = selectedRow["Nom"].ToString();
                    txtprenom.Text = selectedRow["Prenom"].ToString();
                    txtcin.Text = selectedRow["Cin"].ToString();
                    txttel.Text = selectedRow["Telephone"].ToString();
                    txtsalaire.Text = selectedRow["Salaire"].ToString();
                    txtcode.Text = selectedRow["CodeAdmin"].ToString();
                    btnModifierEMP.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la sélection de l'employé : " + ex.Message);
                }
            }
        }
    }
}
