using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace subtitleTL
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetTempFileName();
            path = @"I:\Archive\Archive Progs\I2660108s.stl";
            //path = args[0];
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                Debug.WriteLine(fs.Length);
                byte[] b = new byte[fs.Length];
                byte[] n = new byte[0x80];
                for (int i = 0; i < n.Length; i++)
                    n[i] = 0x8F;
                UTF8Encoding temp = new UTF8Encoding(true);

                fs.Read(b, 0x000, b.Length);
               
                int j = 0x400;
                int count = 0;
                while (j < fs.Length)
                {
                    byte old = 0;
                    int c = 0;
                    for (int i = j; i < j+0x7F; i++)
                    {
                        if ((b[i] == old) && (old == 0x8A))
                        {
                            //Debug.WriteLine("{0:X}", b[i]);
                            count++;
                        }
                        else
                        {
                            n[c++] = b[i];
                        }
                        old = b[i];

                    }
                    for (int i = 0; i < n.Length; i++)
                    {
                        b[j + i] = n[i];
                        //Debug.WriteLine(n[i]);
                    }
                    j = j + 0x80;
                }
                Debug.WriteLine("8A 8A ocurrences = " + count);
                fs.Close();

                using (FileStream fs2 = File.Open("monka.stl", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                   
                    // Add some information to the file.
                    fs2.Write(b, 0, b.Length);
                    fs2.Close();
                }
            }
        }
    }
}
