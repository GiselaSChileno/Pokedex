using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using domini;
using Negocio;
using System.Configuration;

namespace Pokedex
{
    public partial class AgregarPokemon : Form

    {
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;

        public AgregarPokemon()
        {
            InitializeComponent();
            Text = "Nuevo Pókemon";
        }
        public AgregarPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pókemon"; //Nombre de la ventana que se carga cuando usamos este constructor
        }

        private void lbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbAceptar_Click(object sender, EventArgs e)
        {
            
            PokemonNogocio negocio = new PokemonNogocio();  //siempre declarar el objeto

            try
            {
                if(pokemon == null)
                {
                    pokemon = new Pokemon();
                }
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                if(archivo != null && !(tbUrlImagen.Text.ToUpper().Contains("HTTP"))) 
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["PokeImagenes"] + archivo.SafeFileName);
                    pokemon.UrlImagen = ConfigurationManager.AppSettings["PokeImagenes"] + archivo.SafeFileName; //para que se guarde la ruta de la copia
                }
                else
                {
                    pokemon.UrlImagen = tbUrlImagen.Text;
                }
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem; // le estoy asegurando que es un elemento
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;

                if(pokemon.Id != 0 )
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                
                    negocio.agregar(pokemon);
                    MessageBox.Show("agregado exitosamente");

                }


                    Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AgregarPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            try
            {
                cboTipo.DataSource = elementoNegocio.Listar();
                //decirle al desplegable cual quiero que sea su clave y cual su value y despues decirle cual es el value seleccionado
                cboTipo.ValueMember = "Id"; //escondido
                cboTipo.DisplayMember = "Descripcion"; //son los nombres de la propiedades, lo que se muestra
                    //valor clave
                cboDebilidad.DataSource = elementoNegocio.Listar();
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";

                if ( pokemon != null )
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;    
                    txtDescripcion.Text = pokemon.Descripcion;
                    tbUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void tbUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(tbUrlImagen.Text);
        }

        private void cargarImagen(string imagen)//para capturar el error cada vez que pueda aparecer es que se crea un método de carga de la imagen
        {
            try
            {
                pbPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                pbPokemon.Load("C:\\Users\\s0412\\Downloads\\descarga.jpeg"); //carga imagen por defecto, puede estar en una carpate de drive o dropbox
            }
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg";

            if(archivo.ShowDialog() == DialogResult.OK)
            {
                tbUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
                
            }
        }
    }
}