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
using Logica;
namespace PresentacionWPF.Forms
{
    /// <summary>
    /// Lógica de interacción para UcNuevoContacto.xaml
    /// </summary>
    public partial class UcNuevoContacto : UserControl
    {
        public UcNuevoContacto()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DtFechaNacimiento.DisplayDateEnd = DateTime.Today;
            DtFechaNacimiento.SelectedDate = DateTime.Today;

            CboGrupo.ItemsSource = Enum.GetValues(typeof(Clasificacion));
            CboGrupo.SelectedItem = Clasificacion.Default;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Capturar datos del formulario
            string nombre = this.TxtNombre.Text.Trim();
            string apellido = this.TxtApellido.Text.Trim();
            string telefono = this.TxtTelefono.Text.Trim();
            DateTime fechaNacimiento = (DateTime)DtFechaNacimiento.SelectedDate;
            Clasificacion grupo = (Clasificacion)CboGrupo.SelectedItem;

            // validar datos
            List<string> errores = new List<string>();
            if(nombre == string.Empty)
            {
                errores.Add("Debe ingresar un nombre");
            }
            if (apellido == string.Empty)
            {
                errores.Add("Debe ingresar un apellido");
            }

            if(errores.Count == 0)
            {
                // Guardar datos en BLL
                Persona nuevaPersona = new Persona();
                nuevaPersona.Nombre = nombre;
                nuevaPersona.Apellido = apellido;
                nuevaPersona.Telefono = int.Parse(telefono);
                nuevaPersona.FechaNacimiento = fechaNacimiento;
                nuevaPersona.Grupo = grupo;

                ContactoBLL cbll = new ContactoBLL();
                cbll.Agregar(nuevaPersona);

                // Limpiar formulario
                this.TxtNombre.Text = string.Empty;
                this.TxtApellido.Text = string.Empty;
                this.TxtTelefono.Text = string.Empty;
                DtFechaNacimiento.SelectedDate = DateTime.Today;
                CboGrupo.SelectedItem = Clasificacion.Default;

                // Mensajes
                MessageBox.Show(nombre+" se agrego correctamente a los contactos", "GUARDADO EXITOSO", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //errores
                string msjError = string.Join("\n", errores);
                MessageBox.Show(msjError,"ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
            }




        }
    }
}
