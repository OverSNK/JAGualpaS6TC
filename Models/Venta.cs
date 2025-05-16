using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAGualpaS6TC.Models
{
    public class Venta
    {
        public int Idventa { get; set; }
        public int Idcliente { get; set; }
        public int Idproducto { get; set; }
        public DateTime Fecha { get; set; }
        public required string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }

    }
}
