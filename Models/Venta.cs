using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAGualpaS6TC.Models
{
    public class Venta
    {
        public int Id_venta { get; set; }
        public int Id_cliente { get; set; }
        public int Id_producto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }

    }
}
