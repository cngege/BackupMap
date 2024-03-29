﻿using CSR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Compression;
using Tools.Fileoperate;
using System.Diagnostics;

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
        public static string BakcupPath = @"plugins\BackUpMap"; //备份存档默认的保存路径和配置文件的存放路径;
        public static string OldBakcupPath = @"..\BackUpMap";   //旧的备份和配置文件的路径
        public static string RunPath;

        public static MCCSAPI Mapi;
        public static System.Timers.Timer TimerTick;

        //记值变量
        public static bool HavePlayer;                          //上一次备份到目前位置 是否有玩家进来过游戏
        public static bool PrepareBackup = false;               //已准备备份,等待调用出可备份文件列表

        public static string Temp = BakcupPath + @"\tmp";                      //如果要压缩备份、存档的临时存储目录

        private static InIFile ini;


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
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
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
            if (Profile.EnabledThreshold && (GetHardDiskSpace(AppDomain.CurrentDomain.BaseDirectory) < Folder.GetDirectorySize(MapPath) * Profile.Threshold))
            {
                Console.WriteLine("[BackupMap] 磁盘空间小于阙值，备份终止");
                return false;
            }

            if (SeedCMD(SaveHold))
            {
                PrepareBackup = true;
                new Thread(() =>
                {
                    Console.WriteLine("[{0} BackupMap ]5S后开始备份存档", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    SeedCMD(SaveHold);
                    Thread.Sleep(1000 * 5);
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
                if (string.IsNullOrEmpty(player)) player = "[]";
                /*
                // 没有玩家时输出空字符串，有玩家时输出JSON数组
                if (string.IsNullOrEmpty(player) || player.Trim() == "[]")
                {
                    HavePlayer = false;
                    //Console.WriteLine("DEBUG:当前服务器没有玩家");
                }
                */
                if (Tools.Data.JSON.parse<List<dynamic>>(player).Count == 0)
                {
                    HavePlayer = false;
                }
            }

            StartBackup();
        }

        private static bool check_old_config()
        {
            //配置文件(夹)迁移
            if (File.Exists(RunPath + OldBakcupPath + @"\BackupMap.ini"))
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                //将旧路径中的配置文件移动到新的文件夹下
                if (!Directory.Exists(BakcupPath))
                {
                    Directory.CreateDirectory(BakcupPath);
                }
                if(!File.Exists(BakcupPath + @"\BackupMap.ini")) //如果目标文件不存在
                {
                    File.Move(RunPath + OldBakcupPath + @"\BackupMap.ini", BakcupPath + @"\BackupMap.ini");
                    Console.WriteLine("[BackupMap] 检测到旧配置文件并自动迁移成功。");
                }
                else
                {
                    File.Delete(RunPath + OldBakcupPath + @"\BackupMap.ini");
                    Console.WriteLine("[BackupMap] 新配置文件已存在，删除旧配置文件。");
                }
                Console.ForegroundColor = color;
                return true;
            }
            return false;
        }

        //检测配置配置文件
        public static void CheckDeploy()
        {
            if (Directory.Exists(BakcupPath) || check_old_config())
            {
                if (File.Exists(BakcupPath+@"\BackupMap.ini") || check_old_config())
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

                    //备份完成之后要打开的文件或脚本或程序(附带参数)
                    Profile.run = ini.Read("Cmd", "Run", "0");

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
            new Thread(() => {
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
            BakcupPath = RunPath + BakcupPath;              //备份后存档存放的文件夹的绝对路径
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
                String t_tmp = DateTime.Now.Ticks.ToString();
                //
                if (PrepareBackup)
                {
                    if (output.IndexOf("Data saved. Files are now ready to be copied") != -1 || output.IndexOf("数据已保存。文件现已可供复制") != -1)
                    {
                        Console.WriteLine("已获取备份文件列表，准备备份");
                        new Thread(() =>
                        {
                            string savepath = string.Format("{0}\\{1}\\", Profile.HomeDire, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                            //备份
                            int count = BackupDB(output.Split(new char[] { '\n' })[1], Profile.Zip? Temp+@"\"+ t_tmp + @"\" : savepath);
                            if (Profile.Zip)
                            {
                                try
                                {
                                    Directory.CreateDirectory(savepath);
                                    ZipFile.CreateFromDirectory(Temp + @"\" + t_tmp + @"\" + MapDirName, savepath + MapDirName + ".zip");
                                }
                                catch (Exception error)
                                {
                                    Console.WriteLine("[BackupMap Error] 执行压缩失败,将备份的文件夹复制到备份目录，错误：{0}", error);
                                    Folder.Copy(Temp + @"\" + t_tmp + @"\" + MapDirName, savepath + @"\");
                                }
                                Directory.Delete(Temp + @"\" + t_tmp, true);
                            }
                            if (count != 0)
                            {
                                if (count == -1) {
                                    Console.WriteLine("因备份失败而终止");
                                    PrepareBackup = false;
                                    SeedCMD(SaveResume);
                                    return;
                                }
                                Console.WriteLine("备份结束有{0}个文件备份失败", count);
                            }
                            else
                            {
                                Console.WriteLine("存档全部复制成功");
                            }


                            //备份之后执行的操作
                            if (Profile.run!=null && Profile.run != "0" && File.Exists(Profile.run))
                            {
                                bool hasspace = (savepath + MapDirName).IndexOf(" ") != -1;

                                if (Profile.Zip)
                                {
                                    Process.Start(Profile.run,String.Format("{0} {1}","zip", hasspace? "\"" + savepath + MapDirName + ".zip\"" : savepath + MapDirName + ".zip"));
                                }
                                else
                                {
                                    Process.Start(Profile.run, String.Format("{0} {1}", "dir", hasspace?"\""+ savepath + MapDirName+"\"" : savepath + MapDirName));
                                }
                            }
                            //Thread.Sleep(1000 * 2);
                            SeedCMD(SaveResume);
                            PrepareBackup = false;
                            //告诉插件 存档存储已恢复 可以再次备份
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

            api.addBeforeActListener("onServerCmd", e => 
            {
                var ex = (ServerCmdEvent)BaseEvent.getFrom(e);
                if (ex.cmd.ToLower() == "backupmap")
                {
                    if(PrepareBackup)
                    {
                        Console.WriteLine("上次备份尚未结束");
                    }
                    else
                    {
                        StartBackup();  //手动备份
                    }
                    return false;
                }
                return true;
            });

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
        //备份完成之后要运行的脚本或程序
        public static string run;

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