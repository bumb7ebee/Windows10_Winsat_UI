using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private Task asenkronTest()
        {
            return Task.Factory.StartNew(() =>
            {
                testiBaslat();
            });
        }

        public void testiBaslat()
        {
            Process process = new Process();
            process.StartInfo.FileName = "winsat.exe";
            process.StartInfo.Arguments = "formal";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

        }
        private void sistemBul()
        {
            string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey skey = key.OpenSubKey(subKey);

            string isim = skey.GetValue("ProductName").ToString();

            if (isim.IndexOf("Windows 7") != -1)
            {
                pictureBox1.ImageLocation = "windows7.png";
            }
            else if(isim.IndexOf("Windows 8")!=-1){
                pictureBox1.ImageLocation="windows8.png";
            }
            else if (isim.IndexOf("Windows 10") != -1)
            {
                pictureBox1.ImageLocation = "windows10.png";
            }
            else
            {
                MessageBox.Show("Geçersiz işletim sistemi!");
                Application.Exit();
            }
        }

        private void XML()
        {
            //string[] dosyalar = Directory.GetFiles(@"C:\Windows\Performance\WinSAT\DataStore\", "*.xml");
            //List<string> bulunanlar = new List<string>();

            //foreach (string dosya in dosyalar)
            //{
            //    if (dosya.IndexOf("Formal.Assessment (Recent)") != -1)
            //    {
            //        bulunanlar.Add(dosya);
            //    }
            //}

            //metroLabel11.Text = bulunanlar[0];

            string yol=@"C:\Windows\Performance\WinSAT\DataStore\";
            string highDir = "";
            DateTime lastHigh = new DateTime(1900, 1, 1);

            foreach (string subdir in Directory.GetFiles(yol, "*.xml"))
            {
                FileInfo fi = new FileInfo(subdir);
                DateTime created = fi.LastWriteTime;

                if (subdir.IndexOf("Formal.Assessment") != -1)
                {
                    if (created > lastHigh)
                    {
                        highDir = subdir;
                        lastHigh = created;
                    }
                }

                
            }
            

            XmlDocument xml = new XmlDocument();
            xml.Load(highDir);
            XmlNodeList xnList = xml.SelectNodes("/WinSAT/WinSPR");
            foreach (XmlNode dugum in xnList)
            {
                metroLabel1.Text = dugum["CpuScore"].InnerText.ToString();
                metroLabel2.Text = dugum["MemoryScore"].InnerText.ToString();
                metroLabel3.Text = dugum["GraphicsScore"].InnerText.ToString();
                metroLabel4.Text = dugum["GamingScore"].InnerText.ToString();
                metroLabel5.Text = dugum["DiskScore"].InnerText.ToString();


                

            }

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            metroButton3.Enabled = false;
            sistemBul();             
            metroProgressSpinner1.Value = 40;

        }

        private async void metroButton1_Click(object sender, EventArgs e)
        {
            metroButton1.Enabled = false;
            metroButton2.Enabled = false;
            metroButton3.Enabled = true;
            metroProgressSpinner1.Visible = true;
            metroProgressSpinner1.Enabled = true;
            groupBox1.Enabled = false;
            await asenkronTest();
            metroProgressSpinner1.Visible = false;
            groupBox1.Enabled = true;
            metroButton1.Enabled = true;
            metroButton2.Enabled = true;
            metroButton3.Enabled = false;
            XML();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            XML();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("WinSAT");
            foreach (var process in processes)
            {
                process.Kill();
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("WinSAT");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }


    }
}
