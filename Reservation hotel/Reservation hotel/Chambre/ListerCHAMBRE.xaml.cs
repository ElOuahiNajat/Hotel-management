using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Windows.Input;

namespace Reservation_hotel.Chambre
{
    public partial class ListerCHAMBRE : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public ListerCHAMBRE()
        {
            InitializeComponent();
        }

        private void txtsearchChambre_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                string searchQuery = txtsearchChambre.Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT * FROM Chambres 
                WHERE 
                    CAST(NumeroChambre AS NVARCHAR) LIKE @search OR
                    Disponibilite LIKE @search OR
                    CAST(TypeChambreId AS NVARCHAR) LIKE @search", con);

                    cmd.Parameters.AddWithValue("@search", $"%{searchQuery}%");

                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvChambreListe.ItemsSource = dt.DefaultView;
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
                if (dgvChambreListe.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une chambre à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette chambre ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                DataRowView selectedRow = (DataRowView)dgvChambreListe.SelectedItem;
                int numeroChambre = (int)selectedRow["NumeroChambre"];

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Chambres WHERE NumeroChambre = @NumeroChambre", con);
                    cmd.Parameters.AddWithValue("@NumeroChambre", numeroChambre);
                    cmd.ExecuteNonQuery();
                }

                RefreshDataGrid();
                MessageBox.Show("La chambre a été supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvChambreListe_Loaded(object sender, RoutedEventArgs e)
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Chambres", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvChambreListe.ItemsSource = dt.DefaultView;
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
            if (dgvChambreListe.Items.Count == 0)
            {
                MessageBox.Show("Aucune donnée disponible pour l'exportation !", "Erreur d'exportation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dgvChambreListe.SelectAll();
            ApplicationCommands.Copy.Execute(null, dgvChambreListe);

            string clipboardData = (string)Clipboard.GetData(DataFormats.Text);
            if (!string.IsNullOrEmpty(clipboardData))
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook = xlapp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets[1];

                for (int j = 0; j < dgvChambreListe.Columns.Count; j++)
                {
                    xlsheet.Cells[1, j + 1] = dgvChambreListe.Columns[j].Header.ToString();
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

        private void Button_AddChambre_Click(object sender, RoutedEventArgs e)
        {
            AjouterCHAMBRE addChambreWindow = new AjouterCHAMBRE();
            addChambreWindow.ShowDialog();
            RefreshDataGrid();
        }

        private void Button_ModifyChambre_Click(object sender, RoutedEventArgs e)
        {
            ModifierCHAMBRE modifyChambreWindow = new ModifierCHAMBRE();
            modifyChambreWindow.ShowDialog();
            RefreshDataGrid();
        }
    }
}