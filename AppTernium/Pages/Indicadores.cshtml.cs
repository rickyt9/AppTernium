using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using AppTernium.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppTernium.Pages
{
    public class IndicadoresModel : PageModel
    {
        [BindProperty]
        public List<Score> ListScores { get; set; }
        private string ACCESS_TOKEN;

        public async Task OnGetAsync()
        {

            ACCESS_TOKEN = HttpContext.Session.GetString("token");

            string responseContent = "[]";
            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts");


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL);

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
                ListScores = JsonConvert.DeserializeObject<List<Score>>(responseContent);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }
    }
}
