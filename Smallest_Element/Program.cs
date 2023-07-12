using System;

namespace smallest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter array size: ");
            int n = int.Parse(Console.ReadLine());

            int[] arr = new int[n];


            Console.WriteLine("Enter array elements!");
            for(int i = 0; i < n; i++)
            {
                Console.Write("arr[{0}]: ", i);
                arr[i] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Min value in this array is: " + value_Min(arr));
        }

        public static int value_Min(int[] arr)
        {
            int element_min = arr[0];

            foreach (int i in arr)
            {
                if(i < element_min)
                {
                    element_min = i;
                }
            }

            return element_min;
        }
    }
}