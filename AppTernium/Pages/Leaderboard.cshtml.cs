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

        public async Task OnGetAsync(string result) {

            ACCESS_TOKEN = HttpContext.Session.GetString("token");

            string responseContent = "[]";
            string url;

            if (result == null) {
                url = "https://chatarrap-api.herokuapp.com/attempts/scoresWeek";
            } else {
                switch (result) {
                    case "S":
                        url = "https://chatarrap-api.herokuapp.com/attempts/scoresWeek";
                        break;
                    case "M":
                        url = "https://chatarrap-api.herokuapp.com/attempts/scores";
                        break;
                    case "G":
                        url = "https://chatarrap-api.herokuapp.com/attempts";
                        break;
                    default:
                        url = "https://chatarrap-api.herokuapp.com/attempts/scoresWeek/";
                        break;
                }
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
                ListScores = JsonConvert.DeserializeObject<List<Score>>(responseContent);
            } else {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }

        public IActionResult OnPostMyMethod() {
            selectedFilter = Request.Form["myDrpDown"];

            return RedirectToPage("Leaderboard", new { result = selectedFilter });
        }

    }
}
