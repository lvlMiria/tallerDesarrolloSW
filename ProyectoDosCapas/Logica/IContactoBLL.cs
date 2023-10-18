using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public interface IContactoBLL
    {
        void Agregar(Persona per);
        List<Persona> Listar();
        List<Persona> Listar(Clasificacion grupo);
    }
}
