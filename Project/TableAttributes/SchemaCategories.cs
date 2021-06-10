using System;
using System.Collections.Generic;
using System.Text;

namespace TableAttributes
{
    public static class SchemaCategories
    {
        public enum CategoriesName
        {
            BusinessAcceptsCreditCards,
            RestaurantsReservations,
            WheelchairAccessible,
            OutdoorSeating,
            GoodForKids,
            RestaurantsGoodForGroups,
            RestaurantsDelivery,
            RestaurantsTakeOut,
            WiFi,
            BikeParking
        }

        public enum CategoriesValue
        {
            True, 
            free
        }

        public enum CategoriesFood
        {
            breakfast,
            lunch,
            brunch,
            dinner,
            dessert,
            latenight
        }
    }
}
