using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSIPBIDemo.Models;
using FSIPBIDemo.Services;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Shyjus;


namespace FSIPBIDemo.Controllers
{
    public class current
    {
        public DateTime time { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public int air_temperature { get; set; }
        public decimal sea_level_pressure { get; set; }
        public decimal station_pressure { get; set; }
        public string pressure_trend { get; set; }
        public int relative_humidity { get; set; }
        public int wind_avg { get; set; }
        public int wind_direction { get; set; }
        public string wind_direction_cardinal { get; set; }
        public string wind_gust { get; set; }
        public int solar_radiation { get; set; }
        public int uv { get; set; }
        public string brightness { get; set; }
        public int feels_like { get; set; }
        public int lightning_strike_count_last_1hr { get; set; }
        public int lightning_strike_count_last_3hr { get; set; }
        public string lightning_strike_last_distance { get; set; }
        public string lightning_strike_last_distance_msg { get; set; }
        public DateTime lightning_strike_last_epoch { get; set; }
        public decimal precip_accum_local_day { get; set; }
        public decimal precip_accum_local_yesterday { get; set; }
        public int precip_minutes_local_day { get; set; }
        public int precip_minutes_local_yesterday { get; set; }
        public string is_precip_local_day_rain_check { get; set; }
        public string is_precip_local_yesterday_rain_check { get; set; }
    }
    public static class HttpContextServerVariableExtensions { }

    [Authorize]
    public class HomeController : Controller
    {

        private PowerBiServiceApi powerBiServiceApi;
        public HomeController(PowerBiServiceApi powerBiServiceApi)
        {
            this.powerBiServiceApi = powerBiServiceApi;
        }

        [AllowAnonymous]


        public async Task<ActionResult> Index()
        {
            string Url = "https://swd.weatherflow.com/swd/rest/better_forecast?station_id=54207&units_temp=f&units_wind=mph&units_pressure=mmhg&units_precip=in&units_distance=mi&token=a307d230-391e-4b32-a807-80b15072982e";
            //var url = "https://api.weather.gov/alerts/active?zone=TXC121";  // Pull only Active Alerts for Denton County
            var url = "https://api.weather.gov/alerts?zone=TXC121"; // Pull both Active and InActive Alerts for Denton County
            //var url = "https://api.weather.gov/alerts/active?area=TX";  // Alerts from across Texas... for testing
            var httpRequest2 = (HttpWebRequest)WebRequest.Create(url);
            string area = "";
            string warning = "";
            bool go = true;
            DateTime onset;
            DateTime expires;
            DateTime ends;
            string severity;
            string evnt;
            bool wrnwtch = false;
            int i = 1;





            DateTime dt3 = new DateTime(2022, 12, 13, 10, 45, 20);
            var httpRequest = (HttpWebRequest)WebRequest.Create(Url);

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            string br = httpRequest.Headers["User-Agent"];

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JToken token1 = JToken.Parse(result);
                JObject obs = (JObject)token1.SelectToken("current_conditions");

                //DateTime etime = Convert.ToDateTime(Epoch(Convert.ToString(obs["time"])));

                current record = new current
                {
                    time = DateTime.Now,
                    conditions = Convert.ToString(obs["conditions"]),
                    icon = Convert.ToString(obs["icon"]),
                    air_temperature = Convert.ToInt16(obs["air_temperature"]),
                    sea_level_pressure = Convert.ToDecimal(obs["sea_level_pressure"]),
                    station_pressure = Convert.ToDecimal(obs["station_pressure"]),
                    pressure_trend = Convert.ToString(obs["pressure_trend"]),
                    relative_humidity = Convert.ToInt16(obs["relative_humidity"]),
                    wind_avg = Convert.ToInt16(obs["wind_avg"]),
                    wind_direction = Convert.ToInt16(obs["wind_direction"]),
                    wind_direction_cardinal = Convert.ToString(obs["wind_direction_cardinal"]),
                    wind_gust = Convert.ToString(obs["wind_gust"]),
                    solar_radiation = Convert.ToInt16(obs["solar_radiation"]),
                    uv = Convert.ToInt16(obs["uv"]),
                    brightness = Convert.ToString(obs["brightness"]),
                    feels_like = Convert.ToInt16(obs["feels_like"]),
                    lightning_strike_count_last_1hr = Convert.ToInt16(obs["lightning_strike_count_last_1hr"]),
                    lightning_strike_count_last_3hr = Convert.ToInt16(obs["lightning_strike_count_last_3hr"]),
                    lightning_strike_last_distance = Convert.ToString(obs["lightning_strike_last_distance"]),
                    lightning_strike_last_distance_msg = Convert.ToString(obs["lightning_strike_last_distance_msg"]),
                    lightning_strike_last_epoch = DateTime.Now,
                    //lightning_strike_last_epoch = Convert.ToDateTime(Epoch(Convert.ToString(obs["lightning_strike_last_epoch"]))),
                    precip_accum_local_day = Convert.ToDecimal(obs["precip_accum_local_day"]),
                    precip_accum_local_yesterday = Convert.ToDecimal(obs["precip_accum_local_yesterday"]),
                    precip_minutes_local_day = Convert.ToInt16(obs["precip_minutes_local_day"]),
                    precip_minutes_local_yesterday = Convert.ToInt16(obs["precip_minutes_local_yesterday"]),
                    is_precip_local_day_rain_check = Convert.ToString(obs["is_precip_local_day_rain_check"]),
                    is_precip_local_yesterday_rain_check = Convert.ToString(obs["is_precip_local_yesterday_rain_check"])

                };

                //   var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                // General Weather Info
                ViewBag.Temp = Convert.ToString(record.air_temperature);
                ViewBag.Hum = Convert.ToString(record.relative_humidity);
                ViewBag.Feel = Convert.ToString(record.feels_like);
                ViewBag.Rad = Convert.ToString(record.solar_radiation);

                // Other Weather
                ViewBag.Pres = Convert.ToString(record.station_pressure);
                ViewBag.Trnd = Convert.ToString(record.pressure_trend);
                ViewBag.UV = Convert.ToString(record.uv);
                ViewBag.Bright = Convert.ToString(record.brightness);
                ViewBag.Wind = Convert.ToString(record.wind_avg);
                ViewBag.WindD = Convert.ToString(record.wind_direction);
                ViewBag.WindowDC = Convert.ToString(record.wind_direction_cardinal);
                ViewBag.Gust = Convert.ToString(record.wind_gust);

                //Rain Info                
                //ViewBag.Rain = Convert.ToString(record.precip_accum_local_day);
                ViewBag.Rain = record.precip_accum_local_day;
                ViewBag.RainY = record.precip_accum_local_yesterday;
                ViewBag.RainPM = Convert.ToString(record.precip_minutes_local_day);
                ViewBag.RainPMY = Convert.ToString(record.precip_minutes_local_yesterday);
                ViewBag.LgtCnt1hr = Convert.ToString(record.lightning_strike_count_last_1hr);
                ViewBag.LgtCnt3hr = Convert.ToString(record.lightning_strike_count_last_3hr);
                ViewBag.LgtDst = Convert.ToString(record.lightning_strike_last_distance);
                ViewBag.LgtDstm = Convert.ToString(record.lightning_strike_last_distance_msg);

                //return View();

            }
            try
            {
                httpRequest2.Accept = "application/json";
                httpRequest2.UserAgent = "lantanaweather.com, markm@msn.com";


                var httpResponse2 = (HttpWebResponse)httpRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JToken token1 = JToken.Parse(result);

                    ViewBag.result = result;

                    if (Convert.ToString(token1.SelectToken("features")).Length > 2)
                    {
                        ViewBag.go = go;
                        i = 0;
                        //while (go)
                        //{
                        //area = Convert.ToString(token1.SelectToken("features[" + 1 + "].properties.areaDesc"));
                        //warning = Convert.ToString(token1.SelectToken("features[" + i + "].properties.headline"));
                        //evnt = Convert.ToString(token1.SelectToken("features[" + i + "].properties.event"));
                        //severity = Convert.ToString(token1.SelectToken("features[" + i + "].properties.severity"));

                        //try
                        //{
                        //    onset = Convert.ToDateTime(token1.SelectToken("features[" + i + "].properties.onset"));
                        //    expires = Convert.ToDateTime(token1.SelectToken("features[" + i + "].properties.expires"));
                        //    ends = Convert.ToDateTime(token1.SelectToken("features[" + i + "].properties.ends"));
                        //}
                        //catch {
                        //    onset = DateTime.Now;
                        //    expires = DateTime.Now;
                        //    ends = DateTime.Now;
                        //}

                        area = Convert.ToString(token1.SelectToken("features[0].properties.areaDesc"));
                        warning = Convert.ToString(token1.SelectToken("features[0].properties.headline"));
                        evnt = Convert.ToString(token1.SelectToken("features[0].properties.event"));
                        severity = Convert.ToString(token1.SelectToken("features[0].properties.severity"));

                        try
                        {
                            onset = Convert.ToDateTime(token1.SelectToken("features[0].properties.onset"));
                            expires = Convert.ToDateTime(token1.SelectToken("features[0].properties.expires"));
                            ends = Convert.ToDateTime(token1.SelectToken("features[0].properties.ends"));
                        }
                        catch
                        {
                            onset = DateTime.Now;
                            expires = DateTime.Now;
                            ends = DateTime.Now;
                        }





                        wrnwtch = warning.Contains("Watch") | warning.Contains("Warning");
                        ViewBag.warn = wrnwtch;

                        //if (dt3 > onset & dt3 < ends)
                        //if (DateTime.Now > onset & DateTime.Now < ends)
                        //{
                        ViewBag.evnt = evnt;
                        ViewBag.warning = warning;
                        ViewBag.counties = area;
                        //}

                        if (area.Length < 4)
                        {
                            go = false;
                        }
                        i++;

                        //string b = Shyjus.BrowserDetection;
                        //ViewBag.browser = bh;


                        ViewBag.browser = br;


                        //ViewBag.browser = <script> navigator.userAgent </script>;

                        //}
                    }

                }
            }
            catch { };
            return View();
        }
        /*
        public async Task<IActionResult> Embed() { 
 
        // replace these two GUIDs with the workspace ID and report ID you recorded earlier 
        Guid workspaceId = new Guid("2d038cde-39d8-405d-a57a-72eacf4d19b3"); 
        Guid reportId = new Guid("3b5ed814-fd17-4d30-8732-289a396aaa11"); 
 
        var viewModel = await powerBiServiceApi.GetReport(workspaceId, reportId); 
 
        return View(viewModel); 
        } 
        */

        [AllowAnonymous]
        public async Task<IActionResult> Demo(string workspaceId)
        {

            try
            {
                Guid guidTest = new Guid(workspaceId);
                var viewModel = await this.powerBiServiceApi.GetEmbeddedViewModel(workspaceId);
                return View(viewModel as object);
            }
            catch
            {
                var firstWorkspace = await this.powerBiServiceApi.GetFirstWorkspace();
                if (firstWorkspace == null)
                {
                    return RedirectToPage("/Error");
                }
                else
                {
                    return RedirectToPage("/Demo", null, new { workspaceId = firstWorkspace.Id });
                }
            }
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult More()
        {
            string Url = "https://swd.weatherflow.com/swd/rest/better_forecast?station_id=<Your Station ID Goes Here>&units_temp=f&units_wind=mph&units_pressure=mmhg&units_precip=in&units_distance=mi&token=<Your Token Goes Here>";

            var httpRequest = (HttpWebRequest)WebRequest.Create(Url);

            httpRequest.Accept = "application/json";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JToken token1 = JToken.Parse(result);
                JObject obs = (JObject)token1.SelectToken("current_conditions");

                //DateTime etime = Convert.ToDateTime(Epoch(Convert.ToString(obs["time"])));

                current record = new current
                {
                    time = DateTime.Now,
                    conditions = Convert.ToString(obs["conditions"]),
                    icon = Convert.ToString(obs["icon"]),
                    air_temperature = Convert.ToInt16(obs["air_temperature"]),
                    sea_level_pressure = Convert.ToDecimal(obs["sea_level_pressure"]),
                    station_pressure = Convert.ToDecimal(obs["station_pressure"]),
                    pressure_trend = Convert.ToString(obs["pressure_trend"]),
                    relative_humidity = Convert.ToInt16(obs["relative_humidity"]),
                    wind_avg = Convert.ToInt16(obs["wind_avg"]),
                    wind_direction = Convert.ToInt16(obs["wind_direction"]),
                    wind_direction_cardinal = Convert.ToString(obs["wind_direction_cardinal"]),
                    wind_gust = Convert.ToString(obs["wind_gust"]),
                    solar_radiation = Convert.ToInt16(obs["solar_radiation"]),
                    uv = Convert.ToInt16(obs["uv"]),
                    brightness = Convert.ToString(obs["brightness"]),
                    feels_like = Convert.ToInt16(obs["feels_like"]),
                    lightning_strike_count_last_1hr = Convert.ToInt16(obs["lightning_strike_count_last_1hr"]),
                    lightning_strike_count_last_3hr = Convert.ToInt16(obs["lightning_strike_count_last_3hr"]),
                    lightning_strike_last_distance = Convert.ToString(obs["lightning_strike_last_distance"]),
                    lightning_strike_last_distance_msg = Convert.ToString(obs["lightning_strike_last_distance_msg"]),
                    lightning_strike_last_epoch = DateTime.Now,
                    //lightning_strike_last_epoch = Convert.ToDateTime(Epoch(Convert.ToString(obs["lightning_strike_last_epoch"]))),
                    precip_accum_local_day = Convert.ToDecimal(obs["precip_accum_local_day"]),
                    precip_accum_local_yesterday = Convert.ToDecimal(obs["precip_accum_local_yesterday"]),
                    precip_minutes_local_day = Convert.ToInt16(obs["precip_minutes_local_day"]),
                    precip_minutes_local_yesterday = Convert.ToInt16(obs["precip_minutes_local_yesterday"]),
                    is_precip_local_day_rain_check = Convert.ToString(obs["is_precip_local_day_rain_check"]),
                    is_precip_local_yesterday_rain_check = Convert.ToString(obs["is_precip_local_yesterday_rain_check"])

                };

                //   var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                // General Weather Info
                ViewBag.Temp = Convert.ToString(record.air_temperature);
                ViewBag.Hum = Convert.ToString(record.relative_humidity);
                ViewBag.Feel = Convert.ToString(record.feels_like);
                ViewBag.Rad = Convert.ToString(record.solar_radiation);

                // Other Weather
                ViewBag.Pres = Convert.ToString(record.station_pressure);
                ViewBag.Trnd = Convert.ToString(record.pressure_trend);
                ViewBag.UV = Convert.ToString(record.uv);
                ViewBag.Bright = Convert.ToString(record.brightness);
                ViewBag.Wind = Convert.ToString(record.wind_avg);
                ViewBag.WindD = Convert.ToString(record.wind_direction);
                ViewBag.WindowDC = Convert.ToString(record.wind_direction_cardinal);
                ViewBag.Gust = Convert.ToString(record.wind_gust);

                //Rain Info                
                ViewBag.Rain = Convert.ToString(record.precip_accum_local_day);
                ViewBag.RainY = Convert.ToString(record.precip_accum_local_yesterday);
                ViewBag.RainPM = Convert.ToString(record.precip_minutes_local_day);
                ViewBag.RainPMY = Convert.ToString(record.precip_minutes_local_yesterday);
                ViewBag.LgtCnt1hr = Convert.ToString(record.lightning_strike_count_last_1hr);
                ViewBag.LgtCnt3hr = Convert.ToString(record.lightning_strike_count_last_3hr);
                ViewBag.LgtDst = Convert.ToString(record.lightning_strike_last_distance);
                ViewBag.LgtDstm = Convert.ToString(record.lightning_strike_last_distance_msg);

                return View();

            }
        }



        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






    }
}

