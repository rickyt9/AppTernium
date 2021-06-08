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
using MySql.Data.MySqlClient;


namespace AppTernium.Pages
{
    public class LeaderboardPracticaModel : PageModel
    {
        [BindProperty]
        public List<Score> ListaScore { get; set; }
        public string selectedFilter { get; set; }

        public void OnGet()
        {
            ListaScore = GetScoresDB();
            selectedFilter = "S";
        }

        private List<Score> GetScoresDB()
        {
            string connectionString = "Server = 127.0.0.1; Port = 3306; Database = terniumbd; Uid = root; password = celestials;";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * FROM terniumbd.partida ORDER BY puntos desc;";

            Score scr = new Score();
            List<Score> ListaS = new List<Score>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    scr = new Score();
                    scr.username = reader["username"].ToString();
                    scr.score = Convert.ToInt32(reader["puntos"]);
                    ListaS.Add(scr);
                }
            }
            conexion.Dispose();
            return ListaS;
        }
    }
}
