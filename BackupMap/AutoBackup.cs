using CSR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Compression;
using Tools.Fileoperate;

namespace BackupMap
{


    public class AutoBackup
    {
        //CMD
        public static string SaveHold = "save hold";
        public static string SaveQuery = "save query";
        public static string SaveResume = "save resume";

        public static string MapPath;                           //如:world\CNGEGE        最后不带\
        public static string MapDirName;                        //如:CNGEGE
        public static string BakcupPath = @"..\BackUpMap";      //备份存档默认的保存路径和配置文件的存放路径;
        public static string RunPath;

        public static MCCSAPI Mapi;
        public static System.Timers.Timer TimerTick;

        //记值变量
        public static bool HavePlayer;                          //上一次备份到目前位置 是否有玩家进来过游戏
        public static bool PrepareBackup = false;               //已准备备份,等待调用出可备份文件列表

        public static string Temp = @"..\tmp";                  //如果要压缩备份、存档的临时存储目录

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
            return Mapi.runcmd(cmd);
        }


        public static bool StartBackup()
        {
            //检查磁盘剩余空间是否满足要求
            if (Profile.EnabledThreshold && (GetHardDiskSpace(AppDomain.CurrentDomain.BaseDirectory) < GetDirectoryLength(MapPath) * Profile.Threshold))
            {
                Console.WriteLine("磁盘空间小于阙值，备份终止");
                return false;
            }

            if (SeedCMD(SaveHold))
            {
                new Thread(() =>
                {
                    Console.WriteLine("[{0} BackupMap ]5S后开始备份存档", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    SeedCMD(SaveHold);
                    Thread.Sleep(1000 * 5);
                    PrepareBackup = true;
                    SeedCMD(SaveQuery);

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
            //时钟TICK到时间后被执行
            //Console.WriteLine("["+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"]Tick被执行");
            if (Profile.NeedPlayerBakcup)
            {
                if (!HavePlayer)            //如果没有玩家来过
                {
                    return;
                }

                //如果备份的时候没有玩家在线则将表示改为false
                string player = Mapi.getOnLinePlayers();
                if (string.IsNullOrEmpty(player) || player.Trim() == "[]")
                {
                    HavePlayer = false;
                    //Console.WriteLine("DEBUG:当前服务器没有玩家");
                }
            }

            StartBackup();
        }

        //检测配置配置文件
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

                    //备份时是否压缩存档到ZIP文件
                    Profile.Zip = ini.Read("SaveMap", "Zip", "0") != "0";

                    TimerTick.Enabled = true;
                    TimerTick.Start();

                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(BakcupPath);
            }

            //显示窗口
            new Thread(()=> {
                System.Windows.Forms.Application.Run(new Form1());
            }).Start();
              
        }


        /// <summary>
        /// 分解可备份文件列表 并备份文件
        /// </summary>
        /// <param name="list">MCPE复制文件列表字符串</param>
        /// <returns>复制不完全的文件数量,如果全部成功则为0,出错为-1</returns>
        public static int BackupDB(string list,string savepath)
        {
            try
            {
                string[] bplist = list.Split(new char[] { ',' });
                //string savepath = string.Format("{0}\\{1}\\", Profile.HomeDire, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                int reval = 0;                                      //复制不完全的文件数

                
                foreach (var item in bplist)
                {
                    string[] _list = item.Split(new char[] { ':' });
                    string path = _list[0].Trim().Replace(@"/",@"\");
                    long size = long.Parse(_list[1].Trim());
                    if (Directory.Exists(Path.GetDirectoryName(savepath + path)) == false)
                    {
                        new DirectoryInfo(Path.GetDirectoryName(savepath + path)).Create();//如何这个文件的文件夹不存在 则创建一个文件夹 
                    }
                    File.Copy("worlds\\" + path, savepath + path);
                    if (new FileInfo("worlds\\" + path).Length < size)
                    {
                        reval++;
                    }
                }
                return reval;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                return -1;
            }
        }


        public static void init(MCCSAPI api)
        {
            Mapi = api;
            TimerTick = new System.Timers.Timer();
            RunPath = FileSys.GetPath();
            BakcupPath = RunPath + @"..\BackUpMap";
            Profile.HomeDire = BakcupPath;
            GetGameMap();
            CheckDeploy();                                  //检查配置文件是否存在,不存在则打开窗口进行配置

            //TimerTick.Interval = 1000 * 1 * 10;           //1H执行一次 第一次执行就在这个时间后 单位ms
            TimerTick.Elapsed += new System.Timers.ElapsedEventHandler(OnTick);
            //GetGameMap();
            //监听事件：玩家加入世界
            api.addAfterActListener("onLoadName", e =>
            {
                HavePlayer = true;
                return true;
            });

            //监听事件：后台指令输出信息
            api.addBeforeActListener("onServerCmdOutput", e =>
            {
                var ex = (ServerCmdOutputEvent)BaseEvent.getFrom(e);
                string output = ex.output;

                //
                if (PrepareBackup)
                {
                    if (output.IndexOf("Data saved. Files are now ready to be copied") != -1 || output.IndexOf("数据已保存。文件现已可供复制") != -1)
                    {
                        Console.WriteLine("已获取备份文件列表，准备备份");
                        PrepareBackup = false;
                        //告诉系统 存档存储已恢复
                        new Thread(() =>
                        {
                            string savepath = string.Format("{0}\\{1}\\", Profile.HomeDire, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                            //备份
                            int count = BackupDB(output.Split(new char[] { '\n' })[1], Profile.Zip? Temp+@"\" : savepath);
                            if (count != 0)
                            {
                                if (count == -1) { Console.WriteLine("因备份失败而终止");return; }
                                Console.WriteLine("备份结束有{0}个文件备份失败", count);
                                
                            }
                            else
                            {
                                Console.WriteLine("存档全部复制成功");
                            }
                            if (Profile.Zip)
                            {
                                try
                                {
                                    Directory.CreateDirectory(savepath);
                                    ZipFile.CreateFromDirectory(Temp + @"\" + MapDirName, savepath + MapDirName + ".zip");
                                }
                                catch (Exception error)
                                {
                                    Console.WriteLine("[BackupMap Error] 执行压缩失败,将备份的文件夹复制到备份目录，错误：{0}", error);
                                    CopyDirectorysAndFiles(Temp+ @"\" + MapDirName, savepath + @"\");
                                }
                                Directory.Delete(Temp + @"\" + MapDirName,true);
                            }
                            Thread.Sleep(1000 * 2);
                            SeedCMD(SaveResume);
                        }).Start();

                        return false;                                       //禁止在控制面板上显示这个回显
                    }
                    else
                    {
                        // TODO:当出现还没有准备完成的时候执行

                    }
                }

                return true;
            });

        }

        /// <summary>
        /// 文件夹复制
        /// </summary>
        /// <param name="srcdir">原文件夹</param>
        /// <param name="dest">目的文件夹</param>
        private static void CopyDirectorysAndFiles(string src, string dest)
        {
            DirectoryInfo srcdir = new DirectoryInfo(src);
            string destPath = dest;
            if (dest.LastIndexOf('\\') != (dest.Length - 1))
            {
                dest += "\\";
            }
            if (src.LastIndexOf('\\') != (src.Length - 1))
            {
                destPath = dest + srcdir.Name + "\\";
                src += "\\";
            }

            
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            FileInfo[] files = srcdir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(destPath + file.Name, true);
            }
            DirectoryInfo[] dirs = srcdir.GetDirectories();
            foreach (DirectoryInfo dirInfo in dirs)
            {
               CopyDirectorysAndFiles(dirInfo.FullName, destPath);
            }
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
        //是否备份后压缩存档到ZIP文件
        public static bool Zip = false;

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
            try
            {
                BackupMap.AutoBackup.init(api);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }
    }
}