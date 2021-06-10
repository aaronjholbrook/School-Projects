using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPTS451_Milestone1
{
    class Sorter
    {
        public string doSort(int type, string statement)
        {
            if(type == 1)
            {
                return nameSort(statement);
            }
            else if(type == 2)
            {
                return starSort(statement);
            }
            else if(type == 3)
            {
                return tipSort(statement);
            }
            else if(type == 4)
            {
                return checkinSort(statement);
            }
            return nameSort(statement);
        }


        // order list by name
        public string nameSort(string statement)
        {
            statement.Replace("ORDER BY stars DESC", string.Empty);
            statement.Replace("ORDER BY numtips DESC", string.Empty);
            statement.Replace("ORDER BY numcheckins DESC", string.Empty);
            statement.Replace(";", "  ORDER BY name_;");
            return statement;
        }

        //order list by highest rating
        public string starSort(string statement)
        {
            statement.Replace("ORDER BY name_", string.Empty);
            statement.Replace("ORDER BY numtips DESC", string.Empty);
            statement.Replace("ORDER BY numcheckins DESC", string.Empty);
            statement.Replace(";", "  ORDER BY stars DESC;");
            return statement;
        }

        //order list by highest number of tips
        public string tipSort(string statement)
        {
            statement.Replace("ORDER BY name_", string.Empty);
            statement.Replace("ORDER BY stars DESC", string.Empty);
            statement.Replace("ORDER BY numcheckins DESC", string.Empty);
            statement.Replace(";", "  ORDER BY numtips DESC;");
            return statement;
        }

        //order list by highest number of check ins
        public string checkinSort(string statement)
        {
            statement.Replace("ORDER BY name_", string.Empty);
            statement.Replace("ORDER BY stars DESC", string.Empty);
            statement.Replace("ORDER BY numtips DESC", string.Empty);
            statement.Replace(";", "  ORDER BY numcheckins DESC;");
            return statement;
        }
    }
}
