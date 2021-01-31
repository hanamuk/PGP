using System;
using System.Windows.Forms;
using System.Drawing;
using _0525ShapeManagerClient.WebReference;

namespace _0525ShapeManagerClient
{
    public partial class Client1 : Form
    {
        ShapeManager shapemanager = new ShapeManager();

        public Client1()
        {
            InitializeComponent();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            int size = 30;
            System.Drawing.Color color = System.Drawing.Color.Red;

            Graphics g = this.panel1.CreateGraphics();
            Pen pen = new Pen(color, 3);

            Rectangle rec = new Rectangle(x, y, size, size);
            g.DrawRectangle(pen, rec);

            Shape shape = new Shape();
            shape.X = x;
            shape.Y = y;
            shape.Size = size;
            shape.Dt = DateTime.Now;

            shapemanager.ShapeSave(shape);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            Client2 client2 = new Client2();
            client2.ShowDialog();
        }
    }
}
