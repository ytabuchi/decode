using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace decodeDemo.Models
{
    public class GetTemperature
    {
        /// <summary>
        /// 温度の json データを取得します。
        /// </summary>
        /// <param name="from">開始日</param>
        /// <param name="to">終了日</param>
        /// <returns></returns>
        public async Task<List<Temperature>> GetTemperatureAsync(DateTime from, DateTime to)
        {
            var uri = new Uri(
                string.Format(
                "http://azuretemperature.azurewebsites.net/api/temp?from={0}&to={1}", from, to));

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(uri); 
                    response.EnsureSuccessStatusCode(); // StatusCode が 200 以外なら Exception

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            var json = await streamReader.ReadToEndAsync();
                            var res = JsonConvert.DeserializeObject<List<Temperature>>(json);
                            return res;
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Error: {0}\n{1}", e.Message, e.InnerException);
#endif
                return null;
                //throw;
            }
            
        }

    }
}
