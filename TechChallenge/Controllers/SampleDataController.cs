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
using TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException;
using TechChallenge.WebAPICaller.Retry;
using TechChallenge.WebAPICaller.Retry.RetryException;

namespace TechChallenge.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        [HttpGet("[action]")]
        public IEnumerable<object> WeatherForecasts()
        {
            try
            {
                return TryingGetValues().Result;
            }
            catch (Exception ex)
            {
                Exception currentException = ex;
                while (!(currentException is RetryAttemptNotAllowedException) && !(currentException is RetryLimitException) && currentException.InnerException != null)
                {
                    currentException = currentException.InnerException;
                }

                if (currentException is RetryAttemptNotAllowedException || currentException is RetryLimitException)
                {
                    throw new Exception("Service unavailable. Try again later.");
                }

                throw;
            }
        }

        private async Task<List<object>> TryingGetValues()
        {
            return await RetryHandler.Execute(GetAsync, 6, 1);
        }

        public async Task<List<object>> GetAsync()
        {           
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/values");
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return string.IsNullOrEmpty(stringResult) ? new List<object>() : JsonConvert.DeserializeObject<List<ForecastWeather>>(stringResult)
                    .Select(fw => (object)new
                    {
                        dateFormatted = fw.Date,
                        temperatureC = fw.TempC,
                        temperatureF = fw.TempF,
                        summary = fw.Summary
                    }).ToList();
            }

            throw new Exception("Fail in receive the response.");      
        }
    }
}
