# Minecraft BDS CSR BackupMap

```Minecraft``` ```BDS``` ```CSR``` ```plugin```

---

本类库是 Minecraft基岩版服务端的扩展插件, 依赖[梦之故里]开发的插件载入器和BDSNetRunner插件
以及我的另一个开源类库 Tools，相关链接如下：

| 项目 | 关联地址 | 
| ------ | ------ | 
| MinecraftBDS | [https://www.minecraft.net/en-us/download/server/bedrock/][1] |
| BDSNetRunner | [https://github.com/zhkj-liuxiaohua/BDSNetRunner][2]          | 
| Tools        | [https://github.com/cngege/Tools][3]                          |

## 使用方法 ##

 1. 下载梦之故里启动器
 2. 下载Minecraft BDS
 3. 下载梦之故里BDSNetRunner插件
 4. 下载本插件及依赖(Tools)
 5. 将Minecraft BDS所有文件移入梦之故里启动器文件夹中MCPE文件夹下
 6. 按照Mojang官方教程配置好BDS文件
 7. 将BDSNetRunner放入MCPE同级的MCModDll文件夹中
 8. 在MCPE文件夹中新建CSR文件夹,并将本插件和Tools.dll文件放入该文件夹
 9. 启动梦之故里启动器(双击MCModDllExe文件夹中的debug.bat批处理,或者根据教程在网页端启动)
 10. 在BDS命令窗口中提示 ``` [BackupMap] 自动备份组件已加载。 ``` 即可等待出现配置面板
 11. 面板第一个输入控件处填插件循环备份存档间隔时间 仅填数字,总时间不要小于一分钟
 12. 面板第二个输入控件填备份的存档将要保存到的文件夹,右边按钮点击后可更方便选择
 13. 下面的复选框控制 备份到的文件夹有重名文件时是跳过还是覆盖
 14. 面板右下角按钮点击后将配置写入到本地配置文件中(``` BackUpMap/BackupMap.ini ```),没有出现提示表示成功
 15. 点击面板右上角❌关闭面板窗口  无需重启服务端插件已开始工作

![此处输入图片的描述][4]



## 注意❗：插件仅第一次打开才会弹出面板进行配置。如果需要重新配置,可以删除 ```BackUpMap/BackupMap.ini``` 文件，重启服务端,即会再次弹出配置面板。


  [1]: https://www.minecraft.net/en-us/download/server/bedrock/
  [2]: https://github.com/zhkj-liuxiaohua/BDSNetRunner
  [3]: https://github.com/cngege/Tools
  [4]: https://ae03.alicdn.com/kf/Ue706e923bc79457db85f4caf63578338I.jpg