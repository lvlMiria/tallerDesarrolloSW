using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica;

namespace Presentacion
{
    class Program
    {
        static void Main(string[] args)
        {
            ContactoBLL cbll = new ContactoBLL();
            Persona persona1 = new Persona("Helena","Klein",12345678);
            Persona persona2 = new Persona("Juan Carlos", "Bodoque", 87654321);
            Persona persona3 = new Persona("Lucas", "Perez", 23456789);

            persona1.Grupo = Clasificacion.Familia;

            cbll.Agregar(persona1);
            cbll.Agregar(persona2);
            cbll.Agregar(persona3);

            Console.WriteLine("Todos los contactos");
            foreach (Persona per in cbll.Listar())
            {
                Console.WriteLine(per);
            }

            Console.WriteLine("\nGrupo Familia");
            foreach (Persona per in cbll.Listar(Clasificacion.Familia))
            {
                Console.WriteLine(per);
            }

            Console.ReadLine();
        }
    }
}
