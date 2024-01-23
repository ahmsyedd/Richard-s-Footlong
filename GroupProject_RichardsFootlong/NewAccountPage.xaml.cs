using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public sealed partial class NewAccountPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        public NewAccountPage()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();
            string passwordC = txtCPassword.Password.ToString();
            bool isSuccess = false;
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordC))
            {
                ShowMsg("Error", "All fields must be filled.");
            }
            else if(password != passwordC)
            {
                ShowMsg("Error", "Passwords do not match");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string query = "Insert into Users (Username, Password) values (@user, @p);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue ("@p", password);
                    conn.Open();
                    cmd.ExecuteScalar();


                    conn.Close();
                    isSuccess = true;

                }
                catch (Exception ex)
                {
                    ShowMsg("Error", ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    if(isSuccess == true)
                    {
                        ShowMsg("Success", "User has been created. Redirecting to Login Page.");
                        Frame.Navigate(typeof(LoginPage));
                    }
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
