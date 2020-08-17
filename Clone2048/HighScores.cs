using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clone2048
{
    [Serializable]
    public class HighScores
    {
        public List<int> scores;
        public List<string> names;
        public HighScores()
        {
            scores = new List<int>();
            names = new List<string>();
            for(int i=0;i<10;i++)
            {
                scores.Add(0);
                names.Add("....................");
            }
        }

        public bool CheckIfNewHighScore(int checkValue)
        {
            foreach(int score in scores)
            {
                if (checkValue > score)
                    return true;
            }
            return false;
        }

        public void InsertNewHighScore(int newScore,string newName)
        {
            scores.RemoveAt(scores.Count);
            names.RemoveAt(names.Count);

            scores.Add(newScore);
            names.Add(newName);

            scores.Sort();
            names.Sort();
        }
    }
}
