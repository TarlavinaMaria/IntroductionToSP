using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Management;
using System.Diagnostics.Eventing.Reader;

namespace InterprocessCommunication
{
    public partial class Form1 : Form
    {
        const uint WM_SETTEXT = 0x0C;
        [DllImport("user32.dll")]
        public static extern 
            IntPtr SendMessage(IntPtr hwnd, uint uMsg, int wParam, [MarshalAs(UnmanagedType.LPStr)]string lParam);
        List<Process> processes = new List<Process>();
        int count = 0;
        public Form1()
        {
            InitializeComponent();
            LoadAvailableAssemblies();
            buttonStop.Enabled = false;
            buttonCloseWindow.Enabled = false;
        }
        void LoadAvailableAssemblies()
        {
            //Application.StartupPath - приложение, Application.ExecutablePath - где находится
            string except = new FileInfo(Application.ExecutablePath).Name;
            except.Substring(0, except.IndexOf("."));
            string[] files = Directory.GetFiles(Application.StartupPath, "*.exe");
            foreach(string file in files)
            {
                string fileName = new FileInfo(file).Name;
                if(fileName.IndexOf(except) == -1)
                {
                    listBoxAssemblies.Items.Add(fileName);
                }
            }
        }
        void RunProcess(string assemblyName)
        {
            //Process proc = Process.Start(assemblyName.Split('.')[0]);           
            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo(assemblyName);
            proc.Start();
            processes.Add(proc);
            if(Process.GetCurrentProcess().Id == GetParantProcessId(proc.Id))
            {
                MessageBox.Show(this, proc.ProcessName + " дочерний процесс текущего процесса.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                proc.Exited += Proc_Exited;
                SendMessage(proc.MainWindowHandle, WM_SETTEXT, 0, $"Chiled process #{count++}");
                if(!listBoxProcesses.Items.Contains(proc.ProcessName))
                {
                    listBoxProcesses.Items.Remove(listBoxAssemblies.SelectedItem);
                }
            }
        }
        void Proc_Exited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Process proc = sender as Process;
            listBoxProcesses.Items.Remove(proc.ProcessName);
            listBoxAssemblies.Items.Add(proc.ProcessName);
            processes.Add(proc);
            count--;
            int index = 0;
            foreach(Process process in processes)
            {
                SendMessage(process.MainWindowHandle, WM_SETTEXT, 0, $"Child process# {++index}");
            }
        }
        int GetParantProcessId(int id)
        {
            int parentId = 0;
            using (System.Management.ManagementObject obj = new ManagementObject($"win32_process.handle = {id}"))
            {
                obj.Get();
                parentId = Convert.ToInt32(obj["ParentProcessId"]);
            }
            return parentId;
        }
        //delegate — это тип, который представляет ссылки на методы с определенным списком параметров и типом возвращаемого значения.
        delegate void ProcessDelegate(Process proc);
        void ExecuteOnProcessByName(string processName, ProcessDelegate function)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach(Process process in processes) 
            {
                if(Process.GetCurrentProcess().Id == GetParantProcessId(process.Id))
                {
                    function(process);
                }
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            RunProcess(listBoxAssemblies.SelectedItems.ToString());
        }
        void Kill(Process proc)
        {
            proc.Kill();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            ExecuteOnProcessByName(listBoxProcesses.SelectedItems.ToString(), Kill);
            listBoxProcesses.Items.Remove(listBoxProcesses.SelectedItems);
        }
        void CloseMainWindow(Process proc)
        {
            proc.CloseMainWindow();
        }
        private void buttonCloseWindow_Click(object sender, EventArgs e)
        {
            ExecuteOnProcessByName(listBoxProcesses.SelectedItems.ToString(), CloseMainWindow);
            listBoxProcesses.Items.Remove(listBoxProcesses.SelectedItems);
        }
        void Refresh(Process proc)
        {
            proc.Refresh();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ExecuteOnProcessByName(listBoxProcesses.SelectedItems.ToString(), Refresh);
        }

        private void listBoxAssemblies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxAssemblies.SelectedItems.Count == 0) buttonStart.Enabled = false;
            else buttonStart.Enabled = true;
        }

        private void listBoxProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxProcesses.SelectedItems.Count == 0)
            {
                buttonStop.Enabled = false;
                buttonCloseWindow.Enabled = false;
            }
            else
            {
                buttonStop.Enabled = true;
                buttonCloseWindow.Enabled = true;
            }
        }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
