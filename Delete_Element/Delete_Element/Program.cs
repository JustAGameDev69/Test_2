using System;

namespace delete_Element
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter array size: ");
            int n = int.Parse(Console.ReadLine());

            int[] arr = new int[n];

            Console.WriteLine("Enter array elements!");
            for (int i = 0; i < n; i++)
            {
                Console.Write("arr[{0}]: ", i);
                arr[i] = int.Parse(Console.ReadLine());
            }

            delete_Element(arr, n);

        }

        public static string delete_Element(int[] arr, int n)
        {

            Console.Write("Enter detele element position: ");
            int delete_pos = int.Parse(Console.ReadLine());

            int[] arr_New = new int[n - 1];

            for (int i = 0; i < n-1; i++)
            {
                if (delete_pos == i || delete_pos < i)
                {
                    arr_New[i] = arr[i + 1];
                }
                else if (delete_pos > i)
                {
                    arr_New[i] = arr[i];
                }
            }
            Console.Write("New array after delete : [ ");
            for (int j = 0; j < arr_New.Length; j++)
            {
                Console.Write(arr_New[j]);
                Console.Write(" ");
            }
            Console.Write("]");

            return "";
        }
    }
}