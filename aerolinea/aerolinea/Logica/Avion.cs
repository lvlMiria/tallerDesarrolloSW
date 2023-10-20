using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public enum Estado
    {
        Disponible,
        Reparando,
        Retirado
    }
    public class Avion
    {
        //Código, estado (Disponible, Reparando, Retirado) y año de fabricación

        private int _codigo;
        private Estado _estado;
        private int _anoFabricacion;

        public int Codigo { get => _codigo; set => _codigo = value; }
        public Estado Estado { get => _estado; set => _estado = value; }
        public int AnoFabricacion { get => _anoFabricacion; set => _anoFabricacion = value; }

        public Avion()
        {
               
        }

        public Avion(int codigo, Estado estado, int anoFabricacion)
        {
            Codigo = codigo;
            Estado = estado;
            AnoFabricacion = anoFabricacion;
        }

        public override string ToString()
        {
            return Codigo + "|" + Estado + "|" + AnoFabricacion;
        }

    }
}
