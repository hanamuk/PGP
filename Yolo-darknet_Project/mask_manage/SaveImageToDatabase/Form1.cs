using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SaveImageToDatabase
{
    public partial class Form1 : MetroFramework.Forms.MetroForm//Form
    {
        //string fileName;
        List<MyPicture> list;

        public Form1()
        {
            InitializeComponent();
        }

        Image ConvertBinarytoImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {

                return Image.FromStream(ms);
            }
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView.FocusedItem !=null)
            {
                pictureBox.Image = ConvertBinarytoImage(list[listView.FocusedItem.Index].Data);
                lblFilename.Text = listView.FocusedItem.SubItems[0].Text;
            }
        }

        byte[] ConvertImagetoBinary(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }

        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            using (PicEntities2 db = new PicEntities2())
            {
                list = db.MyPicture.ToList();

                foreach (MyPicture pic in list)//Mypicture 테이블 명 
                {
                    ListViewItem item;
                    item = new ListViewItem(pic.FileName);
                    if (pic.status != "처리필요")
                        item.SubItems.Add(pic.status);
                    else
                    {
                        string temp = "처리필요";
                        item.SubItems.Add(temp);
                    }
                    string Lat = pic.X.ToString();
                    string Lon = pic.Y.ToString();
                    item.SubItems.Add(Lat);
                    item.SubItems.Add(Lon);
     
                    listView.Items.Add(item);
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string url = "https://bit-map-project.neocities.org/";//https://pothole-map.neocities.org/  , https://bit-map-project.neocities.org/,http://localhost:8888/
                this.webBrowser1.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnIng_Click(object sender, EventArgs e)
        {
            using(PicEntities2 db = new PicEntities2())
            {
                int temp = listView.FocusedItem.Index;
                string temp2 = listView.Items[temp].Text;
                int data = int.Parse(temp2);
                db.MyPicture.Find(data).status = "처리중";
                db.SaveChanges();

                listView.Items.Clear();

                list = db.MyPicture.ToList();
                foreach (MyPicture pic in list)//Mypicture 테이블 명 
                {
                    ListViewItem item;
                    item = new ListViewItem(pic.FileName);
                    if (pic.status != "처리필요")
                        item.SubItems.Add(pic.status);
                    else
                    {
                        string temp3 = "처리필요";
                        item.SubItems.Add(temp3);
                    }

                    string Lat = pic.X.ToString();
                    string Lon = pic.Y.ToString();
                    item.SubItems.Add(Lat);
                    item.SubItems.Add(Lon);

                    listView.Items.Add(item);

                }
            }
        }
       
        private async void btnFinish_Click(object sender, EventArgs e)
        {
            using(PicEntities2 db =new PicEntities2())//WB31Entities 이거만 바꿔주면된다 DB이름 설정할때 Pic1Entities
            {
                int temp = listView.FocusedItem.Index;
                string temp2 = listView.Items[temp].Text;
                int data = int.Parse(temp2);
                db.MyPicture.Find(data).status = "처리완료";
                db.SaveChanges();


                listView.Items.Clear();

                list = db.MyPicture.ToList();
                foreach (MyPicture pic in list) // MyPicture테이블
                {
                    ListViewItem item;
                    item = new ListViewItem(pic.FileName);
                    if (pic.status != "처리필요")
                        item.SubItems.Add(pic.status);
                    else
                    {
                        string temp3 = "처리필요";
                        item.SubItems.Add(temp3);
                    }

                    string Lat = pic.X.ToString();
                    string Lon = pic.Y.ToString();
                    item.SubItems.Add(Lat);
                    item.SubItems.Add(Lon);

                    listView.Items.Add(item);
                }
               
            }
        }

        

       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (PicEntities2 db = new PicEntities2())
            {
                int temp = listView.FocusedItem.Index;
                string temp2 = listView.Items[temp].Text;
                int data = int.Parse(temp2);
                db.MyPicture.Remove(db.MyPicture.Find(data));
                db.SaveChanges();

                listView.Items.Clear();

                list = db.MyPicture.ToList();

                foreach (MyPicture pic in list) //Mypicture 테이블 명 
                {
                    ListViewItem item;
                    item = new ListViewItem(pic.FileName);
                    if (pic.status != "처리필요")
                        item.SubItems.Add(pic.status);
                    else
                    {
                        string temp3 = "처리필요";
                        item.SubItems.Add(temp3);
                    }

                    string Lat = pic.X.ToString();
                    string Lon = pic.Y.ToString();
                    item.SubItems.Add(Lat);
                    item.SubItems.Add(Lon);

                    listView.Items.Add(item);

                }


            }
        }
    }
}
