using Evade;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Evade.Util
{
    static class HighScoreManager
    {
        //read attribute
        public static string ReadAttribute(string attribute)
        {
            if (!File.Exists(Constants.HIGHSCOREFILE))
            {
                CreateTemplateFile();
            }
            string value;
            try
            {
                using (StreamReader sr = new StreamReader(Constants.HIGHSCOREFILE, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        value = sr.ReadLine();
                        if (value.Length > 0 && value[0] != '#')
                        {
                            if (value.Remove(value.IndexOf('=')) == attribute)
                            {
                                if (value.IndexOf('=') != value.Length)
                                    return value.Substring(value.IndexOf('=') + 1);
                            }
                        }
                    }
                }
            }
            catch (IOException) { }
            return "";
        }

        //write attribute
        public static void WriteAttribute(string attribute, string value)
        {

            if (!File.Exists(Constants.HIGHSCOREFILE))
            {
                CreateTemplateFile();
            }
            try
            {
                List<string> fContent = new List<string>();
                using (StreamReader sr = new StreamReader(Constants.HIGHSCOREFILE, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        fContent.Add(sr.ReadLine());
                    }
                }
                for (int i = 0; i < fContent.Count; i++)
                {
                    if (fContent[i].Length > 0 && fContent[i][0] != '#')
                    {
                        if (fContent[i].Remove(fContent[i].IndexOf('=')) == attribute)
                        {
                            fContent[i] = attribute + "=" + value;
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(Constants.HIGHSCOREFILE, false, Encoding.UTF8))
                {
                    foreach (string line in fContent)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            catch (IOException) { }
        }

        private static void CreateTemplateFile()
        {
            try
            {
                using (FileStream fs = new FileStream(Constants.HIGHSCOREFILE, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("#highscore");
                        sw.WriteLine("name=XXX");
                        sw.WriteLine("time=0,0");
                    }
                }
            }
            catch (IOException) { }
        }
    }
}
