using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domini
{
    public class Pokemon //tiene que ser public para que sea vista desde otro lugar
    {
        //clase que define el objeto que voy a utilizar

        public int Id { get; set; } 
        [DisplayName("Número")]  //sirve para agregar fechas, tíldes y detalles como espacios a los nombres de la columna
        public int Numero {  get; set; }  
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; } 
        public string UrlImagen { get; set; }  
        public Elemento Tipo { get; set; }
        public Elemento Debilidad { get; set; }

    }
}
