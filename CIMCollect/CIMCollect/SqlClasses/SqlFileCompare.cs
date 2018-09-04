using System;
using System.Collections.Generic;
using System.IO;
using StringExtensionsIsLike;


namespace Similarity
{
    class SqlFileCompare
    {
        SqlAbstract sHash = new SqlAbstract();

        //public float CompareSql(Hasher h, List<string> otherdirs, string basefile)
        public float CompareSql(List<string> otherdirs, string basefile)
        {
            float smallestMatchWords = float.MaxValue;
            Console.Write($"{basefile}   ");
            var thisFilename = Path.GetFileName(basefile);
            var rawbase = File.ReadAllLines(basefile);
            //var basestring = SqlCleaner.Clean(rawbase, HideAscii: false);
            //var BaseHasAscii = sHash.AddScript(basestring);
            //var baseAbstract = sHash.ToString();
            var baseAbstract = sHash.Abstract(rawbase);
            var baseHasAscii = sHash.HasAscii;
            var baseHash = sHash.Allhash;
            sHash.Clear();
            Console.WriteLine(baseAbstract.MaxLength(14));

            var basehash = new SqlAbstract(baseAbstract);

            ////var basehash =  h.Phrase(basestring);
            //var bashHash = h.UInt32Hash(basestring);
            //var basewordhash = h.UInt32HashSql(basestring);
            //File.AppendAllLines("AllSql.csv", h.SqlWords(basestring));

            foreach (var xdir in otherdirs)
            {
                var pathnames = xdir.Split(Path.DirectorySeparatorChar);
                var thispathname = pathnames[3];
                var otherfilename = Path.Combine(xdir, thisFilename);
                if (File.Exists(otherfilename))
                {
                    var rawother = File.ReadAllLines(otherfilename);
                    //var otherstring = SqlCleaner.Clean(rawother, HideAscii: false);
                    //var HasAscii = sHash.AddScript(otherstring);
                    //var otherAbstract = sHash.ToString();
                    var otherAbstract = sHash.Abstract(rawother);
                    if (sHash.HasAscii)
                    {
                        ;
                    }
                    //var otherZip = sHash.ToGZ();  // note to self, you can't easliy compress compressed stuff
                    //var otherUnzip = MemoryZip.UnZip(otherZip);
                    //if (otherAbstract != otherUnzip) throw new Exception("Zip/Unzip failed");
                    sHash.Clear();
                    //Console.WriteLine(String.Format("{0,11}.{1}", thispathname, thisFilename));
                    //float compressed = otherstring.Length == 0 ? 0.0f : (float)otherAbstract.Length / (float)otherstring.Length;
                    //float zipped = otherZip.Length == 0 ? 0.0f : (float)otherZip.Length / (float)otherstring.Length;
                    //if (compressed > zipped)
                    //{
                    //    ;
                    //}
                    //Console.Write((compressed < zipped) ? " c " : " z "); 
                    //Console.Write(otherAbstract);
                    var matchAbstract = String.CompareOrdinal(baseAbstract, otherAbstract) == 0 ? " MATCH" : " at...";
                    float matchPercent = basehash.Similarity(otherAbstract);
                    if (matchPercent < smallestMatchWords) smallestMatchWords = matchPercent;
                    //Console.WriteLine(String.Format(" {0} {1,7:P2} Size {2,7:P2} {3}", matchAbstract, matchPercent, compressed, otherAbstract.MaxLength(14)));
                    //Console.WriteLine(String.Format(" {0} {1,7:P2} {2}", matchAbstract, matchPercent, otherAbstract.MaxLength(14)));

                }
            }
            Console.WriteLine();
            return smallestMatchWords;
        }

    }
}
