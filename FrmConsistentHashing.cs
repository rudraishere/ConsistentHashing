using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsistentHashingLib;

namespace ConsistentHashingNs
{
    public partial class FrmConsistentHashing : Form
    {
        public FrmConsistentHashing()
        {
            InitializeComponent();
        }
        private List<string> data = new List<string> // data keys 
        {
            "6EZ4bsIPhy",
            "vVeDjw3zSw",
            "vj2MNQWvWB",
            "PiyhXGhT9f",
            "NTjhhoRDpt",
            "fG1XyClmCQ",
            "YaHwT70iSd",
            //"q5TWTBgWqu",
            //"uoZkt6Pnnr",
            //"aBzaKP1ZpH",
            //"wSY1rRAR3C",
            //"xKMmaJYeZU",
            //"xL8uIdk3fj",
            //"Go1xEQS1Hd",
            //"qYKT3pYa9C",
            //"1khrRqiKel",
            //"JOAL4cxxNX",
            //"z74xh4MK8p",
            //"PUHEqU09KL",
            //"M6fuv2x5D5",
            //"AcrxZRhZ16",
            //"TgBn7IsTxf",
            //"uxInJ1Pq6i",
            //"iZbjR2hqK9",
            //"t5X5ZwiNiQ",
            //"Z2NiJ85wRy",
            //"cFH5s3LDrk",
            //"KgR5W84UXV",
            //"aQ2WXSeVI2",
            //"jkeeAhxqnw",
            //"71BsbEOVqb",
            //"RofzcgGLVA",
        };
        private List<string> nodes = new List<string> // Node keyss
        {
            "RUDYTEST01",
            "RUDYTEST02",
            "RUDYTEST03",
            "RUDYTEST04",
            "RUDYTEST05",
            //"RUDYTEST06",
            //"RUDYTEST07",
            //"RUDYTEST08",
            //"RUDYTEST09",
            //"RUDYTEST10",
            //"RUDYTEST11",
            //"RUDYTEST12",
            //"RUDYTEST13",
            //"RUDYTEST14",
            //"RUDYTEST15",
            //"RUDYTEST16"
        };
        private int _nodeSpaceSize = 360; // low value will have less crowded node and data labels
        private ConsistentHashing _csObj;
        
        private void FrmConsistentHashing_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Set data
            _csObj = new ConsistentHashing(nodes, data, _nodeSpaceSize, NodeSpacing.Equidistant);
            _csObj.SetNodes();
            //Use this section to call the node operations
            //_csObj.AddNode("GGHSRTDF34");
            //_csObj.RemoveNode("RUDYTEST10");

            // Draw a circle.
            float cx = 850 / 2f;
            float cy = 850 / 2f;
            float rx = Math.Min(cx, cy) - 10;
            float ry = rx;
            var nodePen = new Pen(Color.Orange, 8.0F);
            var dataPen = new Pen(Color.Green, 5.0F);

            DrawTickedCircle(e.Graphics, Pens.Blue, nodePen, dataPen,
                cx, cy, rx, ry, 100, 0.1f);
        }

        private void DrawTickedCircle(
            Graphics gr, Pen circle_pen, Pen tick_pen, Pen data_pen,
            float cx, float cy, float rx, float ry,
            float num_theta,
             float tick_fraction)
        {
            // Draw the circle.
            List<PointF> points = new List<PointF>();
            float dtheta = (float)(2 * Math.PI / num_theta);
            float theta = 0;
            for (int i = 0; i < num_theta; i++)
            {
                float x = (float)(cx + rx * Math.Cos(theta));
                float y = (float)(cy + ry * Math.Sin(theta));
                points.Add(new PointF(x, y));
                theta += dtheta;
            }
            gr.DrawPolygon(circle_pen, points.ToArray());

            // Draw the tick marks.
            var nodes = _csObj.NodeSet;
            dtheta = (float)(2 * Math.PI / nodes.Count);
            theta = 0;
            float rx1 = rx * (1 - tick_fraction);
            float ry1 = ry * (1 - tick_fraction);
            for (int i = 0; i < nodes.Keys.Count; i++)
            {
                var degreeAngle = _csObj.NodeMap.Keys.ToList()[i];
                var nodeAngle = (float)Math.Round(degreeAngle * (2 * Math.PI / _nodeSpaceSize), 6);
                float x1 = (float)(cx + rx * Math.Cos(nodeAngle));
                float y1 = (float)(cy + ry * Math.Sin(nodeAngle));
                float x2 = (float)(cx + rx1 * Math.Cos(nodeAngle));
                float y2 = (float)(cy + ry1 * Math.Sin(nodeAngle));
                gr.DrawLine(tick_pen, x1, y1, x2, y2);
                using (Font font1 = new Font("Times New Roman", 15, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    PointF pointF1 = new PointF(x2, y2);
                    gr.DrawString(_csObj.NodeMap[degreeAngle].ToString() + "(" + degreeAngle + ")", font1, Brushes.Blue, pointF1);
                }
                nodeAngle += dtheta;

                var data = nodes[nodes.Keys.ToList()[i]];

                for (int j = 0; j < data.Count; j++)
                {
                    var dataAngle = (float)Math.Round(data[j] * (2 * Math.PI / _nodeSpaceSize), 6);
                    float p1 = (float)(cx + rx * Math.Cos(dataAngle));
                    float q1 = (float)(cy + ry * Math.Sin(dataAngle));
                    float p2 = (float)(cx + rx1 * Math.Cos(dataAngle));
                    float q2 = (float)(cy + ry1 * Math.Sin(dataAngle));
                    gr.DrawLine(data_pen, p1, q1, p2, q2);
                    using (Font font1 = new Font("Times New Roman", 10, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        PointF pointF1 = new PointF(p1, q1);
                        gr.DrawString(data[j].ToString() + "(" + _csObj.NodeMap[degreeAngle].ToString() + ")", font1, Brushes.Red, pointF1);
                    }
                    dataAngle += dtheta;
                }
            }
        }
    }
}
