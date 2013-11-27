using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
                                              out ulong lpFreeBytesAvailable,
                                              out ulong lpTotalNumberOfBytes,
                                              out ulong lpTotalNumberOfFreeBytes);

        struct OSVERSIONINFO
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public Int16 wServicePackMajor;
            public Int16 wServicePackMinor;
            public Int16 wSuiteMask;
            public Byte wProductType;
            public Byte wReserved;
        }

        [DllImport("kernel32")]
        static extern bool GetVersionEx(ref OSVERSIONINFO osvi);

        public MainWindow()
        {
            InitializeComponent();

            this.freeBytes.Content = (double)this.getFreeBytesAvailable("C:/") / 1000000000 + " Go";
            this.totalSize.Content = (double)this.getTotalBytes("C:/") / 1000000000 + " Go";
            this.osVersion.Content = this.getOsVersion();

        }

        public long getFreeBytesAvailable(String path)
        {
            ulong freeBytesAvailable;
            ulong totalNumberOfBytes;
            ulong totalNumberOfFreeBytes;

            bool success = GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);

            return success ? (long)freeBytesAvailable : -1;
        }

        public long getTotalBytes(String path)
        {
            ulong freeBytesAvailable;
            ulong totalNumberOfBytes;
            ulong totalNumberOfFreeBytes;

            bool success = GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);

            return success ? (long)totalNumberOfBytes : -1;
        }

        public int getOsVersion()
        {
            OSVERSIONINFO os = new OSVERSIONINFO();
            os.dwOSVersionInfoSize = (uint)Marshal.SizeOf(os);
            
            return GetVersionEx(ref os) ? (int)os.dwBuildNumber : -1;
            //return GetVersionEx(ref os) ? (int)os.dwMajorVersion : -1;
            //return GetVersionEx(ref os) ? (int)os.dwMinorVersion : -1;
        }

    }
}
