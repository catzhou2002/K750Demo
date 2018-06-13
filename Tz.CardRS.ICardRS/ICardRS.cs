using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tz.CardRS
{

    /// <summary>
    /// 房卡收发器状态标记
    /// </summary>
    [Flags]
    public enum ECardRSQueryStatus
    {

        无状态 = 0,
        取卡口 = 1,
        读写位置 = 1 << 1,
        未知0 = 1 << 2, //位置3
        卡空 = 1 << 3,
        卡少 = 1 << 4,
        卡堵塞 = 1 << 5,
        卡重叠 = 1 << 6,
        卡箱满 = 1 << 7,
        收卡出错 = 1 << 8,
        发卡出错 = 1 << 9,
        正在收卡 = 1 << 10,
        正在发卡 = 1 << 11,
        卡箱快满 = 1 << 12,
        准备卡失败 = 1 << 13,
        命令不能执行 = 1 << 14,
        回收箱卡满 = 1 << 15,
        查询出错 = 1 << 16


        //        十六进制    二进制 状态
        //0x38 0x30 0x30 0x30	1000 0000 0000 0000	回收箱满
        //0x34 0x30 0x30 0x30	0100 0000 0000 0000	命令不能执行
        //0x32 0x30 0x30 0x30	0010 0000 0000 0000	准备卡失败
        //0x31 0x30 0x30 0x30	0001 0000 0000 0000	正在准备卡（k720）         卡箱预满（k750））见下面说明1
        //0x30 0x38 0x30 0x30	0000 1000 0000 0000	正在发卡
        //0x30 0x34 0x30 0x30	0000 0100 0000 0000	正在收卡
        //0x30 0x32 0x30 0x30	0000 0010 0000 0000	发卡出错
        //0x30 0x31 0x30 0x30	0000 0001 0000 0000	收卡出错
        //0x30 0x30 0x38 0x30	0000 0000 1000 0000	卡箱满(只用于k750)
        //0x30 0x30 0x34 0x30	0000 0000 0100 0000	重叠卡
        //0x30 0x30 0x32 0x30	0000 0000 0010 0000	堵塞卡
        //0x30 0x30 0x31 0x30	0000 0000 0001 0000	卡预空
        //0x30 0x30 0x30 0x38	0000 0000 0000 1000	卡空
        //0x30 0x30 0x30 0x34	0000 0000 0000 0100	卡在传感器3位置
        //0x30 0x30 0x30 0x32	0000 0000 0000 0010	卡在传感器2位置
        //0x30 0x30 0x30 0x31	0000 0000 0000 0001	卡在传感器1位置   
    }
    public enum ECommand
    {
        查询 = 0,
        循环收卡 = 1,
        复位 = 2,
        发卡到读写卡位置 = 3,
        发卡到取卡口 = 4,
        使能前端进卡 = 5
    }
    public interface ICardRS
    {


        /// <summary>
        /// 关闭房卡收发器
        /// </summary>
        void Close();
        /// <summary>
        /// 查询房卡收发器状态
        /// </summary>
        /// <returns>房卡位置相关说明：
        /// <para>当房卡处在读写位置的时候，返回状态包含 <c>取卡口|读写位置</c> </para></returns>
        ECardRSQueryStatus Query();
        /// <summary>
        /// 发卡机命令
        /// </summary>
        /// <param name="eCommand"></param>
        /// <returns></returns>
        bool ExecuteCommand(ECommand eCommand);
    }
}
