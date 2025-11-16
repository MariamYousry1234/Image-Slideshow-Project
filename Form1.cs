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

namespace Image_Slideshow_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

   
        string[] Images;
        int Counter = 0;
        byte TimerCounter = 0;


        void FormOn()
        {
            Counter = 0;
            TimerCounter = 0;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
        }

        void FormOff()
        {
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            lblImageName.Text = "";
            lblNumber.Text = "";
            pbImage.Image = null;
        }
        void ShowImage()
        {
            try
            {
                //Remove Image From Memory
                if (pbImage.Image != null)
                    pbImage.Image.Dispose();

                pbImage.Image = Image.FromFile(Images[Counter]);
                lblImageName.Text = Path.GetFileNameWithoutExtension(Images[Counter]);
                lblNumber.Text = Convert.ToString(Counter + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Image display error: {ex.Message} ","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            btnNext.Enabled = Images.Length > 0;
            btnPrevious.Enabled = Images.Length > 0;
        }

        void LoadImages()
        {
            FolderBrowserDialog ImagesFolder = new FolderBrowserDialog();

            if (ImagesFolder.ShowDialog() == DialogResult.OK)
            {
                Images = Directory.GetFiles(ImagesFolder.SelectedPath, "*.jpg")
                    .Concat(Directory.GetFiles(ImagesFolder.SelectedPath, "*.png")).ToArray();

                //Is Folder Empty
                if (Images.Length == 0)
                {
                    MessageBox.Show("No images found to show", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FormOff();
                    return;
                }
                else
                  FormOn();
                

                ShowImage();

            }

            else
            {
              if(pbImage.Image==null)
                   FormOff();
            }

           
        }


        void ShowNextImage()
        {
            if (Images.Length == ++Counter)
                Counter = 0;
          
            ShowImage();
            TimerCounter = 0;
        }

        void ShowPreviousImage()
        {
            if (--Counter < 0)
                Counter = Images.Length-1;
            
            ShowImage();
            TimerCounter = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages();
            timer1.Start();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowNextImage();

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ShowPreviousImage();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(++TimerCounter == 10)
                ShowNextImage();
          
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            LoadImages();
            timer1.Start();
        }
    }
}
