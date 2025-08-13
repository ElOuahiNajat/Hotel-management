using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Reservation_hotel
{
    public partial class EMP : Window
    {
        public EMP()
        {
            InitializeComponent();
        }

        // Style commun pour les boutons
        private Style GetCommonButtonStyle()
        {
            return new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter(Button.HeightProperty, 80.0),
                    new Setter(Button.WidthProperty, 300.0),
                    new Setter(Button.FontSizeProperty, 16.0),
                    new Setter(Button.FontWeightProperty, FontWeights.Bold),
                    new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center),
                    new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center),
                    new Setter(Button.BackgroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray)),
                    new Setter(Button.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)),
                    new Setter(Button.BorderBrushProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black)),
                    new Setter(Button.BorderThicknessProperty, new Thickness(2)),
                    new Setter(Button.CursorProperty, Cursors.Hand),
                    new Setter(Button.PaddingProperty, new Thickness(10)),
                    new Setter(Button.HorizontalContentAlignmentProperty, HorizontalAlignment.Center),
                    new Setter(Button.VerticalContentAlignmentProperty, VerticalAlignment.Center)
                }
            };
        }

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
            var buttonListerReservations = new Button
            {
                Content = "Lister Réservations",
                Style = GetCommonButtonStyle(), // Appliquer le style commun
                Margin = new Thickness(0, 300, 0, 0) // Marge ajustée
            };
            buttonListerReservations.Click += btn_ListerRESERVATION_Click;
            MainGrid.Children.Add(buttonListerReservations);
            Grid.SetRow(buttonListerReservations, 0);
            Grid.SetColumn(buttonListerReservations, 0);
        }

        // Gestion des chambres
        private void btn_CHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            // Vider les éléments actuels dans MainGrid
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();

            // Ajouter des lignes pour organiser les éléments
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Bouton pour lister les chambres
            var buttonListerChambres = new Button
            {
                Content = "Lister les Chambres",
                Style = GetCommonButtonStyle(), // Appliquer le style commun
                Margin = new Thickness(0, 300, 0, 0) // Marge ajustée
            };
            buttonListerChambres.Click += btn_ListerCHAMBRE_Click;
            MainGrid.Children.Add(buttonListerChambres);
            Grid.SetRow(buttonListerChambres, 0);
        }

        // Méthode pour ouvrir la fenêtre de liste des réservations
        private void btn_ListerRESERVATION_Click(object sender, RoutedEventArgs e)
        {
            Reservation.ListerRESERVATION listerRESERVATION = new Reservation.ListerRESERVATION();
            listerRESERVATION.Show();
        }

        // Méthode pour ouvrir la fenêtre de liste des chambres
        private void btn_ListerCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.ListerCHAMBRE listerCHAMBRE = new Chambre.ListerCHAMBRE();
            listerCHAMBRE.Show();
        }

        // Méthode pour ouvrir la fenêtre d'ajout de chambre
        private void btn_AjouterCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.AjouterCHAMBRE ajouterCHAMBRE = new Chambre.AjouterCHAMBRE();
            ajouterCHAMBRE.Show();
        }

        // Méthode pour ouvrir la fenêtre de modification de chambre
        private void btn_ModifierCHAMBRE_Click(object sender, RoutedEventArgs e)
        {
            Chambre.ModifierCHAMBRE modifierCHAMBRE = new Chambre.ModifierCHAMBRE();
            modifierCHAMBRE.Show();
        }

        // Gestion des types de chambres
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
                Style = GetCommonButtonStyle(), // Appliquer le style commun
                Margin = new Thickness(0, 6, 0, 300) // Marge ajustée
            };
            buttonLister.Click += btn_ListerTypeChambre_Click;
            MainGrid.Children.Add(buttonLister);
            Grid.SetRow(buttonLister, 1);
            Grid.SetColumn(buttonLister, 0);
        }

        // Gestionnaire d'événements pour le bouton "Lister Types de Chambres"
        private void btn_ListerTypeChambre_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de liste des types de chambres
            TypeChambre.ListerTypeChambre listerTypeChambre = new TypeChambre.ListerTypeChambre();
            listerTypeChambre.Show();
        }

        // Logout
        private void btn_LOGOUT_Click(object sender, RoutedEventArgs e)
        {
            // Retour à la MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}