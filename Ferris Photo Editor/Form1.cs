using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ferris_Photo_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.FileName != null)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void BtnOutBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath != null) textBox3.Text = folderBrowserDialog.SelectedPath;
        }

        private async void BtnChange_Click_Async(object sender, EventArgs e)
        {
            string outdir = textBox3.Text;
            if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);
            var files = await Task.Factory.StartNew(() => 
                Ferris_Directory_Scanner.FerrisSearchEngine.ScanDirectories(new string[] { textBox1.Text }, new string[] { "jpg", "png", "jpeg", "gif", "mp4", "mov" }, 
                        new System.Threading.CancellationTokenSource()));
            DateTime dateTime;
            int count = 0;
            foreach(var file in files)
            {
                string parentdir = new FileInfo(file).Directory.Name;
                dateTime = DateTime.ParseExact(parentdir, "yyyyMMdd-HHmmss", CultureInfo.GetCultureInfo("en-US"));
                string outfile = PhotoController.NextAvailableFilename(Path.Combine(outdir, Path.GetFileName(file)));
                FileInfo fi;
                if (Path.GetExtension(file).ToLower() == ".mp4" || Path.GetExtension(file).ToLower() == ".mov")
                {
                    File.Move(file, outfile);
                }
                else
                {
                    PhotoController.SetDateTaken(file, dateTime, outfile);
                }
                
                fi = new FileInfo(outfile);
                fi = setFileDates(fi, dateTime);
                count++;
            }


            //PhotoController.SetDateTaken(textBox1.Text, taken, outdir);
        }

        private FileInfo setFileDates(FileInfo fi, DateTime dt)
        {
            fi.CreationTime = dt;
            fi.CreationTimeUtc = dt;
            fi.LastAccessTime = dt;
            fi.LastAccessTimeUtc = dt;
            fi.LastWriteTime = dt;
            fi.LastWriteTimeUtc = dt;
            return fi;
        }
    }
}
