using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Flota
    {
        private static List<Avion> flota = new List<Avion>();
        public void Agregar(Avion avion)
        {
            flota.Add(avion);
        }

        public List<Avion> Listar()
        {
            return flota;
        }


    }
}
