using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GroupProject_RichardsFootlong
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(MainPage));
        }

        private void btnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewAccountPage));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();
          
            int userId = 0;
            string sqlUser = "";
            string pword = "";
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowMsg("Error", "All fields must be filled.");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();

                    string query = "select * from Users where username = @user;";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@user", username);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                        
                            userId = reader.GetInt32(0);
                            sqlUser = reader.GetString(1);
                            pword = reader.GetString(2);

                            if(password != pword)
                            {
                                throw new Exception("Password incorrect");
                            }

                        }

                    }
                    else
                    {
                        throw new Exception("User not found.");
                    }

                    reader.Close();

                    if(sqlUser == "Admin")
                    {
                        Frame.Navigate(typeof(AdminPage));
                    }
                    else
                    {
                        //keep on passing the userId throughout the session
                        Frame.Navigate (typeof(LandingPageWAcc), userId);
                    }
                  

                }
                catch (Exception ex)
                {
                    ShowMsg("Error", ex.Message);
                }
                finally 
                {
                    cmd.Dispose();
                    conn.Close();

                }
            }
        }

        private async void ShowMsg(string t, string msg)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = t,
                Content = msg,
                CloseButtonText = "Close"
            };

            await dialog.ShowAsync();
        }
    }
}
