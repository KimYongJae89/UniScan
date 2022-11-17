using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp
{
    public static class ConsoleEx
    {
        private enum FileType { Unknown, Disk, Char, Pipe };
        private enum StdHandle {
            Stdin = -10, // 0xFFFFFFF6;
            Stdout = -11, // 0xFFFFFFF5;
            Stderr = -12 // 0xFFFFFFF4;
        };

        [DllImport("kernel32.dll")]
        private static extern FileType GetFileType(IntPtr hdl);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(StdHandle std);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll ", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll ", EntryPoint = "RemoveMenu")]
        extern static int RemoveMenu(IntPtr hMenu, int nPos, int flags);

        static StreamWriter streamWriter;

        public static bool IsAlloced { get; private set; } = false;
        public static bool ShowStackFrame { get; private set; } = true;

        public static bool IsConsoleSizeZero
        {
            get
            {
                try
                {
                    return (0 == (Console.WindowHeight + Console.WindowWidth));
                }
                catch (Exception ex)
                {
                    return true;
                }
            }
        }
        public static bool IsOutputRedirected
        {
            get { return IsConsoleSizeZero && !Console.KeyAvailable; }
        }
        public static bool IsInputRedirected
        {
            get { return IsConsoleSizeZero && Console.KeyAvailable; }
        }

        public static bool IsDebuggerAttached => Debugger.IsAttached;
        public static bool AppendFrom { get; set; } = true;

        public static void Alloc()
        {
            if (!IsAlloced && !Debugger.IsAttached)
            {
                AllocConsole();

                IntPtr standardHandle = GetStdHandle(StdHandle.Stdout);
                SafeFileHandle standardSafeFileHandle = new SafeFileHandle(standardHandle, true);
                FileStream fileStream = new FileStream(standardSafeFileHandle, FileAccess.Write);
                Encoding encoding = Encoding.Default;
                StreamWriter streamWriter = new StreamWriter(fileStream, encoding);
                streamWriter.AutoFlush = true;
                Console.SetOut(streamWriter);

                // 닫기 버튼 클릭 금지
                int WINDOW_HANDLER = FindWindow(null, Console.Title);
                IntPtr CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
                int SC_CLOSE = 0xF060;
                RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);

                IsAlloced = true;

                ConsoleEx.streamWriter = new StreamWriter("Console.txt");
                ConsoleEx.streamWriter.AutoFlush = true;
            }

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string assemblyName = Base.VersionHelper.Instance().AssemblyName;
            string version = Base.VersionHelper.Instance().VersionString;
            string build = Base.VersionHelper.Instance().BuildString;
            WriteLine($"{dateTime}, {assemblyName}, V{version}, B{build}");
        }

        private static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

        public static void Free()
        {
            if (IsAlloced)
            {
                FreeConsole();
                IsAlloced = false;

                streamWriter?.Close();
                streamWriter?.Dispose();
                streamWriter = null;
            }
        }

        private static string GetStack(int depth)
        {
            System.Reflection.MethodBase methodBase = new StackTrace().GetFrame(depth + 1).GetMethod();
            string methodName = methodBase.Name;
            string className = methodBase.ReflectedType.Name;

            return $"{className}::{methodName}";
        }

        private static void WriteLine(string str, int frameDepth)
        {
            if (AppendFrom)
                str = $"{str}\t[from {GetStack(frameDepth)}]";

            Console.WriteLine(str);
            streamWriter?.WriteLine(str);
        }

        public static void WriteLine(string str)
        {
            WriteLine(str, 2);
        }        

        public static void WriteLine(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            switch (color)
            {
                case ConsoleColor.Black:
                    Console.BackgroundColor = ConsoleColor.White; break;
                case ConsoleColor.DarkBlue:
                case ConsoleColor.DarkGreen:
                case ConsoleColor.DarkCyan:
                case ConsoleColor.DarkRed:
                case ConsoleColor.DarkMagenta:
                case ConsoleColor.DarkYellow:
                case ConsoleColor.DarkGray:
                case ConsoleColor.Gray:
                case ConsoleColor.Blue:
                case ConsoleColor.Green:
                case ConsoleColor.Cyan:
                case ConsoleColor.Red:
                case ConsoleColor.Magenta:
                case ConsoleColor.Yellow:
                case ConsoleColor.White:
                    Console.BackgroundColor = ConsoleColor.Black; break;
            }

            WriteLine(str, 2);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
