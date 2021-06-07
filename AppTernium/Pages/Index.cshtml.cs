using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using AppTernium.Models;
using MySql.Data.MySqlClient;


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

            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/users/login");
            HttpClient client = new HttpClient();

            JObject joPeticion = new JObject();
            joPeticion.Add("username", Usuario.username);
            joPeticion.Add("password", Usuario.password);

            var stringContent = new StringContent(joPeticion.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseURL.ToString(), stringContent);

            if (response.IsSuccessStatusCode) {
                responseContent = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(responseContent);

                // InsertUserLogToDb(Usuario);

                HttpContext.Session.SetString("username", Usuario.username);
                HttpContext.Session.SetString("token", result.Value<string>("token"));
                HttpContext.Session.SetString("userId", result.Value<string>("user"));

                return RedirectToPage("Leaderboard", new { result = responseContent });
            }

            return Page();
        }

        private void InsertUserLogToDb(Login user) {
            // Insertar en base de datos
            string connectionString = "Server=127.0.0.1;Port=3306;Database=terniumbd;Uid=root;password=celestials;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "INSERT INTO `terniumbd`.`registros` (`username`,`fecha`) VALUES (@username,now());";
            using var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@username", user.username);
            cmd.Parameters.AddWithValue("@fechaEntrada", DateTime.Now.Date);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }
    }
}
