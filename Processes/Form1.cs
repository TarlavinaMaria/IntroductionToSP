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
        List<Process> process_list;
        public Form1()
        {
            InitializeComponent();
            process_list = new List<Process>();
            richTextBoxProcessName.Text = "notepad";
            //InitProcess();

        }
        //void Form1_Closing(object sender, CancelEventArgs e)
        //{
        //    if (processStack.Count > 0)
        //    {
        //        //myProcess.CloseMainWindow();
        //        //myProcess.Close();
        //    }
        //}
        void InitProcess()
        {
            AllignText();
            myProcess = new Process();
            myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(richTextBoxProcessName.Text);
            myProcess.Start();
            //myProcess = new System.Diagnostics.Process(richTextBoxProcessName.Text);
            process_list.Add(myProcess);

        }
        void AllignText()
        {
            richTextBoxProcessName.SelectAll();
            richTextBoxProcessName.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            InitProcess();
            //myProcess.Start();
            Info();
            //this.TopMost = true;
            //----------------------------------------------------------------------------------------//
            
            //myProcess = Process.Start($"{richTextBoxProcessName.Text}");
            //processStack.Push(myProcess); // Добавляем процесс в стек
            //Info();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            //if (processStack.Count > 0)
            //{
            //    // Завершаем последний процесс
            //    Process lastProcess = processStack.Pop();
            //    if (!lastProcess.HasExited)// если есть процесс
            //    {
            //        lastProcess.Kill();
            //        lastProcess.WaitForExit();
            //    }

            //    // Переводим на передний план предпоследний процесс если таковой есть
            //    if (processStack.Count > 0)
            //    {
            //        Process previousProcess = processStack.Peek(); // Взглянуть на верхушку стека без удаления
            //        if (previousProcess != null && !previousProcess.HasExited)
            //        {
            //            IntPtr handle = previousProcess.MainWindowHandle;
            //            if (handle != IntPtr.Zero)//хранит адрес
            //            {
            //                // Переводим окно процесса на передний план
            //                SetForegroundWindow(handle);
            //            }
            //        }
            //    }
            //}
            //----------------------------------------------------------------------------------------//
            if (process_list.Count > 0)
            {
                myProcess = process_list.Last();
                myProcess.CloseMainWindow();//Закрывает процесс
                myProcess.Close(); // Освобождает ресурсы занимаемые процессом
                process_list.RemoveAt(process_list.Count - 1);
                Info(); 
            }
        }
        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             
            while(process_list.Count > 0)
            {
                process_list.First().CloseMainWindow();
                process_list.First().Close();
                process_list.RemoveAt(0);
            }
        }
        void Info()
        {
            //myProcess = process_list.First();

            if (process_list.Count > 0)
            {
                myProcess = process_list.First();
                labelProcessInfo.Text = $"Total process count: {process_list.Count}\n";
                labelProcessInfo.Text += $"Corent process:\n";
                
                labelProcessInfo.Text += $"PID:                      {myProcess.Id}\n";
                labelProcessInfo.Text += $"BasePriority:             {myProcess.BasePriority}\n";
                labelProcessInfo.Text += $"PriorityClass:            {myProcess.PriorityClass}\n";
                labelProcessInfo.Text += $"StartTime:                {myProcess.StartTime}\n";
                labelProcessInfo.Text += $"UserProcessorTime:        {myProcess.UserProcessorTime}\n";
                labelProcessInfo.Text += $"TotalProcessorTime:       {myProcess.TotalProcessorTime}\n";
                labelProcessInfo.Text += $"SessionId:                {myProcess.SessionId}\n";
                labelProcessInfo.Text += $"ProcessName:              {myProcess.ProcessName}\n";
                labelProcessInfo.Text += $"ProcessorAffinity:        {myProcess.ProcessorAffinity}\n";
                labelProcessInfo.Text += $"Threads:                  {myProcess.Threads.Count}\n"; 
            }
            else
            {
                labelProcessInfo.Text = "Нет запущенных процессов";
            }
        }
    }
}
