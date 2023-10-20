using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentacion
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matriz;

            int op = 0;
            while (op != 5)
            {
                Console.WriteLine("1. Construir Matriz\n2.Llenar Matriz con números al azar\n3.Imprimir Matriz\n4.Imprimir Matriz sin diagonales\n5.Salir");
                Console.Write("Ingrese su opción: ");
                op = int.Parse(Console.ReadLine());

                if (op==1)
                {
                    //1. Crear matriz
                    int dim1, dim2;
                    Console.Write("Valor primera dimensión: ");
                    dim1 = int.Parse(Console.ReadLine());
                    while (dim1 <= 0)
                    {
                        Console.WriteLine("ERROR. Debe ser mayor a 0");
                        Console.Write("Valor primera dimensión: ");
                        dim1 = int.Parse(Console.ReadLine());
                    }

                    Console.Write("Valor segunda dimensión: ");
                    dim2 = int.Parse(Console.ReadLine());
                    while (dim2 <= 0)
                    {
                        Console.WriteLine("ERROR. Debe ser mayor a 0");
                        Console.Write("Valor primera dimensión: ");
                        dim2 = int.Parse(Console.ReadLine());
                    }

                    matriz = new int[dim1, dim2];
                    Console.WriteLine("Matriz de {0} x {1} creada exitosamente.", dim1, dim2);
                    //Fin crear matriz
                }else if (op == 2)
                {
                    //2. Rellenar del 10 al 99
                    Random numero = new Random();
                    for (int i = 0; i < matriz.GetLength(0); i++)
                    {
                        for (int j = 0; j < matriz.GetLength(1); j++)
                        {
                            matriz[i, j] = numero.Next(10, 100);
                        }
                    }
                    //Fin rellenar
                }else if(op == 3)
                {
                    //3. Imprimir ordenada
                    for (int i = 0; i < matriz.GetLength(0); i++)
                    {
                        for (int j = 0; j < matriz.GetLength(1); j++)
                        {
                            Console.Write(matriz[i, j] + " ");
                        }
                        Console.WriteLine("");
                    }
                    //Fin imprimir
                }
                else if(op == 4)
                {
                    //4. Imprimir sin diagonales (¿Verificar que sea cuadrada?)
                    for (int i = 0; i < matriz.GetLength(0); i++)
                    {
                        for (int j = 0; j < matriz.GetLength(1); j++)
                        {
                            if (i == j || j == matriz.GetLength(0) - i - 1)
                            {
                                Console.Write("  ");
                            }
                            else
                            {
                                Console.Write(matriz[i, j] + " ");
                            }
                        }
                        Console.WriteLine("");
                    }
                    //Fin sin diagonales
                }
                else
                {
                    if (op != 5) {
                        Console.WriteLine("Opción inválida");
                    }
                }

            }

            

            

            

            


            Console.ReadLine();
        }
    }
}
