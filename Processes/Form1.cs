using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Processes
{
    public partial class Form1 : Form
    {
        private Stack<Process> processStack = new Stack<Process>();
        public Form1()
        {
            InitializeComponent();
            richTextBoxProcessName.Text = "notepad";
            InitProcess();

        }
        void Form1_Closing(object sender, CancelEventArgs e)
        {
            if (processStack.Count > 0)
            {
                //myProcess.CloseMainWindow();
                //myProcess.Close();
            }
        }
        void InitProcess()
        {
            AllignText();
            //myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(richTextBoxProcessName.Text);

        }
        void AllignText()
        {
            richTextBoxProcessName.SelectAll();
            richTextBoxProcessName.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //InitProcess();
            //myProcess.Start();
            //Info();
            
            myProcess = Process.Start($"{richTextBoxProcessName.Text}.exe");
            processStack.Push(myProcess); // Добавляем процесс в стек
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (processStack.Count > 0)
            {
                // Завершаем последний процесс
                Process lastProcess = processStack.Pop();
                if (!lastProcess.HasExited)
                {
                    lastProcess.Kill();
                    lastProcess.WaitForExit();
                }

                // Переводим на передний план предпоследний процесс если таковой есть
                if (processStack.Count > 0)
                {
                    Process previousProcess = processStack.Peek(); // Взглянуть на верхушку стека без удаления
                    if (previousProcess != null && !previousProcess.HasExited)
                    {
                        IntPtr handle = previousProcess.MainWindowHandle;
                        if (handle != IntPtr.Zero)
                        {
                            // Переводим окно процесса на передний план
                            SetForegroundWindow(handle);
                        }
                    }
                }
                //myProcess.CloseMainWindow();//Закрывает процесс
                //myProcess.Close(); // Освобождает ресурсы занимаемые процессом
            }
        }
        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (processStack.Count > 0)
            {
                //myProcess.CloseMainWindow();//Закрывает процесс
                //myProcess.Close(); // Освобождает ресурсы занимаемые процессом
            }
        }
        void Info()
        {
            labelProcessInfo.Text = "Process Info:\n";
            labelProcessInfo.Text += $"PID:                      {myProcess.Id}\n";
            labelProcessInfo.Text += $"BasePriority:             {myProcess.BasePriority}\n";
            labelProcessInfo.Text += $"PriorityClass:            {myProcess.PriorityClass}\n";
            labelProcessInfo.Text += $"StartTime:                {myProcess.StartTime}\n";
            labelProcessInfo.Text += $"UserProcessorTime:        {myProcess.UserProcessorTime}\n";
            labelProcessInfo.Text += $"TotalProcessorTime:       {myProcess.TotalProcessorTime}\n";
            labelProcessInfo.Text += $"SessionId:                {myProcess.SessionId}\n";
            labelProcessInfo.Text += $"ProcessName:              {myProcess.ProcessName}\n";
            labelProcessInfo.Text += $"Threads:                  {myProcess.Threads.Count}\n";
        }
    }
}
