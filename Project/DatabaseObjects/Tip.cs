﻿namespace CPTS451_Milestone1
{
    public class Tip
    {
        public Tip()
        {
            this.BusinessID = "";
            this.UserID = "";
            this.Date = "";
            this.Likes = 0;
            this.Text = "";
            this.City = "";
            this.Yelping = "";
            this.UserName = "";
        }

        public string BusinessID { get; set; }
        public string UserID { get; set; }
        public string Date { get; set; }
        public int Likes { get; set; }
        public string Text { get; set; }
        public string City { get; set; }

        public string Yelping { get; set; }
        public string UserName { get; set; }
    }
}
