using System.IO;
using System.Text;

namespace Evade.Util
{
    static class HighScoreManager
    {
        public static string ReadAttribute(string attribute)
        {
            if (!File.Exists(Constants.HIGHSCOREFILE))
            {
                CreateTemplateFile();
            }
            try
            {
                using StreamReader sr = new StreamReader(Constants.HIGHSCOREFILE, Encoding.UTF8);
                string value;
                while (!sr.EndOfStream)
                {
                    value = sr.ReadLine();
                    if (value.Length == 0 || value[0] == '#')
                        continue;
                    if (value.Remove(value.IndexOf('=')) == attribute)
                    {
                        if (value.IndexOf('=') != value.Length)
                            return value.Substring(value.IndexOf('=') + 1);
                        else
                            return "";
                    }

                }
            }
            catch (IOException) { }
            return "";
        }

        public static void WriteAttribute(string attribute, string value)
        {
            if (!File.Exists(Constants.HIGHSCOREFILE))
                CreateTemplateFile();
            try
            {
                string[] fContent = File.ReadAllLines(Constants.HIGHSCOREFILE, Encoding.UTF8);
                for (int i = 0; i < fContent.Length; i++)
                {
                    if (fContent[i].Length == 0 || fContent[i][0] == '#') continue;
                    if (fContent[i].Remove(fContent[i].IndexOf('=')) == attribute)
                    {
                        fContent[i] = attribute + "=" + value;
                    }
                }
                File.WriteAllLines(Constants.HIGHSCOREFILE, fContent, Encoding.UTF8);
            }
            catch (IOException) { }
        }

        private static void CreateTemplateFile()
        {
            try
            {
                string[] fContent = new string[] {
                    "#highscore",
                    "name=XXX",
                    "time=0,0"
                };
                File.WriteAllLines(Constants.HIGHSCOREFILE, fContent, Encoding.UTF8);
            }
            catch (IOException) { }
        }
    }
}
