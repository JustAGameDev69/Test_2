using System;
using System.Security.Cryptography.X509Certificates;

namespace Work_Hard
{
    public class Rectangle
    {
        double width, height;

        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public double getArea()
        {
            return width * height;
        }

        public double GetPerimeter()
        {
            return (width + height) / 2;
        }

        public string Display()
        {
            return "Rectangle detail :  {" + "width=" + width + ", height=" + height + "}";
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter rectangle width: ");
            double width = double.Parse(Console.ReadLine());
            Console.Write("Enter rectangle height: ");
            double height = double.Parse(Console.ReadLine());
            Rectangle rectangle1 = new Rectangle(width, height);

            Console.WriteLine(rectangle1.Display());
            Console.WriteLine("Area of this rectangle is: {0}", rectangle1.getArea());
            Console.WriteLine("Perimeter of this rectangle is: {0}", rectangle1.GetPerimeter());
        }
    }


}