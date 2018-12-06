using System;

namespace TechChallenge.Models
{
    public class ForecastWeather
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TempC { get; set; }

        public int TempF { get; set; }

        public string Summary
        {
            get; set;
        }
    }
}
