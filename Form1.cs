using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private Image Sol;
        private Image Earth;
        private Image Moon;
        float IntervalR = 0; 
        float IntervalEar = 0; 
        float IntervalRotateEar = 0;
        float IntervalRotateMoon = 0;
        float IntervalMoonOs = 0; 

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Earth = Image.FromFile("earth.png");
            Moon = Image.FromFile("moon.png");
            Sol = Image.FromFile("sol.png");
            ///////////
            var t = new Timer();// таймер вращения солнца
            t.Interval = 1; 
            t.Enabled = true;
            t.Tick += (s, o) => { IntervalR += trackBar1.Value; Invalidate(); };
            /////////
            var ear = new Timer();//таймер вращения земли вокруг солнца
            ear.Interval = 1;
            ear.Enabled = true;
            ear.Tick += (s, o) => { IntervalEar += (float)trackBar2.Value/100; Invalidate(); };
            //////////
            var RotateEar = new Timer();// таймер вращения Земли вокруг своей оси
            RotateEar.Interval = 1;
            RotateEar.Enabled = true;
            RotateEar.Tick += (s, o) => { IntervalRotateEar += trackBar3.Value; Invalidate(); };
            /////////s
            var RotateMoon = new Timer();// таймер вращения Луны вокруг Земли
            RotateMoon.Interval = 1;
            RotateMoon.Enabled = true;
            RotateMoon.Tick += (s, o) => { IntervalRotateMoon += (float)trackBar4.Value/100; Invalidate(); };
            /////////
            var MoonOs = new Timer();// таймер вращения Луны вокруг своей оси
            MoonOs.Interval = 1;
            MoonOs.Enabled = true;
            MoonOs.Tick += (s, o) => { IntervalMoonOs += trackBar5.Value; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e) // Рисовалка
        {
            e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 380, 340);
            // Солнце
                var s = e.Graphics;
                s.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                s.RotateTransform(IntervalR);
                s.DrawImage(Sol, -Sol.Width / 2, -Sol.Height / 2);
            
            // Земля
            e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 380, 340);
                double x2 = 320;
                double y2 = 320;
                double[,] SolMatrix = raschotEar(IntervalEar, x2, y2);
                var ear = e.Graphics;
                ear.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                ear.TranslateTransform((float)SolMatrix[0, 0] / 2, (float)SolMatrix[1, 0] / 2);
                ear.RotateTransform(IntervalRotateEar);
                ear.DrawImage(Earth, -Earth.Width / 2, -Earth.Height / 2);
             //
            {   // Луна
                e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, (float)SolMatrix[0, 0] / 2 + 380, (float)SolMatrix[1, 0] / 2 + 340);
                var moon = e.Graphics;
                moon.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                double[,] MoonMatrix = raschotEar(IntervalRotateMoon, 130, 130);
                moon.TranslateTransform((float)MoonMatrix[0, 0] / 2, (float)MoonMatrix[1, 0] / 2);
                moon.RotateTransform(IntervalMoonOs);
                moon.DrawImage(Moon, -Moon.Width / 2, -Moon.Height / 2);
            }
        }

        public static double[,] raschotEar(double fi, double x1, double y1) // Рассчёт координат
        {
            double[,] aMatrix = new double[2, 2];
            double[,] bMatrix = new double[2, 1];
            aMatrix[0, 0] = Math.Cos(fi);
            aMatrix[0, 1] = -Math.Sin(fi);
            aMatrix[1, 0] = Math.Sin(fi);
            aMatrix[1, 1] = Math.Cos(fi);
            bMatrix[0, 0] = x1;
            bMatrix[1, 0] = y1;
            double[,] cMatrix = new double[aMatrix.GetLength(0), bMatrix.GetLength(1)];
            for (int i = 0; i < aMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < bMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < bMatrix.GetLength(0); k++)
                    {
                        cMatrix[i, j] += aMatrix[i, k] * bMatrix[k, j];
                    }
                }
            }
            return cMatrix;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = String.Format("Скорость вращения Cолнца: {0}", trackBar1.Value);
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label3.Text = String.Format(" Скорость вращения\nЗемли вокруг Солнца: {0}", trackBar2.Value);
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label4.Text = String.Format(" Скорость вращения\n Земли вокруг оси: {0}", trackBar3.Value);
            
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label5.Text = String.Format(" Скорость вращения\n Луны вокруг Земли: {0}", trackBar4.Value);
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label6.Text = String.Format(" Скорость вращения\n Луны вокруг оси: {0}", trackBar5.Value);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данную программу разработали студенты\nСПбГТИ(ТУ) факультета ИТиУ 475группы:\nОвчинников Роман Сергеевич\nАндрианова Карина Ивановна\nПекер Валерия Александровна", "Информация о разработчике");
        }
    }
}
