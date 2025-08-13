using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Reservation
{
    public partial class ListerRESERVATION : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ListerRESERVATION()
        {
            InitializeComponent();
        }

        private void txtsearchRESERVATION_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                string searchQuery = txtsearchRESERVATION.Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Id, DateDebut, DateFin, Clientid, Chambreld, Employerld 
                FROM Reservation 
                WHERE 
                    CAST(Id AS NVARCHAR) LIKE @search", con);

                    cmd.Parameters.AddWithValue("@search", $"%{searchQuery}%");

                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvRESERVATIONliste.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifie si une réservation est sélectionnée
                if (dgvRESERVATIONliste.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une réservation à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Récupère la ligne sélectionnée
                DataRowView selectedRow = (DataRowView)dgvRESERVATIONliste.SelectedItem;

                // Vérifie que la colonne Id existe
                if (!selectedRow.Row.Table.Columns.Contains("Id"))
                {
                    MessageBox.Show("La colonne 'Id' est introuvable dans les données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Récupère l'ID de la réservation
                int reservationId = (int)selectedRow["Id"];

                // Demande une confirmation avant de supprimer
                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette réservation ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                // Supprime la réservation de la base de données
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Reservation WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", reservationId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("La réservation a été supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Aucune réservation n'a été supprimée.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // Rafraîchit la DataGrid
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvRESERVATIONliste_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvRESERVATIONliste.ItemsSource = dt.DefaultView;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors de la mise à jour de la liste: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dgvRESERVATIONliste.Items.Count == 0)
            {
                MessageBox.Show("No data available to export!", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dgvRESERVATIONliste.SelectAll();
            ApplicationCommands.Copy.Execute(null, dgvRESERVATIONliste);

            string clipboardData = (string)Clipboard.GetData(DataFormats.Text);
            if (!string.IsNullOrEmpty(clipboardData))
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook = xlapp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets[1];

                for (int j = 0; j < dgvRESERVATIONliste.Columns.Count; j++)
                {
                    xlsheet.Cells[1, j + 1] = dgvRESERVATIONliste.Columns[j].Header.ToString();
                }

                string[] rows = clipboardData.Split('\n');
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] cells = rows[i].Split('\t');
                    for (int j = 0; j < cells.Length; j++)
                    {
                        xlsheet.Cells[i + 2, j + 1] = cells[j];
                    }
                }

                xlsheet.Columns.AutoFit();
            }
            else
            {
                MessageBox.Show("No data to export!", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_AddReservation_Click(object sender, RoutedEventArgs e)
        {
            AjouterRESERVATION addReservationWindow = new AjouterRESERVATION();
            addReservationWindow.ShowDialog();
            RefreshDataGrid();
        }

        private void Button_ModifyReservation_Click(object sender, RoutedEventArgs e)
        {
            ModifierRESERV modifyReservationWindow = new ModifierRESERV();
            modifyReservationWindow.ShowDialog();
            RefreshDataGrid();
        }
    }
}