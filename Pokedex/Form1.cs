using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using domini;
using Negocio;

namespace Pokedex
{
    public partial class Form1 : Form
    {
        private List<Pokemon> listaPokemon;
        public Form1()
        {
            InitializeComponent();
        }

        private void dgbPokemon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCampo.Items.Add("Número");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Descripción");
        }

        private void dgbPokemon_SelectionChanged(object sender, EventArgs e)
        {
            if(dgbPokemon.CurrentRow != null)
            {
            Pokemon seleccionado = (Pokemon)dgbPokemon.CurrentRow.DataBoundItem;//le pides que de como objeto el enlace
            cargarImagen(seleccionado.UrlImagen); //carga cada pokemon que selecciones
            }
        }

        private void cargar()
        {
            try
            {
                PokemonNogocio negocio = new PokemonNogocio();
                listaPokemon = negocio.listar(); //primero lo convierte en una lista
                dgbPokemon.DataSource = listaPokemon; // va a la base de datos y devuelve una lista de datos 
                                                      //datasource recibe daos y lo modela en la tabla
                ocultarColumnas();
                cargarImagen(listaPokemon[0].UrlImagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
                dgbPokemon.Columns["UrlImagen"].Visible = false; //Así se esconde una columna
                dgbPokemon.Columns["Id"].Visible = false;
        }
       
        private void cargarImagen(string imagen)//para capturar el error cada vez que pueda aparecer es que se crea un método de carga de la imagen
        {
            try
            {
            PbPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                PbPokemon.Load("C:\\Users\\s0412\\Downloads\\descarga.jpeg"); //carga imagen por defecto, puede estar en una carpate de drive o dropbox
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarPokemon alta = new AgregarPokemon();
            alta.ShowDialog();  //no te permite usar ambas ventanas, solo la nueva en la que estás show() te permite abrir las que sean
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgbPokemon.CurrentRow.DataBoundItem;

            AgregarPokemon modificar = new AgregarPokemon(seleccionado);
            modificar.ShowDialog();  //no te permite usar ambas ventanas, solo la nueva en la que estás show() te permite abrir las que sean
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar(bool logico = false)
        {
            PokemonNogocio negocio = new PokemonNogocio();
            Pokemon seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad deseas eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgbPokemon.CurrentRow.DataBoundItem;

                    if (logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);
                    }
                    else
                    {
                        negocio.eliminar(seleccionado.Id);
                    }
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgbPokemon.DataSource = null;
            dgbPokemon.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private bool validarFiltro()
        {
            if(cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar.");
                return true;   
            }
            if(cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar.");
                return true;
            }
            if(cbxCampo.SelectedItem.ToString() == "Número")
            {
                if(string.IsNullOrEmpty(txtFil.Text))
                {
                    MessageBox.Show("Debes cargar el filtro con un número.");
                        return true;
                }
                if (!(soloNumeros(txtFil.Text)))
                {
                    MessageBox.Show("Sólo números por favor");
                    return true;
                }
            }
            return false;   
        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if(!(char.IsNumber(caracter)))
                    return false;   
            }
            return true;
        }
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNogocio negocio = new PokemonNogocio();

            try
            {
                if(validarFiltro())
                    return;

                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();  
                string filtro = txtFil.Text;
                dgbPokemon.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();

            if(opcion == "Número")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor a");
                cbxCriterio.Items.Add("Menor a");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }
    }
    
}
