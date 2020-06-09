using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();

        }

        private void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            resolution = (int)nudResolution.Value;

            cols = pictureBox1.Width / resolution;
            rows = pictureBox1.Height / resolution;


            Random random = new Random();
            field = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();

        }
        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];


            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursCount = CountNeighbours(x, y);
                    bool isLife = field[x, y];

                    if (!isLife && neighboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (isLife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }

                    if (isLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);

                    }
                }

            }
            field = newField;
            pictureBox1.Refresh();


        }



        private int CountNeighbours(int x, int y)
        {
            int countOfNeightbours = 0;
            int leftTopX = x - 1;
            int leftTopY = y - 1;
            for (int sqrX = leftTopX; sqrX < leftTopX + 3; sqrX++)
            {
                if ((sqrX < 0) || (sqrX >= cols))
                {
                    continue;
                }
                for (int sqrY = leftTopY; sqrY < leftTopY + 3; sqrY++)
                {
                    if ((sqrY < 0) || (sqrY >= rows))
                    {
                        continue;
                    }
                    if (sqrX == x && sqrY == y)
                    {
                        continue;
                    }

                    if (field[sqrX, sqrY])
                    {
                        countOfNeightbours++;
                    }

                }

            }

            return countOfNeightbours;
        }
        private void StopGame()
        {
            if (!timer1.Enabled)
            {
                return;
            }
            timer1.Stop();
            //graphics.Clear(Color.Black);
            //pictureBox1.Refresh();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();

        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();

            //resolution = (int)nudResolution.Value;
            //pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //graphics = Graphics.FromImage(pictureBox1.Image);
            //graphics.FillRectangle(Brushes.Crimson, 0, 0, resolution, resolution);
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                bool validationPassed = ValidateMousePosition(x, y);
                if (validationPassed)
                {
                    field[x, y] = true;
                }
                

            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                bool validationPassed = ValidateMousePosition(x, y);
                if (validationPassed)
                {
                    field[x, y] = false;
                }

            }

        }
        private bool ValidateMousePosition(int x, int y)
        {
            return x>=0 && y>=0 && x < cols && y < rows; 
        }
    }
}
