using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Employe
{
    public partial class ListerEMP : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public ListerEMP()
        {
            InitializeComponent();
        }
        // Search
        //private void Button_Search_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(txtsearchEMP.Text))
        //        {
        //            MessageBox.Show("Veuillez entrer un ID pour la recherche.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            RefreshDataGrid();
        //            return;
        //        }
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            con.Open();
        //            SqlCommand cmd = new SqlCommand("SELECT * FROM Employer WHERE Id = @id", con);
        //            cmd.Parameters.AddWithValue("@id", txtsearchEMP.Text);
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                DataTable dt = new DataTable();
        //                dt.Load(dr);
        //                dgvEMPliste.ItemsSource = dt.DefaultView;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        private void txtsearchEMP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                string searchQuery = txtsearchEMP.Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT * FROM Employer 
                WHERE 
                    CAST(Id AS NVARCHAR) LIKE @search OR
                    Nom LIKE @search OR
                    Prenom LIKE @search OR
                    Cin LIKE @search OR
                    Telephone LIKE @search OR
                    CodeAdmin LIKE @search OR
                    CAST(Salaire AS NVARCHAR) LIKE @search", con);

                    // Ajout du paramètre de recherche
                    cmd.Parameters.AddWithValue("@search", $"%{searchQuery}%");

                    // Charger les données filtrées
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvEMPliste.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Delete



        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvEMPliste.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner un employé à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Afficher la boîte de dialogue de confirmation
                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet employé ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return; // L'utilisateur a annulé la suppression
                }

                DataRowView selectedRow = (DataRowView)dgvEMPliste.SelectedItem;
                int empId = (int)selectedRow["Id"];

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Employer WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", empId);
                    cmd.ExecuteNonQuery();
                }

                RefreshDataGrid();
                MessageBox.Show("L'employé a été supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }










        private void dgvEMPliste_Loaded(object sender, RoutedEventArgs e)
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Employer", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvEMPliste.ItemsSource = dt.DefaultView;
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
            // Vérifie si la DataGrid contient des éléments
            if (dgvEMPliste.Items.Count == 0)
            {
                MessageBox.Show("No data available to export!", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sélectionner toutes les lignes
            dgvEMPliste.SelectAll();
            ApplicationCommands.Copy.Execute(null, dgvEMPliste);

            // Récupérer les données copiées dans le presse-papiers
            string clipboardData = (string)Clipboard.GetData(DataFormats.Text);
            if (!string.IsNullOrEmpty(clipboardData))
            {
                // Initialiser Excel
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                xlapp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWbook = xlapp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets[1];

                // Ajouter les en-têtes
                for (int j = 0; j < dgvEMPliste.Columns.Count; j++)
                {
                    xlsheet.Cells[1, j + 1] = dgvEMPliste.Columns[j].Header.ToString(); // Récupère les noms des colonnes
                }

                // Ajouter les données
                string[] rows = clipboardData.Split('\n'); // Séparer les lignes
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] cells = rows[i].Split('\t'); // Séparer les colonnes
                    for (int j = 0; j < cells.Length; j++)
                    {
                        xlsheet.Cells[i + 2, j + 1] = cells[j]; // Ajouter les données sous les en-têtes
                    }
                }

                // Ajuster automatiquement la largeur des colonnes
                xlsheet.Columns.AutoFit();
            }
            else
            {
                MessageBox.Show("No data to export!", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void Button_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AjouterEMP addEmployeeWindow = new AjouterEMP();
            addEmployeeWindow.ShowDialog();  // Open the Add Employee window
            RefreshDataGrid();  // Refresh the data grid to show the new employee after the window is closed
        }


        private void Button_ModifyEmployee_Click(object sender, RoutedEventArgs e)
        {
            ModifierEMP mo = new ModifierEMP();
            mo.ShowDialog();  // Open the Add Employee window
            RefreshDataGrid();  // Refresh the data grid to show the new employee after the window is closed
        }










    }
}
