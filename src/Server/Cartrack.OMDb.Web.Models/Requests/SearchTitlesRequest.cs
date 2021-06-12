﻿namespace Cartrack.OMDb.Web.Models.Requests
{
    public class SearchTitlesRequest
    {
        public string Title { get; set; }
        public int? Year { get; set; }

        public SearchTitlesRequest(string title, int? year)
        {
            Title = title;
            Year = year;
        }

        public override string ToString()
        {
            return Year.HasValue
                ? $"s={Title}&y={Year}"
                : $"s={Title}";
        }
    }
}