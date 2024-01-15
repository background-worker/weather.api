using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherManager.Models
{
    public class GetWeatherResponse
    {
        public IEnumerable<WeatherDetail> Weather { get; set; }
    }

    public class WeatherDetail
    {
        public string Description { get; set; }
    }
}
