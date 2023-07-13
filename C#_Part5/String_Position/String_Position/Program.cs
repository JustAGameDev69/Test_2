using System;

namespace String_count
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a string: ");
            string user_input = Console.ReadLine();

            Console.Write("Enter word to count: ");
            char count_word = char.Parse(Console.ReadLine());

            word_Count(user_input, count_word);
        }

        public static void word_Count(string user_input, char count_word)
        {
            int count_time = 0;

            foreach(char i in user_input.ToCharArray())
            {
                if(i == count_word)
                {
                    count_time++;
                }
            }
            Console.WriteLine("That word appear {0} times in this context!", count_time);
        }
    }
}