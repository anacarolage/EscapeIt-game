using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeRoom
{
    class Score
    {
        public string name = "";
        public TimeSpan time;

        public Score(string name, TimeSpan time)
        {
            this.name = name;
            this.time = time;

        }


        public static List<Score> ParseScore(List<string> scores)
        {
            List<Score> list = new List<Score>();
            for (int i = 0; i<scores.Count; i++)
            {
                string[] array = scores[i].Split(';');
                Score item = new Score(array[0], TimeSpan.Parse(array[1]));

                list.Add(item);
            }
            return list;
        }

        public static List<string> ScoreToString(List<Score> scores)
        {
            List<string> listString = new List<string>();
            for(int i = 0; i<scores.Count; i++)
            {
                string line = scores[i].name + ";" + scores[i].time.ToString(@"hh\:mm\:ss");
                listString.Add(line);
            }

            return listString;
        }

    }
}
