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


namespace AppTernium.Pages {
    public class LeaderboardPracticaModel : PageModel {
        private const string DB_USER_ID = "root";
        private const string DB_PASSWORD = "12Junio1998";

        [BindProperty]
        public List<Score> ListaScore { get; set; }
        public string selectedFilter { get; set; }

        public void OnGet(string result) {
            ListaScore = GetScoresDB(result);
        }

        private List<Score> GetScoresDB(String result) {
            string connectionString = $"Server = 127.0.0.1; Port = 3306; Database = terniumbd; Uid = {DB_USER_ID}; password = {DB_PASSWORD};";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            string cmdTxt;

            if (result == null) {
                cmdTxt = "SELECT * FROM terniumbd.partida ORDER BY puntos desc;";
                selectedFilter = "G";
            } else {
                switch (result) {
                    case "S":
                        cmdTxt = @"SELECT * FROM terniumbd.partida
                                WHERE  YEARWEEK(`fecha`, 1) = YEARWEEK(CURDATE(), 1)
                                ORDER BY puntos desc;";
                        selectedFilter = "S";
                        break;
                    case "M":
                        cmdTxt = @"SELECT * FROM terniumbd.partida
                                WHERE YEAR(fecha) = YEAR(CURRENT_DATE)
                                AND MONTH(fecha) = MONTH(CURRENT_DATE)
                                ORDER BY puntos desc;";
                        selectedFilter = "M";
                        break;
                    case "G":
                        cmdTxt = "SELECT * FROM terniumbd.partida ORDER BY puntos desc;";
                        selectedFilter = "G";
                        break;
                    default:
                        cmdTxt = "SELECT * FROM terniumbd.partida ORDER BY puntos desc;";
                        selectedFilter = "G";
                        break;
                }
            }


            cmd.CommandText = cmdTxt;

            Score scr = new Score();
            List<Score> ListaS = new List<Score>();
            using (var reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    scr = new Score();
                    scr.username = reader["username"].ToString();
                    scr.score = Convert.ToInt32(reader["puntos"]);
                    ListaS.Add(scr);
                }
            }
            conexion.Dispose();
            return ListaS;
        }

        public IActionResult OnPostMyMethod() {
            selectedFilter = Request.Form["myDrpDown"];

            return RedirectToPage("LeaderboardPractica", new { result = selectedFilter });
        }
    }
}
