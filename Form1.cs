using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace ShowDevices
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            check_device();

            // Получение информации о носителях информации
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady /*&& drive.DriveType == DriveType.Removable*/)
                {
                    string[] mas = { drive.Name, drive.VolumeLabel, drive.DriveFormat, drive.DriveType.ToString(), drive.AvailableFreeSpace.ToString(), drive.TotalFreeSpace.ToString(), drive.TotalSize.ToString() };
                    ListViewItem lvi = new ListViewItem(mas);
                    listView1.Items.Add(lvi);
                    comboBox1.Items.Add(drive.Name);
                }
            }
            await Task.Run(()=> 
            {
                while (Process.GetProcessesByName("ShowDevices").Length > 0)
                {
                    check_device();
                }
            });                        
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Автор программы:\r\nКорюкин Данил,\r\nгруппа: ИТ-30919.", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            comboBox1.Text = "Выбор конкретного носителя:";

            check_device();

            // Получение информации о носителях информации
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady /*&& drive.DriveType == DriveType.Removable*/)
                {
                    string[] mas = { drive.Name, drive.VolumeLabel, drive.DriveFormat, drive.DriveType.ToString(), drive.AvailableFreeSpace.ToString(), drive.TotalFreeSpace.ToString(), drive.TotalSize.ToString() };
                    ListViewItem lvi = new ListViewItem(mas);
                    listView1.Items.Add(lvi);
                    comboBox1.Items.Add(drive.Name);
                }
            }
        }

        private void обновитьСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "Выбор конкретного носителя:";
            listView1.Items.Clear();
            comboBox1.Items.Clear();

            check_device();

            // Получение информации о носителях информации
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady /*&& drive.DriveType == DriveType.Removable*/)
                {
                    string[] mas = { drive.Name, drive.VolumeLabel, drive.DriveFormat, drive.DriveType.ToString(), drive.AvailableFreeSpace.ToString(), drive.TotalFreeSpace.ToString(), drive.TotalSize.ToString() };
                    ListViewItem lvi = new ListViewItem(mas);
                    listView1.Items.Add(lvi);
                    comboBox1.Items.Add(drive.Name);
                }
            }
        }

        /// <summary>
        /// Данный метод проверяет наличие USB устройства к которому "привязана" программа
        /// </summary>
        private void check_device()
        {
    
            string request = "SELECT * FROM Win32_USBHub";
            string checkString = "";

            ManagementObjectSearcher searcer = new ManagementObjectSearcher(request);

            // Формирование строки с перечислением подключенных USB устройств
            foreach (ManagementBaseObject obj in searcer.Get())
            {
                checkString += obj + "\r\n";
            }

            //Проверка на наличие носителя 
            if (!checkString.Contains(@"USB\\VID_13FE&PID_4300\\07000367672A3885"))
            {
                MessageBox.Show("Отсутствует носитель!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
           
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = comboBox1.Text;

            check_device();

            listView1.Items.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady /*&& drive.DriveType == DriveType.Removable*/ && drive.Name == name)
                {
                    string[] mas = { drive.Name, drive.VolumeLabel, drive.DriveFormat, drive.DriveType.ToString(), drive.AvailableFreeSpace.ToString(), drive.TotalFreeSpace.ToString(), drive.TotalSize.ToString() };
                    ListViewItem lvi = new ListViewItem(mas);
                    listView1.Items.Add(lvi);
                }
                
            }
        }

    }
}


