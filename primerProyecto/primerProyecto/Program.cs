using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primerProyecto
{
    class Program
    {
        static void Main(string[] args)
        {
            int num1 = 1, num2 = 3;
            int suma, suma2, num6;
            bool esCSharp = true;
            float num3 = 3.5f;
            decimal num4 = 2.5m;
            string num5 = "14";

            suma = num1 + num2;
            //casteo y parse
            suma2 = num1 + num2 + (int)num3 + (int)num4 + int.Parse(num5);

            Console.WriteLine(num1 + " + "+num2+" = "+suma);
            Console.WriteLine(num1 + " + " + num2 + " + " + num3 + " + " + num4 + " + " + num5 + " = " + suma2);
            Console.WriteLine("Lenguaje C#: "+esCSharp);

            Console.Write("Ingresa valor de num6: ");
            //num6 = int.Parse(Console.ReadLine());
            //Devuelve 0 si sale mal
            int.TryParse(Console.ReadLine(), out num6);

            Console.WriteLine(num6);

            Console.ReadLine();

        }
    }
}
