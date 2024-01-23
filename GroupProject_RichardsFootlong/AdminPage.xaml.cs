using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Store;
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
    public sealed partial class AdminPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        public ObservableCollection<Bread> breadList { get; set; }
        public ObservableCollection<Meat> meatList { get; set; }
        private ContentDialog currentDialog = new ContentDialog { CloseButtonText ="Ok"};
        public AdminPage()
        {
            this.InitializeComponent();
            breadList = new ObservableCollection<Bread>();
            meatList = new ObservableCollection<Meat>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();

        }

        private List<Bread> FetchBreadFromDB()
        {
            List<Bread> fetch = new List<Bread>();

            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select * from Bread;";
                cmd.CommandText = query;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Bread b = new Bread
                    {
                        BreadId = reader.GetInt32(0),
                        BreadType = reader.GetString(1)
                    };
                    fetch.Add(b);
                }
                reader.Close();

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
            return fetch;

        }

        private  List<Meat> FetchMeatFromDB()
        {
            List<Meat> fetch = new List<Meat>();
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select * from Meat;";
                cmd.CommandText = query;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Debug.WriteLine(reader.GetInt32(0) + " " + reader.GetString(1) + ' ' + reader.GetDouble(2));
                    Meat m = new Meat
                    {
                        MeatId = reader.GetInt32(0),
                        MeatType = reader.GetString(1),
                        Price = reader.GetDouble(2)
                    };
                    fetch.Add(m);
                }
                reader.Close();

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
            return fetch;
        }

        private void RefreshData()
        {
            meatList.Clear(); 
            breadList.Clear();
            List<Bread> fetchingData =  FetchBreadFromDB();
            List<Meat> fetchingData2 =  FetchMeatFromDB();
            foreach (var i in fetchingData)
            {
                breadList.Add(i);
            }
            foreach (var i in fetchingData2)
            {
                meatList.Add(i);
            }


            dataBread.ItemsSource = breadList;
            dataMeat.ItemsSource = meatList;
        }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void ShowMsg(string t, string msg)
        {
            if(currentDialog != null)
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

        private void btnAddBread_Click(object sender, RoutedEventArgs e)
        {
           
            string type = txtBreadName.Text;
            if(string.IsNullOrWhiteSpace(type))
            {
                ShowMsg("Error", "All Bread Name Must Be Filled.");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();

                    string query = "Insert into Bread (BreadType) values (@b);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@b", type);
                    conn.Open();
                    cmd.ExecuteScalar();

                    ShowMsg("Success", "You Have Added A New Bread!");
                    conn.Close();
                    RefreshData();
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

        private void btnUpdateBread_Click(object sender, RoutedEventArgs e)
        {
            string type = txtBreadName.Text;
            string id = txtBreadId.Text;
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(id))
            {
                ShowMsg("Error", "All Bread Related Fields Must Be Filled.");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();

                    string query = "Update Bread set BreadType  = @bt where BreadId = @id;";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bt", type);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                    conn.Open();
                    cmd.ExecuteScalar();

                    ShowMsg("Success", "Bread ID: " + id + " Successfully Updated.");
                    conn.Close();
                    RefreshData();
                    txtBreadName.Text = "";
                    txtBreadId.Text = "";
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

        private void btnDeleteBread_Click(object sender, RoutedEventArgs e)
        {
            string id = txtBreadId.Text;
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();

                string query = "delete from Bread where BreadId = @id";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                conn.Open();
                cmd.ExecuteScalar();

                ShowMsg("Success", "Bread Deleted!");
                conn.Close();
                RefreshData();
                txtBreadId.Text = "";
                txtBreadName.Text = "";
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

        private void btnAddMeat_Click(object sender, RoutedEventArgs e)
        {
            string type = txtMeatName.Text;
            string price = txtMeatPrice.Text;
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(price))
            {
                ShowMsg("Error", "Meat Feilds Must Be Filled.");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();

                    string query = "Insert into Meat (MeatType, Price) values (@m, @p);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@m", type);
                    cmd.Parameters.AddWithValue("@p", Convert.ToDouble(price).ToString("F2"));
                    conn.Open();
                    cmd.ExecuteScalar();

                    ShowMsg("Success", "You Have Added A New Meat!");
                    conn.Close();
                    RefreshData();
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

        private void btnUpdateMeat_Click(object sender, RoutedEventArgs e)
        {
            string type = txtMeatName.Text;
            string price = txtMeatPrice.Text; ;
            string id = txtMeatId.Text;
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(price) || string.IsNullOrEmpty(id)) 
            {
                ShowMsg("Error", "All Meat Related Fields Must Be Filled.");
            }
            else
            {
                try
                {
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();

                    string query = "update Meat set MeatType = @mt, Price = @p where MeatId = @id;";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@mt", type);
                    cmd.Parameters.AddWithValue("@p", Convert.ToDouble(price).ToString("F2"));
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteScalar();
                    ShowMsg("Success", "Meat Updated!");
                    conn.Close();
                    RefreshData();

                    txtMeatId.Text = "";
                    txtMeatName.Text = "";
                    txtMeatPrice.Text = "";
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

        private void btnDeleteMeat_Click(object sender, RoutedEventArgs e)
        {
            string id = txtMeatId.Text;
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();

                string query = "delete from Meat where MeatId = @id";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                conn.Open();
                cmd.ExecuteScalar();

                ShowMsg("Success", "Meat Deleted!");
                conn.Close();
                RefreshData();
                txtMeatName.Text = "";
                txtMeatId.Text = "";
                txtMeatPrice.Text = "";
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
    public class Bread
    {
        public int BreadId { get; set; }
        public string BreadType { get; set;}
    }

    public class Meat
    {
        public int MeatId { get; set; }
        public string MeatType { get; set; }
        public double Price { get; set; }

   
    }
}
