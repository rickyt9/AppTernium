using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTernium.Models
{
    public class Medal
    {
        public string fechaDeObtencion { get; set; }
        public int idTipo { get; set; }
        public string descripcion { get; set; }
        public string categoria { get; set; }
        public int idCategoria { get; set; }
    }
}
