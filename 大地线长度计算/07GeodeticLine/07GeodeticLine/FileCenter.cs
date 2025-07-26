using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _07GeodeticLine
{
    internal class FileCenter
    {
        public string[] buf1;
        public string[] buf2;
        public void ReadFile(string path,DataCenter data)
        {
            StreamReader reader = new StreamReader(path);           
            string Line1 = reader.ReadLine();
            buf1 = Line1.Split(' ');

            data.B1 = data.ddmmssTorad(Convert.ToDouble(buf1[0]));
            data.L1 = data.ddmmssTorad(Convert.ToDouble(buf1[1]));

            string Line2 = reader.ReadLine();
            buf2 = Line2.Split(' ');

            data.B2 = data.ddmmssTorad(Convert.ToDouble(buf2[0]));
            data.L2 = data.ddmmssTorad(Convert.ToDouble(buf2[1]));
        }
    }
}
