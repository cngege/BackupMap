using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Fileoperate;
using Tools.Formoperate;

namespace BackupMap
{
    public partial class Form1 : Form
    {
        InIFile ini;

        public Form1()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
            Logo.Dock = DockStyle.Fill;
            Logo.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ini = new InIFile(AutoBackup.BakcupPath + @"\BackupMap.ini");
        }

        //显示LOGO 3S后消失
        private void Logo_Paint(object sender, PaintEventArgs e)
        {

            new System.Threading.Thread(() =>
            {
                System.Threading.Thread.Sleep(1000 * 3);
                Logo.Dispose();
                TitlePanal.Dock = DockStyle.Top;

            }).Start();
        }

        //Title控制窗口移动
        private void TitlePanal_MouseDown(object sender, MouseEventArgs e)
        {
            WinForms.MoveFrom(this.Handle);
        }

        //窗口关闭窗口按钮
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //面板左下角文字超链接网址
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("github.com");
        }


        //面板右下角"完成"点击之后事件
        private void OKbtn_Click(object sender, EventArgs e)
        {


            //检查TickTime
            int tickvalue = 0,  //ms
                tickvalue0 = 0,
                tickvalue1 = 0,
                tickvalue2 = 0;
            try
            {
                tickvalue0 = int.Parse(Hour_input.Text); //H
                tickvalue1 = int.Parse(Minute_input.Text); //M
                tickvalue2 = int.Parse(Second_input.Text); //S

                tickvalue = ((tickvalue0 * 60 * 60) + (tickvalue1 * 60) + tickvalue2) * 1000;
            }
            catch (Exception)
            {
                MessageBox.Show("请检查备份时间输入框是否是数字");
                return;
            }
            if (tickvalue > 1000 * 60)
            {
                //TODO 最后写入ini
                AutoBackup.TimerTick.Interval = tickvalue;
                Profile.TickTime = tickvalue;
            }
            else
            {
                MessageBox.Show("请检查备份时间输入框值是否大于一分钟");
                return;
            }

            //检查保存路径
            if (Directory.Exists(SavePath_input.Text))
            {
                //TODO 最后写入ini
                Profile.HomeDire = SavePath_input.Text;
            }
            else
            {
                MessageBox.Show("保存目录不存在,请检查");
                return;
            }

            //是否跳过
            //TODO 最后写入ini
            Profile.isleapfrog = Isleapfrog_check.Checked;


            //最后开启定时器
            AutoBackup.TimerTick.Enabled = true;
            AutoBackup.TimerTick.Start();

            ini.Write("Tick", "TickTime", Profile.TickTime.ToString());
            ini.Write("SaveMap", "SavePath", Profile.HomeDire);
            ini.Write("SaveMap", "Leapfrog", Isleapfrog_check.Checked ? "1" : "0");

        }

        //点击左下角文字 打开GitHub开源界面
        private void OpenSource_label_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/cngege/BackupMap");
        }

        //点击是否跳过选择框
        private void Isleapfrog_check_CheckedChanged(object sender, EventArgs e)
        {
            Isleapfrog_check.Text = Isleapfrog_check.Checked ? "覆盖" : "跳过";
        }

        //点击选择获取"复制到"文件夹
        [Obsolete]
        private void GetPath_Click(object sender, EventArgs e)
        {
            /*            FolderBrowserDialog dialog = new FolderBrowserDialog();
                        dialog.Description = "选择备份存档要保存的目录"; //提示文字
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            SavePath_input.Text = dialog.SelectedPath;
                        }*/

            System.Threading.Thread s = new System.Threading.Thread(new System.Threading.ThreadStart(()=> {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "选择备份存档要保存的目录"; //提示文字
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SavePath_input.Text = dialog.SelectedPath;
                }
            }));
            s.ApartmentState = System.Threading.ApartmentState.STA;
            s.Start();

        }
    }
}
