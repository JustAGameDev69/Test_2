using System;

namespace ctof
{
    class Program
    {
        static void Main(string[] args)
        {
            double fah, cel;
            int user_choice;

            do
            {
                Console.WriteLine("Menu: ");
                Console.WriteLine("Menu.");
                Console.WriteLine("1. Fahrenheit to Celsius");
                Console.WriteLine("2. Celsius to Fahrenheit");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                user_choice = Int32.Parse(Console.ReadLine());

                switch (user_choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;

                    case 1:
                        Console.Write("Enter fahrenheit degree: ");
                        fah = double.Parse(Console.ReadLine());
                        Console.WriteLine("Fahrenheit to Celsius: " + FahToCelsius(fah));
                        break;

                    case 2:
                        Console.Write("Enter fahrenheit degree: ");
                        cel = double.Parse(Console.ReadLine());
                        Console.WriteLine("Celsius to Fahrenheit: " + CelsiusToFah(cel));
                        break;
                }

            }
            while (user_choice != 0);
        }

        public static double CelsiusToFah(double cel)
        {
            double fah = (9.0 / 5) * cel + 32;

            return fah;
        }
        public static double FahToCelsius(double fah)
        {
            double cel = (5.0 / 9) * (fah - 32);
            
            return cel;
        }
    }
}