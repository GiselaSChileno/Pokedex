using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos //metodo de conexión standart a dotos 
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector;} //esto me da la posibilidad de leer "lector" desde el exterior, le cree una propiedad
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server =.\\SQLEXPRESS; database=POKEDEX_DB; integrated security = true;");
            //al momento de creacion le estoy pasando por parametro la cadena de conexion sin necesidad de asignarlo después coo antes
            comando = new SqlCommand();

        }
        public void setearConsulta (string consulta) //encapsulamos la consulta
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {

            conexion.Open();    
            lector = comando.ExecuteReader();   

            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
            //coneccion de los @ con sus numeros
                
        }

        public void cerrarConexion()
        {
            if (lector != null) 
            {
            lector.Close();
            }
            conexion.Close();
        }
    }
}
