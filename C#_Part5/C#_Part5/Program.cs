using System;
using System.Text.Json.Serialization;
using System.Transactions;
using System.Collections.Generic;

namespace Test_method
{
    class Program
    {
        static void Main(string[] args) 
        {
            List<int> simple_List = new List<int>();

            Console.WriteLine("Enter list size: ");
            int list_size = int.Parse(Console.ReadLine());

            for (int i = 0; i < list_size; i++)
            {
                Console.WriteLine("Enter element number {0}: ", i);
                int list_elements = int.Parse(Console.ReadLine());
                simple_List.Add(list_elements);
            }

            Console.WriteLine("List sau khi nhap:");
            foreach (int element in simple_List)
            {
                Console.Write("{0} ", element);
            }
        }

        static List<int> Increasing_value(List<int>simple_List)
        {
            List<int> increase_list = new List<int>();

            increase_list = simple_List.Sort();

            return increase_list;
        }

        static List<int> Decreasing_value()
        {
            List<int> decrease_list = new List<int>();


            return decrease_list;
        }


    }
}