using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ioio.Category;

namespace ioio.Common.Util
{
    public class Memory
    {
        #region DllImports
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            bool bInheritHandle,
            Int32 dwProcessId
            );

#if WINXP
#else


        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
#endif



        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);


        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, out ulong lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] IntPtr lpBuffer, UIntPtr nSize, out ulong lpNumberOfBytesRead);



        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(
        IntPtr hObject
        );



        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);

        // Added to avoid casting to UIntPtr
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          UIntPtr lpStartAddress, // raw Pointer into remote process  
          UIntPtr lpParameter,
          uint dwCreationFlags,
          out IntPtr lpThreadId
        );

        [DllImport("kernel32")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool lpSystemInfo);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        // privileges
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_FREE = 0x10000;
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;

        private const uint PAGE_READONLY = 0x02;
        const uint PAGE_READWRITE = 0x04;
        const uint PAGE_WRITECOPY = 0x08;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;
        private const uint PAGE_EXECUTE_WRITECOPY = 0x80;
        private const uint PAGE_EXECUTE = 0x10;
        private const uint PAGE_EXECUTE_READ = 0x20;

        private const uint PAGE_GUARD = 0x100;
        private const uint PAGE_NOACCESS = 0x01;

        private uint MEM_PRIVATE = 0x20000;
        private uint MEM_IMAGE = 0x1000000;

        #endregion

        /// <summary>
        /// The process handle that was opened. (Use OpenProcess function to populate this variable)
        /// </summary>
        public IntPtr pHandle;
        Dictionary<string, CancellationTokenSource> FreezeTokenSrcs = new Dictionary<string, CancellationTokenSource>();
        public Process theProc = null;


        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        

        /// <summary>
        /// Open the PC game process with all security and access rights.
        /// </summary>
        /// <param name="proc">Use process name or process ID here.</param>
        /// <returns></returns>
        public bool OpenProcess(int pid)
        {
            //if (!IsAdmin())
            //{
            //    Console.WriteLine("WARNING: You are NOT running this program as admin! Visit https://github.com/erfg12/memory.dll/wiki/Administrative-Privileges");
                
            //}

            try
            {
                Console.WriteLine(1);
                if (theProc != null && theProc.Id == pid)
                    return true;
                Console.WriteLine(pid);
                if (pid <= 0)
                {
                    Console.WriteLine("ERROR: OpenProcess given proc ID 0.");
                    return false;
                }
                
                theProc = Process.GetProcessById(pid);

                if (theProc != null && !theProc.Responding)
                {
                    Debug.WriteLine("ERROR: OpenProcess: Process is not responding or null.");
                    return false;
                }

                pHandle = OpenProcess(0x1F0FFF, true, pid);
                Process.EnterDebugMode();

                if (pHandle == IntPtr.Zero)
                {
                    var eCode = Marshal.GetLastWin32Error();
                }

                mainModule = theProc.MainModule;

                GetModules();

                // Lets set the process to 64bit or not here (cuts down on api calls)
                Is64Bit = Environment.Is64BitOperatingSystem && (IsWow64Process(pHandle, out bool retVal) && !retVal);

                Debug.WriteLine("Program is operating at Administrative level. Process #" + theProc + " is open and modules are stored.");

                return true;
            }
            catch {
                Debug.WriteLine("ERROR: OpenProcess has crashed. Are you trying to hack a x64 game? https://github.com/erfg12/memory.dll/wiki/64bit-Games");
                return false;
            }
        }

       
        /// <summary>
        /// Open the PC game process with all security and access rights.
        /// </summary>
        /// <param name="proc">Use process name or process ID here.</param>
        /// <returns></returns>
        public bool OpenProcess(string proc)
        {
            //return OpenProcess(GetProcIdFromName(proc));
            Process[] _p = Process.GetProcessesByName(proc);
            if (_p.Length == 0)
                return false;
            int pid = _p[0].Id;
            try
            {
                if (theProc != null && theProc.Id == pid)
                    return true;
             
                if (pid <= 0)
                {
             
                    return false;
                }

                theProc = _p[0];

                if (theProc != null && !theProc.Responding)
                {
             
                    return false;
                }
             
                pHandle = OpenProcess(0x1F0FFF, true, pid);
             
                //Process.EnterDebugMode();
             
                if (pHandle == IntPtr.Zero)
                {
                    var eCode = Marshal.GetLastWin32Error();
                }
             
                mainModule = theProc.MainModule;

                GetModules();

                // Lets set the process to 64bit or not here (cuts down on api calls)
                Is64Bit = Environment.Is64BitOperatingSystem && (IsWow64Process(pHandle, out bool retVal) && !retVal);


                return true;
            }
            catch
            {
                Debug.WriteLine("ERROR: OpenProcess has crashed. Are you trying to hack a x64 game? https://github.com/erfg12/memory.dll/wiki/64bit-Games");
                return false;
            }
        }

        /// <summary>
        /// Check if program is running with administrative privileges. Read about it here: https://github.com/erfg12/memory.dll/wiki/Administrative-Privileges
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Check if opened process is 64bit. Used primarily for getCode().
        /// </summary>
        /// <returns>True if 64bit false if 32bit.</returns>
        private bool _is64Bit;
        public bool Is64Bit
        {
            get { return _is64Bit; }
            private set { _is64Bit = value; }
        }


        /// <summary>
        /// Builds the process modules dictionary (names with addresses).
        /// </summary>
        public void GetModules()
        {
            if (theProc == null)
                return;

            modules.Clear();
            foreach (ProcessModule Module in theProc.Modules)
            {
                if (!string.IsNullOrEmpty(Module.ModuleName) && !modules.ContainsKey(Module.ModuleName))
                    modules.Add(Module.ModuleName, Module.BaseAddress);
            }
        }

        /// <summary>
        /// Get the process ID number by process name.
        /// </summary>
        /// <param name="name">Example: "eqgame". Use task manager to find the name. Do not include .exe</param>
        /// <returns></returns>
        public int GetProcIdFromName(string name) //new 1.0.2 function
        {
            Process[] processlist = Process.GetProcesses();

            if (name.Contains(".exe"))
                name = name.Replace(".exe", "");

            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName.Equals(name, StringComparison.CurrentCultureIgnoreCase)) //find (name).exe in the process list (use task manager to find the name)
                    return theprocess.Id;
            }

            return 0; //if we fail to find it
        }



        /// <summary>
        /// Get code from ini file.
        /// </summary>
        /// <param name="name">label for address or code</param>
        /// <param name="file">path and name of ini file</param>
        /// <returns></returns>
        public string LoadCode(string name, string file)
        {
            StringBuilder returnCode = new StringBuilder(1024);
            uint read_ini_result;

            if (file != "")
                read_ini_result = GetPrivateProfileString("codes", name, "", returnCode, (uint)returnCode.Capacity, file);
            else
                returnCode.Append(name);

            return returnCode.ToString();
        }


        /// <summary>
        /// Dictionary with our opened process module names with addresses.
        /// </summary>
        public Dictionary<string, IntPtr> modules = new Dictionary<string, IntPtr>();

        /// <summary>
        /// Make a named pipe (if not already made) and call to a remote function.
        /// </summary>
        /// <param name="func">remote function to call</param>
        /// <param name="name">name of the thread</param>
        public void ThreadStartClient(string func, string name)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream(name))
            {
                if (!pipeStream.IsConnected)
                    pipeStream.Connect();

                //MessageBox.Show("[Client] Pipe connection established");
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    if (!sw.AutoFlush)
                        sw.AutoFlush = true;
                    sw.WriteLine(func);
                }
            }
        }

        private ProcessModule mainModule;

        #region IoIo
        public static bool IsGameAvailable(string name)
        {
            return Process.GetProcessesByName(name).Length != 0 ? true : false;
        }
        public bool IsCreateGame()
        {
            return false;
        }
        public bool IsJoinGame()
        {
            return false;
        }
        public bool IsStartGame()
        {
            //ReadFloat("")
            try
            {
                return ReadFloat("main+0x3C4B18,0x51C,0x114,0x5C,0x4,0x10", "", true) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool isEndGame()
        {
            return ReadFloat("main+0x3C4B18,0x51C,0x114,0x5C,0x4,0x10", "", true) == 0;
        }

        public int StartGame()
        {
            int result = (int)Category.AoeStatus.StartGame;

            // Read Empire;
            for (int i = 0; i < 8; i++)
            {
                int empire = ReadByte("0x585e88,a9" + (4 + i), "");
                string king = string.Empty;
                int ally = 0;
                int flag = 0;
                if (empire > 0)
                    App.MainViewModel.Match.Empires.Add(new Models.Empire(empire, king, flag, ally));
                else
                    i = 8; //stop loop;
            }
            
            Address.Time = GetCode("main+0x3C4B18,0x51C,0x114,0x5C,0x4,0x10", "");
            
            ProcessModule Module = theProc.Modules[0];
            byte[]  ma = new byte[8];
            ReadProcessMemory(pHandle, (UIntPtr)((int)Module.BaseAddress + Constant.AddressBase[(int)Category.AddressBase.Empire]), ma, (UIntPtr)8, IntPtr.Zero);
            uint num1 = BitConverter.ToUInt32(ma, 0);

            // Reset then Store Empire Match Base in first
            //if(Address.Base.Count > 0)
            //{
            //    Address.Base = new List<UIntPtr>();
            //    Address.Base.Add(new UIntPtr(num1));
            //}
            

            UIntPtr base1 = (UIntPtr)0;
            for (int i = 0; i < 3; i++)
            {
                base1 = new UIntPtr(Convert.ToUInt32(num1 + Constant.Offset[0][i]));
                ReadProcessMemory(pHandle, base1, ma, (UIntPtr)8, IntPtr.Zero);
                num1 = BitConverter.ToUInt32(ma, 0); //ToUInt64 causes arithmetic overflow.
            }
            //Address.EwE.Add(base1);
            uint num2 = num1;
            for (int j = 1; j < 9; j++)
            {
                base1 = new UIntPtr(Convert.ToUInt32(num2 + Constant.Offset[0][3] * j));

                ReadProcessMemory(pHandle, base1, ma, (UIntPtr)8, IntPtr.Zero);
                num1 = BitConverter.ToUInt32(ma, 0); //ToUInt64 causes arithmetic overflow.
                
                for (int i = 4; i < Constant.Offset[0].Length; i++)
                {
                    base1 = new UIntPtr(Convert.ToUInt32(num1 + Constant.Offset[0][i]));
                    ReadProcessMemory(pHandle, base1, ma, (UIntPtr)8, IntPtr.Zero);
                    num1 = BitConverter.ToUInt32(ma, 0); //ToUInt64 causes arithmetic overflow.
                }
                Address.EwE.Add(base1);
            }
            
            return result;
        }

        public void ScoutMatch(ref Models.Match match)
        {
            string report = string.Empty;
            float time = ReadFloat(Address.Time, false);
            //if (time > 0 && (int)time % 10 == 0)
            //    Console.WriteLine("Time:++" + time);
            if (time == 0)
                match.Status = (int)Category.MatchStatus.End;
            else if (ReadByte("ExWE.dll+0x26a48,930","") == 0)
                match.Status = (int)Category.MatchStatus.Pause;
            else if(match.Status == (int)Category.MatchStatus.Pause)
                match.Status = (int)Category.MatchStatus.Resume;
            else
                match.Status = (int)Category.MatchStatus.Start;

            if(match.Status == (int)Category.MatchStatus.Start || match.Status == (int)Category.MatchStatus.Resume)
            {
                // update time
                match.Time = time;

                // update cac chi so khac: kill,death, gold ...
            }
            
        }

        public List<string> getAddressAoe()
        {
            List<string> result = new List<string>();
            // Read Empire;
            for (int i = 0; i < 8; i++)
            {
                int empire = ReadByte("0x585e88,a9" + (4 + i), "");
                string king = string.Empty;
                int ally = 0;
                int flag = 0;
                if (empire > 0)
                    App.MainViewModel.Match.Empires.Add(new Models.Empire(empire, king, flag, ally));
                else
                    i = 8; //stop loop;
            }
            

            //Console.WriteLine("Time: " + ReadFloat("main+0x3C4B18,0x51C,0x114,0x5C,0x4,0x10", "", true)); 

            ProcessModule Module = theProc.Modules[0];
            result.Add("BaseAddress: " + Module.BaseAddress);
            byte[] memoryAddress = new byte[8];
            ReadProcessMemory(pHandle, (UIntPtr)((int)Module.BaseAddress + Constant.AddressBase[0]), memoryAddress, (UIntPtr)8, IntPtr.Zero);
            uint num1 = BitConverter.ToUInt32(memoryAddress, 0);

            UIntPtr base1 = (UIntPtr)0;
            for (int i = 0; i < 3; i++)
            {
                base1 = new UIntPtr(Convert.ToUInt32(num1 + Constant.Offset[0][i]));
                ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)8, IntPtr.Zero);
                num1 = BitConverter.ToUInt32(memoryAddress, 0); //ToUInt64 causes arithmetic overflow.
            }
            Address.EwE.Add(base1);
            string ewf = string.Empty;
            uint num2 = num1;
            for (int j = 1; j < 9; j++)
            {
                base1 = new UIntPtr(Convert.ToUInt32(num2 + Constant.Offset[0][3]*j));


                ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)8, IntPtr.Zero);
                num1 = BitConverter.ToUInt32(memoryAddress, 0); //ToUInt64 causes arithmetic overflow.
                Console.WriteLine(num1);
                for (int i = 4; i < Constant.Offset[0].Length; i++)
                {
                    base1 = new UIntPtr(Convert.ToUInt32(num1 + Constant.Offset[0][i]));
                    ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)8, IntPtr.Zero);
                    num1 = BitConverter.ToUInt32(memoryAddress, 0); //ToUInt64 causes arithmetic overflow.
                }
                Address.EwE.Add(base1);
                ewf += (uint)base1 + "        ";
            }
            result.Add(ewf);
            ewf = string.Empty;
            byte[] memory = new byte[4];
            for(int i = 1; i < Address.EwE.Count; i++)
            {
                if (ReadProcessMemory(pHandle, Address.EwE[i] + 4, memory, (UIntPtr)4, IntPtr.Zero))
                {
                    float address = BitConverter.ToSingle(memory, 0);
                    float returnValue = (float)address;
                    ewf += "Wood: " + returnValue +"        ";
                }
            }
            result.Add(ewf);
            return result;
        }

        public List<string> ScoutEmpireAoe()
        {
            List<string> result = new List<string>();
            string ee = string.Empty;
            for(int i=1; i < 9; i++)
            {
                ee += "Empire" + i+"         ";
            }
            result.Add(ee);
            foreach (var item in Constant.LabelStartFood)
            {
                string ewf = string.Empty;
                byte[] memory = new byte[4];
                for (int i = 1; i < Address.EwE.Count; i++)
                {
                    if (ReadProcessMemory(pHandle, Address.EwE[i] + 4* item.Key, memory, (UIntPtr)4, IntPtr.Zero))
                    {
                        float address = BitConverter.ToSingle(memory, 0);
                        float returnValue = (float)address;
                        ewf += item.Value[0]+ ": " + returnValue + "        ";
                    }
                }
                result.Add(ewf);
            }
            
            return result;
        }

        //public static MakeAddress(string altModule){
        //    string theAddr = moduleName[0];

        //    if (theAddr.Contains("0x")) theAddr = theAddr.Replace("0x", "");
        //    altModule = (IntPtr)Int32.Parse(theAddr, NumberStyles.HexNumber);
        //    ReadProcessMemory(pHandle, (UIntPtr)((Int64)altModule + offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);
        //        Int64 num1 = BitConverter.ToInt64(memoryAddress, 0);

        //        UIntPtr base1 = (UIntPtr)0;

        //        for (int i = 1; i < offsets.Length; i++)
        //        {
        //            base1 = new UIntPtr(Convert.ToUInt64(num1 + offsets[i]));
        //            ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)size, IntPtr.Zero);
        //            num1 = BitConverter.ToInt64(memoryAddress, 0);
        //        }
        //        return base1;
        //}
        #endregion

        #region readMemory
        /// <summary>
        /// Reads up to `length ` bytes from an address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="length">The maximum bytes to read.</param>
        /// <param name="file">path and name of ini file.</param>
        /// <returns>The bytes read or null</returns>
        public byte[] ReadBytes(string code, long length, string file = "")
        {
            byte[] memory = new byte[length];
            UIntPtr theCode = GetCode(code, file);

            if (!ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)length, IntPtr.Zero))
                return null;

            return memory;
        }
        public byte[] ReadBytes(UIntPtr theCode, long length)
        {
            byte[] memory = new byte[length];

            if (!ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)length, IntPtr.Zero))
                return null;

            return memory;
        }

        /// <summary>
        /// Read a float value from an address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <param name="round">Round the value to 2 decimal places</param>
        /// <returns></returns>
        public float ReadFloat(string code, string file = "", bool round = true)
        {
            byte[] memory = new byte[4];

            UIntPtr theCode;
            theCode = GetCode(code, file);
            try
            {
                if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                {
                    float address = BitConverter.ToSingle(memory, 0);
                    float returnValue = (float)address;
                    if (round)
                        returnValue = (float)Math.Round(address, 2);
                    return returnValue;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public float ReadFloat(UIntPtr theCode, bool round = true)
        {
            byte[] memory = new byte[4];

            try
            {
                if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                {
                    float address = BitConverter.ToSingle(memory, 0);
                    float returnValue = (float)address;
                    if (round)
                        returnValue = (float)Math.Round(address, 2);
                    return returnValue;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Read a string value from an address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <param name="length">length of bytes to read (OPTIONAL)</param>
        /// <param name="zeroTerminated">terminate string at null char</param>
        /// <returns></returns>
        public string ReadString(string code, string file = "", int length = 32, bool zeroTerminated = true)
        {
            byte[] memoryNormal = new byte[length];
            UIntPtr theCode;
            theCode = GetCode(code, file);
            if (ReadProcessMemory(pHandle, theCode, memoryNormal, (UIntPtr)length, IntPtr.Zero))
                return (zeroTerminated) ? Encoding.UTF8.GetString(memoryNormal).Split('\0')[0] : Encoding.UTF8.GetString(memoryNormal);
            else
                return "";
        }
        public string ReadString(UIntPtr theCode, int length = 32, bool zeroTerminated = true)
        {
            byte[] memoryNormal = new byte[length];
            if (ReadProcessMemory(pHandle, theCode, memoryNormal, (UIntPtr)length, IntPtr.Zero))
                return (zeroTerminated) ? Encoding.UTF8.GetString(memoryNormal).Split('\0')[0] : Encoding.UTF8.GetString(memoryNormal);
            else
                return "";
        }

        /// <summary>
        /// Read a double value
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <param name="round">Round the value to 2 decimal places</param>
        /// <returns></returns>
        public double ReadDouble(string code, string file = "", bool round = true)
        {
            byte[] memory = new byte[8];

            UIntPtr theCode;
            theCode = GetCode(code, file);
            try
            {
                if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)8, IntPtr.Zero))
                {
                    double address = BitConverter.ToDouble(memory, 0);
                    double returnValue = (double)address;
                    if (round)
                        returnValue = (double)Math.Round(address, 2);
                    return returnValue;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public double ReadDouble(UIntPtr theCode, bool round = true)
        {
            byte[] memory = new byte[8];
            try
            {
                if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)8, IntPtr.Zero))
                {
                    double address = BitConverter.ToDouble(memory, 0);
                    double returnValue = (double)address;
                    if (round)
                        returnValue = (double)Math.Round(address, 2);
                    return returnValue;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public int ReadUIntPtr(UIntPtr code)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, code, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Read an integer from an address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <returns></returns>
        public int ReadInt(string code, string file = "")
        {
            byte[] memory = new byte[4];
            UIntPtr theCode;
            theCode = GetCode(code, file);
            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }
        public int ReadInt(UIntPtr lpBaseAddress){
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, lpBaseAddress, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }
        /// <summary>
        /// Read a long value from an address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <returns></returns>
        public long ReadLong(string code, string file = "")
        {
            byte[] memory = new byte[16];
            UIntPtr theCode;

            theCode = GetCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)16, IntPtr.Zero))
                return BitConverter.ToInt64(memory, 0);
            else
                return 0;
        }
        public long ReadLong(UIntPtr theCode)
        {
            byte[] memory = new byte[16];
            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)16, IntPtr.Zero))
                return BitConverter.ToInt64(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Read a UInt value from address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        /// <returns></returns>
        public UInt64 ReadUInt(string code, string file = "")
        {
            byte[] memory = new byte[4];
            UIntPtr theCode;
            theCode = GetCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToUInt64(memory, 0);
            else
                return 0;
        }
        public UInt64 ReadUInt(UIntPtr theCode)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToUInt64(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Reads a 2 byte value from an address and moves the address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="moveQty">Quantity to move.</param>
        /// <param name="file">path and name of ini file (OPTIONAL)</param>
        /// <returns></returns>
        public int Read2ByteMove(string code, int moveQty, string file = "")
        {
            byte[] memory = new byte[4];
            UIntPtr theCode;
            theCode = GetCode(code, file);

            UIntPtr newCode = UIntPtr.Add(theCode, moveQty);

            if (ReadProcessMemory(pHandle, newCode, memory, (UIntPtr)2, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Reads an integer value from address and moves the address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="moveQty">Quantity to move.</param>
        /// <param name="file">path and name of ini file (OPTIONAL)</param>
        /// <returns></returns>
        public int ReadIntMove(string code, int moveQty, string file = "")
        {
            byte[] memory = new byte[4];
            UIntPtr theCode;
            theCode = GetCode(code, file);

            UIntPtr newCode = UIntPtr.Add(theCode, moveQty);

            if (ReadProcessMemory(pHandle, newCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Get UInt and move to another address by moveQty. Use in a for loop.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="moveQty">Quantity to move.</param>
        /// <param name="file">path and name of ini file (OPTIONAL)</param>
        /// <returns></returns>
        public ulong ReadUIntMove(string code, int moveQty, string file = "")
        {
            byte[] memory = new byte[8];
            UIntPtr theCode;
            theCode = GetCode(code, file, 8);

            UIntPtr newCode = UIntPtr.Add(theCode, moveQty);

            if (ReadProcessMemory(pHandle, newCode, memory, (UIntPtr)8, IntPtr.Zero))
                return BitConverter.ToUInt64(memory, 0);
            else
                return 0;
        }

        /// <summary>
        /// Read a 2 byte value from an address. Returns an integer.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and file name to ini file. (OPTIONAL)</param>
        /// <returns></returns>
        public int Read2Byte(string code, string file = "")
        {
            byte[] memoryTiny = new byte[4];

            UIntPtr theCode;
            theCode = GetCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memoryTiny, (UIntPtr)2, IntPtr.Zero))
                return BitConverter.ToInt32(memoryTiny, 0);
            else
                return 0;
        }

        public int Read2Byte(UIntPtr theCode)
        {
            byte[] memoryTiny = new byte[4];
            if (ReadProcessMemory(pHandle, theCode, memoryTiny, (UIntPtr)2, IntPtr.Zero))
                return BitConverter.ToInt32(memoryTiny, 0);
            else
                return 0;
        }

        /// <summary>
        /// Read 1 byte from address.
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and file name of ini file. (OPTIONAL)</param>
        /// <returns></returns>
        public int ReadByte(string code, string file = "")
        {
            byte[] memoryTiny = new byte[1];

            UIntPtr theCode = GetCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memoryTiny, (UIntPtr) 1, IntPtr.Zero))
                return memoryTiny[0];

            return 0;
        }

        public int ReadByte(UIntPtr theCode)
        {
            byte[] memoryTiny = new byte[1];

            if (ReadProcessMemory(pHandle, theCode, memoryTiny, (UIntPtr)1, IntPtr.Zero))
                return memoryTiny[0];

            return 0;
        }

        /// <summary>
        /// Reads a byte from memory and splits it into bits
        /// </summary>
        /// <param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        /// <param name="file">path and file name of ini file. (OPTIONAL)</param>
        /// <returns>Array of 8 booleans representing each bit of the byte read</returns>
        public bool[] ReadBits(string code, string file = "")
        {
            byte[] buf = new byte[1];

            UIntPtr theCode = GetCode(code, file);

            bool[] ret = new bool[8];

            if (!ReadProcessMemory(pHandle, theCode, buf, (UIntPtr) 1, IntPtr.Zero))
                return ret;
            

            if (!BitConverter.IsLittleEndian)
                throw new Exception("Should be little endian");

            for (var i = 0; i < 8; i++)
                ret[i] = Convert.ToBoolean(buf[0] & (1 << i));

            return ret;

        }


        #endregion

        #region writeMemory
        ///<summary>
        ///Write to memory address. See https://github.com/erfg12/memory.dll/wiki/writeMemory() for more information.
        ///</summary>
        ///<param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        ///<param name="type">byte, 2bytes, bytes, float, int, string, double or long.</param>
        ///<param name="write">value to write to address.</param>
        ///<param name="file">path and name of .ini file (OPTIONAL)</param>
        ///<param name="stringEncoding">System.Text.Encoding.UTF8 (DEFAULT). Other options: ascii, unicode, utf32, utf7</param>
        public bool WriteMemory(string code, string type, string write, string file = "", System.Text.Encoding stringEncoding = null)
        {
            byte[] memory = new byte[4];
            int size = 4;

            UIntPtr theCode;
            theCode = GetCode(code, file);

            if (type.ToLower() == "float")
            {
                memory = BitConverter.GetBytes(Convert.ToSingle(write));
                size = 4;
            }
            else if (type.ToLower() == "int")
            {
                memory = BitConverter.GetBytes(Convert.ToInt32(write));
                size = 4;
            }
            else if (type.ToLower() == "byte")
            {
                memory = new byte[1];
                memory[0] = Convert.ToByte(write, 16);
                size = 1;
            }
            else if (type.ToLower() == "2bytes")
            {
                memory = new byte[2];
                memory[0] = (byte)(Convert.ToInt32(write) % 256);
                memory[1] = (byte)(Convert.ToInt32(write) / 256);
                size = 2;
            }
            else if (type.ToLower() == "bytes")
            {
                if (write.Contains(",") || write.Contains(" ")) //check if it's a proper array
                {
                    string[] stringBytes;
                    if (write.Contains(","))
                        stringBytes = write.Split(',');
                    else
                        stringBytes = write.Split(' ');
                    //Debug.WriteLine("write:" + write + " stringBytes:" + stringBytes);

                    int c = stringBytes.Count();
                    memory = new byte[c];
                    for (int i = 0; i < c; i++)
                    {
                        memory[i] = Convert.ToByte(stringBytes[i], 16);
                    }
                    size = stringBytes.Count();
                }
                else //wasnt array, only 1 byte
                {
                    memory = new byte[1];
                    memory[0] = Convert.ToByte(write, 16);
                    size = 1;
                }
            }
            else if (type.ToLower() == "double")
            {
                memory = BitConverter.GetBytes(Convert.ToDouble(write));
                size = 8;
            }
            else if (type.ToLower() == "long")
            {
                memory = BitConverter.GetBytes(Convert.ToInt64(write));
                size = 8;
            }
            else if (type.ToLower() == "string")
            {
                if (stringEncoding == null)
                    memory = System.Text.Encoding.UTF8.GetBytes(write);
                else
                    memory = stringEncoding.GetBytes(write);
                size = memory.Length;
            }
            //Debug.Write("DEBUG: Writing bytes [TYPE:" + type + " ADDR:" + theCode + "] " + String.Join(",", memory) + Environment.NewLine);
            return WriteProcessMemory(pHandle, theCode, memory, (UIntPtr)size, IntPtr.Zero);
        }

        /// <summary>
        /// Write to address and move by moveQty. Good for byte arrays. See https://github.com/erfg12/memory.dll/wiki/Writing-a-Byte-Array for more information.
        /// </summary>
        ///<param name="code">address, module + pointer + offset, module + offset OR label in .ini file.</param>
        ///<param name="type">byte, bytes, float, int, string or long.</param>
        /// <param name="write">byte to write</param>
        /// <param name="moveQty">quantity to move</param>
        /// <param name="file">path and name of .ini file (OPTIONAL)</param>
        /// <returns></returns>
        public bool WriteMove(string code, string type, string write, int moveQty, string file = "")
        {
            byte[] memory = new byte[4];
            int size = 4;

            UIntPtr theCode;
            theCode = GetCode(code, file);

            if (type == "float")
            {
                memory = new byte[write.Length];
                memory = BitConverter.GetBytes(Convert.ToSingle(write));
                size = write.Length;
            }
            else if (type == "int")
            {
                memory = BitConverter.GetBytes(Convert.ToInt32(write));
                size = 4;
            }
            else if (type == "double")
            {
                memory = BitConverter.GetBytes(Convert.ToDouble(write));
                size = 8;
            }
            else if (type == "long")
            {
                memory = BitConverter.GetBytes(Convert.ToInt64(write));
                size = 8;
            }
            else if (type == "byte")
            {
                memory = new byte[1];
                memory[0] = Convert.ToByte(write, 16);
                size = 1;
            }
            else if (type == "string")
            {
                memory = new byte[write.Length];
                memory = System.Text.Encoding.UTF8.GetBytes(write);
                size = write.Length;
            }

            UIntPtr newCode = UIntPtr.Add(theCode, moveQty);

            Debug.Write("DEBUG: Writing bytes [TYPE:" + type + " ADDR:[O]" + theCode + " [N]" + newCode + " MQTY:" + moveQty + "] " + String.Join(",", memory) + Environment.NewLine);
            Thread.Sleep(1000);
            return WriteProcessMemory(pHandle, newCode, memory, (UIntPtr)size, IntPtr.Zero);
        }

        /// <summary>
        /// Write byte array to addresses.
        /// </summary>
        /// <param name="code">address to write to</param>
        /// <param name="write">byte array to write</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        public void WriteBytes(string code, byte[] write, string file = "")
        {
            UIntPtr theCode;
            theCode = GetCode(code, file);
            WriteProcessMemory(pHandle, theCode, write, (UIntPtr)write.Length, IntPtr.Zero);
        }

        /// <summary>
        /// Takes an array of 8 booleans and writes to a single byte
        /// </summary>
        /// <param name="code">address to write to</param>
        /// <param name="bits">Array of 8 booleans to write</param>
        /// <param name="file">path and name of ini file. (OPTIONAL)</param>
        public void WriteBits(string code, bool[] bits, string file = "")
        {
            if(bits.Length != 8)
                throw new ArgumentException("Not enough bits for a whole byte", nameof(bits));

            byte[] buf = new byte[1];

            UIntPtr theCode = GetCode(code, file);

            for (var i = 0; i < 8; i++)
            {
                if (bits[i])
                    buf[0] |= (byte)(1 << i);
            }

            WriteProcessMemory(pHandle, theCode, buf, (UIntPtr) 1, IntPtr.Zero);
        }

        /// <summary>
        /// Write byte array to address
        /// </summary>
        /// <param name="address">Address to write to</param>
        /// <param name="write">Byte array to write to</param>
        public void WriteBytes(UIntPtr address, byte[] write)
        {
            WriteProcessMemory(pHandle, address, write, (UIntPtr)write.Length, out IntPtr bytesRead);
        }

        #endregion

        /// <summary>
        /// Convert code from string to real address. If path is not blank, will pull from ini file.
        /// </summary>
        /// <param name="name">label in ini file or code</param>
        /// <param name="path">path to ini file (OPTIONAL)</param>
        /// <param name="size">size of address (default is 8)</param>
        /// <returns></returns>
        public UIntPtr GetCode(string name, string path = "", int size = 8)
        {
            string theCode = "";
            if (Is64Bit)
            {
                //Debug.WriteLine("Changing to 64bit code...");
                if (size == 8) size = 16; //change to 64bit
                return Get64BitCode(name, path, size); //jump over to 64bit code grab
            }

            if (path != "")
                theCode = LoadCode(name, path);
            else
                theCode = name;

            if (theCode == "")
            {
                //Debug.WriteLine("ERROR: LoadCode returned blank. NAME:" + name + " PATH:" + path);
                return UIntPtr.Zero;
            }
            else
            {
                //Debug.WriteLine("Found code=" + theCode + " NAME:" + name + " PATH:" + path);
            }

            // remove spaces
            if (theCode.Contains(" "))
                theCode.Replace(" ", String.Empty);

            if (!theCode.Contains("+") && !theCode.Contains(",")) return new UIntPtr(Convert.ToUInt32(theCode, 16));

            string newOffsets = theCode;

            if (theCode.Contains("+"))
                newOffsets = theCode.Substring(theCode.IndexOf('+') + 1);

            byte[] memoryAddress = new byte[size];

            if (newOffsets.Contains(','))
            {
                List<int> offsetsList = new List<int>();

                string[] newerOffsets = newOffsets.Split(',');
                foreach (string oldOffsets in newerOffsets)
                {
                    string test = oldOffsets;
                    if (oldOffsets.Contains("0x")) test = oldOffsets.Replace("0x","");
                    int preParse = 0;
                    if (!oldOffsets.Contains("-"))
                        preParse = Int32.Parse(test, NumberStyles.AllowHexSpecifier);
                    else
                    {
                        test = test.Replace("-", "");
                        preParse = Int32.Parse(test, NumberStyles.AllowHexSpecifier);
                        preParse = preParse * -1;
                    }
                    offsetsList.Add(preParse);
                }
                int[] offsets = offsetsList.ToArray();

                if (theCode.Contains("base") || theCode.Contains("main"))
                    ReadProcessMemory(pHandle, (UIntPtr)((int)mainModule.BaseAddress + offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);
                else if (!theCode.Contains("base") && !theCode.Contains("main") && theCode.Contains("+"))
                {
                    string[] moduleName = theCode.Split('+');
                    IntPtr altModule = IntPtr.Zero;
                    if (!moduleName[0].Contains(".dll") && !moduleName[0].Contains(".exe"))
                    {
                        string theAddr = moduleName[0];
                        if (theAddr.Contains("0x")) theAddr = theAddr.Replace("0x", "");
                        altModule = (IntPtr)Int32.Parse(theAddr, NumberStyles.HexNumber);
                    }
                    else
                    {
                        try
                        {
                            altModule = modules[moduleName[0]];
                        }
                        catch
                        {
                            Debug.WriteLine("Module " + moduleName[0] + " was not found in module list!");
                            Debug.WriteLine("Modules: " + string.Join(",", modules));
                        }
                    }
                    ReadProcessMemory(pHandle, (UIntPtr)((int)altModule + offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);
                }
                else
                    ReadProcessMemory(pHandle, (UIntPtr)(offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);

                uint num1 = BitConverter.ToUInt32(memoryAddress, 0); //ToUInt64 causes arithmetic overflow.

                UIntPtr base1 = (UIntPtr)0;

                for (int i = 1; i < offsets.Length; i++)
                {
                    base1 = new UIntPtr(Convert.ToUInt32(num1 + offsets[i]));
                    ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)size, IntPtr.Zero);
                    num1 = BitConverter.ToUInt32(memoryAddress, 0); //ToUInt64 causes arithmetic overflow.
                }
                return base1;
            }
            else // no offsets
            {
                int trueCode = Convert.ToInt32(newOffsets, 16);
                IntPtr altModule = IntPtr.Zero;
                //Debug.WriteLine("newOffsets=" + newOffsets);
                if (theCode.Contains("base") || theCode.Contains("main"))
                    altModule = mainModule.BaseAddress;
                else if (!theCode.Contains("base") && !theCode.Contains("main") && theCode.Contains("+"))
                {
                    string[] moduleName = theCode.Split('+');
                    if (!moduleName[0].Contains(".dll") && !moduleName[0].Contains(".exe"))
                    {
                        string theAddr = moduleName[0];
                        if (theAddr.Contains("0x")) theAddr = theAddr.Replace("0x", "");
                        altModule = (IntPtr)Int32.Parse(theAddr, NumberStyles.HexNumber);
                    }
                    else
                    {
                        try
                        {
                            altModule = modules[moduleName[0]];
                        }
                        catch
                        {
                            Debug.WriteLine("Module " + moduleName[0] + " was not found in module list!");
                            Debug.WriteLine("Modules: " + string.Join(",", modules));
                        }
                    }
                }
                else
                    altModule = modules[theCode.Split('+')[0]];
                return (UIntPtr)((int)altModule + trueCode);
            }
        }

        

        /// <summary>
        /// Convert code from string to real address. If path is not blank, will pull from ini file.
        /// </summary>
        /// <param name="name">label in ini file OR code</param>
        /// <param name="path">path to ini file (OPTIONAL)</param>
        /// <param name="size">size of address (default is 16)</param>
        /// <returns></returns>
        public UIntPtr Get64BitCode(string name, string path = "", int size = 16)
        {
            string theCode = "";
            if (path != "")
                theCode = LoadCode(name, path);
            else
                theCode = name;

            if (theCode == "")
                return UIntPtr.Zero;

            // remove spaces
            if (theCode.Contains(" "))
                theCode.Replace(" ", String.Empty);

            string newOffsets = theCode;
            if (theCode.Contains("+"))
                newOffsets = theCode.Substring(theCode.IndexOf('+') + 1);

            byte[] memoryAddress = new byte[size];

            if (!theCode.Contains("+") && !theCode.Contains(",")) return new UIntPtr(Convert.ToUInt64(theCode, 16));

            if (newOffsets.Contains(','))
            {
                List<Int64> offsetsList = new List<Int64>();

                string[] newerOffsets = newOffsets.Split(',');
                foreach (string oldOffsets in newerOffsets)
                {
                    string test = oldOffsets;
                    if (oldOffsets.Contains("0x")) test = oldOffsets.Replace("0x", "");
                    Int64 preParse = 0;
                    if (!oldOffsets.Contains("-"))
                        preParse = Int64.Parse(test, NumberStyles.AllowHexSpecifier);
                    else
                    {
                        test = test.Replace("-", "");
                        preParse = Int64.Parse(test, NumberStyles.AllowHexSpecifier);
                        preParse = preParse * -1;
                    }
                    offsetsList.Add(preParse);
                }
                Int64[] offsets = offsetsList.ToArray();

                if (theCode.Contains("base") || theCode.Contains("main"))
                    ReadProcessMemory(pHandle, (UIntPtr)((Int64)mainModule.BaseAddress + offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);
                else if (!theCode.Contains("base") && !theCode.Contains("main") && theCode.Contains("+"))
                {
                    string[] moduleName = theCode.Split('+');
                    IntPtr altModule = IntPtr.Zero;
                    if (!moduleName[0].Contains(".dll") && !moduleName[0].Contains(".exe"))
                        altModule = (IntPtr)Int64.Parse(moduleName[0], System.Globalization.NumberStyles.HexNumber);
                    else
                    {
                        try
                        {
                            altModule = modules[moduleName[0]];
                        }
                        catch
                        {
                            Debug.WriteLine("Module " + moduleName[0] + " was not found in module list!");
                            Debug.WriteLine("Modules: " + string.Join(",", modules));
                        }
                    }
                    ReadProcessMemory(pHandle, (UIntPtr)((Int64)altModule + offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);
                }
                else // no offsets
                    ReadProcessMemory(pHandle, (UIntPtr)(offsets[0]), memoryAddress, (UIntPtr)size, IntPtr.Zero);

                Int64 num1 = BitConverter.ToInt64(memoryAddress, 0);

                UIntPtr base1 = (UIntPtr)0;

                for (int i = 1; i < offsets.Length; i++)
                {
                    base1 = new UIntPtr(Convert.ToUInt64(num1 + offsets[i]));
                    ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)size, IntPtr.Zero);
                    num1 = BitConverter.ToInt64(memoryAddress, 0);
                }
                return base1;
            }
            else
            {
                Int64 trueCode = Convert.ToInt64(newOffsets, 16);
                IntPtr altModule = IntPtr.Zero;
                if (theCode.Contains("base") || theCode.Contains("main"))
                    altModule = mainModule.BaseAddress;
                else if (!theCode.Contains("base") && !theCode.Contains("main") && theCode.Contains("+"))
                {
                    string[] moduleName = theCode.Split('+');
                    if (!moduleName[0].Contains(".dll") && !moduleName[0].Contains(".exe"))
                    {
                        string theAddr = moduleName[0];
                        if (theAddr.Contains("0x")) theAddr = theAddr.Replace("0x", "");
                        altModule = (IntPtr)Int64.Parse(theAddr, NumberStyles.HexNumber);
                    }
                    else
                    {
                        try
                        {
                            altModule = modules[moduleName[0]];
                        }
                        catch
                        {
                            Debug.WriteLine("Module " + moduleName[0] + " was not found in module list!");
                            Debug.WriteLine("Modules: " + string.Join(",", modules));
                        }
                    }
                }
                else
                    altModule = modules[theCode.Split('+')[0]];
                return (UIntPtr)((Int64)altModule + trueCode);
            }
        }

        /// <summary>
        /// Close the process when finished.
        /// </summary>
        public void CloseProcess()
        {
            if (pHandle == null)
                return;

            CloseHandle(pHandle);
            theProc = null;
        }


#if WINXP
#else
        
#endif

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }




        public string MSize()
        {
            if (Is64Bit)
                return ("x16");
            else
                return ("x8");
        }


#if WINXP
#else

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public UIntPtr minimumApplicationAddress;
            public UIntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        public struct MEMORY_BASIC_INFORMATION32
        {
            public UIntPtr BaseAddress;
            public UIntPtr AllocationBase;
            public uint AllocationProtect;
            public uint RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        public struct MEMORY_BASIC_INFORMATION64
        {
            public UIntPtr BaseAddress;
            public UIntPtr AllocationBase;
            public uint AllocationProtect;
            public uint __alignment1;
            public ulong RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
            public uint __alignment2;
        }

        public struct MEMORY_BASIC_INFORMATION
        {
            public UIntPtr BaseAddress;
            public UIntPtr AllocationBase;
            public uint AllocationProtect;
            public long RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        public ulong GetMinAddress()
        {
            SYSTEM_INFO SI;
            GetSystemInfo(out SI);
            return (ulong)SI.minimumApplicationAddress;
        }
#endif
    }
}