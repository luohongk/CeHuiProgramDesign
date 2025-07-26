using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10Trop_NEIL
{
    internal class Report
    {
        public static string Cal_Show(DataCenter data)
        {
            string text = "--------------计算结果-------------\n\n";
            text += $"{"Station",-10}{"E",-6}{"ZHD",-8}{"m_d(E)",-8}{"ZwD",-8}{"m_w(E)",-8}{"delta_S",-8}\n";
            foreach(Epoch e in data.epoches)
            {
                text += $"{e.Station_Name,-10}{e.E,-6}{e.ZHD,-8:f3}{e.mdE,-8:f3}{e.ZWD,-8:f3}{e.mwE,-8:f3}{e.delta_S,-8:f3}\n";
            }
            return text;
        }
    }
}
