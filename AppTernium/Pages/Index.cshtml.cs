using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using AppTernium.Models;

namespace AppTernium.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public void OnGet() {

        }

        [BindProperty]
        public Login Usuario { get; set; }
        public string Mensaje { get; set; }

        public async Task<IActionResult> OnPost() {
            string responseContent = "[]";

            // Buscamos el recurso
            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/users/login");

            // Creamos el cliente para que haga nuestra peticion
            HttpClient client = new HttpClient();

            // Armamos nuestra peticion
            JObject joPeticion = new JObject();
            joPeticion.Add("username", Usuario.username);
            joPeticion.Add("password", Usuario.password);

            var stringContent = new StringContent(joPeticion.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseURL.ToString(), stringContent);

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToPage("Response", new { result = responseContent });
        }
    }
}
