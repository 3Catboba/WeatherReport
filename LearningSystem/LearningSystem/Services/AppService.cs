using LearningSystem.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services
{
    public static class AppService
    {

        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&units=metric&appid=115755479fb967da01cccef7ca283966", latitude, longitude));
            return JsonConvert.DeserializeObject<Root>(response);
        }


        public static async Task<Root> GetWeatherByCity(string city)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&appid=115755479fb967da01cccef7ca283966", city));
            return JsonConvert.DeserializeObject<Root>(response);
        }
    }
}