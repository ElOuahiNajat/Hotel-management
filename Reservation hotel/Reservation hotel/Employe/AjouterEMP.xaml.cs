using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel.Employe
{
    public partial class AjouterEMP : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public AjouterEMP()
        {
            InitializeComponent();
            GenerateAccessCode();
        }
        private void GenerateAccessCode()
        {
            Random random = new Random();
            int nbr = random.Next(1000, 10000);
            string accessCode = $"@EMP{nbr}";
            txtaccesCode.Text = accessCode;
        }

        private void btnajout_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtnom.Text;
            string prenom = txtprenom.Text;
            string telephone = txttel.Text;
            string cin = txtcin.Text;
            string salaire = txtsalaire.Text;
            string code = txtaccesCode.Text;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Employer (Nom, Prenom, Cin, Telephone, CodeAdmin, Salaire) VALUES (@Nom, @Prenom, @Cin, @Telephone, @CodeAdmin, @Salaire)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Cin", cin);
                    cmd.Parameters.AddWithValue("@Telephone", telephone);
                    cmd.Parameters.AddWithValue("@CodeAdmin", code);
                    cmd.Parameters.AddWithValue("@Salaire", salaire);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employer added successfully!");
                        txtnom.Text = string.Empty;
                        txtprenom.Text = string.Empty;
                        txtcin.Text = string.Empty;
                        txttel.Text = string.Empty;
                        txtsalaire.Text = string.Empty;
                        GenerateAccessCode();
                        code = txtaccesCode.Text;
                    }
                    else
                    {
                        MessageBox.Show("Failed to add employer.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
