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

namespace AppTernium.Pages {
    public class ProfileModel : PageModel {

        [BindProperty]
        public List<Attempt> ListAttempts { get; set; }
        private string ACCESS_TOKEN;
        private string USERNAME;
        public int examCount;
        public int perfectCount;

        public async Task OnGetAsync()
        {
            ACCESS_TOKEN = HttpContext.Session.GetString("token");
            //USERNAME = HttpContext.Session.GetString("username");
            USERNAME = "alberto";

            string responseContent = "[]";

            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL.ToString());

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
                ListAttempts = JsonConvert.DeserializeObject<List<Attempt>>(responseContent);
                examCount = 0;
                foreach (Attempt a in ListAttempts)
                {
                    if (a.username != null && a.username == USERNAME) examCount++;
                }
                perfectCount = 0;
                foreach (Attempt a in ListAttempts)
                {
                    if (a.correct == a.questions) perfectCount++;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }
    }
}
