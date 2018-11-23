using QDDL.BLL.Sockets.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.Sockets
{
    public interface IProtocol
    {

        /// <summary>
        /// 开始工作
        /// </summary>
        /// <param name="ip">扳手IP地址</param>
        /// <param name="port">扳手端口号</param>
        void Start(string ip, int port);

        /// <summary>
        /// 重启
        /// </summary>
        void ReStart();
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="torque">标准扭矩</param>
        /// <param name="maxtor">最大扭矩</param>
        /// <param name="mintor">最小扭矩</param>
        /// <param name="PsetID">PsetID</param>
        /// <returns></returns>
        void SendCommand(ToolWrench toolwrench);

        void SendLock();
        /// <summary>
        /// return clinet.isclose
        /// </summary>
        /// <returns></returns>
        bool GetClientStart();

        event EventHandler<ReciveEventArgs> ReciveHandler;
        event EventHandler<ReciveEventArgs> SendHandler;
        event EventHandler<ReciveEventArgs> RequestHandler;
    }
}
