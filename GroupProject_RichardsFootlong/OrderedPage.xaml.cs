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
    public sealed partial class OrderedPage : Page
    {

        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;

        private bool isAccount = false;
        private OrderRender order;
        public OrderedPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int breadId = Get_BreadId(order.BreadName);
            int meatId = Get_MeatId(order.MeatName);



            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "";
                if(isAccount == true)
                {
                   query = "insert into Orders (UserId, BreadId, MeatId, Cheese, Quantity) values(@uid, @bid, @mid, @c, @q);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@uid", order.user.userId);
                    cmd.Parameters.AddWithValue("@bid",breadId);
                    cmd.Parameters.AddWithValue("@mid", meatId);
                    cmd.Parameters.AddWithValue("@c", order.Cheese);
                    cmd.Parameters.AddWithValue("@q", order.Quantity);

                }
                else
                {
                    query = "insert into Orders (BreadId, MeatId, Cheese, Quantity) values(@bid, @mid, @c, @q);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bid", breadId);
                    cmd.Parameters.AddWithValue("@mid", meatId);
                    cmd.Parameters.AddWithValue("@c", order.Cheese);
                    cmd.Parameters.AddWithValue("@q", order.Quantity);
                }



                conn.Open();
                cmd.ExecuteScalar();
                conn.Close();

                Get_OID();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In Loaded");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void Get_OID()
        {
            try
            {
                int oid = 0;

                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query2 = "select top 1 OrderId from Orders order by OrderId desc;";
                cmd.CommandText = query2;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    oid = reader.GetInt32(0);
                }
                reader.Close();
                conn.Close();
                Insert_VegOrder(oid);
                Insert_SauceOrder(oid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In OID");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }


        private int Get_BreadId(string name)
        {
            int id = 0;
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select BreadId from Bread where BreadType = @bt;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@bt", name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    id = Convert.ToInt32(reader["BreadId"]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In BreadId");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return id;
        }

        private int Get_MeatId(string name)
        {
            int id = 0;
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "select MeatId from Meat where MeatType = @bt;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@bt", name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In MeatId");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return id;
        }

        private void Insert_VegOrder(int oid)
        {
            try
            {
                for(int i = 0; i <order.Veggie.Count; i++)
                {
                    int id = 0;
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string getVId = "select VeggieId from Veggies where Name = @bt";
                    cmd.CommandText = getVId;
                    cmd.Parameters.AddWithValue("@bt", order.Veggie[i]);
                    conn.Open();
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        id = sqlDataReader.GetInt32(0);
                    }
                    sqlDataReader.Close();
                    conn.Close();


                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string query = "insert into Veg_Order (OrderId, VeggieId) Values(@oid, @vid);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@oid", oid);
                    cmd.Parameters.AddWithValue("@vid", id);
                    conn.Open();
                    cmd.ExecuteScalar();
                    conn.Close();

                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In Veg_Order");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void Insert_SauceOrder(int oid)
        {
            try
            {
                for(int i = 0; i < order.Sauce.Count; i++)
                {
                    int id = 0;
                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string getBId = "select SauceId from Sauces where Name = @bt";
                    cmd.CommandText = getBId;
                    cmd.Parameters.AddWithValue("@bt", order.Sauce[i]);
                    conn.Open();
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        id = sqlDataReader.GetInt32(0);
                    }
                    sqlDataReader.Close();
                    conn.Close();

                    conn.ConnectionString = connString;
                    cmd = conn.CreateCommand();
                    string query = "insert into Sauce_Order (OrderId, SauceId) Values(@oid, @sid);";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@oid", oid);
                    cmd.Parameters.AddWithValue("@sid", id);
                    conn.Open();
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("In Sauce_Order");
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is OrderRender r)
            {
                order = new OrderRender 
                {
                    user = r.user,
                    MeatName = r.MeatName,
                    BreadName = r.BreadName,
                    Cheese = r.Cheese,
                    Sauce = r.Sauce,
                    Veggie = r.Veggie,
                    Quantity = r.Quantity
                };
                isAccount = order.user.isUserFlag;
            }
          
        }

        private void btnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            if (isAccount == true)
            {
                Frame.Navigate(typeof(LandingPageWAcc), order.user.userId);
            }
            if (isAccount == false)
            {
                Frame.Navigate(typeof(LandingPage));
            }
        }
    }
}
