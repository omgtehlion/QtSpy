using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Script.Serialization;
using Winapi;

namespace QtSpy
{
    class QtProcess
    {
        public Process Process;
        public FileVersionInfo QtVersion;
        bool Injected;
        NamedPipeClientStream Pipe;
        StreamReader PipeReader;
        JavaScriptSerializer Json = new JavaScriptSerializer();


        static readonly HashSet<int> nonQtProcesses = new HashSet<int>();
        static readonly Dictionary<int, QtProcess> qtProcesses = new Dictionary<int, QtProcess>();

        const string Dumplib = "dumplib.dll";

        [DllImport(Dumplib)]
        static extern int InjectInto(int pid);

        void Inject()
        {
            if (Injected)
                return;

            var dumplib = Path.GetFullPath(Dumplib); // TODO: fix path

            if (Process.Modules.Cast<ProcessModule>().Any(m =>
                Path.GetFullPath(m.FileName).Equals(dumplib, StringComparison.InvariantCultureIgnoreCase))) {
                Injected = true;
                return;
            }

            var ret = InjectInto(Process.Id);
            // TODO: check ret != 0
            Injected = true;
        }

        NamedPipeClientStream OpenPipe()
        {
            if (Pipe != null)
                return Pipe;

            Inject();

            NamedPipeClientStream result = null;
            try {
                result = new NamedPipeClientStream("QtSpy_" + Process.Id);
                result.Connect(100);
            } catch {
                if (result != null)
                    result.Dispose();
                return null;
            }
            PipeReader = new StreamReader(result);
            return Pipe = result;
        }

        public Dictionary<string, object> ExecCommand(object cmd)
        {
            if (OpenPipe() == null)
                return null;

            var buff = Encoding.UTF8.GetBytes(Json.Serialize(cmd) + "\n");
            Pipe.Write(buff, 0, buff.Length);
            Pipe.Flush();
            var s = PipeReader.ReadLine();
            if (s == null)
                return null;
            return (Dictionary<string, object>)Json.DeserializeObject(s);
        }

        public static QtProcess FromWindow(WindowBase w)
        {
            var pid = w.WindowThreadProcessId;

            if (nonQtProcesses.Contains(pid))
                return null;

            if (qtProcesses.ContainsKey(pid))
                return qtProcesses[pid];

            try {
                var proc = Process.GetProcessById(pid);
                foreach (var m in proc.Modules.Cast<ProcessModule>().Where(
                    m => m.ModuleName.Equals("QtGui4.dll", StringComparison.InvariantCultureIgnoreCase))) {
                    var qproc = new QtProcess {
                        Process = proc,
                        QtVersion = m.FileVersionInfo,
                    };
                    qtProcesses.Add(pid, qproc);
                    return qproc;
                }
            } catch { }

            nonQtProcesses.Add(pid);
            return null;
        }
    }
}
