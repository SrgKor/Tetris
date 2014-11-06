using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris_1._0
{
    // класс отвечает за поиск и удаление заполненных линий в колодце (массив 10х20)
    class KolodezManager
    {
        internal bool УдалиНахренЗаполненныеЛинииИзКолодцаЕслиЕсть()
        {
            bool fl = false;
            for (int y = 19; y > 0; y--)
                if (IsCompleteLine(y))
                {
                    RemoveThisLineAndDrop(y);
                    y++;
                    fl = true;
                }
            return fl;
        }

        private void RemoveThisLineAndDrop(int y)
        {
            for (int yy = y; yy > 0; yy--)
                for (int x = 0; x < 10; x++)
                    Form1_GameField.kolodez[x, yy] = Form1_GameField.kolodez[x, yy - 1];
            Form1_GameField.score += 10;
            
        }

        private bool IsCompleteLine(int y)
        {
            for (int x = 0; x < 10; x++)
                if (Form1_GameField.kolodez[x, y] == 0)
                    return false;
            return true;
        }
    }
}
