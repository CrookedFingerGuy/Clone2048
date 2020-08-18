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
        public class HighScoreWithName:IComparable
        {
            public int score;
            public string name;

            public HighScoreWithName(int sc,string na)
            {
                score = sc;
                name = na;
            }

            public HighScoreWithName()
            {

            }

            int IComparable.CompareTo(object obj)
            {
                HighScoreWithName h1 = (HighScoreWithName)obj;
                if (h1.score > this.score)
                    return 1;

                if (h1.score < this.score)
                    return -1;
                else
                    return 0;
            }
        }

        

        public List<HighScoreWithName> scores;
        public HighScores()
        {
            scores = new List<HighScoreWithName>();
        }

        public bool CheckIfNewHighScore(int checkValue)
        {
            foreach(HighScoreWithName sc in scores)
            {
                if (checkValue > (int)sc.score)
                    return true;
            }
            return false;
        }

        public void InsertNewHighScore(int newScore,string newName)
        {            
            scores.RemoveAt(scores.Count-1);

            scores.Add(new HighScoreWithName(newScore,newName));

            scores.Sort();
        }
    }
}
