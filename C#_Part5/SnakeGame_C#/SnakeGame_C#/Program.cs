using System;
using System.ComponentModel.Design;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;
using static System.Formats.Asn1.AsnWriter;

namespace Snake_Game
{
    class Program
    {
        public Random rand = new Random();
        const int win_height = 20, win_width = 60;
        const int panel = 10;
        int[] snake_tail_x = new int[100];
        int[] snake_tail_y = new int[100];
        int fruit_pos_x, fruit_pos_y, player_point, snake_tail, snake_head_x, snake_head_y;
        bool game_over, game_start, game_reset, is_printed, move_hor, move_ver;
        string snake_dir, snake_predir;
        public ConsoleKeyInfo key_input = new ConsoleKeyInfo();
        void start_Menu()
        {
            Console.SetWindowSize(win_height, win_width + panel);
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("!~~~~~~> SNAKE GAME <~~~~~~!");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any buttons to play!!!");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Game Rule: ");
            Console.WriteLine("Use arrow keys to move the snake!");
            Console.WriteLine("You mission is try to eat food as much as you can");
            Console.WriteLine("Don't eat your-self or hit the wall okay!");
            Console.WriteLine("Good luck and thank for play!!!");

            key_input = Console.ReadKey();
            if (key_input.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }

        }
        void Setup()
        {
            snake_dir = "RIGHT"; snake_predir = ""; //Ngay khi vào game, rắn di chuyển sang phải.
            player_point = 0;  //điểm người chơi mặc định là 0.
            snake_tail = 0;  //Mặc định thì rắn sẽ có đầu và thêm 3 phần đuôi.
            game_over = game_reset = is_printed = false;
            snake_head_x = 30;      //vị trí khi bắt đầu game
            snake_head_y = 10;      //vị trí khi bắt đầu game trục y
            Fruit_Spawner();        //Gọi hàm này để in ra thức ăn của rắn
        }

        void Fruit_Spawner()
        {
            fruit_pos_x = rand.Next(1, win_width -1);
            fruit_pos_y = rand.Next(1, win_height - 1);
        }

        void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < win_height; i++)
            {
                for (int j = 0; j < win_width; j++)
                {
                    if (i == 0 || i == win_height - 1) //vien khung ben tren va duoi
                        Console.Write("-");
                    else if (j == 0 || j == win_width - 1) //vien khung ben trai va phai
                        Console.Write("|");
                    else if (fruit_pos_x == j && fruit_pos_y == i) // qua
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("*");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (snake_head_x == j && snake_head_y == i) // dau cua con ran
                        Console.Write("+");
                    else
                    {   //than con ran
                        is_printed = false;
                        for (int k = 0; k < snake_tail; k++)
                        {
                            if (snake_tail_x[k] == j && snake_tail_y[k] == i)
                            {
                                Console.Write("-"); //than con ran
                                is_printed = true;
                            }
                        }
                        if (!is_printed) Console.Write(" "); //o trong khung hinh
                    }
                }
                Console.WriteLine(); //xuong dong cuoi hang
            }
            //Hien thi panel thong tin
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Diem cua ban: " + player_point);
        }

        void Update()
        {
            while (!game_over)
            {
                user_input();
                game_logic();
                Render();
                if (game_reset) break;
                Thread.Sleep(100);
            }
            if (game_over)
            {
                player_lose();
            }
        }


        void user_input()
        {
            while (Console.KeyAvailable)
            {
                key_input = Console.ReadKey();

                snake_predir = snake_dir;
                switch (key_input.Key)
                {
                    case ConsoleKey.LeftArrow:
                        snake_dir = "LEFT";
                        break;
                    case ConsoleKey.RightArrow:
                        snake_dir = "RIGHT";
                        break;
                    case ConsoleKey.UpArrow:
                        snake_dir = "UP";
                        break;
                    case ConsoleKey.DownArrow:
                        snake_dir = "DOWN";
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0); ;
                        break;
                    case ConsoleKey.P:
                        snake_dir = "STOP";
                        break;
                }
            }
        }

        void game_logic()
        {
            int pre_pos_x = snake_tail_x[0], pre_pos_y = snake_tail_y[0];
            int curr_pos_x, curr_pos_y;

            if (snake_dir != "STOP")
            {
                snake_tail_x[0] = snake_head_x;
                snake_tail_y[0] = snake_head_y;

                for (int i = 1; i < snake_tail; i++)
                {
                    curr_pos_x = snake_tail_x[i];
                    curr_pos_y = snake_tail_y[i];
                    snake_tail_x[i] = pre_pos_x;
                    snake_tail_y[i] = pre_pos_y;
                    pre_pos_x = curr_pos_x;
                    pre_pos_y = curr_pos_y;
                }

            }

            switch (snake_dir)
            {
                case "LEFT":
                    snake_head_x--;
                    break;
                case "RIGHT":
                    snake_head_x++;
                    break;
                case "UP":
                    snake_head_y--;
                    break;
                case "DOWN":
                    snake_head_y++;
                    break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear(); //xoa cac hien thi tren man hinh
                            Console.WriteLine("!~~~~~~> SNAKE GAME <~~~~~~!");
                            Console.WriteLine("YOU TIRED?TAKE A BREAK!");
                            Console.WriteLine("- Press P to continue!");
                            Console.WriteLine("- Press R to play again!");
                            Console.WriteLine("- Press Q to quit the game!");

                            key_input = Console.ReadKey();
                            if (key_input.Key == ConsoleKey.Q) Environment.Exit(0);
                            if (key_input.Key == ConsoleKey.R)
                            {
                                game_reset = true; 
                                break;
                            }
                            if (key_input.Key == ConsoleKey.P) //tiep tuc choi du lieu dang luu TailX, TailY, ....
                                break;
                        }
                    }
                    snake_dir = snake_predir;
                    break;
            }
            if (snake_head_x <=0 ||snake_head_x >= win_width-1|| snake_head_y <= 0|| snake_head_y >= win_height -1)
            {
                game_over = true;
            }
            else { game_over = false; }

            if (snake_head_x == fruit_pos_x && snake_head_y == fruit_pos_y)
            {
                player_point += 1;
                snake_tail++;
                Fruit_Spawner();
            }

            if (((snake_dir == "LEFT" && snake_predir != "UP") && (snake_dir == "LEFT" && snake_predir != "DOWN")) || ((snake_dir == "RIGHT" && snake_predir != "UP") && (snake_dir == "RIGHT" && snake_predir != "DOWN")))
            {
                move_hor = true;
            }
            else
            {
                move_hor = false;
            }
            if (((snake_dir == "UP" && snake_predir != "LEFT") && (snake_dir == "UP" && snake_predir != "RIGHT")) || ((snake_dir == "DOWN" && snake_predir != "LEFT") && (snake_dir == "DOWN" && snake_predir != "RIGHT")))
            {
                move_ver = true;
            }
            else
            {
                move_ver = false;
            }

            for (int i =1; i < snake_tail; i++)
            {
                if(snake_head_x == snake_tail_x[i] && snake_head_y == snake_tail_y[i])
                {
                    if (move_hor || move_ver)
                    {
                        game_over = false;
                    }
                    else
                    {
                        game_over = true;
                    }
                }
                if (fruit_pos_x== snake_tail_x[i] && fruit_pos_y == snake_tail_y[i])
                {
                    Fruit_Spawner();
                }
            }


        }

        void player_lose()
        {
            Console.WriteLine("------SNAKE GAME------");
            Console.WriteLine("Your Lose!");
            Console.WriteLine("- Press R to play again!");
            Console.WriteLine("- Press Q to quit the game!");

            while (true)
            {
                key_input = Console.ReadKey();
                if (key_input.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
                else if(key_input.Key == ConsoleKey.R)
                {
                    game_reset = true;
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Program snake_Game = new Program();
            snake_Game.start_Menu();
            while (true)
            {
                snake_Game.Setup();
                snake_Game.Update();
                Console.Clear();
            }
        }

    }
}