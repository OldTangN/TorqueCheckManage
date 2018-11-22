using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QDDL.Comm
{
    public class FileOperate
    { 
        public static string ReadFileFromPath(string path)
        {
            StringBuilder templateContent = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        templateContent.Append(line + "\n");
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                //LogUtil.WriteLog(null, "ReadFileFromPath" + ex.Message); 
            }
            return templateContent.ToString();
        }

        public static bool CheckFileExist(string fullname)
        {
            return File.Exists(fullname);
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="fullName">保存的文件路径(若文件所在目录不存在，则创建)</param>
        /// <param name="context">文件内容</param>
        public static bool CreateFile(string fullName, string content)
        {
            try
            {
                string DirectoryName =  Path.GetDirectoryName(fullName);
                if (Directory.Exists(DirectoryName) == false)  //若目录不存在，则创建
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                if(File.Exists(fullName))
                {
                    File.Delete(fullName);
                }
                FileStream fs = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Flush();
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(content);
                sw.Flush();
                sw.Close(); 
            }
            catch (Exception ex)
            {
                //LogUtil.WriteLog(null, "CreateFile" + ex.Message); 
                return false;
            }
            return true;
        }

        public void CopyFile(string FormerFile, string toFile, int SectSize)
        {
            FileStream fileToCreate = new FileStream(toFile, FileMode.Create); //创建目的文件，如果已存在将被覆盖
            fileToCreate.Close(); //关闭所有资源
            fileToCreate.Dispose(); //释放所有资源
            FileStream  FormerOpen = new FileStream(FormerFile, FileMode.Open, FileAccess.Read);//以只读方式打开源文件
            FileStream ToFileOpen = new FileStream(toFile, FileMode.Append, FileAccess.Write); //以写方式打开目的文件
            //根据一次传输的大小，计算传输的个数
            int FileSize; //要拷贝的文件的大小
            //如果分段拷贝，即每次拷贝内容小于文件总长度
            if (SectSize < FormerOpen.Length)
            {
                byte[] buffer = new byte[SectSize]; //根据传输的大小，定义一个字节数组
                int copied = 0; //记录传输的大小
                while (copied <= ((int)FormerOpen.Length - SectSize)) //拷贝主体部分
                {
                    FileSize = FormerOpen.Read(buffer, 0, SectSize); //本次读取的字节长度
                    FormerOpen.Flush(); //清空缓存
                    ToFileOpen.Write(buffer, 0, SectSize); //向目的文件写入字节
                    ToFileOpen.Flush(); //清空缓存
                    ToFileOpen.Position = FormerOpen.Position; //使源文件和目的文件流的位置相同
                    copied += FileSize; //记录已拷贝的大小
                }
                int left = (int)FormerOpen.Length - copied; //获取剩余大小
                FileSize = FormerOpen.Read(buffer, 0, left); //读取剩余的字节
                FormerOpen.Flush(); //清空缓存
                ToFileOpen.Write(buffer, 0, left); //写入剩余的部分
                ToFileOpen.Flush(); //清空缓存
            }
            //如果整体拷贝，即每次拷贝内容大于文件总长度
            else
            {
                byte[] buffer = new byte[FormerOpen.Length]; //获取文件的大小
                FormerOpen.Read(buffer, 0, (int)FormerOpen.Length); //读取源文件的字节
                FormerOpen.Flush(); //清空缓存
                ToFileOpen.Write(buffer, 0, (int)FormerOpen.Length); //写放字节
                ToFileOpen.Flush(); //清空缓存
            }
            FormerOpen.Close(); //释放所有资源
            ToFileOpen.Close(); //释放所有资源
            //MessageBox.Show("文件复制完成");
        }
    }
}
