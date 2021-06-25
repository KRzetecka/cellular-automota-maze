using CAApplication;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CAWindowsForms
{
    public partial class CALabirynth : Form
    {
        Labirynth Lab;
        Image imgExit, imgObstacle, imgWall, imgRobot, imgVoid;
        Image currentImage;
        string path, directoryAssets, directoryLabirynths;
        int voidValue, obstacleValue;
        int[,] dist;
        List<Tuple<int, int>> pathToShow;

        public CALabirynth()
        {
            InitializeComponent();
            string Startup = Application.StartupPath;
            Lab = new Labirynth();
            path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            directoryLabirynths = System.IO.Path.GetDirectoryName(path) + @"\labirynths\";
            directoryAssets = System.IO.Path.GetDirectoryName(path) + @"\assets\";
            imgVoid = Image.FromFile(directoryAssets + "Void.png");
            imgRobot = Image.FromFile(directoryAssets + "Robot.png");
            imgWall = Image.FromFile(directoryAssets + "Wall.png");
            imgObstacle = Image.FromFile(directoryAssets + "Obstacle.png");
            imgExit = Image.FromFile(directoryAssets + "Exit.png");


            sclCell.Items.Add("Void");
            sclCell.Items.Add("Obstacle");
            voidValue = 1;
            obstacleValue = 3;

            labirynthRefresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            labirynthRefresh();
        }

        private async void BtnNext_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = false;
            
            NextClick();

            await Task.Delay(100);

            btnNext.Enabled = true;
        }

        private void btnValue_Click(object sender, EventArgs e)
        {
            Value();
        }

        private void Value()
        {
            if (int.TryParse(txtValue.Text, out int n))
            {
                if (n > -1 && n < 1001)
                {
                    if (sclCell.SelectedItem.ToString() == "Void")
                    {
                        Lab.ChangeValues(0, n, n, obstacleValue);
                        voidValue = n;
                    }
                    else if (sclCell.SelectedItem.ToString() == "Obstacle")
                    {
                        Lab.ChangeValues(3, n, voidValue, n);
                        obstacleValue = n;
                    }
                }
                else
                {
                    MessageBox.Show("The vaule needs to be between 0 and 1000");
                }
            }
            else
            {
                //MessageBox.Show("The vaule needs to be a number");
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            Lab.ChangeValues(0, 1, 1, 3);
            voidValue = 1;
            Lab.ChangeValues(3, 3, 1, 3);
            obstacleValue = 3;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            GenerateLabPicture(true);
            try
            {
                if (showLabBox.Image != null)
                {
                    showLabBox.Image.Dispose();
                }
            }
            catch
            {
                Console.WriteLine("obraz siadł");
            }
            currentImage = Image.FromFile(@"assets\tmp.png");
            showLabBox.Image = currentImage;

        }

        private void CALabirynth_Resize(object sender, EventArgs e)
        {
            if (showLabBox.Image == null) return;
            if (showLabBox.Image.Width > showLabBox.Width || showLabBox.Image.Height > showLabBox.Height)
            {
                showLabBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else showLabBox.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void sclLabirynth_Format(object sender, ListControlConvertEventArgs e)
        {
            //string tmp = ((CALabirynth)e.ListItem).Name;
            string tmp = (string)e.Value;
            string[] result = tmp.Split('.');
            tmp = result[0];
            e.Value = tmp;
            //MessageBox.Show(tmp);
        }

        private void sclCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sclCell.SelectedItem.ToString() == "Void")
            {
                txtValue.Text = voidValue.ToString();
            }
            else if (sclCell.SelectedItem.ToString() == "Obstacle")
            {
                txtValue.Text = obstacleValue.ToString();
            }
        }

        private void SclBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lab = new Labirynth(sclLabirynth.SelectedItem.ToString());
        }
        
        private void NextClick()
        {
            try
            {
                if (showLabBox.Image != null)
                {
                    showLabBox.Image.Dispose();
                }
            }
            catch
            {
                Console.WriteLine("Accessing tmp picture error");
            }

            Value();
            Lab.CellularAutomata(chkPunishment.Checked, voidValue, obstacleValue);
            Lab.NextStep();
            //Lab.CellularAutomata(chkPunishment.Checked, voidValue, obstacleValue);
            dist = new int[Lab.size,Lab.size];
            try
            {
                dist = Lab.getDist();
            }
            catch { dist = null; }
            
            pathToShow = Lab.getPath();
            if (Lab.isDone == true) PrintScore();
            //else ConsoleBox.Text = Lab.valuePrint();

            File.Delete(directoryAssets + "tmp.png");

            GenerateLabPicture(false);

            currentImage = GetCopyImage(@"assets\tmp.png");
            showLabBox.Image = currentImage;
        }

        private void labirynthRefresh()
        {
            DirectoryInfo d = new DirectoryInfo(directoryLabirynths);
            FileInfo[] Files = d.GetFiles("*.labirynth");
            sclLabirynth.Items.Clear();
            foreach (FileInfo file in Files)
            {
                sclLabirynth.Items.Add(file.Name);
            }
            if (currentImage != null)
            {
                //currentImage.Dispose();
            }
        }

        private void GenerateLabPicture(bool firsttime)
        {
            if (showLabBox.Image != null)
            {
                showLabBox.Image.Dispose();
            }

            int pixelWidth = 20;
            int labSize = Lab.size;

            using (var bmp = new Bitmap(labSize * pixelWidth, labSize * pixelWidth))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);

                    for (int i = 0; i < labSize; i++)
                    {
                        for (int j = 0; j < labSize; j++)
                        {
                            if (Lab.matrix[i, j].type == 0)
                            {
                                PointF point = new PointF(j * pixelWidth, i * pixelWidth);
                                g.DrawImage(imgVoid, point);

                                if(firsttime == false) {
                                    if(dist !=null){
                                        if (dist[i, j] < 99 && chkShowValues.Checked)
                                        {
                                            //int tmpx = pixelWidth / 2, tmpy = pixelWidth / 2;
                                            PointF pointText = new PointF(j * pixelWidth, i * pixelWidth);
                                            Font arialFont = new Font("Arial", 10);
                                            string number = dist[i, j].ToString();
                                            g.DrawString(number, arialFont, Brushes.Black, pointText);
                                            if (pathToShow != null)
                                            {
                                                foreach (var item in pathToShow)
                                                {
                                                    if (i == item.Item1 && j == item.Item2)
                                                    {
                                                        g.DrawString(number, arialFont, Brushes.LimeGreen, pointText);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                }

                            }
                            if (Lab.matrix[i, j].type == 1)
                            {
                                PointF point = new PointF(j * pixelWidth, i * pixelWidth);
                                g.DrawImage(imgWall, point);
                            }
                            if (Lab.matrix[i, j].type == 2)
                            {
                                PointF point = new PointF(j * pixelWidth, i * pixelWidth);
                                g.DrawImage(imgRobot, point);
                            }
                            if (Lab.matrix[i, j].type == 3)
                            {
                                PointF point = new PointF(j * pixelWidth, i * pixelWidth);
                                g.DrawImage(imgObstacle, point);

                                if (firsttime == false)
                                {
                                    if (dist != null)
                                    {
                                        if (dist[i, j] < 99 && chkShowValues.Checked)
                                        {
                                            //int tmpx = pixelWidth / 2, tmpy = pixelWidth / 2;
                                            PointF pointText = new PointF(j * pixelWidth, i * pixelWidth);
                                            //Font arialFont = new Font("Arial", 10);
                                            Font arialFont2 = new Font("Arial", 10, FontStyle.Bold);
                                            string number = dist[i, j].ToString();
                                            g.DrawString(number, arialFont2, Brushes.Red, pointText);
                                            //g.DrawString(number, arialFont, Brushes.White, pointText);
                                        }
                                    }
                                }

                            }
                            if (Lab.matrix[i, j].type == 4)
                            {
                                PointF point = new PointF(j * pixelWidth, i * pixelWidth);
                                g.DrawImage(imgExit, point);
                            }
                        }
                    }
                    bmp.Save(@"assets\tmp.png", ImageFormat.Png);
                }
            }
        }

        private Image GetCopyImage(string path)
        {
            using (Image im = Image.FromFile(path))
            {
                Bitmap bm = new Bitmap(im);
                return bm;
            }
        }

        private void PrintScore()
        {
            ConsoleBox.Text = "";
            ConsoleBox.Text += "Steps robot made: " + Lab.steps + "\n";
            ConsoleBox.Text += "Idle turns robot made: " + Lab.blocked + "\n";
            ConsoleBox.Text += "All Turns: " + Lab.turns + "\n";
            //ConsoleBox.Text += "Shortest path length: " + Lab.shortests + "\n";
        }

    }
}
