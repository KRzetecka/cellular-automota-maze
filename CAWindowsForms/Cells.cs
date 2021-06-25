using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using System.Text.RegularExpressions;

namespace CAApplication
{
    

    class Labirynth
    {
        //Stats 
        public int steps, blocked, turns, shortests=0;

        //data - testowy string z labirynem do konsoli
        public Cell[,] matrix;
        public int[,] matrixValues;
        public int[,] matrixTmpValues;
        public int[,] matrixPunishmentValues;

        public string data;
        public string pathToView = "";
        public bool isDone;

        public int size = 8;
        
        public int obstacleNum=0;
        private int randTmp1, randTmp2;

        

        int robotLastMoveDirection; //0-Up, 1-Right, 2-Down, 3-Left

        private ObstacleCell[] obstacleList;

        //The test constructor
        public Labirynth()
        {
            matrix = new Cell[size, size];
            matrixValues = new int[size, size];
            //========================================
            for (int i = 0; i < 8; i++)
            {
                matrix[0, i] = new WallCell();
                matrix[i, 0] = new WallCell();
            }

            for (int i = 1; i < 7; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    matrix[i, j] = new VoidCell();
                }
            }

            for (int i = 1; i < 8; i++)
            {
                matrix[7, i] = new WallCell();
                matrix[i, 7] = new WallCell();
            }
            //=========================================
            matrix[5, 5].type = 2;
            matrix[2, 4].type = 4;
        }


        public Labirynth(string source)
        {
            isDone = false;
            steps = 0;
            blocked = 0;
            turns = 0;
            shortests = 0;

            string path = Application.StartupPath + @"\labirynths\" + source;
            if (File.Exists(path))
            {
                
                using (StreamReader sr = File.OpenText(path))
                {
                    int obstacleID=0;
                    string[] sizeLine = sr.ReadLine().Split(';');
                    int x = int.Parse(sizeLine[0]), y = int.Parse(sizeLine[1]);
                    size = x;
                    matrix = new Cell[x, x];

                    string labr = sr.ReadToEnd();
                    sr.Close();
                    labr.Trim();
                    labr = Regex.Replace(labr, @"\t|\n|\r", "");
                    using (StringReader sr2 = new StringReader(labr))
                    {
                        matrixPunishmentValues = new int[x,x];
                        for (int i = 0; i < x; i++)
                        {
                            for (int j = 0; j < x; j++)
                            {
                                if (sr2.Peek() == '0') matrix[i, j] = new VoidCell();
                                else if (sr2.Peek() == '1') matrix[i, j] = new WallCell();
                                else if (sr2.Peek() == '2') matrix[i, j] = new RobotCell();
                                else if (sr2.Peek() == '3') 
                                {
                                    matrix[i, j] = new ObstacleCell(obstacleID, true);
                                    
                                    obstacleID++;
                                }
                                else if (sr2.Peek() == '4') matrix[i, j] = new ExitCell();
                                sr2.Read();

                                matrixPunishmentValues[i, j] = 0;
                            }
                        }
                        sr2.Close();
                    }
                    obstacleList = new ObstacleCell[obstacleID];

                    //Generate destination for each obstacle
                    int obsIterator = 0;
                    bool Exit = true;
                    if(obstacleID != 0)
                    {
                        while (Exit == true)
                        {
                            for (int i = 0; i < x; i++)
                            {
                                for (int j = 0; j < x; j++)
                                {
                                    if (matrix[i, j].type == 3 && matrix[i, j].id == obsIterator)
                                    {
                                        int tmp1 = 0, tmp2 = 0, tmp3 = 0, tmp4 = 0;
                                        GenerateDestination();
                                        tmp1 = randTmp1;
                                        tmp2 = randTmp2;
                                        GenerateDestination();
                                        tmp3 = randTmp1;
                                        tmp4 = randTmp2;
                                        matrix[i, j] = new ObstacleCell(obsIterator, tmp1, tmp2, i, j, true, i, j);
                                        obstacleList[obsIterator] = new ObstacleCell(obsIterator, tmp1, tmp2, i, j, true, i, j);
                                        //Console.Out.WriteLine("IM nr{0}, from {1},{2} going to {3},{4}", obsIterator, i, j,tmp1,tmp2);
                                        obsIterator++;
                                        if (obsIterator == obstacleID) Exit = false;
                                    }

                                }
                            }
                            obstacleNum = obstacleID;
                        }
                    }
                    matrixValues = new int[size, size];
                }
            }
            else
            {
                MessageBox.Show("Can't load");
            }
           
    }


        List<Tuple<int, int>> Solve(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i,j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width) ));

            dist[starting_height,starting_width] = 0;
            
            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h,w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }
                path[h,w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width) break;

                if (visited[h,w]) continue;

                visited[h,w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight = matrixValues[h + i, w + j];
                        if ((dist[h,w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int,int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
            }
            return path[ending_height, ending_width];
        }

        int SolveShortest(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width)));

            dist[starting_height, starting_width] = 0;

            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h, w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }
                path[h, w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width) break;//found exit

                if (visited[h, w]) continue;

                visited[h, w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight;
                        //int weight = matrixValues[h + i, w + j];
                        //if (matrix[h + i, w + j].type == 3 || matrix[h + i, w + j].type == 0) weight = 1;
                        if (matrix[h + i, w + j].type != 1)
                        {
                            weight = 1;
                        }else
                        {
                            weight = int.MaxValue;
                        }
                        

                        if ((dist[h, w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            //Console.Out.WriteLine("{0} weight\t {1} current\t{2} path", weight, dist[h, w], path[h, w].Count);
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int, int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
                Console.Out.Write("\n");
            }
            return path.Length;
        }

        List<Tuple<int, int>> SolveObs(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width)));

            dist[starting_height, starting_width] = 0;

            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h, w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }
                
                path[h, w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width)
                {
                    //MessageBox.Show("im herelollolol");
                    break;//found exit
                }
                if (visited[h, w]) continue;

                visited[h, w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight = matrix[h + i, w + j].value;

                        if (matrix[h + i, w + j].type == 4 || matrix[h + i, w + j].type == 2) weight = int.MaxValue;

                        if ((dist[h, w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            //Console.Out.WriteLine("{0} weight\t {1} current\t{2} path", weight, dist[h, w], path[h, w].Count);
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int, int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
                Console.Out.Write("\n");
            }
            
            return path[ending_height, ending_width];
        }

        List<Tuple<int, int>> SolveObsOnMyWay(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width)));

            dist[starting_height, starting_width] = 0;

            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h, w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }

                path[h, w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width)
                {
                    //MessageBox.Show("im herelollolol");
                    break;//found exit
                }
                if (visited[h, w]) continue;

                visited[h, w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight = matrix[h + i, w + j].value;

                        if (matrix[h + i, w + j].type == 3) weight = int.MaxValue;

                        if ((dist[h, w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            //Console.Out.WriteLine("{0} weight\t {1} current\t{2} path", weight, dist[h, w], path[h, w].Count);
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int, int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
                Console.Out.Write("\n");
            }
            return path[ending_height, ending_width];
        }

        int[,] SolveDist(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width)));

            dist[starting_height, starting_width] = 0;

            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h, w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }
                path[h, w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width) break;

                if (visited[h, w]) continue;

                visited[h, w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight = matrixValues[h + i, w + j];
                        if ((dist[h, w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int, int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
            }
            return dist;
        }

        public int[,] getDist()
        {
            int[,] tmp = new int[size,size];
            int xExit = 0, yExit = 0;
            int xRobot = 0, yRobot = 0;
            //Szukanie wyjscia
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j].type == 4)
                    {
                        xExit = i;
                        yExit = j;
                    }
                    if (matrix[i, j].type == 2)
                    {
                        xRobot = i;
                        yRobot = j;
                    }
                }
            }
            tmp = SolveDist(xExit, yExit, xRobot, yRobot);
            return tmp;
        }
        List<Tuple<int, int>> getThePath(int starting_height, int starting_width, int ending_height, int ending_width)
        {
            int[,] dist = new int[size, size];
            bool[,] visited;
            List<Tuple<int, int>>[,] path = new List<Tuple<int, int>>[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            visited = new bool[size, size];
            CAWindowsForms.SortedMultiSet<Tuple<int, int>> s = new CAWindowsForms.SortedMultiSet<Tuple<int, int>>();
            s.Add(new Tuple<int, int>(1, TWO_TO_SINGLE(starting_height, starting_width)));

            dist[starting_height, starting_width] = 0;

            while (!s.isEmpty())
            {
                Tuple<int, int> p = s.Min;

                s.Remove(p);

                int h = p.Item2;
                int w = p.Item2 % size;
                h /= size;

                if (path[h, w] == null)
                {
                    path[h, w] = new List<Tuple<int, int>>();
                }
                path[h, w].Add(new Tuple<int, int>(h, w));
                if (h == ending_height && w == ending_width) break;

                if (visited[h, w]) continue;

                visited[h, w] = true;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        int weight;
                        try
                        {
                            weight = matrixValues[h + i, w + j];
                        }
                        catch { return null; }
                        
                        if ((dist[h, w] + weight) < dist[h + i, w + j] && weight != int.MaxValue)
                        {
                            path[h + i, w + j] = new List<Tuple<int, int>>();
                            path[h, w].ForEach((item) =>
                            {
                                path[h + i, w + j].Add(new Tuple<int, int>(item.Item1, item.Item2));
                            });
                            dist[h + i, w + j] = dist[h, w] + weight;

                            s.Add(new Tuple<int, int>(dist[h + i, w + j], TWO_TO_SINGLE(h + i, w + j)));
                        }
                    }
                }
            }
            return path[ending_height, ending_width];
        }
        public List<Tuple<int, int>> getPath()
        {
            //List<Tuple<int, int>>[,] tmp = new List<Tuple<int, int>>[size, size];
            int xExit = 0, yExit = 0;
            int xRobot = 0, yRobot = 0;
            //Szukanie wyjscia
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j].type == 4)
                    {
                        xExit = i;
                        yExit = j;
                    }
                    if (matrix[i, j].type == 2)
                    {
                        xRobot = i;
                        yRobot = j;
                    }
                }
            }
            List<Tuple<int, int>> tmp = getThePath(xExit, yExit, xRobot, yRobot);
            return tmp;
        }



        public void PrintMatrix()
        {
            data = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (matrix[i, j].type == 0) data += "0";
                    if (matrix[i, j].type == 1) data += "#";
                    if (matrix[i, j].type == 2) data += "R";
                    if (matrix[i, j].type == 3) data += "o";
                    if (matrix[i, j].type == 4) data += "E";
                }
                data += "\n";
            }
        }
        int TWO_TO_SINGLE(int x, int y) {
            return ((x) * size + (y));
        }
        public void ChangeValues(int group, int newValue, int voidVal, int obsVal)
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (matrix[i, j].type == group)
                    {
                        matrix[i, j].value = newValue;
                        if (matrixValues == null) CellularAutomata(false, voidVal, obsVal);
                        //matrixValues[i, j] = newValue;
                    }
                }
            }
        }

        public bool GenerateDestination()
        {
            //CAWindowsForms.Randomizer newOne;
            bool Exit = true;
            int infLoopCouner = 0;
            while(Exit == true)
            {

                randTmp1 = CAWindowsForms.Randomizer.Between(1, size - 2);
                randTmp2 = CAWindowsForms.Randomizer.Between(1, size - 2);

                if(matrix[randTmp1,randTmp2].type == 0)
                {
                    return true;
                }
                else
                {
                    infLoopCouner++;
                }
                if(infLoopCouner == 500)
                {
                    MessageBox.Show("sth went wrong with generating desination for obstacles idk");
                    return false;
                }
            }
            return false;
        }

        public void NextStep()
        {
            if (isDone == true) return;

            

            turns++;
            //RobotsSpecialCellValues(false);

            int xExit = 0, yExit = 0;
            int newX, newY;
            //Szukanie wyjscia
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j].type == 4)
                    {
                        xExit = i;
                        yExit = j;
                    }
                }
            }

            //Szukanie robota/robotów i wykonanie tury (djixtra)

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j].type == 2)
                    {
                        /*if(shortests == 0)
                        {
                            shortests = SolveShortest(i, j, xExit, yExit)-2;
                        }*/                       
                        matrixPunishmentValues[i, j] += matrix[i, j].value;

                        List<Tuple<int, int>> shortestPath = Solve(i, j, xExit, yExit);
                        if (shortestPath == null) goto outta1;
                        pathToView = "";
                        for (int tmp = 0; tmp < shortestPath.Count; tmp++)
                        {
                            pathToView += "{" + shortestPath[tmp].Item1.ToString() + ", " + shortestPath[tmp].Item2 + "}   ";
                        }
                        newX = shortestPath[1].Item1;
                        newY = shortestPath[1].Item2;

                        if(turns == 1)
                        //{
                        //    shortests = shortestPath.Count-1;
                        //}

                        if (matrix[newX, newY].type == 3)
                        {
                            shortestPath = SolveObsOnMyWay(i, j, xExit, yExit);
                            if (shortestPath == null) goto outta1;
                            newX = shortestPath[1].Item1;
                            newY = shortestPath[1].Item2;
                        }

                        if (matrix[newX, newY].type == 0)
                        {
                            matrix[newX, newY] = new RobotCell();
                            matrix[i, j] = new VoidCell();

                            steps++;
                        }
                        else if (matrix[newX, newY].type == 4)
                        {
                            matrix[newX, newY] = new RobotCell();
                            matrix[i, j] = new VoidCell();
                            isDone = true;

                            steps++;
                        }
                        else
                        {
                            blocked++;
                        }
                        goto outta1;
                    }
                }
            }


        //Szukanie przeszkód i wykonanie tury przez obstacle
        outta1:

            foreach (var obstacleCell in obstacleList)
            {
                //Change direction when destiny reached
                if (obstacleCell.Destination == true && obstacleCell.xObsDest1 == obstacleCell.x && obstacleCell.yObsDest1 == obstacleCell.y)
                {
                    obstacleCell.Destination = false; //If he gets to 1st point go to 2nd
                }
                else if (obstacleCell.Destination == false && obstacleCell.xObsDest2 == obstacleCell.x && obstacleCell.yObsDest2 == obstacleCell.y)
                {
                    obstacleCell.Destination = true; //If he gets to 2nd point go to 1st
                }

                if (obstacleCell.Destination == true)
                {
                    try
                    {
                        List<Tuple<int, int>> shortestPath2 = SolveObs(obstacleCell.x, obstacleCell.y, obstacleCell.xObsDest1, obstacleCell.yObsDest1);
                        if (shortestPath2 == null) break;
                        newX = shortestPath2[1].Item1;
                        newY = shortestPath2[1].Item2;
                        if (matrix[newX, newY].type == 0)
                        {
                            matrix[newX, newY] = new ObstacleCell(obstacleCell.id, obstacleCell.xObsDest1, obstacleCell.yObsDest1, obstacleCell.xObsDest2, obstacleCell.yObsDest2, true, newX, newY);
                            matrix[obstacleCell.x, obstacleCell.y] = new VoidCell();
                            obstacleList[obstacleCell.id] = new ObstacleCell(obstacleCell.id, obstacleCell.xObsDest1, obstacleCell.yObsDest1, obstacleCell.xObsDest2, obstacleCell.yObsDest2, true, newX, newY);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error 100");
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        List<Tuple<int, int>> shortestPath2 = SolveObs(obstacleCell.x, obstacleCell.y, obstacleCell.xObsDest2, obstacleCell.yObsDest2);
                        if (shortestPath2 == null) break;
                        newX = shortestPath2[1].Item1;
                        newY = shortestPath2[1].Item2;
                        if (matrix[newX, newY].type == 0)
                        {
                            matrix[newX, newY] = new ObstacleCell(obstacleCell.id, obstacleCell.xObsDest1, obstacleCell.yObsDest1, obstacleCell.xObsDest2, obstacleCell.yObsDest2, false, newX, newY);
                            matrix[obstacleCell.x, obstacleCell.y] = new VoidCell();
                            obstacleList[obstacleCell.id] = new ObstacleCell(obstacleCell.id, obstacleCell.xObsDest1, obstacleCell.yObsDest1, obstacleCell.xObsDest2, obstacleCell.yObsDest2, false, newX, newY);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error 101");
                        continue;
                    }
                }



            } 



            //============================================
            /*
            if (obstacleNum == 0) return;

            int idCounter = 0;
            
            while (idCounter < obstacleNum)
            {
                if (idCounter == 99) return;
                for (int i = 0; i < size; i++)
                {
                    if (idCounter == 99) return;
                    for (int j = 0; j < size; j++)
                    {
                        if (idCounter == 99) return;
                        if (matrix[i, j].type == 3 && matrix[i, j].id == idCounter)
                        {
                            if(idCounter > 10)
                            {
                                return;
                            }


                            if (matrix[i, j].whereImGoingLord == true && matrix[i, j].xObsDest1 == i && matrix[i, j].yObsDest1 == j)
                            {
                                matrix[i, j].whereImGoingLord = false; //If he gets to 1st point go to 2nd
                            }
                            else if (matrix[i, j].whereImGoingLord == false && matrix[i, j].xObsDest2 == i && matrix[i, j].yObsDest2 == j)
                            {
                                matrix[i, j].whereImGoingLord = true; //If he gets to 2nd point go to 1st
                            }

                            List<Tuple<int, int>> shortestPath2 = null;

                            if (matrix[i, j].whereImGoingLord == true)
                            {
                                
                                shortestPath2 = SolveObs(i, j, matrix[i, j].xObsDest1, matrix[i, j].yObsDest1);
                                if (shortestPath2 == null) break;
                                newX = shortestPath2[1].Item1;
                                newY = shortestPath2[1].Item2;

                                if (matrix[newX, newY].type == 0)
                                {
                                   matrix[newX, newY] = new ObstacleCell(idCounter, matrix[i, j].xObsDest1, matrix[i, j].yObsDest1, matrix[i, j].xObsDest2, matrix[i, j].yObsDest2, true);
                                   matrix[i, j] = new VoidCell();

                                }
                            }
                            else
                            {
                                shortestPath2 = SolveObs(i, j, matrix[i, j].xObsDest2, matrix[i, j].yObsDest2);
                                if (shortestPath2 == null) break;
                                newX = shortestPath2[1].Item1;
                                newY = shortestPath2[1].Item2;


                                if (matrix[newX, newY].type == 0)
                                {
                                   matrix[newX, newY] = new ObstacleCell(idCounter, matrix[i, j].xObsDest1, matrix[i, j].yObsDest1, matrix[i, j].xObsDest2, matrix[i, j].yObsDest2, false);
                                   matrix[i, j] = new VoidCell();

                                    
                                }
                            }
                            idCounter++;
                        }
                    }
                }
            }
            */

        }

        public void CellularAutomata(bool isRobotPunished, int voidVal, int obsVal)
        {
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j].type == 0)
                    {
                        matrixValues[i, j] = voidVal;
                    }
                    else if (matrix[i, j].type == 3)
                    {
                        matrixValues[i, j] = obsVal;
                    }
                    else if (matrix[i, j].type == 1)
                    {
                    }
                    else
                    {
                        matrixValues[i, j] = 1;
                    }
                }
            }
                    

            //8 Cells around obstacle increase value
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    
                    if (matrix[i, j].type == 3)
                    {
                        for (int ii = i - 1; ii <= i + 1; ii++)
                        {
                            for (int jj = j - 1; jj <= j + 1; jj++)
                            {
                                //if (ii == i && jj == j) continue;
                                if (matrix[ii, jj].type == 1 || matrix[ii, jj].type == 4) continue;

                                matrixValues[ii, jj] += matrix[i,j].value;
                            }
                        }
                    }


                }
            }

            //other void cells increase/decrease value depend on surrounding (CA)
            int free = 0;
            double up, down=0;

            while (free < 2)
            {
                if(free == 0)
                {
                    for (int i = 1; i < size - 1; i++)
                    {
                        for (int j = 1; j < size - 1; j++)
                        {

                            if (matrix[i, j].type == 0)
                            {
                                up = 0;
                                for (int ii = i - 1; ii <= i + 1; ii++)
                                {
                                    for (int jj = j - 1; jj <= j + 1; jj++)
                                    {
                                        if (ii == i && jj == j) continue;
                                        if (matrix[ii, jj].type == 1 || matrix[ii, jj].type == 4) continue;

                                        up = Math.Max(matrix[ii, jj].value, up);
                                        down += 1;
                                    }
                                }
                                if (down == 0) continue;
                                matrixValues[i, j] = (int)Math.Ceiling(up / 2);

                            }

                        }
                    }
                }
                else
                {
                    for (int i = 1; i < size - 1; i++)
                    {
                        for (int j = 1; j < size - 1; j++)
                        {

                            if (matrix[i, j].type == 0)
                            {
                                up = 0;
                                for (int ii = i - 1; ii <= i + 1; ii++)
                                {
                                    for (int jj = j - 1; jj <= j + 1; jj++)
                                    {
                                        if (ii == i && jj == j) continue;
                                        if (matrix[ii, jj].type == 1 || matrix[ii, jj].type == 4) continue;

                                        up = Math.Max(matrixValues[ii, jj], up);
                                        down += 1;
                                    }
                                }
                                if (down == 0) continue;
                                matrixValues[i, j] = (int)Math.Ceiling(up /2);

                            }

                        }
                    }
                }
                free++;
            }

            //If robot is punished
            if (isRobotPunished)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrixValues[i, j] += matrixPunishmentValues[i, j];
                    }
                }
            }

            //add walls infinity
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(matrix[i,j].type == 1)
                    {
                        matrixValues[i, j] = int.MaxValue;
                    }
                }
            }

        }

        public string valuePrint()
        {
            string s = "";
            //s += "\n\n";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(matrixValues[i, j] == int.MaxValue)
                    {
                        s += "# ";
                    }
                    else
                    {
                        //s += matrixValues[i, j].ToString("0.00") + " ";
                        s += matrixValues[i, j] + " ";
                    }
                    
                }
                s += "\n";
            }
            return s;
        }
        public string valueGet(int x, int y)
        {
            string s = "";
            s += matrixValues[x, y];
            return s;
        }


    };






    public class Cell
    {
        public int obstacleValue = 3, voidValue = 1;

        public int type, value, id;
        public int xObsDest1, yObsDest1;
        public int xObsDest2, yObsDest2;
        public bool Destination;
    };
    class VoidCell : Cell
    {
        public VoidCell()
        {
            type = 0;
            value = voidValue;
        }
    };
    class WallCell : Cell
    {
        public WallCell()
        {
            type = 1;
            value = int.MaxValue;
        }
    };
    class RobotCell : Cell
    {
        public RobotCell()
        {
            type = 2;
            value = 0;
        }
    };
    class ObstacleCell : Cell
    {
        public int x, y;
        public ObstacleCell(int newid, bool wayDecision)
        {
            id = newid;
            type = 3;
            value = obstacleValue;
            Destination = wayDecision;
        }
        public ObstacleCell(int newid, int dest1x, int dest1y, int dest2x, int dest2y, bool wayDecision, int X, int Y)
        {
            x = X;
            y = Y;
            id = newid;
            type = 3;
            value = obstacleValue;
            xObsDest1 = dest1x;
            yObsDest1 = dest1y;
            xObsDest2 = dest2x;
            yObsDest2 = dest2y;
            Destination = wayDecision;
        }
    };
    class ExitCell : Cell
    {

        public ExitCell()
        {
            type = 4;
            //value = int.MaxValue;
        }
    };
}
