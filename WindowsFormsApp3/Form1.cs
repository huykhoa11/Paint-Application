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

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        enum DrawKind {LINE, RECTANGLE, CIRCLE, ELLIPSE, FREE }; //図形の種類

        bool flagMouseDown = false ;
        bool btnColorClick = false;
        int x1, y1, x2, y2;
        int penWidth = 5;

        DrawKind drawkind;
        ColorDialog colorDlg = new ColorDialog();
        Pen pen;
        SolidBrush brush;
        Bitmap bmp1;
        Graphics g;
        

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbDrawKind.Text = "Line";
            cmbPenStyle.Text = "Solid";
            txtPenWidth.Text = "1";
            Console.WriteLine("Hello");
            pen = new Pen(Color.Black, penWidth);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }


        private void cmbPenStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPenStyle.Text == "Solid")
            {
                if(btnColorClick == false)  pen = new Pen(Color.Black, penWidth);
                else pen = new Pen(colorDlg.Color, penWidth);
            }

            else if (cmbPenStyle.Text == "Dash")
            {
                pen.DashStyle= System.Drawing.Drawing2D.DashStyle.Dash;
            }

            else if (cmbPenStyle.Text == "Dot")
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }

            else if (cmbPenStyle.Text == "DashDot")
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            }

            else if (cmbPenStyle.Text == "DashDotDot")
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            }

            else if (cmbPenStyle.Text == "Fill")
            {
                brush = new SolidBrush(colorDlg.Color);
            }
        }

        private void cmbDrawKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDrawKind.Text == "Line")
            {
                drawkind = DrawKind.LINE;
            }
            else if (cmbDrawKind.Text == "Rectangle")
            {
                drawkind = DrawKind.RECTANGLE;
            }
            else if (cmbDrawKind.Text == "Circle")
            {
                drawkind = DrawKind.CIRCLE;
            }
            else if (cmbDrawKind.Text == "Ellipse")
            {
                drawkind = DrawKind.ELLIPSE;
            }
            else if(cmbDrawKind.Text == "Free")
            {
                drawkind = DrawKind.FREE;
            }
        }


        private void txtPenWidth_TextChanged(object sender, EventArgs e)
        {
            penWidth = Convert.ToInt32(txtPenWidth.Text);
            pen = new Pen(colorDlg.Color, penWidth);
        }

        private void btnColor_Click(object sender, System.EventArgs e)
        {
            btnColorClick = true;
            colorDlg.ShowDialog();
            btnColor.BackColor = colorDlg.Color;
            pen = new Pen(colorDlg.Color, penWidth);
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = e.X;
            y1 = e.Y;
            if (drawkind != DrawKind.FREE) pictureBox1.Cursor = Cursors.Cross;

            flagMouseDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {   

            if (flagMouseDown == true)
            {
                x2 = e.X;
                y2 = e.Y;
                pictureBox1.Refresh();
                bmp1 = new Bitmap(pictureBox1.Image);
                g = Graphics.FromImage(bmp1);
                if (drawkind == DrawKind.FREE)
                {
                    g.DrawLine(pen, new Point(x1, y1), e.Location);
                    pictureBox1.Image = bmp1;
                    x1 = x2;
                    y1 = y2;
                }
            }
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bmp1 = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(bmp1);
            int xx1, xx2, yy1, yy2, xc, yc, rr, w, h;
            if (drawkind == DrawKind.LINE)
            {
                if (cmbPenStyle.Text == "Fill") g.DrawLine(new Pen(colorDlg.Color, penWidth), x1, y1, x2, y2);
                else g.DrawLine(pen, x1, y1, x2, y2);
            }

            else if (drawkind == DrawKind.RECTANGLE)
            {
                w = Math.Abs(x2 - x1);
                h = Math.Abs(y2 - y1);
                if (cmbPenStyle.Text == "Fill") g.FillRectangle(new SolidBrush(colorDlg.Color), x1, y1, w, h);
                else
                {
                    g.DrawRectangle(pen, x1, y1, w, h);
                    pictureBox1.Image = bmp1;
                }
            }

            else if (drawkind == DrawKind.CIRCLE)
            {
                rr = (int)(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) / 2.0);
                xc = (x1 + x2) / 2;
                yc = (y1 + y2) / 2;
                xx1 = xc - rr;
                xx2 = xc + rr;
                yy1 = yc - rr;
                yy2 = yc + rr;
                w = xx2 - xx1;
                h = yy2 - yy1;
                if (cmbPenStyle.Text == "Fill") g.FillEllipse(new SolidBrush(colorDlg.Color), xx1, yy1, w, h);
                else
                {
                    g.DrawEllipse(pen, xx1, yy1, w, h);
                    pictureBox1.Image = bmp1;
                }       
            }

            else if (drawkind == DrawKind.ELLIPSE)
            {
                w = x2 - x1;
                h = y2 - y1;
                if (cmbPenStyle.Text == "Fill") g.FillEllipse(new SolidBrush(colorDlg.Color), x1, y1, w, h);
                else
                {
                    g.DrawEllipse(pen, x1, y1, w, h);
                    pictureBox1.Image = bmp1;
                }
            }

            else if (drawkind == DrawKind.FREE)
            {

                Console.WriteLine("dmm");
                g.DrawLine(pen, new Point(x1, y1), e.Location);
                pictureBox1.Image = bmp1;
                x1 = x2;
                y1 = y2;
            }
            g.Dispose();       
            pictureBox1.Image = bmp1;
            flagMouseDown = false;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (flagMouseDown == true)
            {
                int xx1, xx2, yy1, yy2, xc, yc, rr, w, h;
                if (drawkind == DrawKind.LINE)
                {
                    if (cmbPenStyle.Text == "Fill") e.Graphics.DrawLine(new Pen(colorDlg.Color, penWidth), x1, y1, x2, y2); 
                    else e.Graphics.DrawLine(pen, x1, y1, x2, y2);
                }

                else if (drawkind == DrawKind.RECTANGLE)
                {
                    w = Math.Abs(x2 - x1);
                    h = Math.Abs(y2 - y1);
                    if (cmbPenStyle.Text == "Fill") e.Graphics.FillRectangle(new SolidBrush(colorDlg.Color), x1, y1, w, h);
                    else e.Graphics.DrawRectangle(pen, x1, y1, w, h);
                }

                else if (drawkind == DrawKind.CIRCLE)
                {
                    rr = (int)(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) / 2.0);
                    xc = (x1 + x2) / 2;
                    yc = (y1 + y2) / 2;
                    xx1 = xc - rr;
                    xx2 = xc + rr;
                    yy1 = yc - rr;
                    yy2 = yc + rr;
                    w = xx2 - xx1;
                    h = yy2 - yy1;
                    if (cmbPenStyle.Text == "Fill") e.Graphics.FillEllipse(new SolidBrush(colorDlg.Color), xx1, yy1, w, h);
                    else e.Graphics.DrawEllipse(pen, xx1, yy1, w, h);
                }

                else if (drawkind == DrawKind.ELLIPSE)
                {
                    w = x2 - x1;
                    h = y2 - y1;
                    if (cmbPenStyle.Text == "Fill") e.Graphics.FillEllipse(new SolidBrush(colorDlg.Color), x1, y1, w, h);
                    else e.Graphics.DrawEllipse(pen, x1, y1, w, h);
                }
            }
        }

        private void versionInfomationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Tran Huy Khoa - 2021", "Inform",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            colorDlg.Color = Color.Black;
            btnColor.BackColor = default(Color);
            btnColor.Focus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            saveFileDialog1.ShowDialog();
            bmp1.Save(saveFileDialog1.FileName);
            MessageBox.Show("File saved successfully", "Inform");
        }


        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                Bitmap newBmp = new Bitmap(openFileDialog1.FileName);

                g = Graphics.FromImage(bmp);
                g.DrawImage(newBmp, 0, 0, newBmp.Width, newBmp.Height);
                pictureBox1.Image = bmp;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
