using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09Section_Cal
{
    internal class Report
    {
        public static void OpenShow(DataCenter data, DataGridView dataGridView)
        {
            foreach (Point p in data.KP_Points)
            {
                DataGridViewRow row = new DataGridViewRow();
                string[] values = { p.Name, p.X.ToString("f3"), p.Y.ToString("f3"), p.H.ToString("f3") };
                foreach (string value in values)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = value;
                    row.Cells.Add(cell);
                }
                dataGridView.Rows.Add(row);
            }
        }
        public static void Cal_Ver_Show(Algo go, RichTextBox richTextBox)
        {
            String text = null;
            text += "---------------纵断面计算结果---------------\r\n";
            text += $"纵断面长度：{go.Ver_Len:f3}\r\n";
            text += $"纵断面面积：{go.Ver_S:f3}\r\n";
            text += "线路主点：\r\n";
            text += $"{"点名",-8}{"X(m)",-12}{"Y(m)",-12}{"Z(m)",-12}\r\n";
            foreach(Point p in go.Z_Points)
            {
                text += $"{p.Name,-8}{p.X,-12:f3}{p.Y,-12:f3}{p.H,-12:f3}\r\n";
            }
            richTextBox.Text = text;
        }
        public static void Cal_Hor1_Show(Algo go, RichTextBox richTextBox)
        {
            String text = null;
            text += "---------------横断面 1 计算结果---------------\r\n";
            text += $"横断面 1 面积：{go.Hor1_S:f3}\r\n";
            text += "线路主点：\r\n";
            text += $"{"点名",-8}{"X(m)",-12}{"Y(m)",-12}{"Z(m)",-12}\r\n";
            foreach (Point p in go.N1_Points)
            {
                text += $"{p.Name,-8}{p.X,-12:f3}{p.Y,-12:f3}{p.H,-12:f3}\r\n";
            }
            richTextBox.Text = text;
        }
        public static void Cal_Hor2_Show(Algo go, RichTextBox richTextBox)
        {
            String text = null;
            text += "---------------横断面 2 计算结果---------------\r\n";
            text += $"横断面 2 面积：{go.Hor2_S:f3}\r\n";
            text += "线路主点：\r\n";
            text += $"{"点名",-8}{"X(m)",-12}{"Y(m)",-12}{"Z(m)",-12}\r\n";
            foreach (Point p in go.N2_Points)
            {
                text += $"{p.Name,-8}{p.X,-12:f3}{p.Y,-12:f3}{p.H,-12:f3}\r\n";
            }
            richTextBox.Text = text;
        }
    }
}
