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
using MySql.Data.MySqlClient;

namespace AppTernium.Pages {
    public class ProfileModel : PageModel {

        [BindProperty]
        public List<Attempt> ListAttempts { get; set; }
        public List<Medal> ListMedals { get; set; }
        public User user { get; set; }
        private string ACCESS_TOKEN;
        private string USERNAME;
        private string USERID;
        public int examCount;
        public int perfectCount;

        public async Task OnGetAsync()
        {
            ACCESS_TOKEN = HttpContext.Session.GetString("token");
            USERNAME = HttpContext.Session.GetString("username");
            USERID = HttpContext.Session.GetString("userId");

            string responseContent2 = "[]";
            Uri baseURL2 = new Uri($"https://chatarrap-api.herokuapp.com/users/{USERID}");
            HttpClient client2 = new HttpClient();
            client2.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);
            HttpResponseMessage response2 = await client2.GetAsync(baseURL2.ToString());
            responseContent2 = await response2.Content.ReadAsStringAsync();
            USERID=responseContent2;
            user = JsonConvert.DeserializeObject<User>(responseContent2);

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

                ListMedals = GetMedDB(USERNAME);

                /*
                perfectCount = 0;
                foreach (Attempt a in ListAttempts)
                {
                    if (a.correct == a.questions) perfectCount++;
                }
                */
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }

        private List<Medal> GetMedDB(string username)
        {
            string connectionString = "Server = 127.0.0.1; Port = 3306; Database = terniumbd; Uid = root; password = celestials;";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = $@"SELECT `terniumbd`.`medallausuario`.`idMedalla`, `terniumbd`.`tipomedalla`.`descripcion`, `terniumbd`.`categoriamedalla`.`categoria` 
FROM `terniumbd`.`medallausuario` 
join `terniumbd`.`tipomedalla` ON `terniumbd`.`medallausuario`.`idMedalla` = `terniumbd`.`tipomedalla`.`idMedalla` 
join `terniumbd`.`categoriamedalla` ON `terniumbd`.`tipomedalla`.`idCategoria` = `terniumbd`.`categoriamedalla`.`idCategoria`
Where `terniumbd`.`medallausuario`.`username` = @username";
            cmd.Parameters.AddWithValue("@username", username);

            Medal med = new Medal();
            ListMedals = new List<Medal>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    med = new Medal();
                    med.idMedalla = Convert.ToInt32(reader["idMedalla"]);
                    med.descripcion = reader["descripcion"].ToString();
                    med.categoria = reader["categoria"].ToString();
                    ListMedals.Add(med);
                }
            }
            conexion.Dispose();
            return ListMedals;
        }

    }
}
