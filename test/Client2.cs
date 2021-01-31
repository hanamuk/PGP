using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _0525ShapeManagerClient.WebReference;

namespace _0525ShapeManagerClient
{
    public partial class Client2 : Form
    {
        ShapeManager shapemanager = new ShapeManager();

        public Client2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Shape[] shapelist = shapemanager.GetShapeList();
            if (shapelist == null)
                return;

            Shape sh;
            for (int idx = 0; shapelist[idx] != null; idx++) 
            {
                sh = shapelist[idx];
                ListViewItem lvi = new ListViewItem(sh.X.ToString());
                lvi.SubItems.Add(sh.Y.ToString());
                lvi.SubItems.Add(sh.Size.ToString());
                lvi.SubItems.Add(sh.Dt.ToString());
                listView1.Items.Add(lvi);
            }
        }
    }
}
