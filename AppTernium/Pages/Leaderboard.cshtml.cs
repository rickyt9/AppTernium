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

namespace AppTernium.Pages {
    public class LeaderboardModel : PageModel {

        [BindProperty]
        public List<Score> ListScores { get; set; }
        private string ACCESS_TOKEN;
        public string selectedFilter { get; set; }
        public List<SelectListItem> Options { get; set; }
        public string test { get; set; }

        public async Task OnGetAsync() {

            this.Options = new List<SelectListItem> {
                    new SelectListItem { Text = "SEMANAL", Value = "1" },
                    new SelectListItem { Text = "MENSUAL", Value = "2" },
                    new SelectListItem { Text = "GLOBAL", Value = "3" },
                };
            selectedFilter = "1";


            ACCESS_TOKEN = HttpContext.Session.GetString("token");

            string responseContent = "[]";

            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts/scores");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL.ToString());

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
                ListScores = JsonConvert.DeserializeObject<List<Score>>(responseContent);
            } else {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }

        //public async Task<IActionResult> OnPost()
        public IActionResult OnPostMyMethod()
        {

            if (Options[0].Selected) selectedFilter = Options[0].Value;
            if (Options[1].Selected) selectedFilter = Options[1].Value;
            if (Options[2].Selected) selectedFilter = Options[2].Value;
            test = selectedFilter;
            return RedirectToPage("Leaderboard", new { result = selectedFilter });
        }

    }
}
