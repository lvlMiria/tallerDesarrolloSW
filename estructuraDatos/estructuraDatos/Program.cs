using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estructuraDatos
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] dias = new string[5];  ARREGLO
            string[] dias = {"lunes","martes","miercoles","jueves","viernes"};

            Console.WriteLine("Con for\n");
            for (int i = 0; i < dias.Length; i++)
            {
                Console.WriteLine(dias[i]);
            }

            Console.WriteLine("\nCon foreach\n");
            foreach (string dia in dias)
            {
                Console.WriteLine(dia);
            }


            Console.WriteLine("\nMatriz\n");
            int[,] sala = new int[8, 10]; //[,] indica dos dim. Se rellena automatica% con 0.
            Console.WriteLine("Matriz de {0} x {1}\n",sala.GetLength(0), sala.GetLength(1));
            Console.WriteLine("Contenido posición [0,0]: "+sala[0,0]+"\n");

            Console.WriteLine("Recorriendo con doble for");
            for (int f = 0; f < sala.GetLength(0); f++)
            {
                for (int c = 0; c < sala.GetLength(1); c++)
                {
                    Console.Write(sala[f,c]+" ");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("\nRellendando con valores al azar");
            int[,] otra = new int[8, 10];
            Random rand = new Random();
            for (int f = 0; f < otra.GetLength(0); f++)
            {
                for (int c = 0; c < otra.GetLength(1); c++)
                {
                    otra[f, c] = rand.Next(0,2);
                    Console.Write(otra[f, c] + " ");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("\nListas\n");
            List<string> colores = new List<string>();

            colores.Add("Azul");
            colores.Add("Verde");
            colores.Add("Morado");

            colores.Insert(0,"Rojo"); //inserta en una posición

            Console.WriteLine("Recorriendo con foreach\n");
            foreach (string color in colores)
            {
                Console.WriteLine(color);
            }

            Console.WriteLine("Total de elementos en colores: "+colores.Count);

            Console.ReadLine();
        }
    }
}
