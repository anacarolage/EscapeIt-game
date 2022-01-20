using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;


namespace EscapeRoom
{
    static class FileHandler
    {
        public static void Save(Score score)
        {
            List<string> lines = Load();
            List<Score> listScore = new List<Score>();

            if (lines != null)
            {
               listScore = Score.ParseScore(lines);
            }

            listScore.Add(score);

            listScore = listScore.OrderBy(o => o.time).ToList();

            while (listScore.Count > 10)
            {
                listScore.RemoveAt(listScore.Count - 1);
            }

            List<string> listString = Score.ScoreToString(listScore);

            StreamWriter sw = new StreamWriter(@"../../../high_score/highScore.txt");

            foreach(string s in listString)
            {
                sw.WriteLine(s);

            }

            sw.Flush();
            sw.Close();

        }


        public static List<string> Load()
        {

            List<string> lines = new List<string>();
            string line;

            StreamReader sr = new StreamReader(@"../../../high_score/highScore.txt");
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);

            }

            sr.Close();

            return lines;

        }
    }
}
