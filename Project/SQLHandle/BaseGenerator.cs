using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using TableAttributes;

namespace SQLHandling
{
    public class BaseGenerator
    {
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//




        // Business Search Methods




        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//


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
        public string checkInBase(int type, string businessID)
        {
            string baseString = "SELECT";
            if(type == 1)
            {
                baseString += "month_, day_, year_, time_ FROM checkin WHERE business_id = '"
                                    + businessID + "' ORDER BY year_;";
            }
            else
            {
                baseString += "* FROM checkin WHERE business_id = '"
                                    + businessID + "' ORDER BY year_ DESC;";
            }

            return baseString;
        }

        //returns SQL statement for getting cities after a state is selected
        public string cityBase(string state)
        {
            return "SELECT distinct city FROM business WHERE state_ = '"
                                + state
                                + "' ORDER BY city;";
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
            for (int i = 0; i < location.Count; i++)
            {
                if (i == 0)
                {
                    baseString += " state_ = '" + location[i] + "' ";
                }
                else
                {
                    if (char.IsDigit(location[i][0]))
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

        //returns SQL statement for the hours of a specific business
        public string hoursBase(string businessID)
        {
            return "SELECT DISTINCT open_, close_ FROM hours WHERE business_id = '"
                                    + businessID + "' AND dayofweek = '"
                                    + DateTime.Today.DayOfWeek.ToString() + "';";
        }

        //returns SQL statement for the different attributes of a specific business
        public string attributeBase(string businessID)
        {
            return "SELECT DISTINCT attribute_name, attribute_value FROM attribute WHERE business_id = '"
                                        + businessID + "';";
        }

        //returns SQL statement for creating a new tip
        public string tipInsertBase(string businessID, string userID, string text)
        {
            return "INSERT INTO tip VALUES ('"
                                    + businessID + "', '"
                                    + userID + "', '"
                                    + DateTime.Now.ToString() + "', "
                                    + 0 + ", '" + text + "');";
        }

        //returns SQL statement for creating a new checkin
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

        //returns SQL statement for getting userID's based on specified user name
        public string userIDBase(string name)
        {
            return "SELECT DISTINCT user_id FROM users WHERE name_ = '" + name + "' ORDER BY user_id;";
        }

        //returns SQL statement for getting User info
        public string userBase(int type, string userID)
        {
            string baseString = "SELECT name_, average_stars, fans, cool, tipcount, funny, totallikes, useful,";
            if(type == 1)
            {
                baseString += " user_latitude, user_longitude, yelping_since FROM users WHERE user_id = '" +
                userID + "';";
            }
            else
            {
                baseString += " yelping_since FROM users WHERE user_id = '" +
               userID + "';";
            }
            return baseString;
        }

        //returns SQL statement for getting list of friends from a specified user
        public string friendsListBase(string userID)
        {
            return "SELECT DISTINCT user_id, friend_id FROM (users NATURAL JOIN friend) " +
                "WHERE user_id = '" + userID + "';";
        }

        //returns SQL statement for getting  latest tips of a specified users friends
        public string friendTipBase(string userID)
        {
            return " SELECT userName, name_ AS businessName, city, date_, text_, likes_ FROM " +
                "(SELECT user_id, users.name_ AS userName, business_id, date_, text_, likes_ FROM(users NATURAL JOIN tip)" +
                "ORDER BY date_ DESC) AS friendTips NATURAL JOIN business WHERE user_id = '" + 
                userID + "' LIMIT 1;";
        }

        //returns SQL statement for updating a specified users or business' lat and lon
        public string updateLocationBase(string lat, string lon, string ID)
        {
            return "update users set user_latitude = " + lat + ", user_longitude = " + lon +
                    "where user_id = '" + ID + "'; ";
        }


        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//




        // Business Owner Page Methods




        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        // *******************************************************************************************************//
        
        //returns list of businessID's based on a specified business name
        public string businessIDBase(string name)
        {
            return "Select business_id from business where name_  = '" + name + "';";
        }

        public string business(string businessID)
        {
            return "select name_, city, state_, address, postal_code, stars, reviewcount, numtips, numcheckins, " +
                "latitude_business, longitude_business " +
                "from business " +
                "where business_id = '" + businessID + "' order by business_id;";
        }

        public string businessTips(string businessID)
        {
            return "select u.name_, u.yelping_since, ti.date_, ti.likes_, ti.text_ " +
                "from tip as ti, users as u, business as b " +
                "where b.business_id = '" + businessID + "' " +
                "and ti.business_id = '" + businessID + "' and ti.user_id = u.user_id " +
                "order by date_ desc;";
        }
    }
}
