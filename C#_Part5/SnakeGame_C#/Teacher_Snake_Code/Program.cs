using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CGO_Buoi06_SnakeGame
{
    class Program
    {
        // Parameter
        public Random rand = new Random();
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        int score, headX, headY, fruitX, fruitY, nTail, game_Speed;
        int[] TailX = new int[100];
        int[] TailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool game_over, reset, isprinted, horizontal, vertical;
        string dir, pre_dir;

        //Hien thi man hinh bat dau
        void ShowBanner()
        {
            Console.SetWindowSize(width, height + panel); //height còn thêm thông báo panel
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
            Console.WriteLine("Good luck and thank for play!!!");
            Console.WriteLine("Press P to pause/continue the game!");
            Console.WriteLine("Press Q to quit the game!");
            //Doi nguoi choi bam phim


            keypress = Console.ReadKey();    //input key???
            if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);

        }
        //Cai dat thong so ban dau game
        void Setup()
        {
            dir = "RIGHT"; pre_dir = ""; //mac dinh di chuyen sang phai
            score = 0; nTail = 0;
            game_over = reset = isprinted = false;
            headX = 30; //vi tri dau tien con ran (dam bao ko vuot qua width)
            headY = 10; //vi tri dau tien con ran (dam bao ko vuot qua height)
            randomQua();//sinh ngau nhien phan qua 
        }
        //Sinh ngau nhien diem an qua
        void randomQua()
        {
            fruitX = rand.Next(1, width - 1); //ko lay gia tri 0 va width vi BIEN
            fruitY = rand.Next(1, height - 1);//ko lay gia tri 0 va heigth vi BIEN
        }
        //Cap nhat man hinh khi thao tac
        void Update()
        {
            if (score < 5)
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
                //con choi tiep, chua co chet!!!
                CheckInput(); //cho bam phim
                Logic();      //kiem tra phim bam
                Render();     //hien thi man hinh

                if (reset) break;
                Thread.Sleep(game_Speed); //chay process trong vong 1000ml 
            }
            if (game_over) Lose();
        }
        //Dieu khien phim
        void CheckInput()
        {
            while (Console.KeyAvailable)
            {
                //CHo bam phim bat ky 
                keypress = Console.ReadKey();
                //luu lai thao tac phim truoc do
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
        //Kiem tra phim nhan dieu huong
        void Logic()
        {
            int preX = TailX[0], preY = TailY[0]; // (x,y)
            int tempX, tempY;
            //0 1 2 3 4 => Con rắn ăn thêm quà            //x 0 1 2 3 4 => Chen them vo mang
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
                            Console.Clear(); //xoa cac hien thi tren man hinh
                            Console.WriteLine("===SNAKE GAME===");
                            Console.WriteLine("GAME PAUSE!");
                            Console.WriteLine("- Press 'P' to continue!");
                            Console.WriteLine("- Press 'R' to play again!");
                            Console.WriteLine("- Press 'Q' to quit the game!");

                            keypress = Console.ReadKey();
                            if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);
                            if (keypress.Key == ConsoleKey.R)
                            {
                                reset = true; break;
                            }
                            if (keypress.Key == ConsoleKey.P) //tiep tuc choi du lieu dang luu TailX, TailY, ....
                                break;
                        }
                    }
                    dir = pre_dir; break;
            }
            //kiem tra va cham vat can 
            if (headX <= 0 || headX >= width - 1 || headY <= 0 || headY >= height - 1)
                game_over = true;
            else game_over = false;
            //kiem tra diem an qua
            if (headX == fruitX && headY == fruitY) //trung toa do
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
                nTail++;    //tang kich thuoc con rang    
                randomQua();//khoi tao diem qua moi
            }
            //kiem tra di chuyen lien tuc
            //kiem tra di chuyen ngang LEFT , RIGHT
            if (((dir == "LEFT" && pre_dir != "UP") && (dir == "LEFT" && pre_dir != "DOWN")) || ((dir == "RIGHT" && pre_dir != "UP") && (dir == "RIGHT" && pre_dir != "DOWN")))
                horizontal = true; //di chuyen lien tuc theo chieu ngang
            else horizontal = false;
            //kiem tra di chuyen doc UP, DOWN
            if (((dir == "UP" && pre_dir != "LEFT") && (dir == "UP" && pre_dir != "RIGHT")) || ((dir == "DOWN" && pre_dir != "LEFT") && (dir == "DOWN" && pre_dir != "RIGHT")))
                vertical = true; //di chuyen lien tuc theo chieu doc
            else vertical = false;

            //kiem tra con ran va cham than con ran
            for (int i = 1; i < nTail; i++)
            {
                if (headX == TailX[i] && headY == TailY[i])
                {
                    //quay dau 180
                    if (horizontal || vertical) game_over = false;
                    else game_over = true;
                }
                if (fruitX == TailX[i] && fruitY == TailY[i]) //qua sinh trung than con ran -> sinh lai ngau nhien qua
                    randomQua();
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
                    else if (fruitX == j && fruitY == i) // qua
                    {
                        Random_Fruit();
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (headX == j && headY == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("o");
                    }
                    else
                    {   //than con ran
                        isprinted = false;
                        for (int k = 0; k < nTail; k++)
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
            Console.WriteLine("Your score: " + score);
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
                keypress = Console.ReadKey();
                if (keypress.Key == ConsoleKey.Q) Environment.Exit(0);
                if (keypress.Key == ConsoleKey.R)
                {
                    reset = true; break;
                }
            }
        }

        void Random_Fruit()
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