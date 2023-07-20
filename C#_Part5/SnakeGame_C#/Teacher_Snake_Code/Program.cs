using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_1
{
    class Program
    {
        public Random rand = new Random();
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        int score, headX, headY, fruitX, fruitY, nTail, game_Speed, boom_pos_x, boom_pos_y;
        int[] TailX = new int[100];
        int[] TailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool game_over, game_reset, is_printed, horizontal, vertical;
        string dir, pre_dir;

        void ShowBanner() //Console Start Screen
        {
            Console.SetWindowSize(width, height + panel); //height + panel
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false; 
            Console.WriteLine("!~~~~~~> SNAKE GAME <~~~~~~!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any buttons to play!!!");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Game Rule: ");
            Console.WriteLine("Use arrow keys to move the snake!");
            Console.WriteLine("You mission is try to eat food as much as you can");
            Console.WriteLine("Don't eat your-self or hit the wall okay!");
            Console.WriteLine("Good luck and thank for play!!!");
            Console.WriteLine("Press P to pause/continue the game!");
            Console.WriteLine("Press Q to quit the game!");


            keypress = Console.ReadKey();    //input key
            if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);

        }
        //Game start information!
        void Setup()
        {
            dir = "LEFT"; pre_dir = ""; //When game start, move left
            score = 0; nTail = 2;
            game_over = game_reset = is_printed = false;
            headX = 30;
            headY = 10;
            Random_Fruit_Pos();
            Random_Boom();
        }
        void Random_Fruit_Pos()
        {
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);
        }

        void Random_Boom()
        {
            boom_pos_x = rand.Next(1, width - 1);
            boom_pos_y = rand.Next(1, height - 1);
        }

        //Screen Update
        void Update()
        {
            if (score < 5) //Game speed increase per player_score
            {
                game_Speed = 100;
            }
            else if (score >= 5)
            {
                for (int i = 5; i < 20; i++)
                {
                    game_Speed--;
                }
            }
            else if (score >= 20)
            {
                game_Speed = 60;
            }

            while (!game_over)
            {
                CheckInput(); 
                Logic();      
                Render();     

                if (game_reset) break;
                Thread.Sleep(game_Speed); //Set game speed
            }
            if (game_over) Lose();
        }
        //Dieu khien phim
        void CheckInput()
        {
            while (Console.KeyAvailable)
            {
                keypress = Console.ReadKey();
                pre_dir = dir;
                switch (keypress.Key)
                {
                    case ConsoleKey.LeftArrow: dir = "LEFT"; break;
                    case ConsoleKey.RightArrow: dir = "RIGHT"; break;
                    case ConsoleKey.DownArrow: dir = "DOWN"; break;
                    case ConsoleKey.UpArrow: dir = "UP"; break;

                    case ConsoleKey.P: dir = "STOP"; break;   //play -> P (pause) -> stop
                    case ConsoleKey.Q: Environment.Exit(0); break;
                }
            }
        }

        void Logic()
        {
            int preX = TailX[0], preY = TailY[0]; // (x,y)
            int tempX, tempY;

            if (dir != "STOP")
            {
                TailX[0] = headX; TailY[0] = headY;

                for (int i = 1; i < nTail; i++)
                {
                    tempX = TailX[i]; tempY = TailY[i];
                    TailX[i] = preX; TailY[i] = preY;
                    preX = tempX; preY = tempY;
                }
            }
            switch (dir)
            {
                case "LEFT": headX--; break;
                case "RIGHT": headX++; break;
                case "UP": headY--; break;
                case "DOWN": headY++; break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear(); 
                            Console.WriteLine("===SNAKE GAME===");
                            Console.WriteLine("GAME PAUSE!");
                            Console.WriteLine("- Press 'P' to continue!");
                            Console.WriteLine("- Press 'R' to play again!");
                            Console.WriteLine("- Press 'Q' to quit the game!");

                            keypress = Console.ReadKey();
                            if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);
                            if (keypress.Key == ConsoleKey.R)
                            {
                                game_reset = true; break;
                            }
                            if (keypress.Key == ConsoleKey.P)
                                break;
                        }
                    }
                    dir = pre_dir; break;
            }
         
            if (headX <= 0 || headX >= width - 1 || headY <= 0 || headY >= height - 1)
                game_over = true;
            else game_over = false;
           
            if (headX == fruitX && headY == fruitY)
            {
                int point_stage1 = rand.Next(1, 6);
                int point_stage2 = rand.Next(1, 11);
                int point_stage3 = rand.Next(1, 21);
                if (score < 10)
                {
                    score += point_stage1;
                }
                else if (score >= 10 && score < 30)
                {
                    score += point_stage2;
                }
                else if (score >= 30)
                {
                    score += point_stage3;
                }
                nTail++;    //snake tail ++
                Random_Fruit_Pos(); //Spawn new fruit
            }
            if (headX == boom_pos_x && headY == boom_pos_y)
            {
                Lose();
            }

            //Side Move Check
            if (((dir == "LEFT" && pre_dir != "UP") && (dir == "LEFT" && pre_dir != "DOWN")) || ((dir == "RIGHT" && pre_dir != "UP") && (dir == "RIGHT" && pre_dir != "DOWN")))
                horizontal = true; 
            else horizontal = false;
            //MOVE UP AND DOWN CHECK
            if (((dir == "UP" && pre_dir != "LEFT") && (dir == "UP" && pre_dir != "RIGHT")) || ((dir == "DOWN" && pre_dir != "LEFT") && (dir == "DOWN" && pre_dir != "RIGHT")))
                vertical = true; 
            else vertical = false;
            
            for (int i = 1; i < nTail; i++)
            {
                if (headX == TailX[i] && headY == TailY[i])
                {
                    if (horizontal || vertical) game_over = false;
                    else game_over = true;
                }
                if (fruitX == TailX[i] && fruitY == TailY[i]) //fruit_pos == snake_pos => respawn fruit
                    Random_Fruit_Pos();
                if (boom_pos_x == TailX[i] && boom_pos_y == TailY[i]) //boom_pos == snake_pos => respawn bom
                    Random_Boom();
            }
        }
      
        void Render() //Draw
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1) //Draw top and bottom wall!
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("-");
                    }
                    else if (j == 0 || j == width - 1) // Draw side wall!
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("|");
                    }
                    else if (fruitX == j && fruitY == i) // Draw fruit
                    {
                        Random_Fruit();
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (boom_pos_x == j && boom_pos_y == i) //Draw Bom
                    {
                        Console.Write("#");
                    }
                    else if (headX == j && headY == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("o");
                    }
                    else
                    {   //Snake tail
                        is_printed = false;
                        for (int k = 0; k < nTail; k++)
                        {
                            if (TailX[k] == j && TailY[k] == i)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("*"); //Draw Snake Tail
                                is_printed = true;
                            }
                        }
                        if (!is_printed) Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            //Information panel
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Your score: " + score);
        }
        
        void Lose() //Hit wall or bom
        {
            Console.WriteLine("!~~~~~~> SNAKE GAME <~~~~~~!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You Lose!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("- Press 'R' to play again!");
            Console.WriteLine("- Press 'Q' to quit !");

            while (true)
            {
                keypress = Console.ReadKey();
                if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);
                if (keypress.Key == ConsoleKey.R)
                {
                    game_reset = true; break;
                }
            }
        }

        void Random_Fruit() //Change fruit color 
        {
            if (score <= 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");
            }
            else if (score > 5 && score < 15)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("%");
            }
            else if(score >= 15)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("!");
            }

        }

        static void Main(string[] args)
        {
            Program snakegame = new Program();
            snakegame.ShowBanner();
            while (true)
            {
                snakegame.Setup();
                snakegame.Update();
                Console.Clear();
            }
        }
    }
}