using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.OnlineId;
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
    public sealed partial class LandingPageWAcc : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        private int userId;
        private ContentDialog currentDialog = new ContentDialog { CloseButtonText = "Ok" };
        public LandingPageWAcc()
        {
            this.InitializeComponent();
        }

        private void Page_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();

                string query = "select Username from Users where UserId = @id";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtUsername.Text = "Welcome " + reader.GetString(0);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ShowMsg("Error",ex.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void imgUser_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AccSummaryPage), userId);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //need to send the userId
            ActiveUser user = new ActiveUser { 
                isUserFlag = true,
                userId = this.userId,
            };

            Frame.Navigate(typeof(OrderPage), user);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is int)
            {
                this.userId = (int)e.Parameter;

            }
        }

        private async void ShowMsg(string t, string msg)
        {
            if (currentDialog != null)
            {
                currentDialog.Hide();
            }
            ContentDialog dialog = new ContentDialog
            {
                Title = t,
                Content = msg,
                CloseButtonText = "Close"
            };
            currentDialog = dialog;
            await dialog.ShowAsync();
        }
    }
}
