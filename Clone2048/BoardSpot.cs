using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clone2048
{
    public class BoardSpot
    {
        public double percentToGenerateA4;
        public int X;
        public int Y;
        public int Value;

        public BoardSpot(double p)
        {
            percentToGenerateA4 = p;
            Value = 0;
        }

        public void GenerateANewPiece(GameStateData lgsd)
        {
            Random rand = new Random();

            if (rand.NextDouble() > percentToGenerateA4)
                this.Value = 2;
            else
                this.Value = 4;

            int blank = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (lgsd.boardValues[i, j] == 0)
                        blank++;
                }
            }

            if (blank != 0)
            {
                int rPos = rand.Next(1, blank);
                int counter = 0; ;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (lgsd.boardValues[i, j] == 0)
                        {
                            counter++;
                            if (counter == rPos)
                            {
                                this.X = i;
                                this.Y = j;
                            }
                        }

                    }
                }
            }
            else
                this.Value = 0;
        }
    }
}
