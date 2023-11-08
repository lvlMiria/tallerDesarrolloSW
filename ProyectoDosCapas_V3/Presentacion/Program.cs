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

            Persona persona1 = new Persona("Nicolas","Saavedra",12345678);
            Persona persona2 = new Persona("Juan", "Perez", 87654321);
            Persona persona3 = new Persona("Maria", "Toro", 789456123);

            persona1.Grupo = Clasificacion.Trabajo;
            persona3.Grupo = Clasificacion.Trabajo;

            cbll.Agregar(persona1);
            cbll.Agregar(persona2);
            cbll.Agregar(persona3);

            Console.WriteLine("Grupo Default(todos los contactos)");
            foreach (Persona per in cbll.Listar())
            {
                Console.WriteLine(per);
            }

            Console.WriteLine("\nContactos grupo Trabajo");
            foreach (Persona per in cbll.Listar(Clasificacion.Trabajo))
            {
                Console.WriteLine(per);
            }

            Console.ReadLine();
        }
    }
}
