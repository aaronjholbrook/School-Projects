using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPTS451_Milestone1
{
    class BaseGenerator
    {
        List<String> baseList = new List<string>();

        //returns SQL statement for getting all the states
        public string stateBase()
        {
            return "SELECT distinct state_ FROM business ORDER BY state_";
        }

        //returns SQL statement for when business table is getting filled and need the users location is being taken
        public string userLocationBase(string userID)
        {
            return "select user_latitude, user_longitude from users where user_id = '" + userID + "';";
        }

        //returns SQL statement for when a business table is getting filled and need to calculate the distnaces
        public string distnaceBase(string userLat, string userLon, string businessLat, string businessLon)
        {
            return "select distnaceCalculation(" + userLat + "," + userLon + "," + businessLat + "," + businessLon + ");";
        }

        //returns SQL statement for getting tips from a specific business
        public string tipBase(string businessID)
        {
            return "SELECT name_, date_, likes_, text_ FROM (tip NATURAL JOIN users) WHERE business_id = '"
                                    + businessID + "' ORDER BY date_;";
        }

        //returns SQL statement to get the checkins for a specific business
        public string checkInBase(string businessID)
        {
            return "SELECT month_, day_, year_, time_ FROM checkin WHERE business_id = '"
                                    + businessID + "' ORDER BY year_;";
        }

        //returns all cases of geting postal codes
        public string postalBase(List<string> location)
        {
            string baseString = "SELECT distinct postal_code FROM business WHERE";
            for (int i = 0; i < location.Count; i++)
            {
                if (i == 0)
                {
                    baseString += " state_ = '" + location[i] + "' ";
                }
                else
                {
                    baseString += " AND city = '" + location[i] + "' ";
                }

            }
            return baseString + "ORDER BY postal_code;";
        }

        //returns all base cases of business for business search
        public string businessBase(List<string> location)
        {
            string baseString = "SELECT distinct name_, state_, city, business_id, postal_code, address, "
                                    + "latitude_business, longitude_business, stars, numtips, numcheckins "
                                    + "FROM business WHERE";
            for(int i = 0; i<location.Count; i++)
            {
                if(i == 0)
                {
                    baseString += " state_ = '" + location[i] + "' ";
                }
                else 
                {
                    if(char.IsDigit(location[i][0]))
                    {
                        baseString += " AND postal_code = '" + location[i] + "' ";
                    }
                    else
                    {
                        baseString += " AND city = '" + location[i] + "' ";
                    }
                }
            }

            return baseString + ";";
        }

        //returns all the cases for getting categories
        public string categoryBase(string businessID)
        {
            return "SELECT DISTINCT category_name FROM Categories WHERE business_id = '" + businessID + "' ;";
        }

        public string tipInsertBase(string businessID, string userID, string text)
        {
            return "INSERT INTO tip VALUES ('"
                                    + businessID + "', '"
                                    + userID + "', '"
                                    + DateTime.Now.ToString() + "', "
                                    + 0 + ", '" + text + "');";
        }

        public string checkinInsert(string businessID)
        {
            string baseString = "INSERT INTO checkin VALUES ('"
                                    + businessID + "', '"
                                    + DateTime.Now.Year + "', '";
            if (DateTime.Now.Month < 10)
            {
                baseString += "0";
            }
            baseString += DateTime.Now.Month + "', '";
            if (DateTime.Now.Day < 10)
            {
                baseString += "0";
            }
            baseString += DateTime.Now.Day + "', '"
                + DateTime.Now.TimeOfDay.ToString() + "')";
            return baseString + ";";
        }

        //returns list of SQL statements to be called once state is selected
        public List<string> StateSelectBase(string selectedState)
        {
            baseList.Clear();
            List<string> location = new List<string>() { selectedState};
            //statement for getting cities for combo box
            baseList.Add("SELECT distinct city FROM business WHERE state_ = '"
                                + selectedState
                                + "' ORDER BY city;");

            //statement for geting postal codes for combo box
            baseList.Add(postalBase(location));
            return this.baseList;
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
            baseList.Add(categoryBase(businessID));

            // statement for getting attributes of business for list box
            baseList.Add("SELECT DISTINCT attribute_name, attribute_value FROM attribute WHERE business_id = '"
                                        + businessID + "';");
            return this.baseList;
        }
    }
}
