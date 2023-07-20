using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CGO_Buoi06_SnakeGame
{
    class Program
    {
        // Parameter
        public Random rand = new Random();
        public ConsoleKeyInfo key_input = new ConsoleKeyInfo();
        int player_score, snake_head_x, snake_head_y, fruit_pos_x, fruit_pos_y, snake_tail, game_Speed, boom_pos_x, boom_pos_y;
        int[] TailX = new int[100];
        int[] TailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool game_over, game_reset, isprinted, move_hor, move_ver;
        string dir, pre_dir;

        //Hien thi man hinh bat dau
        void ShowBanner()
        {
            Console.SetWindowSize(height, width + panel); //height còn thêm thông báo panel
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;   //ẩn cỏn trỏ nháy
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
            Console.WriteLine("Don't eat the boom '#' too!");
            Console.WriteLine("Good luck and thank for play!!!");
            Console.WriteLine("Press P to pause/continue the game!");
            Console.WriteLine("Press Q to quit the game!");
            //Doi nguoi choi bam phim


            key_input = Console.ReadKey();    //input key???
            if (key_input.Key == ConsoleKey.Q) Environment.Exit(0);

        }
        //Game start information!
        void Setup()
        {
            dir = "LEFT"; pre_dir = ""; //When game start, move left
            player_score = 0; snake_tail = 0;
            game_over = game_reset = isprinted = false;
            snake_head_x = 30; //vi tri dau tien con ran (dam bao ko vuot qua width)
            snake_head_y = 10; //vi tri dau tien con ran (dam bao ko vuot qua height)
            randomQua();//sinh ngau nhien phan qua
            Random_Boom();
        }
        void randomQua()
        {
            fruit_pos_x = rand.Next(1, width - 1); //ko lay gia tri 0 va width vi BIEN
            fruit_pos_y = rand.Next(1, height - 1);//ko lay gia tri 0 va heigth vi BIEN
        }

        void Random_Boom()
        {
            boom_pos_x = rand.Next(1, width - 1);
            boom_pos_y = rand.Next(1, height - 1);
        }

        //Screen Update
        void Update()
        {
            if (player_score < 5) //Game speed increase per player_score
            {
                game_Speed = 100;
            }
            else if (player_score >= 5)
            {
                for (int i = 5; i < 20; i++)
                {
                    game_Speed--;
                }
            }
            else if (player_score >= 20)
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
                //CHo bam phim bat ky 
                key_input = Console.ReadKey();
                //luu lai thao tac phim truoc do
                pre_dir = dir;
                switch (key_input.Key)
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
        //Kiem tra phim nhan dieu huong
        void Logic()
        {
            int preX = TailX[0], preY = TailY[0]; // (x,y)
            int tempX, tempY;
            //0 1 2 3 4 => Con rắn ăn thêm quà            //x 0 1 2 3 4 => Chen them vo mang
            if (dir != "STOP")
            {
                TailX[0] = snake_head_x; TailY[0] = snake_head_y;

                for (int i = 1; i < snake_tail; i++)
                {
                    tempX = TailX[i]; tempY = TailY[i];
                    TailX[i] = preX; TailY[i] = preY;
                    preX = tempX; preY = tempY;
                }
            }
            switch (dir)
            {
                case "LEFT": snake_head_x--; break;
                case "RIGHT": snake_head_x++; break;
                case "UP": snake_head_y--; break;
                case "DOWN": snake_head_y++; break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear(); //xoa cac hien thi tren man hinh
                            Console.WriteLine("===SNAKE GAME===");
                            Console.WriteLine("GAME PAUSE!");
                            Console.WriteLine("- Press 'P' to continue!");
                            Console.WriteLine("- Press 'R' to play again!");
                            Console.WriteLine("- Press 'Q' to quit the game!");

                            key_input = Console.ReadKey();
                            if (key_input.Key == ConsoleKey.Q) Environment.Exit(0);
                            if (key_input.Key == ConsoleKey.R)
                            {
                                game_reset = true; break;
                            }
                            if (key_input.Key == ConsoleKey.P) //tiep tuc choi du lieu dang luu TailX, TailY, ....
                                break;
                        }
                    }
                    dir = pre_dir; break;
            }
            //kiem tra va cham vat can 
            if (snake_head_x <= 0 || snake_head_x >= width - 1 || snake_head_y <= 0 || snake_head_y >= height - 1)
                game_over = true;
            else game_over = false;
            //kiem tra diem an qua
            if (snake_head_x == fruit_pos_x && snake_head_y == fruit_pos_y) //trung toa do
            {
                int point_stage1 = rand.Next(1, 6);
                int point_stage2 = rand.Next(1, 11);
                int point_stage3 = rand.Next(1, 21);
                if (player_score < 10)
                {
                    player_score += point_stage1;
                }
                else if (player_score >= 10 && player_score < 30)
                {
                    player_score += point_stage2;
                }
                else if (player_score >= 30)
                {
                    player_score += point_stage3;
                }
                snake_tail++;    //tang kich thuoc con rang    
                randomQua();//khoi tao diem qua moi
            }
            if (snake_head_x == boom_pos_x && snake_head_y == boom_pos_y)
            {
                Lose();
            }


            //kiem tra di chuyen lien tuc
            //kiem tra di chuyen ngang LEFT , RIGHT
            if (((dir == "LEFT" && pre_dir != "UP") && (dir == "LEFT" && pre_dir != "DOWN")) || ((dir == "RIGHT" && pre_dir != "UP") && (dir == "RIGHT" && pre_dir != "DOWN")))
                move_hor = true; //di chuyen lien tuc theo chieu ngang
            else move_hor = false;
            //kiem tra di chuyen doc UP, DOWN
            if (((dir == "UP" && pre_dir != "LEFT") && (dir == "UP" && pre_dir != "RIGHT")) || ((dir == "DOWN" && pre_dir != "LEFT") && (dir == "DOWN" && pre_dir != "RIGHT")))
                move_ver = true; //di chuyen lien tuc theo chieu doc
            else move_ver = false;

            //kiem tra con ran va cham than con ran
            for (int i = 1; i < snake_tail; i++)
            {
                if (snake_head_x == TailX[i] && snake_head_y == TailY[i])
                {
                    //quay dau 180
                    if (move_hor || move_ver) game_over = false;
                    else game_over = true;
                }
                if (fruit_pos_x == TailX[i] && fruit_pos_y == TailY[i]) //qua sinh trung than con ran -> sinh lai ngau nhien qua
                    randomQua();
                if (boom_pos_x == TailX[i] && boom_pos_y == TailY[i]) //qua sinh trung than con ran -> sinh lai ngau nhien qua
                    Random_Boom();
            }
        }
        //Hien thi thay doi man hinh
        void Render()
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
                    else if (fruit_pos_x == j && fruit_pos_y == i) // qua
                    {
                        Random_Fruit();
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (boom_pos_x == j && boom_pos_y == i)
                    {
                        Console.ForegroundColor= ConsoleColor.White;
                        Console.Write("#");
                    }
                    else if (snake_head_x == j && snake_head_y == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("o");
                    }
                    else
                    {   //than con ran
                        isprinted = false;
                        for (int k = 0; k < snake_tail; k++)
                        {
                            if (TailX[k] == j && TailY[k] == i)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("*"); //than con ran
                                isprinted = true;
                            }
                        }
                        if (!isprinted) Console.Write(" "); //o trong khung hinh
                    }
                }
                Console.WriteLine(); //xuong dong cuoi hang
            }
            //Hien thi panel thong tin
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Your score: " + player_score);
        }
        //Xy ly game thua
        void Lose()
        {
            Console.WriteLine("!~~~~~~> SNAKE GAME <~~~~~~!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You Lose!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("- Press 'R' to play again!");
            Console.WriteLine("- Press 'Q' to quit !");

            while (true)
            {
                key_input = Console.ReadKey();
                if (key_input.Key == ConsoleKey.Q) Environment.Exit(0);
                if (key_input.Key == ConsoleKey.R)
                {
                    game_reset = true; break;
                }
            }
        }

        void Random_Fruit()
        {
            if (player_score <= 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");
            }
            else if (player_score > 5 && player_score < 15)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("%");
            }
            else if (player_score >= 15)
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