using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechChallenge.Models;

namespace TechChallenge.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        [HttpGet("[action]")]
        public IEnumerable<ForecastWeather> WeatherForecasts()
        {
            return GetAsync(CancellationToken.None).Result;
        }

        public async Task<List<ForecastWeather>> GetAsync(CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            cancellationToken.ThrowIfCancellationRequested();

            HttpResponseMessage response = await client.GetAsync("api/values", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ForecastWeather>>(stringResult);
            }

            return new List<ForecastWeather>();
        }
    }
}
