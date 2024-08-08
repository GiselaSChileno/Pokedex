using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio

{
    public class Elemento
    {
        public int Id {  get; set; }    
        public string Descripcion { get; set; }
        public override string ToString() 
            //sobreescribes el metodo to string para que cuando por defecto convierta el objeto en un string
            //no sea su direccion sino lo que quieras ver
        {
            return Descripcion;
        }
    }
}
