using CSR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools.Fileoperate;

namespace BackupMap
{


    public class AutoBackup
    {
        public static string SaveHold = "save hold";
        public static string SaveResume = "save resume";

        public static string MapPath;                           //如:world\CNGEGE        最后不带\
        public static string MapDirName;                        //如:CNGEGE
        public static string BakcupPath = @"..\BackUpMap";      //备份存档默认的保存路径和配置文件的存放路径;
        public static string RunPath;

        public static MCCSAPI mapi;
        public static System.Timers.Timer TimerTick;
        //记值变量
        public static bool havePlayer;                          //上一次备份到目前位置 是否有玩家进来过游戏

        private static InIFile ini;






        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir">[复制此文件夹]</param>
        /// <param name="desdir">[复制至此文件夹]如果以\结尾的才创建要复制的同名文件夹</param>
        /// <param name="overwrite">是否覆盖</param>
        public static void CopyDirectory(string srcdir, string desdir, bool overwrite)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))        //输出参数是不是以 \ 结尾的
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, currentdir, overwrite);
                }
                else if(File.Exists(file)) // 否则如果是文件直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;

                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    if (overwrite || !File.Exists(srcfileName))
                    {
                        File.Copy(file, srcfileName, true);
                    }
                    
                }
            }//foreach
        }//function end

        /// <summary>
        /// 获取指定路径的大小
        /// </summary>
        /// <param name="dirPath">路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            long len = 0;
            //判断该路径是否存在（是否为文件夹）
            if (!Directory.Exists(dirPath))
            {
                //查询文件的大小
                len = new FileInfo(dirPath).Length;
                
            }
            else
            {
                //定义一个DirectoryInfo对象
                DirectoryInfo di = new DirectoryInfo(dirPath);

                //通过GetFiles方法，获取di目录中的所有文件的大小
                foreach (FileInfo fi in di.GetFiles())
                {
                    len += fi.Length;
                }
                //获取di中所有的文件夹，并存到一个新的对象数组中，以进行递归
                DirectoryInfo[] dis = di.GetDirectories();
                if (dis.Length > 0)
                {
                    for (int i = 0; i < dis.Length; i++)
                    {
                        len += GetDirectoryLength(dis[i].FullName);
                    }
                }
            }
            return len;
        }

        /// <summary>
        /// 获取服务器存档文件夹的名字
        /// </summary>
        /// <returns></returns>
        public static string GetGameMap()
        {
            List<string> lists = new List<string>();
            DirectoryInfo folder = new DirectoryInfo("worlds/");
            DirectoryInfo[] files = folder.GetDirectories();
            foreach (var dir in files)
            {
                if (File.Exists(@"worlds\" + dir.ToString() + @"\level.dat"))
                {
                    MapPath = RunPath+@"worlds\" + dir.ToString();
                    MapDirName = dir.ToString();
                    return MapDirName;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取分区的剩余空间大小
        /// </summary>
        /// <param name="str_HardDisk">当前路径或当前路径的盘符</param>
        /// <returns></returns>
        public static long GetHardDiskSpace(string str_HardDisk)
        {
            if (str_HardDisk.IndexOf(":") != -1)
            {
                str_HardDisk = str_HardDisk.Substring(0, str_HardDisk.IndexOf(":"));
            }
            str_HardDisk = str_HardDisk + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDisk)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return 0;
        }



        /// <summary>
        /// 向服务器发送后台指令
        /// </summary>
        /// <param name="cmd">指令的内容</param>
        /// <returns>返回是否发送成功</returns>
        public static bool SeedCMD(string cmd)
        {
            return mapi.runcmd(cmd);
        }

        public static void AddAfterActListener(string onkey, MCCSAPI.EventCab call)
        {
            mapi.addAfterActListener(onkey,call);
        }

        public static bool StartBackup()
        {
            if (Profile.NeedPlayerBakcup)
            {
                if (!havePlayer)
                {
                    return false;
                }

                //如果备份的时候没有玩家在线则将表示改为false
                string player = mapi.getOnLinePlayers();
                if (string.IsNullOrEmpty(player) || player == "[]")
                {
                    havePlayer = false;
                    Console.WriteLine("DEBUG:当前服务器没有玩家");
                }
            }

            //检查磁盘剩余空间是否满足要求
            if (!Profile.EnabledThreshold || GetHardDiskSpace(AppDomain.CurrentDomain.BaseDirectory) < GetDirectoryLength(MapPath) * Profile.Threshold)
            {
                Console.WriteLine("磁盘空间小于阙值");
                return false;
            }

            if (SeedCMD(SaveHold))
            {
                new Thread(() =>
                {
                    Console.WriteLine("[{0} BackupMap ]5S后开始备份存档", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    SeedCMD(SaveHold);
                    Thread.Sleep(1000 * 5);
                    string output = string.Format("{0}\\{1}_{2}", Profile.HomeDire, MapDirName , DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                    
                    CopyDirectory(MapPath, output, !Profile.Isleapfrog);                //复制文件夹

                    //判断复制完成的文件大小是不是不比原来的存档文件大小小??? 大于等于=成功
                    if (GetDirectoryLength(MapPath) <= GetDirectoryLength(output))
                    {
                        Console.WriteLine("备份完成");
                    }
                    else
                    {
                        Console.WriteLine("备份可能出现问题,没有完全拷贝完成");
                    }
                    SeedCMD(SaveResume);

                }).Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void OnTick(object source, System.Timers.ElapsedEventArgs e)
        {
            //Console.WriteLine("["+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"]Tick被执行");
            StartBackup();
        }

        public static void CheckDeploy()
        {
            if (Directory.Exists(BakcupPath))
            {
                if (File.Exists(BakcupPath+@"\BackupMap.ini"))
                {
                    // TODO 读取配置文件并载入
                    // TODO 循环备份的时间 ms  小于一分钟就抛出异常
                    ini = new InIFile(BakcupPath + @"\BackupMap.ini");
                    try
                    {
                        Profile.TickTime = Int32.Parse(ini.Read("Tick","TickTime","0"));
                        if (Profile.TickTime < 1000*60)
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Error]Tick时间必须大于一分钟");
                            return;
                        }
                        TimerTick.Interval = Profile.TickTime;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Error]配置文件中Tick循环时间项出错:"+e.ToString());
                    }

                    // TODO 保存的总文件夹
                    Profile.HomeDire = ini.Read("SaveMap","SavePath", MapPath);

                    // TODO 阙值
                    Profile.EnabledThreshold = ini.Read("Threshold","Enabled", "0") != "0";
                    if (Profile.EnabledThreshold)
                    {
                        Profile.Threshold = int.Parse(ini.Read("Threshold", "Threshold", "0"));
                    }

                    // TODO 备份的相同文件夹名称的存档是否覆盖或跳过
                    Profile.Isleapfrog = ini.Read("SaveMap", "Leapfrog","0") != "0";

                    // TODO 是否只在有玩家玩过服务器的情况下备份存档
                    Profile.NeedPlayerBakcup = ini.Read("SaveMap", "NeedPlayer", "0") != "0";

                    TimerTick.Enabled = true;
                    TimerTick.Start();

                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(BakcupPath);
            }
            // TODO 显示窗口

            //Task 异步加载
            //new Task(()=> {
            //    System.Windows.Forms.Form Form = new Form1();
            //    Form.ShowDialog();
            //}).Start();

            new Thread(()=> {
                System.Windows.Forms.Application.Run(new Form1());
            }).Start();
              
        }

        public static void init(MCCSAPI api)
        {
            mapi = api;
            TimerTick = new System.Timers.Timer();
            RunPath = FileSys.GetPath();
            BakcupPath = RunPath + @"..\BackUpMap";
            Profile.HomeDire = BakcupPath;
            GetGameMap();
            CheckDeploy();                                  //检查配置文件是否存在,不存在则打开窗口进行配置

            //TimerTick.Interval = 1000 * 1 * 10; //1H执行一次 第一次执行就在这个时间后 单位ms
            TimerTick.Elapsed += new System.Timers.ElapsedEventHandler(OnTick);


            //GetGameMap();
            //监听事件：玩家加入世界
            AddAfterActListener("onLoadName", (e) => { havePlayer = true; return true; });
        }
    }

    public class Profile
    {
        //循环备份时间
        public static int TickTime;
        //保存存档的父文件夹 路径
        public static string HomeDire = AutoBackup.BakcupPath;
        //保存目录有相同的Map是否跳过[true:1]/覆盖[false:0]
        public static bool Isleapfrog;
        //距离上次备份需要玩家进入过游戏才备份
        public static bool NeedPlayerBakcup;
        //备份时是否检测剩余空间
        public static bool EnabledThreshold;
        //[需要前项开启] 剩余空间必须要大于备份存档的倍数;
        public static int Threshold = 1;

    }

}




namespace CSR
{
    partial class Plugin
    {
        /// <summary>
        /// 通用调用接口，需用户自行实现
        /// </summary>
        /// <param name="api">MC相关调用方法</param>
        public static void onStart(MCCSAPI api)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("[BackupMap] 自动存档备份组件已加载。");

            Console.ForegroundColor = color;

            // TODO 此接口为必要实现
            BackupMap.AutoBackup.init(api);
        }
    }
}