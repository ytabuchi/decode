using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace decode01
{
    class Json
    {
        public static async Task<Root> GetJsonAsync()
        {
            using (var client = new HttpClient())
            {
                var str = await client.GetStringAsync("http://xmdemo1.azurewebsites.net/json/tempdata.json");
                var res = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Root>(str));
                return res;
            }
        }
    }

    /// <summary>
    /// Json から自動生成したクラスです。(http://xmdemo1.azurewebsites.net/json/tempdata.json)
    /// </summary>
    public class Root
    {
        public class TempData
        {
            public string date { get; set; }
            public List<int> temp { get; set; }
        }
        public List<TempData> tempdata { get; set; }
    }
}
