using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerolinea
{
    class Program
    {
        static void Main(string[] args)
        {
            Avion avion = new Avion(1234,Estado.Disponible,2013);
            Console.WriteLine(avion.Codigo);


            Console.ReadLine();
        }
    }
}
