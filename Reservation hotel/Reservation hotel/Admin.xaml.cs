using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using System.Data;
using System;

using System.Windows.Input;

namespace Reservation_hotel
{
    public partial class Admin : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public Admin()
        {
            InitializeComponent();
        }

        private void btn_HOME_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous content and reset rows
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            // Add 1 row and 1 column to the MainGrid to center the form
            MainGrid.RowDefinitions.Add(new RowDefinition());
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Create a container to center the form
            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 30, 0, 0)
            };

            // Title label
            var labelTitle = new Label
            {
                Content = "Total Employees, Clients, and Reservations",
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGray),
                Margin = new Thickness(0, 10, 0, 0)
            };
            container.Children.Add(labelTitle);

            // Create a form to hold Employee, Client, and Reservation totals
            var formContainer = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            // Employee count label
            var employeeBorder = new Border
            {
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkSlateGray),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = new CornerRadius(10),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
                Child = new Label
                {
                    Name = "labelEmployeesCount",
                    Content = "Loading... Please wait.",
                    FontSize = 48,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)
                }
            };
            formContainer.Children.Add(employeeBorder);

            // Client count label
            var clientBorder = new Border
            {
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkSlateGray),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = new CornerRadius(10),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
                Child = new Label
                {
                    Name = "labelClientsCount",
                    Content = "Loading... Please wait.",
                    FontSize = 48,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)
                }
            };
            formContainer.Children.Add(clientBorder);

            // Reservation count label
            var reservationBorder = new Border
            {
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkSlateGray),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = new CornerRadius(10),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
                Child = new Label
                {
                    Name = "labelReservationsCount",
                    Content = "Loading... Please wait.",
                    FontSize = 48,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)
                }
            };
            formContainer.Children.Add(reservationBorder);

            // Add formContainer to the main container
            container.Children.Add(formContainer);

            // Add the container to the MainGrid in row 0, column 0
            Grid.SetRow(container, 0);
            Grid.SetColumn(container, 0);
            MainGrid.Children.Add(container);

            // Fetch the total number of employees, clients, and reservations
            int employeeCount = GetEmployeeCount();
            int clientCount = GetClientCount();
            int reservationCount = GetReservationCount();

            // Update the label content with the counts
            ((Label)employeeBorder.Child).Content = $"{employeeCount} Employees";
            ((Label)clientBorder.Child).Content = $"{clientCount} Clients";
            ((Label)reservationBorder.Child).Content = $"{reservationCount} Reservations";
        }

        // Method to fetch the number of employees from the database
        private int GetEmployeeCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Employer";  // Use your actual table name for employees
                    SqlCommand cmd = new SqlCommand(query, conn);
                    count = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching employee count: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return count;
        }

        // Method to fetch the number of clients from the database
        private int GetClientCount()
{
    int count = 0;
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Client";  // Use your actual table name for clients
            SqlCommand cmd = new SqlCommand(query, conn);
            count = (int)cmd.ExecuteScalar();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error fetching client count: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    return count;
}

// Method to fetch the number of reservations from the database
private int GetReservationCount()
{
    int count = 0;
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Reservation";  // Use your actual table name for reservations
            SqlCommand cmd = new SqlCommand(query, conn);
            count = (int)cmd.ExecuteScalar();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error fetching reservation count: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    return count;
}








        //Employe
        private void btn_EMP_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            // Define rows and columns to create space for centering
            for (int i = 0; i < 2; i++)
            {
                MainGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                MainGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            }

            // Create the button
            var buttonLister = new System.Windows.Controls.Button
            {
                Content = "Explorer les Employés",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center, // Center horizontally
                VerticalAlignment = System.Windows.VerticalAlignment.Center, // Center vertically
                Margin = new System.Windows.Thickness(30), // Margin around the button
                Width = 250,
                Height = 150,
                FontSize = 24,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black), // Black background
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White), // White text
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray), // Gray border
                BorderThickness = new System.Windows.Thickness(2), // Border thickness
                FontWeight = System.Windows.FontWeights.Bold // Bold text
            };





            // Set the button in the first row and first column
            Grid.SetRow(buttonLister, 0); // Row 0
            Grid.SetColumn(buttonLister, 0); // Column 0 (adjust as needed)

            // Center the button in the middle of the grid
            Grid.SetRowSpan(buttonLister, 2); // Spanning across 2 rows
            Grid.SetColumnSpan(buttonLister, 2); // Spanning across 2 columns

            // Add event handler for button click
            buttonLister.Click += btn_ListerEMP_Click;

            // Add the button to the grid
            MainGrid.Children.Add(buttonLister);
        }

        //var buttonAjouter = new System.Windows.Controls.Button
        //{
        //    Content = "Ajouter Employes",
        //    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
        //    VerticalAlignment = System.Windows.VerticalAlignment.Top,
        //    Margin = new System.Windows.Thickness(0, 30, 30, 0),
        //    Width = 250,
        //    Height = 150,
        //    FontSize = 24,
        //};
        //Grid.SetRow(buttonAjouter, 0);
        //buttonAjouter.Click += btn_AjouterEMP_Click;
        //MainGrid.Children.Add(buttonAjouter);

        //var buttonModifier = new System.Windows.Controls.Button
        //{
        //    Content = "Modifier Employes",
        //    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
        //    VerticalAlignment = System.Windows.VerticalAlignment.Top,
        //    Margin = new System.Windows.Thickness(30, 30, 0, 0),
        //    Width = 250,
        //    Height = 150,
        //    FontSize = 24
        //};
        //Grid.SetRow(buttonModifier, 1);
        //buttonModifier.Click += btn_ModifierEMP_Click;
        //MainGrid.Children.Add(buttonModifier);





        private void btn_AjouterEMP_Click(object sender, RoutedEventArgs e)
        {
            Employe.AjouterEMP ajouterEMP = new Employe.AjouterEMP();
            ajouterEMP.Show();
        }
        private void btn_ModifierEMP_Click(object sender, RoutedEventArgs e)
        {
            Employe.ModifierEMP modifierEMP = new Employe.ModifierEMP();
            modifierEMP.Show();
        }
        private void btn_ListerEMP_Click(object sender, RoutedEventArgs e)
        {
            Employe.ListerEMP listerEMP = new Employe.ListerEMP();
            listerEMP.Show();
        }

        //Client

        // Variable pour stocker la DataGrid
        private DataGrid gridListeCLT;

        // Variable pour stocker la TextBox de recherche
        private TextBox txtSearchCLT;

        private void btn_CLT_Click(object sender, RoutedEventArgs e)
        {
            // Vider les éléments actuels dans MainGrid
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();

            // Ajouter des lignes pour organiser les éléments
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Ajouter un bouton d'exportation
            var btnExportCLT = new Button
            {
                Name = "btnExportCLT",
                Content = "Export",
                Margin = new Thickness(0, 90, 0, 0),
                Height = 60,
                Width = 120,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,

                // Fond gris et texte jaune
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black),
                BorderThickness = new Thickness(2),
                Cursor = Cursors.Hand,

                // Padding et alignement du texte
                Padding = new Thickness(10),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Appliquer les coins arrondis
            btnExportCLT.Padding = new Thickness(10);

            // Ajouter les événements de survol
            btnExportCLT.MouseEnter += (s, e) =>
            {
                btnExportCLT.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
                btnExportCLT.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            };

            btnExportCLT.MouseLeave += (s, e) =>
            {
                btnExportCLT.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                btnExportCLT.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
            };

            btnExportCLT.Click += btnExport_Click;
            MainGrid.Children.Add(btnExportCLT);
            Grid.SetRow(btnExportCLT, 0);

            // Ajouter un champ de recherche
            StackPanel searchPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(20)
            };

            // Créer l'icône de recherche
            TextBlock searchIcon = new TextBlock
            {
                Text = "🔍", // Utilisation d'un emoji comme icône de recherche
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
                Margin = new Thickness(5, 0, 10, 0)
            };

            // Créer le TextBox pour la recherche
            txtSearchCLT = new TextBox
            {
                Name = "txtSearchCLT",
                Height = 30,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                BorderThickness = new Thickness(2),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray),
                Margin = new Thickness(0),
                Width = 200,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
            };

            // Ajouter un effet au focus
            txtSearchCLT.GotFocus += (s, e) =>
            {
                txtSearchCLT.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
            };

            txtSearchCLT.LostFocus += (s, e) =>
            {
                txtSearchCLT.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            };

            // Ajouter l'icône et le TextBox au StackPanel
            searchPanel.Children.Add(searchIcon);
            searchPanel.Children.Add(txtSearchCLT);

            // Ajouter le StackPanel au conteneur principal (MainGrid)
            MainGrid.Children.Add(searchPanel);
            Grid.SetRow(searchPanel, 0);

            // Ajouter une DataGrid pour afficher la liste des clients
            gridListeCLT = new DataGrid
            {
                Name = "gridListeCLT",
                Margin = new Thickness(20),
                AutoGenerateColumns = true,
                CanUserAddRows = false,
                IsReadOnly = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black),
                BorderThickness = new Thickness(1),
            };

            // Modifier l'en-tête pour être gris
            gridListeCLT.HeadersVisibility = DataGridHeadersVisibility.Column;
            gridListeCLT.LoadingRow += (s, e) =>
            {
                e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
            };

            // Personnaliser la cellule de la DataGrid
            gridListeCLT.CellStyle = new Style(typeof(DataGridCell))
            {
                Setters = {
            new Setter(DataGridCell.BackgroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White)),
            new Setter(DataGridCell.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black))
        }
            };

            MainGrid.Children.Add(gridListeCLT);
            Grid.SetRow(gridListeCLT, 1);

            gridListeCLT.AutoGeneratingColumn += (s, e) =>
            {
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            };

            // Charger les données des clients
            LoadClients(gridListeCLT, txtSearchCLT);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si la DataGrid contient des éléments
            if (gridListeCLT.Items.Count == 0)
            {
                MessageBox.Show("No data available to export!", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sélectionner toutes les lignes
            gridListeCLT.SelectAll();
            ApplicationCommands.Copy.Execute(null, gridListeCLT);

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
                for (int j = 0; j < gridListeCLT.Columns.Count; j++)
                {
                    xlsheet.Cells[1, j + 1] = gridListeCLT.Columns[j].Header.ToString(); // Récupère les noms des colonnes
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

        // Méthode pour charger les données dans la DataGrid
        private void LoadClients(DataGrid grid, TextBox searchBox)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Id, Nom, Prenom, Cin, Telephone, Email FROM Client";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    grid.ItemsSource = dt.DefaultView;

                    // Ajouter un filtre sur la recherche
                    searchBox.TextChanged += (s, e) =>
                    {
                        string filter = searchBox.Text.Trim();
                        if (!string.IsNullOrEmpty(filter))
                        {
                            DataView dv = dt.DefaultView;
                            dv.RowFilter = $"Nom LIKE '%{filter}%' OR Prenom LIKE '%{filter}%' OR Email LIKE '%{filter}%'";
                            grid.ItemsSource = dv;
                        }
                        else
                        {
                            grid.ItemsSource = dt.DefaultView;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //Reservation
        // Gestion des réservations
        private void btn_RES_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            // Ajouter des lignes et des colonnes pour organiser les boutons
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

           
            
            // Bouton pour lister les réservations
            var buttonLister = new Button
            {
                Content = "Lister Réservations",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 280),
                Width = 300,
                Height = 300,
                FontSize = 24,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray),
                BorderThickness = new Thickness(2),
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };
            buttonLister.Click += btn_ListerRESERVATION_Click; // Mise à jour du nom de l'événement
            MainGrid.Children.Add(buttonLister);
            Grid.SetRow(buttonLister, 1);
            Grid.SetColumn(buttonLister, 0);
        }

        // Méthode pour ouvrir la fenêtre d'ajout de réservation
        private void btn_AjouterRESERVATION_Click(object sender, RoutedEventArgs e)
        {
            Reservation.AjouterRESERVATION ajouterRESERVATION = new Reservation.AjouterRESERVATION(); // Mise à jour du nom de la classe
            ajouterRESERVATION.Show();
        }

        // Méthode pour ouvrir la fenêtre de modification de réservation
        private void btn_ModifierRESERVATION_Click(object sender, RoutedEventArgs e)
        {
            Reservation.ModifierRESERV modifierRESERVATION = new Reservation.ModifierRESERV(); // Mise à jour du nom de la classe
            modifierRESERVATION.Show();
        }

        // Méthode pour ouvrir la fenêtre de liste des réservations
        private void btn_ListerRESERVATION_Click(object sender, RoutedEventArgs e)
        {
            Reservation.ListerRESERVATION listerRESERVATION = new Reservation.ListerRESERVATION(); // Mise à jour du nom de la classe
            listerRESERVATION.Show();
        }


        //Chambre
        private void btn_CHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            // Vider les éléments actuels dans MainGrid
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();

            // Ajouter des lignes pour organiser les éléments
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Ajouter un bouton pour lister les chambres
            var buttonLister = new Button
            {
                Content = "Lister les Chambres",
                Margin = new Thickness(0, 90, 0, 0),
                Height = 60,
                Width = 120,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black),
                BorderThickness = new Thickness(2),
                Cursor = Cursors.Hand,
                Padding = new Thickness(10),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            buttonLister.Click += btn_ListerCHAMBRE_Click;
            MainGrid.Children.Add(buttonLister);
            Grid.SetRow(buttonLister, 0);

         

        }

        private void btn_ListerCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.ListerCHAMBRE listerCHAMBRE = new Chambre.ListerCHAMBRE();
            listerCHAMBRE.Show();
        }

        private void btn_AjouterCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.AjouterCHAMBRE ajouterCHAMBRE = new Chambre.AjouterCHAMBRE();
            ajouterCHAMBRE.Show();
        }

        private void btn_ModifierCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.ModifierCHAMBRE modifierCHAMBRE = new Chambre.ModifierCHAMBRE();
            modifierCHAMBRE.Show();
        }


        //type chambre


        private void btn_TYPE_CHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            // Vider le contenu actuel de MainGrid
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            // Ajouter des lignes et des colonnes pour organiser les boutons
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        

            // Bouton pour lister les types de chambres
            var buttonLister = new Button
            {
                Content = "Lister Types de Chambres",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 60, 0, 0),
                Width = 300,
                Height = 150,
                FontSize = 24,
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray),
                BorderThickness = new Thickness(2),
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };
            buttonLister.Click += btn_ListerTypeChambre_Click;
            MainGrid.Children.Add(buttonLister);
            Grid.SetRow(buttonLister, 1);
            Grid.SetColumn(buttonLister, 0);
        }

        // Gestionnaire d'événements pour le bouton "Ajouter Type de Chambre"
        private void btn_AjouterTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre d'ajout de type de chambre
            TypeChambre.AjouterTypeChambre ajouterTypeChambre = new TypeChambre.AjouterTypeChambre();
            ajouterTypeChambre.Show();
        }

        // Gestionnaire d'événements pour le bouton "Lister Types de Chambres"
        private void btn_ListerTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de liste des types de chambres
            TypeChambre.ListerTypeChambre listerTypeChambre = new TypeChambre.ListerTypeChambre();
            listerTypeChambre.Show();
        }


        //logout 
        // Méthode pour gérer le clic sur le bouton Logout
        private void btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Fermer la fenêtre actuelle (Admin)
            this.Close();
        }
    }
}
