using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tesseract;

namespace OCR2
{
    public partial class Form1 : Form
    {
        public int i = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScreenShot();
        }
        public void OCR()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var img = new Bitmap(openFileDialog1.FileName);
                var ocr = new TesseractEngine("./tessdata", "eng");
                var sonuc = ocr.Process(img);
                richTextBox1.Text = sonuc.GetText();
            }
        }
        public void ScreenShot()
        {
            this.Hide(); //Hide form
            System.Threading.Thread.Sleep(1000); //Hiding time (Enough to get a screenshot)
            SendKeys.Send("{PRTSC}");
            Image myImage = Clipboard.GetImage();
            pictureBox1.Image = myImage;
            myImage.Save(Directory.GetCurrentDirectory() +"//Images//" + i.ToString() +"screenshot.jpg");
            this.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ScreenShot();
            pictureBox1.Image = new Bitmap(Directory.GetCurrentDirectory() + "//Images//" + i.ToString() + "screenshot.jpg");
            i++;
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.MouseEnter += new EventHandler(pictureBox1_MouseEnter);
            Controls.Add(pictureBox1);
            
        }
        int crpX, crpY, recW, recH;
        public Pen crpPen = new Pen(Color.White);
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Cursor = Cursors.Cross;
                crpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                crpX = e.X;
                crpY = e.Y;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "Dimensions :" + recW + "," + recH;
            Cursor = Cursors.Default;
            Bitmap bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);

            Bitmap crpImg = new Bitmap(recW, recH);

            for (int i = 0; i < recW; i++)
            {
                for (int j = 0; j < recH; j++)
                {
                    Color pxlclr = bmp2.GetPixel(crpX + i, crpY + j);
                    crpImg.SetPixel(i, j, pxlclr);
                }
            }
            pictureBox1.Image = (Image)crpImg;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Cross;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pictureBox1.Refresh();
                recW = e.X - crpX;
                recH = e.Y - crpY;
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawRectangle(crpPen, crpX, crpY, recW, recH);
                g.Dispose();
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Default;
        }

    }
}
