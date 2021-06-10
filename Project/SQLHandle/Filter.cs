
﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using TableAttributes;

namespace SQLHandling
{
    public class Filter
    {
        public string FilterQuery(List<CheckBox> filterBoxes, List<CheckBox> filterPrices, List<CheckBox> filterFood, List<string> categories, string state, string city, string zipcode)
        {
            StringBuilder statement = new StringBuilder();
            statement.Append(this.AttachState(state));
            statement.Append(this.AttachCity(city));
            statement.Append(this.AttachZipcode(zipcode));
            statement.Append(this.AttachPriceCheckBox(filterPrices));
            statement.Append(this.AttachCategoriesCheckBoxes(filterBoxes));
            statement.Append(this.AttachFoodCheckBoxes(filterFood));
            statement.Append(this.AttachCategory(categories));
            statement.Append(";");
            return statement.ToString();
        }
        public StringBuilder AttachState(string state)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT name_, state_, city, business_id, postal_code," +
                " address,latitude_business, longitude_business, stars, numtips, numcheckins" +
                " FROM business WHERE state_ = '" + state + "' ");
            return builder;
        }
        public StringBuilder AttachCity(string city)
        {
            StringBuilder builder = new StringBuilder();
            if (city == string.Empty)
            {
                builder.Append("");
                return builder;
            }
            else
            {
                builder.Append(" AND city = '" + city + "' ");
            }

            return builder;
        }
        public StringBuilder AttachZipcode(string zipcode)
        {
            StringBuilder builder = new StringBuilder();
            if (zipcode == string.Empty)
            {
                builder.Append("");
            }
            else
            {
                builder.Append(" AND postal_code = '" + zipcode + "' ");
            }
            return builder;
        }
        public StringBuilder AttachPriceCheckBox(List<CheckBox> filterPrices)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0, prices = 1; i < filterPrices.Count; i++, prices++)
            {
                if (Convert.ToBoolean(filterPrices[i].IsChecked))
                {
                    builder.Append(" AND business_id IN (SELECT B.business_id  FROM business AS B, attribute AS A_ "
                           + "WHERE B.business_id = A_.business_id AND (A_.attribute_name = "
                           + "'RestaurantsPriceRange2' AND A_.attribute_value = '"); // ')) ";
                    builder.Append(prices.ToString() + "'))");
                    break;
                }
            }
            return builder;
        }
        public StringBuilder AttachCategoriesCheckBoxes(List<CheckBox> filterBoxes)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < filterBoxes.Count; i++)
            {
                SchemaCategories.CategoriesName name = (SchemaCategories.CategoriesName)i;
                if (Convert.ToBoolean(filterBoxes[i].IsChecked) && i != 8)
                {
                    builder.Append(" AND business_id IN(SELECT B.business_id  FROM business AS B, attribute AS A_ "
                            + "WHERE B.business_id = A_.business_id AND (A_.attribute_name = "
                            + "'"); // ' AND A_.attribute_value = 'True')) ");
                    builder.Append(name.ToString() + "' AND A_.attribute_value = 'True'))");
                }
                else if (Convert.ToBoolean(filterBoxes[i].IsChecked) && i == 8)
                {
                    builder.Append(" AND business_id IN(SELECT B.business_id  FROM business AS B, attribute AS A_ "
                            + "WHERE B.business_id = A_.business_id AND (A_.attribute_name = "
                            + "'"); // ' AND A_.attribute_value = 'True')) ");
                    builder.Append(name.ToString() + "' AND A_.attribute_value = 'free'))");
                }
            }
            return builder;
        }
        public StringBuilder AttachFoodCheckBoxes(List<CheckBox> filterFood)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < filterFood.Count; i++)
            {
                SchemaCategories.CategoriesFood food = (SchemaCategories.CategoriesFood)i;

                if (Convert.ToBoolean(filterFood[i].IsChecked))
                {
                    builder.Append(" AND business_id IN (SELECT B.business_id  FROM business AS B, attribute AS A_ "
                            + "WHERE B.business_id = A_.business_id AND (A_.attribute_name = '"); // ' AND A_.attribute_value = 'True')) ");
                    builder.Append(food.ToString() + "' AND A_.attribute_value = 'True')) ");
                }
            }
            return builder;
        }
        public StringBuilder AttachCategory(List<string> categories)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < categories.Count; i++)
            {
                builder.Append(" AND business_id IN (SELECT B.business_id  FROM business AS B, Categories AS C "
                            + "WHERE B.business_id = C.business_id AND (C.category_name = '"
                            + categories[i].ToString() + "' )) ");
            }
            return builder;
        }
    }
}
