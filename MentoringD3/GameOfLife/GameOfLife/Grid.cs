using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {
        private int SizeX;
        private int SizeY;
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private static Random rnd;
        private Canvas drawCanvas;
        private Ellipse[,] cellsVisuals;

        private static List<long> timing = new List<long>();

        public Grid(Canvas c)
        {
            this.drawCanvas = c;
            rnd = new Random();
            this.SizeX = (int)(c.Width / 5);
            this.SizeY = (int)(c.Height / 5);
            this.cells = new Cell[this.SizeX, this.SizeY];
            this.nextGenerationCells = new Cell[this.SizeX, this.SizeY];
            this.cellsVisuals = new Ellipse[this.SizeX, this.SizeY];

            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    this.cells[i, j] = new Cell(i, j, 0, false);
                    this.nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                }

            SetRandomPattern();
            InitCellsVisuals();
            UpdateGraphics();
        }

        public void Clear()
        {
            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    this.cells[i, j] = new Cell(i, j, 0, false);
                    this.nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    this.cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }

        void MouseMove(object sender, MouseEventArgs e)
        {
            var cellVisual = sender as Ellipse;

            int i = (int)cellVisual.Margin.Left / 5;
            int j = (int)cellVisual.Margin.Top / 5;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.cells[i, j].IsAlive)
                {
                    this.cells[i, j].IsAlive = true;
                    this.cells[i, j].Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    this.cellsVisuals[i, j].Fill = this.cells[i, j].IsAlive
                                                  ? (this.cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
                                                  : Brushes.Gray;
                }
        }

        public void InitCellsVisuals()
        {
            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    this.cellsVisuals[i, j] = new Ellipse();
                    this.cellsVisuals[i, j].Width = this.cellsVisuals[i, j].Height = 5;
                    double left = this.cells[i, j].PositionX;
                    double top = this.cells[i, j].PositionY;
                    this.cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
                    this.cellsVisuals[i, j].Fill = Brushes.Gray;
                    this.drawCanvas.Children.Add(this.cellsVisuals[i, j]);

                    this.cellsVisuals[i, j].MouseMove += this.MouseMove;
                    this.cellsVisuals[i, j].MouseLeftButtonDown += this.MouseMove;
                }
            UpdateGraphics();
        }

        public static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.5;
        }

        public void SetRandomPattern()
        {
            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                    this.cells[i, j].IsAlive = GetRandomBoolean();
        }

        public void UpdateToNextGeneration()
        {
            var a = new Stopwatch();
            a.Start();

            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    this.cells[i, j].IsAlive = this.nextGenerationCells[i, j].IsAlive;
                    this.cells[i, j].Age = this.nextGenerationCells[i, j].Age;
                }
            UpdateGraphics();

            a.Stop();
            //timing.Add(a.ElapsedMilliseconds);
        }

        public void Update()
        {
            var a = new Stopwatch();
            a.Start();

            bool alive = false;
            int age = 0;

            for (int i = 0; i < this.SizeX; i++)
                for (int j = 0; j < this.SizeY; j++)
                {
                    if (CalculateNextGeneration(i, j, ref alive, ref age))
                    {
                        this.nextGenerationCells[i, j].IsAlive = alive;
                        this.nextGenerationCells[i, j].Age = age;
                    }
                }

            //Parallel.For(0, this.SizeX, x =>
            //{
            //    Parallel.For(0, this.SizeY, y =>
            //    {
            //        bool alive = false;
            //        int age = 0;

            //        if (CalculateNextGeneration(x, y, ref alive, ref age))
            //        {
            //            this.nextGenerationCells[x, y].IsAlive = alive;
            //            this.nextGenerationCells[x, y].Age = age;
            //        }
            //    });
            //});

            UpdateToNextGeneration();

            a.Stop();
            timing.Add(a.ElapsedMilliseconds);
        }

        public bool CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)
        {
            isAlive = this.cells[row, column].IsAlive;
            age = this.cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
                return true;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                this.cells[row, column].Age++;
                isAlive = true;
                age = this.cells[row, column].Age;
                return true;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
                return true;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
                return true;
            }

            return false;
        }

        public int CountNeighbors(int i, int j)
        {
            int count = 0;

            if (i != this.SizeX - 1 && this.cells[i + 1, j].IsAlive) count++;
            if (i != this.SizeX - 1 && j != this.SizeY - 1 && this.cells[i + 1, j + 1].IsAlive) count++;
            if (j != this.SizeY - 1 && this.cells[i, j + 1].IsAlive) count++;
            if (count > 3) return count;
            if (i != 0 && j != this.SizeY - 1 && this.cells[i - 1, j + 1].IsAlive) count++;
            if (count > 3) return count;
            if (i != 0 && this.cells[i - 1, j].IsAlive) count++;
            if (count > 3) return count;
            if (i != 0 && j != 0 && this.cells[i - 1, j - 1].IsAlive) count++;
            if (count > 3) return count;
            if (j != 0 && this.cells[i, j - 1].IsAlive) count++;
            if (count > 3) return count;
            if (i != this.SizeX - 1 && j != 0 && this.cells[i + 1, j - 1].IsAlive) count++;
            return count;
        }
    }
}