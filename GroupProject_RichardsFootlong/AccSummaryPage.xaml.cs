using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
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
    public sealed partial class AccSummaryPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        private int userId;
        private ContentDialog currentDialog = new ContentDialog { CloseButtonText = "Ok" };
        private ObservableCollection<FormattedOrders> orderList = new ObservableCollection<FormattedOrders>();
        public AccSummaryPage()
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
                conn.Close();
                Load_Orders();
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

        private void Load_Orders()
        {
            
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select * from Orders where UserId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int counter = 1;
                while (reader.Read())
                {
                    FormattedOrders order = new FormattedOrders
                    {
                        ID = (int)counter++,
                        OrderId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        BreadId = reader.GetInt32(2),
                        MeatId = reader.GetInt32(3),
                        Cheese = reader.GetString(4),
                        Quantity = reader.GetInt32(5)
                    };

                    orderList.Add(order);
                }
                reader.Close();
                conn.Close(); 
                for(int i = 0; i < orderList.Count; i++)
                {
                
                    orderList[i].MeatName = Get_MeatName(orderList[i ].MeatId);
                    orderList[i].BreadName = Get_BreadName(orderList[i].BreadId);
                    orderList[i].Veggie = get_Toppings(orderList[i].OrderId);
                    orderList[i].Sauce = Get_Sauces(orderList[i].OrderId);
                    orderList[i].Price = (Convert.ToDouble(GetPrice(orderList[i].MeatId)) * orderList[i].Quantity).ToString("C");
                }

                lstAccSummary.ItemsSource = orderList;
                
            }
            catch (Exception e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private string Get_MeatName(int id)
        {
 
            string name = "";
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select MeatType from Meat where MeatId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = reader.GetString(0);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return name;
        }

        private string Get_BreadName(int id)
        {
            string name = "";
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select BreadType from Bread where BreadId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = reader.GetString(0);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return name;
        }

        private List<string> get_Toppings(int oid)
        {
            List<String> toppings = new List<string>();
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select VeggieId from Veg_Order where OrderId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", oid);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    toppings.Add(reader.GetInt32(0).ToString());
                }
                reader.Close();
                conn.Close();
                for(int i = 0; i <  toppings.Count; i++)
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string query2 = "select Name from Veggies where VeggieId = @id;";
                    cmd.CommandText = query2;
                    cmd.Parameters.AddWithValue("@id", toppings[i]);
                    conn.Open();
                    SqlDataReader reader2 = cmd.ExecuteReader();
                    while(reader2.Read())
                    {
                        toppings[i] = reader2.GetString(0);
                    }
                    reader2.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose( );
                conn.Close();
            }


            return toppings;
        }

        private List<string> Get_Sauces(int oid)
        {
            List<string> sauces = new List<string>();
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select SauceId from Sauce_Order where OrderId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", oid);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sauces.Add(reader.GetInt32(0).ToString());
                }
                reader.Close();
                conn.Close();
                for (int i = 0; i < sauces.Count; i++)
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string query2 = "select Name from Sauces where SauceId = @id;";
                    cmd.CommandText = query2;
                    cmd.Parameters.AddWithValue("@id", sauces[i]);
                    conn.Open();
                    SqlDataReader reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        sauces[i] = reader2.GetString(0);
                    }
                    reader2.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return sauces;
        }

        private string GetPrice(int meatId)
        {
            double price = 0;
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select Price from Meat where MeatId = @id;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", meatId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    price = reader.GetDouble(0);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                ShowMsg("Error", e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
           
            return (price * 1.13).ToString();

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LandingPageWAcc), userId);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
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
    public class FormattedOrders
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int BreadId { get; set; }
        public int MeatId { get; set; }
        public string Cheese { get; set; }
        public int Quantity { get; set; }

        public string Price { get; set; }

        public string BreadName { get; set; }
        public string MeatName { get; set; }

        public List<string> Veggie { get; set; }
        public List<string> Sauce { get; set; }
    }
}
