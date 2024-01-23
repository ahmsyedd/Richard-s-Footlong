using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class OrderSummaryPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        private OrderRender order;
        private ObservableCollection<OrderRender> receipt = new ObservableCollection<OrderRender>();
        public OrderSummaryPage()
        {
            this.InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();
                string query = "Select Price from Meat where MeatType = @mt;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@mt", order.MeatName);


                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtSubTotal.Text = reader.GetDouble(0).ToString();
                    order.Price = reader.GetDouble(0).ToString();
                }
                
                reader.Close();

                txtSubTotal.Text = (Convert.ToDouble(txtSubTotal.Text) * order.Quantity).ToString();

                txtTax.Text = (Convert.ToDouble(txtSubTotal.Text) * 0.13).ToString();
                txtTotal.Text = (Convert.ToDouble(txtSubTotal.Text) + Convert.ToDouble(txtTax.Text)).ToString();

                txtSubTotal.Text = Convert.ToDouble(txtSubTotal.Text).ToString("C"); 
                txtTax.Text = Convert.ToDouble(txtTax.Text).ToString("C");
                txtTotal.Text = Convert.ToDouble(txtTotal.Text).ToString("C");

                receipt.Add(order);

                lstReceipt.ItemsSource = receipt;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally 
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderedPage), order);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is OrderRender r)
            {

                order = new OrderRender 
                { 
                    BreadName = r.BreadName,
                    MeatName = r.MeatName,
                    Cheese = r.Cheese,
                    Quantity = r.Quantity,
                    user = r.user,
                    Sauce = r.Sauce,
                    Veggie = r.Veggie
                };
            }
            
        }

    }
}
