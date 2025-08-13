using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Reservation_hotel
{

    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-9EMSD3K\\SQLEXPRESS;Initial Catalog=HotelDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = CodeInpute.Text.Trim();
            if (input == "NJYS")
            {
                Admin admin = new Admin();
                admin.Show();
                this.Close();
            }
            else if (input.StartsWith("@EMP"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Employer WHERE CodeAdmin = @Employeecode", connection);
                    command.Parameters.AddWithValue("@Employeecode", input);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        EMP employe = new EMP();
                        employe.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Employee not found or invalid.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }
    }
}