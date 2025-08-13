using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Windows.Input;

namespace Reservation_hotel.TypeChambre
{
    public partial class ListerTypeChambre : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ListerTypeChambre()
        {
            InitializeComponent();
        }

        private void txtsearchTypeChambre_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                string searchQuery = txtsearchTypeChambre.Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT * FROM Type_Chambres 
                WHERE 
                    CAST(Id AS NVARCHAR) LIKE @search OR
                    Type LIKE @search OR
                    CAST(Prix AS NVARCHAR) LIKE @search", con);

                    cmd.Parameters.AddWithValue("@search", $"%{searchQuery}%");

                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvTypeChambreListe.ItemsSource = dt.DefaultView;
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
                if (dgvTypeChambreListe.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner un type de chambre à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce type de chambre ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                DataRowView selectedRow = (DataRowView)dgvTypeChambreListe.SelectedItem;
                int typeChambreId = (int)selectedRow["Id"];

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Type_Chambres WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", typeChambreId);
                    cmd.ExecuteNonQuery();
                }

                RefreshDataGrid();
                MessageBox.Show("Le type de chambre a été supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvTypeChambreListe_Loaded(object sender, RoutedEventArgs e)
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Type_Chambres", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvTypeChambreListe.ItemsSource = dt.DefaultView;
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
            if (dgvTypeChambreListe.Items.Count == 0)
            {
                MessageBox.Show("Aucune donnée disponible pour l'exportation !", "Erreur d'exportation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dgvTypeChambreListe.SelectAll();
            ApplicationCommands.Copy.Execute(null, dgvTypeChambreListe);

            string clipboardData = (string)Clipboard.GetData(DataFormats.Text);
            if (!string.IsNullOrEmpty(clipboardData))
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook = xlapp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets[1];

                for (int j = 0; j < dgvTypeChambreListe.Columns.Count; j++)
                {
                    xlsheet.Cells[1, j + 1] = dgvTypeChambreListe.Columns[j].Header.ToString();
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
                MessageBox.Show("Aucune donnée à exporter !", "Erreur d'exportation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_AddTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            AjouterTypeChambre addTypeChambreWindow = new AjouterTypeChambre();
            addTypeChambreWindow.ShowDialog();
            RefreshDataGrid();
        }

        private void Button_ModifyTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            ModifierTypeChambre modifyTypeChambreWindow = new ModifierTypeChambre();
            modifyTypeChambreWindow.ShowDialog();
            RefreshDataGrid();
        }
    }
}