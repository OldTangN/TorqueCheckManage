using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.Comm
{
    public class KeyCheck
    {
        public static bool CheckKeyFile()
        {
            string keyFilePath = AppDomain.CurrentDomain.BaseDirectory + "AxleHead.Key";
            if (FileOperate.CheckFileExist(keyFilePath) == false)
            {
                return false;
            }
            return true;
        }

        private static string ReadKeyFromFile()
        {
            string keyFilePath = AppDomain.CurrentDomain.BaseDirectory + "AxleHead.Key";
            string readKey = FileOperate.ReadFileFromPath(@keyFilePath);

            if (readKey.Contains("\n"))
            {
                readKey = readKey.Substring(0, readKey.Length - 1);
            }
            return readKey;
        }
        public static bool CheckWithFileKey()
        {
            string FileKey = ReadKeyFromFile();
            HardwareInfo hardware = new HardwareInfo();
            string str_preview = hardware.GetHardDiskID() + "@" + hardware.GetMacAddressByNetworkInformation() + "#" + hardware.GetCpuID();
            EncryptUtil.InitKey();
            string encryptStr = EncryptUtil.EncryptDES(EncryptUtil.EncryptDES(str_preview));
            return encryptStr.Equals(FileKey);
        }
    }
}
