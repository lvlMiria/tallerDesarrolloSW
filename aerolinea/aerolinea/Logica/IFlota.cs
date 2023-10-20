using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    interface IFlota
    {
        void Agregar(Avion avion);
        List<Avion> Listar();
        List<Avion> Listar(Estado estado);
    }
}
