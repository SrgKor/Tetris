using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris_1._0
{
    public partial class Form1_GameField : Form
    {
        public static int score = 0; // набранные очки
        public static short fieldWidth = 212;    //ширина поля в пикселях
        public static short fieldHeight = 422;   //высота поля в пикселях
        Brush[] ourColors = {Brushes.PapayaWhip, Brushes.Orange, 
                             Brushes.Blue, Brushes.Red, 
                             Brushes.Green, Brushes.BurlyWood, 
                             Brushes.Violet, Brushes.Tomato, 
                             Brushes.SteelBlue, Brushes.Aqua};
        Graphics g;
        Bitmap field = new Bitmap(fieldWidth, fieldHeight);
        Bitmap[] colorBlocks;
        Figure f;
        public static int[,] kolodez = new int[10, 21]; //колодец есть матрица 10x21, закодированная номерами цветов
        KolodezManager kolodezManager = new KolodezManager();

        public Form1_GameField() //конструктор класса формы
        {
            InitializeComponent();
        }

        
        //создание первой фигуры, инициализация графики
        private void Form1_Load(object sender, EventArgs e) // обработка события Load
        {
            f = new Figure();
            // инициализация рисунка игрового поля field
            g = Graphics.FromImage(field);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 45, 45)), 0, 0, 214, 424);
            
            // инициализируем рисунки кирпичиков
            colorBlocks = new Bitmap[ourColors.Length];
            for (int i = 0; i < ourColors.Length; i++)
            {
                colorBlocks[i] = new Bitmap(21, 21);
                g = Graphics.FromImage(colorBlocks[i]);
                g.FillRectangle(ourColors[i], 0, 0, 21, 21);
                g.DrawLine(new Pen(Brushes.Black, 2), new Point(1, 20), new Point(21, 20));
                g.DrawLine(new Pen(Color.Black, 2), new Point(20, 1), new Point(20, 20));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e) // обработка события Paint
        {
            //отрисовка поля
            e.Graphics.DrawImage(field, 0,0);
            //отрисовка фигуры
            if (f != null)
            {
                for (int x = 0; x < 4; x++)
                    for (int y = 0; y < 4; y++)
                        if (f.picture[x, y])
                            e.Graphics.DrawImage(colorBlocks[f.color], (f.x + x)*21 + 1, (f.y + y)*21 +1);
            }
            //отрисовка блоков колодца
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 20; j++)
                    if (kolodez[i, j] > 0)
                        e.Graphics.DrawImage(colorBlocks[kolodez[i, j]], i * 21 + 1, j * 21 + 1);
            label1.Text = score.ToString();
            label2.Text = timer1.Interval.ToString();
                
        }

        private void timer1_Tick(object sender, EventArgs e) 
        {
            //timer1.Interval = 500;
            
            if (f != null)
            {
                f.y++;
                if (!FigureOk())
                {
                    f.y--;
                    figureStop();
                }
            }
            this.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 37: // влево
                    f.x--;
                    if (!FigureOk())
                        f.x++;
                    break;
                case 39: // вправо
                    f.x++;
                    if (!FigureOk())
                        f.x--;
                    break;
                case 38:// вверх, вращать
                    f.reflex1();
                    if (!FigureOk()) // если фигура выйдет за границы, вернуть в прежнее положение
                    {
                        f.reflex1(); 
                        f.reflex1();
                        f.reflex1();
                    }
                    break;
                case 40://вниз
                    f.y++;
                    if (!FigureOk())
                        f.y--;
                    break;
                case 32://пробел, пауза
                    timer1.Enabled = !timer1.Enabled;
                    break;
            }
            this.Refresh();
        }

        // метод проверяет не залезла ли фигура куда не надо
        public bool FigureOk()
        {
            // выход за боковые границы -- вернуть false
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                        if (f.picture[x, y])
                        {
                            if (f.x + x < 0 | f.x + x > 9) 
                                return false;
                            else if // или выход за нижнюю границу или наезд на колодец - вернуть false
                            ((kolodez[f.x + x, f.y + y] > 0) | (f.y + y == 20))
                                return false;
                        }
            
            return true;
        }

        
        public void figureStop()
        {
            // вписывание фигуры в колодец
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    if (f.picture[x, y])
                        kolodez[f.x + x, f.y + y] = f.color;
            
            bool fl= kolodezManager.УдалиНахренЗаполненныеЛинииИзКолодцаЕслиЕсть();
            if (fl && timer1.Interval > 10)    timer1.Interval -= 10;

            // создание новой фигуры и возможный конец игры
            f = new Figure();
            if (!FigureOk())
            {
                timer1.Enabled = false;
                MessageBox.Show("             SCORE: " + score + "\n           GAME OVER", "Game Over");
            }
        }
    }
}
