using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VideoDemo
{
    internal class UsbApi
    {
        #region Com Item

        private const short INVALID_HANDLE_VALUE = -1;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint CREATE_NEW = 1;
        private const uint CREATE_ALWAYS = 2;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        private struct HID_ATTRIBUTES
        {
            public int Size;
            public ushort VendorID;
            public ushort ProductID;
            public ushort VersionNumber;
        }
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        private class SP_DEVINFO_DATA
        {
            public int cbSize = Marshal.SizeOf<SP_DEVINFO_DATA>();
            public Guid classGuid = Guid.Empty;
            public int devInst = 0;
            public int reserved = 0;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            internal int cbSize;
            internal short devicePath;
        }
        private enum DIGCF
        {
            DIGCF_DEFAULT = 0x1,
            DIGCF_PRESENT = 0x2,
            DIGCF_ALLCLASSES = 0x4,
            DIGCF_PROFILE = 0x8,
            DIGCF_DEVICEINTERFACE = 0x10
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_CAPS
        {
            /// <summary>
            /// Specifies a top-level collection's usage ID.
            /// </summary>
            public System.UInt16 Usage;
            /// <summary>
            /// Specifies the top-level collection's usage page.
            /// </summary>
            public System.UInt16 UsagePage;
            /// <summary>
            /// 输入报告的最大节数数量（如果使用报告ID，则包含报告ID的字节）
            /// Specifies the maximum size, in bytes, of all the input reports (including the report ID, if report IDs are used, which is prepended to the report data).
            /// </summary>
            public System.UInt16 InputReportByteLength;
            /// <summary>
            /// Specifies the maximum size, in bytes, of all the output reports (including the report ID, if report IDs are used, which is prepended to the report data).
            /// </summary>
            public System.UInt16 OutputReportByteLength;
            /// <summary>
            /// Specifies the maximum length, in bytes, of all the feature reports (including the report ID, if report IDs are used, which is prepended to the report data).
            /// </summary>
            public System.UInt16 FeatureReportByteLength;
            /// <summary>
            /// Reserved for internal system use.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public System.UInt16[] Reserved;
            /// <summary>
            /// pecifies the number of HIDP_LINK_COLLECTION_NODE structures that are returned for this top-level collection by HidP_GetLinkCollectionNodes.
            /// </summary>
            public System.UInt16 NumberLinkCollectionNodes;
            /// <summary>
            /// Specifies the number of input HIDP_BUTTON_CAPS structures that HidP_GetButtonCaps returns.
            /// </summary>
            public System.UInt16 NumberInputButtonCaps;
            /// <summary>
            /// Specifies the number of input HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
            /// </summary>
            public System.UInt16 NumberInputValueCaps;
            /// <summary>
            /// Specifies the number of data indices assigned to buttons and values in all input reports.
            /// </summary>
            public System.UInt16 NumberInputDataIndices;
            /// <summary>
            /// Specifies the number of output HIDP_BUTTON_CAPS structures that HidP_GetButtonCaps returns.
            /// </summary>
            public System.UInt16 NumberOutputButtonCaps;
            /// <summary>
            /// Specifies the number of output HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
            /// </summary>
            public System.UInt16 NumberOutputValueCaps;
            /// <summary>
            /// Specifies the number of data indices assigned to buttons and values in all output reports.
            /// </summary>
            public System.UInt16 NumberOutputDataIndices;
            /// <summary>
            /// Specifies the total number of feature HIDP_BUTTONS_CAPS structures that HidP_GetButtonCaps returns.
            /// </summary>
            public System.UInt16 NumberFeatureButtonCaps;
            /// <summary>
            /// Specifies the total number of feature HIDP_VALUE_CAPS structures that HidP_GetValueCaps returns.
            /// </summary>
            public System.UInt16 NumberFeatureValueCaps;
            /// <summary>
            /// Specifies the number of data indices assigned to buttons and values in all feature reports.
            /// </summary>
            public System.UInt16 NumberFeatureDataIndices;
        }
        #endregion
        #region Com Wrapper

        /// <summary>
        /// 过滤设备，获取需要的设备
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="Enumerator"></param>
        /// <param name="HwndParent"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);
        /// <summary>
        /// 获取设备，true获取到
        /// </summary>
        /// <param name="hDevInfo"></param>
        /// <param name="devInfo"></param>
        /// <param name="interfaceClassGuid"></param>
        /// <param name="memberIndex"></param>
        /// <param name="deviceInterfaceData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        /// <summary>
        /// 获取接口的详细信息 必须调用两次 第1次返回长度 第2次获取数据
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <param name="deviceInterfaceData"></param>
        /// <param name="deviceInterfaceDetailData"></param>
        /// <param name="deviceInterfaceDetailDataSize"></param>
        /// <param name="requiredSize"></param>
        /// <param name="deviceInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);
        /// <summary>
        /// 删除设备信息并释放内存
        /// </summary>
        /// <param name="HIDInfoSet"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr HIDInfoSet);


        /// <summary>
        /// 获取设备文件
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="dwDesiredAccess">access mode</param>
        /// <param name="dwShareMode">share mode</param>
        /// <param name="lpSecurityAttributes">SD</param>
        /// <param name="dwCreationDisposition">how to create</param>
        /// <param name="dwFlagsAndAttributes">file attributes</param>
        /// <param name="hTemplateFile">handle to template file</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool WriteFile(System.IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(int hObject);

        /// <summary>
        /// 获得GUID
        /// </summary>
        /// <param name="HidGuid"></param>
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(ref Guid HidGuid);
        [DllImport("hid.dll")]
        private static extern Boolean HidD_GetPreparsedData(IntPtr hidDeviceObject, out IntPtr PreparsedData);
        [DllImport("hid.dll")]
        private static extern uint HidP_GetCaps(IntPtr PreparsedData, out HIDP_CAPS Capabilities);
        [DllImport("hid.dll")]
        private static extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);
        [DllImport("hid.dll")]
        private static extern Boolean HidD_GetAttributes(IntPtr hidDevice, out HID_ATTRIBUTES attributes);




        #endregion

        private int _InputBufferSize;
        private int _OutputBufferSize;
        private FileStream _UsbFileStream = null;

        public static List<string> GetUsbFileNames()
        {
            //\\?\hid#vid_5131&pid_2007#7&252e9bc9&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}
            List<string> items = new List<string>();

            //通过一个空的GUID来获取HID的全局GUID。
            Guid hidGuid = Guid.Empty;
            HidD_GetHidGuid(ref hidGuid);

            //通过获取到的HID全局GUID来获取包含所有HID接口信息集合的句柄。
            //IntPtr hidInfoSet = SetupDiGetClassDevs(ref hidGuid, 0, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);

            IntPtr hidInfoSet = SetupDiGetClassDevs(ref hidGuid, 0, IntPtr.Zero, DIGCF.DIGCF_ALLCLASSES| DIGCF.DIGCF_DEFAULT|  DIGCF.DIGCF_PROFILE| DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);

            //获取接口信息。
            if (hidInfoSet != IntPtr.Zero)
            {

                SP_DEVICE_INTERFACE_DATA interfaceInfo = new SP_DEVICE_INTERFACE_DATA();
                interfaceInfo.cbSize = Marshal.SizeOf(interfaceInfo);

                uint index = 0;
                //检测集合的每个接口
                for(int i=0;i<256;i++)
                {
                    if (!SetupDiEnumDeviceInterfaces(hidInfoSet, IntPtr.Zero, ref hidGuid, index, ref interfaceInfo))
                        continue;
                        int bufferSize = 0;
                    //获取接口详细信息；第一次读取错误，但可取得信息缓冲区的大小
                    SP_DEVINFO_DATA strtInterfaceData = new SP_DEVINFO_DATA();
                    var result = SetupDiGetDeviceInterfaceDetail(hidInfoSet, ref interfaceInfo, IntPtr.Zero, 0, ref bufferSize, null);
                    //第二次调用传递返回值，调用即可成功
                    IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                    Marshal.StructureToPtr(
                        new SP_DEVICE_INTERFACE_DETAIL_DATA
                        {
                            cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA))
                        }, detailDataBuffer, false);


                    if (SetupDiGetDeviceInterfaceDetail(hidInfoSet, ref interfaceInfo, detailDataBuffer, bufferSize, ref bufferSize, null))// strtInterfaceData))
                    {
                        string devicePath = Marshal.PtrToStringAuto(IntPtr.Add(detailDataBuffer, 4));
                        items.Add(devicePath);
                    }
                    Marshal.FreeHGlobal(detailDataBuffer);
                    index++;
                }
            }
            //删除设备信息并释放内存
            SetupDiDestroyDeviceInfoList(hidInfoSet);
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usbFileName">Usb Device Path</param>
        public UsbApi(string usbFileName)
        {
            if (string.IsNullOrEmpty(usbFileName))
                throw new Exception("文件名不能为空");

            var fileHandle = CreateFile(
                 usbFileName,
                 GENERIC_READ | GENERIC_WRITE,// | GENERIC_WRITE,//读写，或者一起
                 FILE_SHARE_READ | FILE_SHARE_WRITE,//共享读写，或者一起
                 0,
                 OPEN_EXISTING,//必须已经存在
                 FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
                 0);
            if (fileHandle == IntPtr.Zero || (int)fileHandle == -1)
                throw new Exception("打开文件失败");

            HidD_GetAttributes(fileHandle, out var attributes);// null);// out var aa);
            HidD_GetPreparsedData(fileHandle, out var preparseData);
            HidP_GetCaps(preparseData, out var caps);
            HidD_FreePreparsedData(preparseData);
            _InputBufferSize = caps.InputReportByteLength;
            _OutputBufferSize = caps.OutputReportByteLength;

            _UsbFileStream = new FileStream(new SafeFileHandle(fileHandle, true), FileAccess.ReadWrite, System.Math.Max(caps.OutputReportByteLength, caps.InputReportByteLength), true);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="array"></param>
        public void Write(byte[] data)
        {
            if (_UsbFileStream == null)
                throw new Exception("Usb设备没有初始化");
            if (data.Length > _OutputBufferSize)
                throw new Exception($"数据太长，超出缓冲区长度({_OutputBufferSize})");
            byte[] outBuffer = new byte[_OutputBufferSize];
            Array.Copy(data, 0, outBuffer, 1, data.Length);
            _UsbFileStream.Write(outBuffer, 0, _OutputBufferSize);

        }
        /// <summary>
        /// 写数据后读取数据
        /// </summary>
        /// <param name="array"></param>
        public byte[] Read()
        {
            byte[] inBuffer = new byte[_InputBufferSize];
            _UsbFileStream.Read(inBuffer, 0, _InputBufferSize);
            return inBuffer;
        }


        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Close()
        {

            if (_UsbFileStream != null)
                _UsbFileStream.Close();
        }
    }
}
