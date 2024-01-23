using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
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
    public sealed partial class OrderPage : Page
    {
        private SqlConnection conn = new SqlConnection();
        private string connString = "Server=(local);database=C_Sharp_Footlong_Project;User=FootlongAdmin;password=12345;";
        private SqlCommand cmd;
        private bool isAccount = false;

        private ActiveUser user;
        private List<String> sauces = new List<String>();
        private List<String> veggies = new List<String>();
        private OrderRender order;

        public OrderPage()
        {
            this.InitializeComponent();
            order = new OrderRender();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBread();
            LoadMeat();
            txtQuant.Text = 1.ToString();
            btnMinus.IsEnabled = false;
        }

        private void LoadBread()
        {
            List<string> items = new List<string>();
            try
            {
                conn.ConnectionString = connString;
                cmd = conn.CreateCommand();

                string query = "Select BreadType From Bread;";
                cmd.CommandText = query;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(reader.GetString(0));
                }
                reader.Close();
                cmbBread.ItemsSource = items;
                cmbBread.SelectedIndex = 0;
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

        private void LoadMeat()
        {
            List<string> items = new List<string>();
            try
            {
                conn.ConnectionString= connString;
                cmd = conn.CreateCommand();
                string query = "Select MeatType From Meat;";
                cmd.CommandText = query;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(reader.GetString(0));
                }
                reader.Close();
                cmbMeat.ItemsSource = items;
                cmbMeat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message);
            }
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            //need a way to determine which one we are on - account or X
            if(isAccount == true)
            {
                Frame.Navigate(typeof(LandingPageWAcc), user.userId);
            }
            if(isAccount == false)
            {
                Frame.Navigate(typeof(LandingPage));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is ActiveUser par)
            {
                user = new ActiveUser
                {
                    isUserFlag = isAccount = par.isUserFlag,
                    userId = par.userId
                };
      
            }
          
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckVeggies();
            CheckSauce();
            ComboBoxItem comboBoxItem = cmbCheese.SelectedItem as ComboBoxItem;
            string selectedCheese = comboBoxItem.Content.ToString();
          
            if (user == null)
            {
                user = new ActiveUser();
                user.isUserFlag = false; user.userId = 0;

            }
            order = new OrderRender
            {
                user = this.user,
                Quantity = Convert.ToInt32(txtQuant.Text),
                Cheese = selectedCheese,
                BreadName = cmbBread.SelectedItem as string,
                MeatName = cmbMeat.SelectedItem as string,
                Sauce = new List<string>(),
                Veggie = new List<string>()
            };
            if(veggies != null)
            {
                order.Veggie.AddRange(veggies);

            }
            if(sauces != null)
            {
                order.Sauce.AddRange(sauces);
            }

            Frame.Navigate(typeof(OrderSummaryPage), order);
          

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

        private void btnAddit_Click(object sender, RoutedEventArgs e)
        {
            int quan = Convert.ToInt32(txtQuant.Text);
            quan++;
            if(quan == 10) 
            {
                txtQuant.Text = quan.ToString();
                btnAddit.IsEnabled = false;
            }
            else
            {
                txtQuant.Text = quan.ToString();
                btnMinus.IsEnabled = true;
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            int quan = Convert.ToInt32(txtQuant.Text);
            quan--;
            if (quan == 1)
            {
                txtQuant.Text = quan.ToString();
                btnMinus.IsEnabled = false;
            }
            else
            {
              
                txtQuant.Text = quan.ToString();
                btnAddit.IsEnabled=true;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cmbBread.SelectedIndex = 0;
            cmbMeat.SelectedIndex = 0;

            chkLettuce.IsChecked = chkTomatoes.IsChecked = chkCucumbers.IsChecked = chkGreenPeppers.IsChecked = chkPickles.IsChecked = chkOlives.IsChecked =
                chkOnions.IsChecked = chkSpinach.IsChecked = false;
            chkMayo.IsChecked = chkMustard.IsChecked = chkCreamySrircha.IsChecked = chkBBQ.IsChecked = chkTeriyaki.IsChecked = chkGarlicAioli.IsChecked = false;

            txtQuant.Text = 1.ToString();
            btnMinus.IsEnabled = false;
            btnAddit.IsEnabled = true;

        }

        private void CheckVeggies()
        {
            if(chkLettuce.IsChecked == true)
            {
                veggies.Add(chkLettuce.Content.ToString());
            }

            if(chkTomatoes.IsChecked == true)
            {
                veggies.Add(chkTomatoes.Content.ToString());
            }

            if (chkOnions.IsChecked == true)
            {
                veggies.Add(chkOnions.Content.ToString());
            }

            if (chkOlives.IsChecked == true)
            {
                veggies.Add(chkOlives.Content.ToString());
            }
            if (chkPickles.IsChecked == true)
            {
                veggies.Add(chkPickles.Content.ToString());
            }
            if (chkGreenPeppers.IsChecked == true)
            {
                veggies.Add(chkGreenPeppers.Content.ToString());
            }
            if (chkCucumbers.IsChecked == true)
            {
                veggies.Add(chkCucumbers.Content.ToString());
            }
            if (chkSpinach.IsChecked == true)
            {
                veggies.Add(chkSpinach.Content.ToString());
            }
        }

        private void CheckSauce()
        {
            if (chkMayo.IsChecked == true)
            {
                sauces.Add(chkMayo.Content.ToString());
            }
            if (chkMustard.IsChecked == true)
            {
                sauces.Add(chkMustard.Content.ToString());
            }
            if (chkGarlicAioli.IsChecked == true)
            {
                sauces.Add(chkGarlicAioli.Content.ToString());
            }
            if (chkTeriyaki.IsChecked == true)
            {
                sauces.Add(chkTeriyaki.Content.ToString());
            }
            if (chkCreamySrircha.IsChecked == true)
            {
                sauces.Add(chkCreamySrircha.Content.ToString());
            }
            if (chkBBQ.IsChecked == true)
            {
                sauces.Add(chkBBQ.Content.ToString());
            }
        }
    }
}
