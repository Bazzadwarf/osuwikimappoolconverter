using System;
using System.IO;

namespace osuwikimappoolconverter
{
    class Program
    {
        //static string[] Serieses = { "UKCC", "OUKT" };

        static string[] MappoolHeaderTranslations = { "## Mappools", // en
                                         "## Mappool", // id
                                         "## 图池", // zh
                                         "## 比赛图池", // zh
                                         "## 圖池", // zh-tw
                                         "## 比賽圖池", // zh-tw
                                         "## 맵풀", // ko
                                         "## 비트맵 풀", // ko
                                         "## Grupos de mapas", // es
                                         "## Paquetes de Mapa", // es
                                         "## Mappoole", // pl
                                         "## Lista de Beatmaps", // pt
                                         "## Список карт", // ru
                                         "## Пул карт", // ru
                                         "## マッププール", // ja
                                         "## ビートマッププール(課題曲)", // ja
                                         "## Map Havuzları", // tr
                                         "## Beatmaps à disposition", // fr
                                         "## Liste des maps", // fr
                                         "## Beatmaps"}; // fr

        static bool IsMappoolHeader(string s)
        {
            foreach (var Header in MappoolHeaderTranslations)
            {
                if (s == Header)
                {
                    return true;
                }
            }

            return false;
        }
        
        static void Main(string[] args)
        {             
            foreach (var Series in Directory.GetDirectories("C:\\osu-wiki\\wiki\\Tournaments")) // Replace with the location of osu-wiki tournament folder
            {
                foreach (var TournamentDir in Directory.GetDirectories(Series))
                {
                    foreach (var TournamentFile in Directory.GetFiles(TournamentDir))
                    {
                        if (TournamentFile.Contains(".md"))
                        {
                            Console.WriteLine(TournamentFile);
                            string[] newContent = CheckMappools(TournamentFile);
                            File.WriteAllText(TournamentFile, string.Join("\n", newContent) + "\n");
                        }
                    }
                }
            }
        }

        static string[] CheckMappools(string file)
        {
            string[] Content = File.ReadAllLines(file);

            bool InMappoolSection = false;
            int Counter = 0;

            for (int i = 0; i < Content.Length; i++)
            {
                if (Content[i].StartsWith("## ") && InMappoolSection)
                {
                    break;
                }

                if (IsMappoolHeader(Content[i]))
                {
                    InMappoolSection = true;
                }

                if (!InMappoolSection)
                {
                    continue;
                }

                if (Counter > 0 && !Content[i].StartsWith("  "))
                {
                    Counter = 0;
                }

                //if (Content[i].StartsWith(" -"))
                //{
                //    break;
                //}

                if (Content[i].StartsWith("  "))
                {
                    int dashLocation = Content[i].IndexOf('-');
                    if (dashLocation < 5 && dashLocation > -1)
                    {
                        Counter++;
                        Content[i] = Content[i].Remove(dashLocation, 1).Insert(dashLocation, Counter + ".");
                    }
                }

                //Console.WriteLine(Content[i]);
            }

            if (!InMappoolSection)
            {
                Console.WriteLine(file + " no mappool section found");
            }

            return Content;
        }
    }
}
