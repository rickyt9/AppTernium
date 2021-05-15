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

        private string ACCESS_TOKEN = "eyJhbGciOiJIUzI1NiJ9.VGVjRXF1aXBvMg.lCVu27kFHVurwY77sD7wWepNLuxyD7GrcK2sJd4KDg4";
        public async Task<IActionResult> OnGet() {
            string responseContent = "[]";

            // Buscamos el recurso
            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts/scores");

            // Creamos el cliente para que haga nuestra peticion
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL.ToString());

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToPage("Response", new { result = responseContent });
        }

    }
}
