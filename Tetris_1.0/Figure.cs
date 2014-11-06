using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris_1._0
{
    class Figure
    {
        public int x, y; 
        public bool[,] picture; // матрица фигуры
        public int color; // номер цвета
        private Random random;

        public Figure() // конструктор случайной фигуры случайного цвета
        {
            x = 3; y = 0;
            random = new Random();
            color = random.Next(1, 10);
            int number = random.Next(0, 5);
            picture = new bool[4, 4];

            switch (number)
            {
                case 0:
                    picture[0, 1] = true; picture[0, 2] = true; picture[1, 2] = true; picture[2, 2] = true;
                    break;
                case 1:
                    picture[0, 1] = true; picture[1, 1] = true; picture[2, 1] = true; picture[3, 1] = true;
                    break;
                case 2:
                    picture[1, 1] = true; picture[2, 1] = true; picture[1, 2] = true; picture[2, 2] = true;
                    break;
                case 3:
                    picture[1, 2] = true; picture[2, 2] = true; picture[3, 2] = true; picture[2, 1] = true;
                    break;
                case 4:
                    picture[2, 0] = true; picture[1, 1] = true; picture[2, 1] = true; picture[1, 2] = true;
                    break;
                default:
                    break;
            }

        }

        // метод вращения фигуры (по часовой стрелке)
        public void reflex1()
        {
            bool[,] picture1 = new bool[4, 4];
            for (int i = 0; i < 4; i++) // отразить матрицу фигуры по диагонали
                for (int j = 0; j < 4; j++)
                    picture1[j, i] = picture[i, j];
            picture = picture1;

            bool[,] picture2 = new bool[4, 4];
            for (int i = 0; i < 4; i++) // отразить матрицу фигуры по вертикали
                for (int j = 0; j < 4; j++)
                    picture2[3 - i, j] = picture[i, j];
            picture = picture2;
        }
    }
}
