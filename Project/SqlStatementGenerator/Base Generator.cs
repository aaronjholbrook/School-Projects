using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;


namespace SqlStatementGenerator
{
    class Base_Generator
    {
        List<String> baseList = new List<string>();

        //returns list of SQL statements to be called once state is selected
        public List<string> StateSelectBase(string selectedState)
        {
            baseList.Clear();

            //statement for getting cities for combo box
            baseList.Add("SELECT distinct city FROM business WHERE state_ = '"
                                + selectedState
                                + "' ORDER BY city;");

            //statement for geting postal codes for combo box
            baseList.Add("SELECT distinct postal_code FROM business WHERE state_ = '"
                                + selectedState
                                + "' ORDER BY postal_code;");
            return this.baseList;
            
        }

        //returns list of SQL statements to be called once postal code is selected
        public List<string> CitySelectBase(string selectedState, string selectedCity)
        {
            baseList.Clear();

            // statement for for getting businesses when state and city are selected
            baseList.Add("SELECT distinct name_, state_, city, business_id, postal_code, address, "
                                    + "latitude_business, longitude_business, stars, numtips, numcheckins "
                                    + "FROM business WHERE state_ = '"
                                    + selectedState + "' AND city = '"
                                    + selectedCity + "' ORDER BY name_;");

            //statement for getting postal codes for combo box
            baseList.Add("SELECT distinct postal_code FROM business WHERE state_ = '"
                                + selectedState + "' AND city = '"
                                + selectedCity + "' ORDER BY postal_code;");

            return this.baseList;
        }

        //returns SQL statement to be called once postal code is selected
        public string postalSelectBase(string selectedState, string selectedZip)
        {         
            // statement for getting businesses with state and postal code selected
            return "SELECT distinct name_, state_, city, business_id, postal_code, address, "
                                    + "latitude_business, longitude_business, stars, numtips, numcheckins "
                                    + "FROM business WHERE state_ = '"
                                    + selectedState + "' AND postal_code = '"
                                    + selectedZip + "' ORDER BY name_;";
        }

        //returns SQL statement to be called once postal code is selected
        public string postalSelectBase(string selectedState, string selectedCity, string selectedZip)
        {
            // statement for getting businesses with state and postal code selected
            return "SELECT distinct name_, state_, city, business_id, postal_code, address, "
                                    + "latitude_business, longitude_business, stars, numtips, numcheckins "
                                    + "FROM business WHERE state_ = '"
                                    + selectedState + "' AND city = '"
                                    + selectedCity + "' AND postal_code = '"
                                    + selectedZip + "' ORDER BY name_;";
        }

        //returns SQL statement for when you need to fill the category list box after postal code is selected
        public string categoryListBoxFiller(string businessID)
        {
            // statement for getting categories to fill list box
            return "SELECT DISTINCT category_name FROM categories WHERE business_id = '"
                                        + businessID + "';";
        }

        //returns list of SQL statements for when you select a business and need to set the hours for the day 
        //and fill the bottom right list box with the categories and attributes
        public List<string> BusinessSelectBase(string businessID)
        {
            baseList.Clear();

            // statement for getting the hours of business based on todays current date
            baseList.Add("SELECT DISTINCT open_, close_ FROM hours WHERE business_id = '"
                                    + businessID + "' AND dayofweek = '"
                                    + DateTime.Today.DayOfWeek.ToString() + "';");

            // statement for getting categories of business for list box
            baseList.Add("SELECT DISTINCT category_name FROM categories WHERE business_id = '"
                                        + businessID + "';");

            // statement for getting attributes of business for list box
            baseList.Add("SELECT DISTINCT attribute_name, attribute_value FROM attribute WHERE business_id = '"
                                        + businessID + "';");
            return this.baseList;
        }

        //returns SQL statement for when business table is getting filled and need the users location is being taken
        public string userLocationBase(string userID)
        {
            // statement for getting a specific users lat and lon
            return "select user_latitude, user_longitude from users where user_id = '" + userID + "';";
        }

        //returns SQL statement for when a business table is getting filled and need to calculate the distnaces
        public string distnaceBase(string userLat, string userLon, string businessLat, string businessLon)
        {
            // statement for getting distnace between user and a business
            return "select distnaceCalculation(" + userLat + "," + userLon + "," + businessLat + "," + businessLon + ");";
        }


    }
}
