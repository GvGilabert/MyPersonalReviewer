using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyPersonalReviewer.Models;
using Newtonsoft.Json;
using static MyPersonalReviewer.Models.GeolocatorApiReplyModel;

namespace MyPersonalReviewer.Services
{
    public class MapApiService
    {
        private readonly IConfiguration _config;
        public MapApiService(IConfiguration config)
        {
            _config = config;
        }
    public async Task<info> GeolocationServiceAsync(string address)
    {
            var client = new HttpClient();
            var url = new System.Uri(_config.GetSection("GeoLocationApi").GetSection("GeocodingURL").Value+
                                     address +
                                     _config.GetSection("GeoLocationApi").GetSection("UrlEndForSearch").Value);
            var response = await client.GetAsync(url);
            string json;

            using (var content = response.Content)
            json = await content.ReadAsStringAsync();
            info addresses = JsonConvert.DeserializeObject<info>(json);
            return addresses;
        }
    }
}