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
using System.IO;

namespace AppTernium.Pages {
    public class ProfileModel : PageModel {

        [BindProperty]
        public List<Attempt> ListAttempts { get; set; }
        public List<Medal> ListMedals { get; set; }
        public List<Medal> ListAllMedals { get; set; }
        public User user { get; set; }
        private string ACCESS_TOKEN;
        private string USERNAME;
        private string USERID;
        public int examCount;
        public int perfectCount;
        public string testing;

        public async Task OnGetAsync()
        {
            ACCESS_TOKEN = HttpContext.Session.GetString("token");
            USERNAME = HttpContext.Session.GetString("username");
            //USERNAME = "alberto";
            USERID = HttpContext.Session.GetString("userId");

            string responseContent2 = "[]";
            Uri baseURL2 = new Uri($"https://chatarrap-api.herokuapp.com/users/{USERID}");
            HttpClient client2 = new HttpClient();
            client2.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);
            HttpResponseMessage response2 = await client2.GetAsync(baseURL2.ToString());
            responseContent2 = await response2.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<User>(responseContent2);

            string responseContent = "[]";

            Uri baseURL = new Uri("https://chatarrap-api.herokuapp.com/attempts");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth_key", ACCESS_TOKEN);

            HttpResponseMessage response = await client.GetAsync(baseURL.ToString());

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
                List<Attempt> ListA = JsonConvert.DeserializeObject<List<Attempt>>(responseContent);
                ListAttempts = new List<Attempt>();
                foreach (Attempt a in ListA)
                {
                    if (a.username != null && a.username == USERNAME) ListAttempts.Add(a);
                }

                ListMedals = GetMedDB(USERNAME);
                ListAllMedals = GetAllMedals();

                if (ListAttempts != null) CheckForMedals();

                


            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.ReasonPhrase);
            }
        }

        private List<Medal> GetAllMedals()
        {
            string connectionString = "Server = 127.0.0.1; Port = 3306; Database = terniumbd; Uid = root; password = celestials;";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = @"SELECT `terniumbd`.`medallasdisponibles`.`idTipo`, `terniumbd`.`medallasdisponibles`.`descripcion`, `terniumbd`.`categoriamedalla`.`categoria`, `terniumbd`.`medallasdisponibles`.`idCategoria`
FROM `terniumbd`.`medallasdisponibles` 
join `terniumbd`.`categoriamedalla` ON `terniumbd`.`medallasdisponibles`.`idCategoria` = `terniumbd`.`categoriamedalla`.`idCategoria`;";

            Medal med = new Medal();
            List<Medal> ListM = new List<Medal>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    med = new Medal();
                    med.idTipo = Convert.ToInt32(reader["idTipo"]);
                    med.descripcion = reader["descripcion"].ToString();
                    med.categoria = reader["categoria"].ToString();
                    med.idCategoria = Convert.ToInt32(reader["idCategoria"]);
                    ListM.Add(med);
                }
            }
            conexion.Dispose();
            return ListM;
        }

        private List<Medal> GetMedDB(string username)
        {
            string connectionString = "Server = 127.0.0.1; Port = 3306; Database = terniumbd; Uid = root; password = celestials;";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = $@"SELECT `terniumbd`.`medallausuario`.`fechaDeObtencion`, `terniumbd`.`medallausuario`.`idTipo`, `terniumbd`.`tipomedalla`.`descripcion`, `terniumbd`.`categoriamedalla`.`categoria` 
FROM `terniumbd`.`medallausuario` 
join `terniumbd`.`tipomedalla` ON `terniumbd`.`medallausuario`.`idTipo` = `terniumbd`.`tipomedalla`.`idTipo` 
join `terniumbd`.`categoriamedalla` ON `terniumbd`.`tipomedalla`.`idCategoria` = `terniumbd`.`categoriamedalla`.`idCategoria`
Where `terniumbd`.`medallausuario`.`username` = @username;";
            cmd.Parameters.AddWithValue("@username", username);

            Medal med = new Medal();
            List<Medal> ListM = new List<Medal>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    med = new Medal();
                    med.fechaDeObtencion = reader["fechaDeObtencion"].ToString();
                    med.idTipo = Convert.ToInt32(reader["idTipo"]);
                    med.descripcion = reader["descripcion"].ToString();
                    med.categoria = reader["categoria"].ToString();
                    ListM.Add(med);
                }
            }
            conexion.Dispose();
            return ListM;
        }

        private void CheckForMedals()
        {
            examCount = ListAttempts.Count;

            perfectCount = 0;
            foreach (Attempt a in ListAttempts)
            {
                if (a.correct == a.questions) perfectCount++;
            }

            List<int> MedalsDB = new List<int>();
            if (examCount >= 5) MedalsDB.Add(1);
            if (examCount >= 10) MedalsDB.Add(2);
            if (examCount >= 50) MedalsDB.Add(3);
            if (examCount >= 100) MedalsDB.Add(4);
            if (perfectCount >= 5) MedalsDB.Add(5);
            if (perfectCount >= 10) MedalsDB.Add(6);
            if (perfectCount >= 50) MedalsDB.Add(7);
            if (perfectCount >= 100) MedalsDB.Add(8);

            InsertMedalsDB(MedalsDB);

        }

        private void InsertMedalsDB(List<int> MedalsDB)
        {
            if (MedalsDB != null)
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=terniumbd;Uid=root;password=celestials;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //string sql = "INSERT INTO `terniumbd`.`registros` (`username`,`fecha`) VALUES (@username,now());";
                string sql = "INSERT INTO `terniumbd`.`medallausuario` (`username`, `fechaDeObtencion`,`idTipo`) VALUES (@username, now(), @tipo);";
                using var cmd = new MySqlCommand(sql, connection);

                foreach (int m in MedalsDB)
                {
                    if (ListMedals != null && !ListMedals.Exists(x => x.idTipo == m))
                    {
                        cmd.Parameters.AddWithValue("@username", USERNAME);
                        cmd.Parameters.AddWithValue("@tipo", m);

                        cmd.Prepare();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
