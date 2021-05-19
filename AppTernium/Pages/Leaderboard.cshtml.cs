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

namespace AppTernium.Pages {
    public class LeaderboardModel : PageModel {

        private string ACCESS_TOKEN;
        public JArray scores = JArray.Parse("[{\"username\": \"alberto\",\"score\": 570},{\"username\": \"borrar\",\"score\": 180},{\"username\": \"UsuarioCalidad\",\"score\": 120}]");

        public async void OnGet() {
            ACCESS_TOKEN = HttpContext.Session.GetString("token");

            string responseContent = "[]";

            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts/scores");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL.ToString());

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseContent);
                scores = JArray.Parse(responseContent);                
            } else {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
                scores = JArray.Parse("[{\"username\": \"alberto\",\"score\": 570},{\"username\": \"borrar\",\"score\": 180},{\"username\": \"UsuarioCalidad\",\"score\": 120}]");
            }
        }

    }
}
