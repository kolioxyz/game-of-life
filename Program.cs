using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace gameOfLife
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            /* GAME OF LIFE
             *            
             * 2d (infinite?) grid
             * cells can be "dead" or "alive"
             * RULES:
             *             
             * Any live cell with fewer than two live neighbours dies
             * Any live cell with two or three live neighbours lives
             * Any live cell with more than three live neighbours dies
             *             
             * Any dead cell with exactly three live neighbours becomes a live cell
             * 
             */

            int gridWidth = 39;
            int gridHeight = 24;

            string pathGlider = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"glider.element");
            string pathGun = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gliderGun.element");
            string pathMwss = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"mwss.element");
            string[] glider = File.ReadAllLines(pathGlider);
            string[] gliderGun = File.ReadAllLines(pathGun);
            string[] mwss = File.ReadAllLines(pathMwss);

            bool[,] grid = new bool[gridHeight, gridWidth];

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                    grid[i, j] = false;
            }

            //addElement(ref grid, 3, 3, mwss);
            addElement(ref grid, 1, 1, gliderGun);

            bool[,] next = new bool[gridHeight, gridWidth];
            Array.Copy(grid, next, grid.Length);

            char[,] screen = gridScreen(grid);
            printScreen(screen);
            int frame = 0;

            while (true)
            {
                //Console.ReadKey();
                for (int i = 1; i < gridHeight - 1; i++)
                {
                    for (int j = 1; j < gridWidth - 1; j++)
                    {
                        bool[] neighs = getNeighbours(grid, j, i);
                        bool test = nextState(neighs, grid[i, j]);
                        next[i, j] = test;
                    }
                }
                Array.Copy(next, grid, next.Length);

                frame++;
                Thread.Sleep(100);

                Console.Clear();
                screen = gridScreen(grid);
                printScreen(screen);
                Console.WriteLine("Frame: " + frame);
            }

            /*
             * no need for classes
             *             
             * optimisation idea:
             * for larger grid, no need to check every single cell
             * make algorithm to find cells surrounding living ones
             * and only compute rules for those
             * 
             * 
             */
        }

        static bool[] getNeighbours(bool[,] grid, int x, int y)
        {
            bool[] output = new bool[8];

            output[0] = grid[y - 1, x - 1];
            output[1] = grid[y, x - 1];
            output[2] = grid[y + 1, x - 1];
            output[3] = grid[y - 1, x];
            output[4] = grid[y + 1, x];
            output[5] = grid[y - 1, x + 1];
            output[6] = grid[y, x + 1];
            output[7] = grid[y + 1, x + 1];
            return output;
        }

        static bool nextState(bool[] neighbours, bool current)
        {
            bool next = false;

            int count = neighbours.Where(x => x).Count();

            if (current)
            {
                if (count < 2 || count > 3)
                    next = false;
                else
                    next = true;
            }
            else
            {
                if (count == 3)
                    next = true;
            }

            return next;
        }

        static void addElement(ref bool[,] grid, int x, int y, string[] element)
        {
            for (int i = 0; i < element.GetLength(0); i++)
            {
                for (int j = 0; j < element[i].Length; j++)
                {
                    if (element[i][j] == 'X')
                        grid[y + i, x + j] = true;
                }
            }
        }

        static void printScreen(char[,] scr)
        {
            for (int i = 0; i < scr.GetLength(0); i++)
            {
                for (int j = 0; j < scr.GetLength(1) - 1; j++) // last char is \n
                {
                    Console.Write(scr[i, j] + " ");
                }
                Console.Write('\n');
            }
        }

        static char[,] gridScreen(bool[,] grid)
        {
            // Return char array representing alive and dead cells
            char[,] output = new char[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j])
                        output[i, j] = '■'; // ■
                    else
                        output[i, j] = '·'; // □ ·
                }
            }
            return output;
        }
    }
}