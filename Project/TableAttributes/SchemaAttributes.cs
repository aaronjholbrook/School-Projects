using System;
using System.Collections.Generic;
using System.Text;

namespace TableAttributes
{
    public static class SchemaAttributes
    {
        public enum Users
        {
            user_id,
            name_,
            average_stars,
            fans,
            cool,
            tipcount,
            funny,
            totallikes,
            useful,
            user_latitude,
            user_longitude,
            yelping_since
        }

        public enum Business
        {
            business_id,
            name_,
            address,
            city,
            state_,
            postal_code,
            latitude_business,
            longitude_business,
            stars,
            reviewcount,
            numcheckins,
            numtips,
            is_open
        }

        public enum Attribute
        {
            business_id,
            attribute_name,
            attribute_value
        }

        public enum Categories
        {
            business_id, 
            category_name
        }

        public enum Checkin
        {
            business_id,
            year_,
            month_,
            day_,
            time_
        }

        public enum Friend
        {
            user_id,
            friend_id
        }

        public enum Hours
        {
            business_id,
            dayofweek,
            close_,
            open_
        }

        public enum Tip
        {
            business_id,
            user_id,
            date_,
            likes_,
            text_
        }
    }
}
