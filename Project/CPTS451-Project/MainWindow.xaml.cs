using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Data;
using SQLHandling;

namespace CPTS451_Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool tipsActive = false;
        private bool commandRead = false;
        private bool checkinsActive = false;
        List<Business> coordinates = new List<Business>();
        List<Business> sortList = new List<Business>();
        string selectedUser;
        User user;
        double distance = 0.0;

        private SQLHandling.BaseGenerator baseGenerator = new SQLHandling.BaseGenerator();
        private SQLHandling.Sort sorter = new SQLHandling.Sort();


        int type = 1;

        List<CheckBox> prices = new List<CheckBox>();
        List<CheckBox> categories = new List<CheckBox>();
        List<CheckBox> food = new List<CheckBox>();

        public MainWindow()
        {
            InitializeComponent();
            AddState();
            AddColumns2BusinessGrid();
            AddColumns2UserFriendsListGrid();
            AddColumns2UserFriendsLatestTipsListGrid();
            AddColumnsCheckinsListGrid();
            AddColumnsTipsListGrid();

            prices.Add(One);
            prices.Add(Two);
            prices.Add(Three);
            prices.Add(Four);


            categories.Add(AcceptsCredit);
            categories.Add(TakesReservations);
            categories.Add(WheelchairAccess);
            categories.Add(WheelchairAccess);
            categories.Add(GoodForKids);
            categories.Add(GoodForGroups);
            categories.Add(Delivery);
            categories.Add(TakeOut);
            categories.Add(FreeWiFi);
            categories.Add(BikeParking);

            food.Add(Breakfast);
            food.Add(Lunch);
            food.Add(Brunch);
            food.Add(Dinner);
            food.Add(Dessert);
            food.Add(LateNight);

        }

        private string BuildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = YelpDB; password = Alb3rt184011!!!";
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Business Search Functions
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AddStateHelper(NpgsqlDataReader reader)
        {
            stateList.Items.Add(reader.GetString(0));
        }

        private void AddState()
        {
            string command = baseGenerator.stateBase();
            ExecuteQuery(command, AddStateHelper);
        }

        private void AddColumns2BusinessGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn
            {
                Binding = new Binding("Name"),
                Header = "BusinessName",
                Width = 150
            };
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn
            {
                Binding = new Binding("Address"),
                Header = "Address",
                Width = 255
            };
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn
            {
                Binding = new Binding("City"),
                Header = "City",
                Width = 100
            };
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn
            {
                Binding = new Binding("State"),
                Header = "State",
                Width = 40
            };
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn
            {
                Binding = new Binding("Distance"),
                Header = "Distance\n(miles)",
                Width = 55
            };
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn
            {
                Binding = new Binding("Stars"),
                Header = "Stars",
                Width = 40
            };
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn
            {
                Binding = new Binding("Tips"),
                Header = "# of Tips",
                Width = 55
            };
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn
            {
                Binding = new Binding("Checkins"),
                Header = "Total\nCheckins",
                Width = 60
            };
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn
            {
                Binding = new Binding("BusinessID"),
                Header = "",
                Width = 0,
                Visibility = Visibility.Hidden
            };
            businessGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn
            {
                Binding = new Binding("ZipCode"),
                Header = "",
                Width = 0,
                Visibility = Visibility.Hidden
            };
            businessGrid.Columns.Add(col10);
        }

        private void AddBusinessGridRow(NpgsqlDataReader reader)
        {
            if (reader.HasRows)
            {

                if (UserIDResults.SelectedIndex > -1 || user != null)
                {

                    
                    if (UserIDResults.SelectedIndex > -1)
                    {
                        string commandUser = baseGenerator.userLocationBase(this.UserIDResults.SelectedItem.ToString());
                        commandRead = false;

                        ExecuteQuery(commandUser, this.AddUser);
                    }

                    string userLatQuery = this.user.Latitude.ToString();
                    string userLongQuery = this.user.Longitude.ToString();
                    

                    string command1 = baseGenerator.distnaceBase(userLatQuery, userLongQuery, reader.GetDouble(6).ToString(), reader.GetDouble(7).ToString());
                    commandRead = false;

                    ExecuteQuery(command1, this.GetDistance);
                }
                Business business = new Business()
                {
                    Name = reader.GetString(0),
                    State = reader.GetString(1),
                    City = reader.GetString(2),
                    BusinessID = reader.GetString(3),
                    ZipCode = reader.GetInt32(4),
                    Address = reader.GetString(5),
                    Latitude = reader.GetDouble(6),
                    Longitude = reader.GetDouble(7),
                    Distance = this.distance,
                    Stars = reader.GetDouble(8),
                    Tips = reader.GetInt32(9),
                    Checkins = reader.GetInt32(10),
                };

                businessGrid.Items.Add(business);
            }
        }

        private void GetDistance(NpgsqlDataReader reader)
        {
            this.distance = reader.GetDouble(0);
        }
        private void AddUser(NpgsqlDataReader reader)
        {
            this.user = new User()
            {
                Latitude = reader.GetDouble(0),
                Longitude = reader.GetDouble(1),
            };
        }

        private void AddCity(NpgsqlDataReader reader)
        {
            cityList.Items.Add(reader.GetString(0));
        }

        private void AddZipCode(NpgsqlDataReader reader)
        {
            zipList.Items.Add(reader.GetInt32(0));
        }

        private void AddCategory(NpgsqlDataReader reader)
        {
            if (!this.CategoryList.Items.Contains(reader.GetString(0)))
            {
                this.CategoryList.Items.Add(reader.GetString(0));
            }
        }
        
        private void AddHours(NpgsqlDataReader reader)
        {
            if (reader.HasRows)
            {
                SelectedBusinessOpenCloseTimes.Text = "Today: Open from " + reader.GetString(0) + " to " + reader.GetString(1);
            }
        }

        private void AddTipList(NpgsqlDataReader reader)
        {
            CheckinTipList.Items.Add(reader.GetString(0) + "\nDate: " + reader.GetString(1) + "\nLikes: " + reader.GetInt32(2) + "\n" + reader.GetString(3));
        }

        private void UpdateTips(NpgsqlDataReader reader)
        {
            if (tipsActive)
            {
                if (businessGrid.SelectedIndex > -1)
                {
                    CheckinTipList.Items.Clear();
                    Business current = businessGrid.SelectedItem as Business;
                    string command = baseGenerator.tipBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command, AddTipList);
                }
            }
        }

        private void CategoriesAttributeList(NpgsqlDataReader reader)
        {
            if (!CheckinTipList.Items.Contains("Categories") && reader.GetName(0).Equals("category_name"))
            {
                CheckinTipList.Items.Add("Categories");
            }
            else if (!CheckinTipList.Items.Contains("Attributes") && reader.GetName(0).Equals("attribute_name"))
            {
                CheckinTipList.Items.Add("Attributes");
            }

            if (reader.GetName(0).Equals("category_name"))
            {
                CheckinTipList.Items.Add(reader.GetString(0));
            }
            else
            {
                if (reader.GetString(1).Equals("True"))
                {
                    CheckinTipList.Items.Add(reader.GetString(0));
                }
                else if (!reader.GetString(0).Equals("True") && !reader.GetString(1).Equals("False") && !reader.GetString(1).Equals("None"))
                {
                    CheckinTipList.Items.Add(reader.GetString(0) + " (" + reader.GetString(1) + ")");
                }
            }
        }

        private void UpdateCheckins(NpgsqlDataReader reader)
        {
            if (checkinsActive)
            {
                if (businessGrid.SelectedIndex > -1)
                {
                    CheckinTipList.Items.Clear();
                    Business current = businessGrid.SelectedItem as Business;
                    string command = baseGenerator.checkInBase(1, current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command, CategoriesAttributeList);
                }
            }
        }

        private void CategoryFilter(NpgsqlDataReader reader)
        {
            List<string> acceptableBusinesses = new List<string>();
            List<Business> businessesToRemove = new List<Business>();

            if (reader.HasRows)
            {
                acceptableBusinesses.Add(reader.GetString(0));
            }

            while (reader.Read())
            {
                acceptableBusinesses.Add(reader["business_id"].ToString());
            }

            foreach (object business in businessGrid.Items)
            {
                Business currentBusiness = business as Business;
                if (!acceptableBusinesses.Contains(currentBusiness.BusinessID))
                {
                    businessesToRemove.Add(currentBusiness);
                }
            }

            foreach (Business business in businessesToRemove)
            {
                businessGrid.Items.Remove(business);
            }
        }

        private void PopulateGrid()
        {
            if (this.stateList.SelectedIndex > -1)
            {
                this.businessGrid.Items.Clear();                
                SQLHandling.Filter filter = new SQLHandling.Filter();
                

                string zip = "";
                string city = "";
                List<string> categories = new List<string>();

                if (this.zipList.SelectedIndex > -1)
                {
                    zip = this.zipList.SelectedItem.ToString();
                }
                else
                {
                    zip = string.Empty;
                }

                if (this.cityList.SelectedIndex > -1)
                {
                    city = this.cityList.SelectedItem.ToString();
                }
                else
                {
                    city = string.Empty;
                }

                if (this.SelectedCategoryList.Items.Count > 0)
                {
                    for (int i = 0; i < this.SelectedCategoryList.Items.Count; i++)
                    {
                        categories.Add(this.SelectedCategoryList.Items.GetItemAt(i).ToString());
                    }
                }
                
                string command = filter.FilterQuery(this.categories, this.prices, this.food, categories,
                    this.stateList.SelectedItem.ToString(), city, zip);

                string sortedCommand = sorter.doSort(this.type, command);
                this.ExecuteQuery(sortedCommand, this.AddBusinessGridRow);
                numBusinessResult.Text = businessGrid.Items.Count + "";
            }
        }

        private void OnePriceChecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = false;
            this.Three.IsEnabled = false;
            this.Four.IsEnabled = false;
            PopulateGrid();


        }

        private void OnePriceUnchecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = true;
            this.Three.IsEnabled = true;
            this.Four.IsEnabled = true;

            PopulateGrid();
        }

        private void TwoPriceChecked(object sender, RoutedEventArgs e)
        {
            this.One.IsEnabled = false;
            this.Three.IsEnabled = false;
            this.Four.IsEnabled = false;
            PopulateGrid();
        }

        private void Two_Unchecked(object sender, RoutedEventArgs e)
        {
            this.One.IsEnabled = true;
            this.Three.IsEnabled = true;
            this.Four.IsEnabled = true;

            PopulateGrid();
        }

        private void ThreePriceChecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = false;
            this.One.IsEnabled = false;
            this.Four.IsEnabled = false;

            PopulateGrid();
        }

        private void Three_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = true;
            this.One.IsEnabled = true;
            this.Four.IsEnabled = true;

            PopulateGrid();
        }

        private void FourPriceChecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = false;
            this.Three.IsEnabled = false;
            this.One.IsEnabled = false;

            PopulateGrid();
        }

        private void Four_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Two.IsEnabled = true;
            this.Three.IsEnabled = true;
            this.One.IsEnabled = true;

            PopulateGrid();
        }

        private void AcceptsCredit_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void AcceptsCredit_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void TakesReservations_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void TakesReservations_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void WheelchairAccess_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }

        private void WheelchairAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void OutdoorSeating_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void OutdoorSeating_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void GoodForKids_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void GoodForKids_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void GoodForGroups_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void GoodForGroups_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Delivery_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Delivery_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void TakeOut_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void TakeOut_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void FreeWiFi_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void FreeWiFi_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void BikeParking_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void BikeParking_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Breakfast_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Breakfast_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Lunch_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Lunch_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Brunch_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Brunch_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Dinner_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Dinner_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Dessert_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void Dessert_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void LateNight_Checked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }

        private void LateNight_Unchecked(object sender, RoutedEventArgs e)
        {
            PopulateGrid();

        }


        private bool CheckboxesChecked()
        {
            // Prices
            if (Convert.ToBoolean(this.One.IsChecked))
            {
                return true;
            }
            else if (Convert.ToBoolean(this.Two.IsChecked))
            {
                return true;
            }
            else if (Convert.ToBoolean(this.Three.IsChecked))
            {
                return true;
            }
            else if (Convert.ToBoolean(this.Four.IsChecked))
            {
                return true;
            }

            // Attributes
            if (Convert.ToBoolean(this.AcceptsCredit.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.TakesReservations.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.WheelchairAccess.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.OutdoorSeating.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.GoodForKids.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.GoodForGroups.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.Delivery.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.TakeOut.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.FreeWiFi.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.BikeParking.IsChecked))
            {
                return true;
            }

            // Meals
            if (Convert.ToBoolean(this.Breakfast.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.Lunch.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.Brunch.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.Dinner.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.Dessert.IsChecked))
            {
                return true;
            }
            if (Convert.ToBoolean(this.LateNight.IsChecked))
            {
                return true;
            }

            return false;
        }

        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cityList.Items.Clear();
            this.zipList.Items.Clear();
            this.SelectedCategoryList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                List<string> location = new List<string>() {stateList.SelectedItem.ToString()};
                
                string command1 = baseGenerator.cityBase(stateList.SelectedItem.ToString());
                commandRead = false;
                ExecuteQuery(command1, AddCity);
                string command2 = baseGenerator.postalBase(location);
                commandRead = false;
                ExecuteQuery(command2, AddZipCode);
            }
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.businessGrid.Items.Clear();
            this.zipList.Items.Clear();
            this.SelectedCategoryList.Items.Clear();
            this.coordinates.Clear();

            if (cityList.SelectedIndex > -1)
            {
                List<string> location = new List<string>() { stateList.SelectedItem.ToString(), cityList.SelectedItem.ToString() };
                if (this.CheckboxesChecked())
                {
                    PopulateGrid();

                }
                else
                {
                    
                    string command1 = sorter.doSort(1, baseGenerator.businessBase(location));
                    commandRead = false;
                    ExecuteQuery(command1, AddBusinessGridRow);
                }

                string command2 = baseGenerator.postalBase(location);
                commandRead = false;
                ExecuteQuery(command2, AddZipCode);
                numBusinessResult.Text = businessGrid.Items.Count + "";
            }

        }

        private void ZipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            CategoryList.Items.Clear();
            SelectedCategoryList.Items.Clear();
            this.coordinates.Clear();

            if (zipList.SelectedIndex > -1 && cityList.SelectedIndex == -1)
            {
                List<string> location = new List<string>() { stateList.SelectedItem.ToString(), zipList.SelectedItem.ToString() };
                if (this.CheckboxesChecked())
                {
                    PopulateGrid();
                }
                else
                {
                    string command1 = sorter.doSort(1, baseGenerator.businessBase(location));

                    commandRead = false;
                    ExecuteQuery(command1, this.AddBusinessGridRow);
                }
                numBusinessResult.Text = businessGrid.Items.Count + "";

                foreach (object item in businessGrid.Items)
                {
                    Business current = item as Business;
                    string command2 = baseGenerator.categoryBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command2, AddCategory);
                }
            }
            else if (zipList.SelectedIndex > -1 && cityList.SelectedIndex > -1)
            {
                List<string> location = new List<string>() { stateList.SelectedItem.ToString(), cityList.SelectedItem.ToString(), zipList.SelectedItem.ToString() };
                if (this.CheckboxesChecked())
                {
                    PopulateGrid();
                }
                else
                {
                    string command1 = sorter.doSort(1, baseGenerator.businessBase(location));
                    commandRead = false;
                    ExecuteQuery(command1, this.AddBusinessGridRow);
                }
                numBusinessResult.Text = businessGrid.Items.Count + "";

                foreach (object item in businessGrid.Items)
                {
                    Business current = item as Business;
                    string command2 = baseGenerator.categoryBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command2, AddCategory);
                }
            }
        }

        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckinTipList.Items.Clear();
            if (businessGrid.SelectedIndex > -1)
            {
                Business current = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if (current.BusinessID != null && current.BusinessID.ToString().CompareTo("") != 0)
                {
                    SelectedBusinessName.Text = "Business Name: " + current.Name;
                    SelectedBusinessAddress.Text = "Address: " + current.Address;

                    string command = baseGenerator.hoursBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command, AddHours);

                    if (commandRead == false)
                    {
                        SelectedBusinessOpenCloseTimes.Text = "Today: This business is closed.";
                    }

                    string command2 = baseGenerator.categoryBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command2, this.CategoriesAttributeList);

                    this.CheckinTipList.Items.Add(string.Empty);

                    string command3 = baseGenerator.attributeBase(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command3, this.CategoriesAttributeList);
                }
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryList.SelectedIndex > -1)
            {
                if (!SelectedCategoryList.Items.Contains(CategoryList.SelectedItem))
                {
                    SelectedCategoryList.Items.Add(CategoryList.SelectedItem);                    
                    PopulateGrid();
                    numBusinessResult.Text = businessGrid.Items.Count + "";
                }
            }
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCategoryList.SelectedIndex > -1)
            {
                businessGrid.Items.Clear();
                SelectedCategoryList.Items.Remove(SelectedCategoryList.SelectedItem);
                PopulateGrid();
                numBusinessResult.Text = businessGrid.Items.Count + string.Empty;
            }
        }

        private void ShowTips_Click(object sender, RoutedEventArgs e)
        {
            CheckinTipList.Items.Clear();

            if (businessGrid.SelectedIndex > -1)
            {
                tipsActive = true;
                checkinsActive = false;
                Business current = businessGrid.SelectedItem as Business;
                string userID = string.Empty;
                if (this.UserIDResults.SelectedItem != null)
                {
                    userID = this.UserIDResults.SelectedItem.ToString();
                }

                TipsWindow.MainWindow tipWindow = new TipsWindow.MainWindow(current.BusinessID.ToString(), userID);
                tipWindow.Show();
            }

           
        }

        private void ShowCheckins_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business current = businessGrid.SelectedItem as Business;
                CheckinCountChart.MainWindow chartWindow = new CheckinCountChart.MainWindow(current.BusinessID);
                chartWindow.Show();
            }
        }

        private void ViewMap_Click(object sender, RoutedEventArgs e)
        {
            this.coordinates.Clear();
            if (UserIDResults.SelectedIndex > -1 || user != null)
            {                
                if(businessGrid.Items.Count > 0)
                {
                    sortList = new List<Business>();
                    this.makeNearestList();
                    for (int i = 0; i<sortList.Count; i++)
                    {
                        this.coordinates.Add(sortList[i]);
                    }
                }
                //this.coordinates.Add(new Microsoft.Maps.MapControl.WPF.Location(this.user.Latitude, this.user.Longitude));
                BingMaps.MainWindow mapWindow = new BingMaps.MainWindow(this.coordinates, true);
                mapWindow.Show();
            } else
            {
                BingMaps.MainWindow mapWindow = new BingMaps.MainWindow(this.coordinates, false);
                mapWindow.Show();
            }            
        }

        private void AddTip_Click(object sender, RoutedEventArgs e)
        {
            if (UserIDResults.SelectedIndex > -1)
            {
                if (businessGrid.SelectedIndex > -1)
                {
                    Business current = businessGrid.SelectedItem as Business;                    
                    string command = baseGenerator.tipInsertBase(current.BusinessID, UserIDResults.SelectedItem.ToString(), TipContent.Text);
                    commandRead = false;
                    ExecuteQuery(command, UpdateTips);
                    
                }
            }
        }

        private void Checkin_Click(object sender, RoutedEventArgs e)
        {
            if (UserIDResults.SelectedIndex > -1)
            {
                if (businessGrid.SelectedIndex > -1)
                {
                    Business current = businessGrid.SelectedItem as Business;
                    string command = baseGenerator.checkinInsert(current.BusinessID);
                    commandRead = false;
                    ExecuteQuery(command, UpdateCheckins);
                }
            }
        }

        private void SortChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BusinessName_Selected(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            
            this.type = 1;

            PopulateGrid();
        }
        private void HighestRate_Selected(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            
            this.type = 2;

            PopulateGrid();
        }

        private void MostTips_Selected(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            
            this.type = 3;

            PopulateGrid();
        }

        private void MostCheckins_Selected(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
           
            this.type = 4;

            PopulateGrid();
        }
        public void makeNearestList()
        {
            for(int i = 0; i<businessGrid.Items.Count; i++)
            {
                sortList.Add(businessGrid.Items[i] as Business);
            }
        }

        public List<Business> quicksort(List<Business> list)
        {
            if (businessGrid.Items.Count > 1)
            {
                int pivot = list.Count / 2;
                Business pivotValue = list[pivot];
                list.RemoveAt(pivot);
                List<Business> smaller = new List<Business>();
                List<Business> greater = new List<Business>();

                foreach (Business item in list)
                {
                    if (item.Distance < pivotValue.Distance)
                    {
                        smaller.Add(item);
                    }
                    else
                    {
                        greater.Add(item);
                    }
                }

                List<Business> sorted = new List<Business>();

                if (smaller.Count > 0)
                {
                    sorted.AddRange(quicksort(smaller));
                }

                sorted.Add(pivotValue);

                if(greater.Count>0)
                {
                    sorted.AddRange(quicksort(greater));
                }                
                return sorted;
            }
            return list;
        }

        private void Nearest_Selected(object sender, RoutedEventArgs e)
        {
            sortList = new List<Business>();
            makeNearestList();
            this.sortList = quicksort(sortList);
            this.businessGrid.Items.Clear();
            for(int i = 0; i<sortList.Count; i++)
            {
                businessGrid.Items.Add(sortList[i]);
            }
            
        }

        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//




        // User Info Page Methods




        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//



        /// <summary>
        /// A method for populating the user's friend's grid.
        /// </summary>
        private void AddColumns2UserFriendsListGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn
            {
                Binding = new Binding("Name"),
                Header = "Friend Name",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn
            {
                Binding = new Binding("StarAverage"),
                Header = "Avg Stars",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn
            {
                Binding = new Binding("Fans"),
                Header = "Fans",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn
            {
                Binding = new Binding("TipCount"),
                Header = "Tip Count",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn
            {
                Binding = new Binding("SignupDate"),
                Header = "Yelping Since",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn
            {
                Binding = new Binding("Cool"),
                Header = "Cool",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn
            {
                Binding = new Binding("Funny"),
                Header = "Funny",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn
            {
                Binding = new Binding("TotalLikes"),
                Header = "Total Likes",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn
            {
                Binding = new Binding("Useful"),
                Header = "Useful",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            friendGrid.Columns.Add(col9);


        }

        /// <summary>
        /// Function for populating the columns for friends' latest tips.
        /// </summary>
        private void AddColumns2UserFriendsLatestTipsListGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn
            {
                Binding = new Binding("UserID"),
                Header = "Friend Name",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn
            {
                Binding = new Binding("BusinessID"),
                Header = "Business",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn
            {
                Binding = new Binding("City"),
                Header = "City",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn
            {
                Binding = new Binding("Date"),
                Header = "Date",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn
            {
                Binding = new Binding("Likes"),
                Header = "Likes",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn
            {
                Binding = new Binding("Text"),
                Header = "Review",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.friendTipGrid.Columns.Add(col6);


        }

        private void UserEditButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.UserLatitude.IsReadOnly && this.UserIDResults.SelectedItem != null)
            {
                this.UserLongitude.IsReadOnly = false;
                this.UserLatitude.IsReadOnly = false;
            }
            else
            {
                this.UserLongitude.IsReadOnly = true;
                this.UserLatitude.IsReadOnly = true;
            }

        }

        private void UserTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserIDResults.Items.Clear();
            string command = baseGenerator.userIDBase(UserTextBox.Text);
            commandRead = false;
            ExecuteQuery(command, AddUserID);
        }

        private void UserIDResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserIDResults.Items.Count != 0)
            {
                this.selectedUser = UserIDResults.SelectedItem.ToString();
            }
            string command = baseGenerator.userBase(1, this.selectedUser);
            commandRead = false;
            ExecuteQuery(command, RetrieveUserInfo);

            if (this.UserIDResults.SelectedItem != null)
            { 
                string commandUser = baseGenerator.userLocationBase(this.UserIDResults.SelectedItem.ToString());
                commandRead = false;

                ExecuteQuery(commandUser, this.AddUser);
            }
            string commandFriends = baseGenerator.friendsListBase(this.selectedUser);

            if (stateList.SelectedItem != null)
            {
                List<string> location = new List<string>() {stateList.SelectedItem.ToString()};
                if(cityList.SelectedItem != null || zipList.SelectedItem != null)
                {
                    if(cityList.SelectedItem != null)
                    {
                        location.Add(cityList.SelectedItem.ToString());
                    }

                    if(zipList.SelectedItem != null)
                    {
                        location.Add(zipList.SelectedItem.ToString());
                    }

                    string commandBusiness = sorter.doSort(1, baseGenerator.businessBase(location));
                    this.businessGrid.Items.Clear();
                    commandRead = false;
                    this.ExecuteQuery(commandBusiness, this.AddBusinessGridRow);
                }
            }

            this.friendGrid.Items.Clear();
            this.friendTipGrid.Items.Clear();

            
            commandRead = true;

            ExecuteQuery(commandFriends, RetrieveFriendsIDs);
            ExecuteQuery(commandFriends, RetrieveFriendsLatestTip);
        }

        private void AddUserID(NpgsqlDataReader reader)
        {
            if (reader.HasRows)
            {
                UserIDResults.Items.Add(reader.GetString(0));
            }
        }

        private void RetrieveUserInfo(NpgsqlDataReader reader)
        {
            UserName.Text = reader.GetString(0);
            UserStars.Text = reader.GetDouble(1) + "";
            UserFans.Text = reader.GetInt32(2) + "";
            UserCoolVoteCount.Text = reader.GetInt32(3) + "";
            UserTipCount.Text = reader.GetInt32(4) + "";
            UserFunnyVoteCount.Text = reader.GetInt32(5) + "";
            UserTotalTipLikes.Text = reader.GetInt32(6) + "";
            UserUsefulVoteCount.Text = reader.GetInt32(7) + "";
            UserLatitude.Text = reader.GetDouble(8) + "";
            UserLongitude.Text = reader.GetDouble(9) + "";
            UserSignupDate.Text = reader.GetString(10);
        }


        private void RetrieveFriendInformation(NpgsqlDataReader reader)
        {

            this.friendGrid.Items.Add(new User
            {
                Name = reader.GetString(0),
                StarAverage = reader.GetDouble(1),
                Fans = reader.GetInt32(2),
                Cool = reader.GetInt32(3),
                TipCount = reader.GetInt32(4),
                Funny = reader.GetInt32(5),
                TotalLikes = reader.GetInt32(6),
                Useful = reader.GetInt32(7),
                SignupDate = reader.GetString(8),

            });
        }

        private void PopulateLatestTipInformation(NpgsqlDataReader reader)
        {
            this.friendTipGrid.Items.Add(new Tip
            {
                UserID = reader.GetString(0),
                BusinessID = reader.GetString(1),
                City = reader.GetString(2),
                Date = reader.GetString(3),
                Likes = reader.GetInt32(5),
                Text = reader.GetString(4),


            });
        }

        private void RetrieveFriendsIDs(NpgsqlDataReader reader)
        {
            commandRead = true;

            string commandFriends = baseGenerator.userBase(2, reader.GetString(1).ToString());


            ExecuteQuery(commandFriends, this.RetrieveFriendInformation);




        }


        private void RetrieveFriendsLatestTip(NpgsqlDataReader reader)
        {
            commandRead = true;

            string commandFriends = baseGenerator.friendTipBase(reader.GetString(1).ToString());


            ExecuteQuery(commandFriends, this.PopulateLatestTipInformation);
        }

        // General Methods

        private void ExecuteQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(BuildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            myf(reader);
                            commandRead = true;
                        }

                        if (commandRead == false)
                        {
                            myf(reader);
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void UserUpdateLocationButtonClicked(object sender, RoutedEventArgs e)
        {
            string latText = this.UserLatitude.Text;
            string longText = this.UserLongitude.Text;

            double lat;
            double long_;

            if (latText != string.Empty && longText != string.Empty)
            {
                Double.TryParse(latText, out double result1);
                lat = result1;

                Double.TryParse(longText, out double result2);
                long_ = result2;

                if (lat >= -90 && lat <= 90 && long_ >= -180 && long_ <= 180)
                {
                    string command = baseGenerator.updateLocationBase(lat.ToString(), long_.ToString(), this.UserIDResults.SelectedItem.ToString());

                    this.ExecuteQuery(command, RetrieveUserInfo);
                    this.UserLatitude.IsReadOnly = true;
                    this.UserLongitude.IsReadOnly = true;
                }
            }
        }




        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//




        // Business Info Methods




        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//

        private void AddColumnsCheckinsListGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn
            {
                Binding = new Binding("Year"),
                Header = "Year",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.CheckinGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn
            {
                Binding = new Binding("Month"),
                Header = "Month",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.CheckinGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn
            {
                Binding = new Binding("Day"),
                Header = "Day",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.CheckinGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn
            {
                Binding = new Binding("Time"),
                Header = "Time",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.CheckinGrid.Columns.Add(col4);
        }


        private void AddColumnsTipsListGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn
            {
                Binding = new Binding("UserID"),
                Header = "Name",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.LatestTipGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn
            {
                Binding = new Binding("Yelping"),
                Header = "Yelping Since",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.LatestTipGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn
            {
                Binding = new Binding("Date"),
                Header = "Date",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.LatestTipGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn
            {
                Binding = new Binding("Likes"),
                Header = "Likes",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.LatestTipGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn
            {
                Binding = new Binding("Text"),
                Header = "Review",
                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto),
            };
            this.LatestTipGrid.Columns.Add(col5);
        }


        private void AddBusinessID(NpgsqlDataReader reader)
        {
            if (reader.HasRows)
            {
                BusinessIDResults.Items.Add(reader.GetString(0));
            }
        }

        private void BusinessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BusinessIDResults.Items.Clear();
            string command = baseGenerator.businessIDBase(this.BusinessTextBox.Text.ToString());
            commandRead = false;
            ExecuteQuery(command, this.AddBusinessID);
        }

        private void BusinessIDResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string command = baseGenerator.business(this.BusinessIDResults.SelectedItem.ToString());

            commandRead = false;
            ExecuteQuery(command, RetriveBusinessInfo);


            string commandCheckins = baseGenerator.checkInBase(2, this.BusinessIDResults.SelectedItem.ToString());

            commandRead = true;
            CheckinGrid.Items.Clear();
            ExecuteQuery(commandCheckins, RetrieveBusinessCheckins);


            string commandTips = baseGenerator.businessTips(BusinessIDResults.SelectedItem.ToString());
            commandRead = true;

            LatestTipGrid.Items.Clear();
            ExecuteQuery(commandTips, RetrieveBusinessTips);
        }

        private void RetrieveBusinessCheckins(NpgsqlDataReader reader)
        {

            this.CheckinGrid.Items.Add(new Checkin
            {
                Year = reader.GetString(1),
                Month = reader.GetString(2),
                Day = reader.GetString(3),
                Time = reader.GetString(4),

            });
        }

        private void RetrieveBusinessTips(NpgsqlDataReader reader)
        {
            this.LatestTipGrid.Items.Add(new Tip
            {
                UserID = reader.GetString(0),
                Yelping = reader.GetString(1),
                Date = reader.GetString(2),
                Likes = reader.GetInt32(3),
                Text = reader.GetString(4),

            }) ;
        }

        private void RetriveBusinessInfo(NpgsqlDataReader reader)
        {
            BusinessUserName.Text = reader.GetString(0);
            BusinessCity.Text = reader.GetString(1);
            BusinessState.Text = reader.GetString(2);
            BusinessAddress.Text = reader.GetString(3);
            BusinessZipcode.Text = reader.GetInt32(4).ToString();
            BusinessStars.Text = reader.GetDouble(5).ToString();
            BusinessReviewCount.Text = reader.GetInt32(6).ToString();
            BusinessTipCount.Text = reader.GetInt32(7).ToString();
            BusinessCheckinCount.Text = reader.GetInt32(8).ToString();
            BusinessLatitude.Text = reader.GetDouble(9).ToString();
            BusinessLongitude.Text = reader.GetDouble(10).ToString();
        }

        private void BusinessEditToggleButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.BusinessLongitude.IsReadOnly && this.BusinessIDResults.SelectedItem != null)
            {
                this.BusinessLatitude.IsReadOnly = false;
                this.BusinessLongitude.IsReadOnly = false;
            }
            else
            {
                this.BusinessLatitude.IsReadOnly = true;
                this.BusinessLongitude.IsReadOnly = true;
            }
        }

        private void BusinessUpdateLocationButtonClicked(object sender, RoutedEventArgs e)
        {
            string latText = this.BusinessLatitude.Text;
            string longText = this.BusinessLongitude.Text;

            double lat;
            double long_;

            if (latText != string.Empty && longText != string.Empty)
            {
                Double.TryParse(latText, out double result1);
                lat = result1;

                Double.TryParse(longText, out double result2);
                long_ = result2;

                if (lat >= -90 && lat <= 90 && long_ >= -180 && long_ <= 180)
                {
                    string command = baseGenerator.updateLocationBase(lat.ToString(), long_.ToString(), this.BusinessIDResults.SelectedItem.ToString());

                    this.ExecuteQuery(command, RetrieveUserInfo);
                    this.BusinessLongitude.IsReadOnly = true;
                    this.BusinessLatitude.IsReadOnly = true;
                }
            }
        }
    }
}
