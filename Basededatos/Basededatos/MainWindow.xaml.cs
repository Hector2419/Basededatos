using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace Basededatos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Trabajadores.mdb";
            MostrarDatos();
        }
        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from DT";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            turno.SelectedIndex = 0;
            txtTelefono.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (turno.Text != "Selecciona Turno")
                    {
                        cmd.CommandText = "insert into DT(Id,Nombre,Turno,Telefono) " + "Values(" + txtId.Text + ",'" + txtNombre.Text + ",'" + turno.Text + ",'" + txtTelefono.Text +"')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Trabajador agregado correctamente...");
                        LimpiaTodo();
                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el Turno....");
                    }
                }
                else
                {
                    cmd.CommandText = "update DT set Nombre='" + txtNombre.Text + "', Turno='" + turno.Text + "',Telefono=" + txtTelefono.Text  +  "' where Id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del Trabajador Actualizados...");
                    LimpiaTodo();
                }
                        MessageBox.Show("Favor de seleccionar el Turno....");
                    }
                }
                else
                {
                    cmd.CommandText = "update DT set Nombre='" + txtNombre.Text + "', Turno='" + turno.Text + "',Telefono=" + txtTelefono.Text  +  "
            }
            else
            {
                MessageBox.Show("Favor de poner el ID de un trabajador.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                turno.Text = row["Turno"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un trabajador de la lista...");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from DT where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Trabajador Eliminado correctamentamente...");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar a un trabajador de la lista...");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
