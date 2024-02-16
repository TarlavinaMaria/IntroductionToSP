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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            lvProcesses.Columns.Add("PID");
            lvProcesses.Columns.Add("Name");
            
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
            //process_list.Add(myProcess);
            lvProcesses.Items.Add(myProcess.Id.ToString());
            lvProcesses.Items[lvProcesses.Items.Count - 1].SubItems.Add(myProcess.ProcessName);
        }
        void AllignText()
        {
            richTextBoxProcessName.SelectAll();
            richTextBoxProcessName.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            InitProcess();
            ////myProcess.Start();
            //Info();
            ////this.TopMost = true;
            //lvProcesses.Items.Add(myProcess);

            //----------------------------------------------------------------------------------------//

            //myProcess = Process.Start($"{richTextBoxProcessName.Text}");
            //processStack.Push(myProcess); // Добавляем процесс в стек
            //Info();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            #region MyCode
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
            #endregion
            //----------------------------------------------------------------------------------------//
            //if (process_list.Count > 0)
            //{
            if (lvProcesses.SelectedItems.Count == 1) DeliteToIndex();
            else
            {
                if (lvProcesses.Items.Count > 0)
                {
                    try
                    {
                        //myProcess = process_list.Last();
                        myProcess = Process.GetProcessById(Convert.ToInt32(lvProcesses.Items[lvProcesses.Items.Count - 1].Text));
                        myProcess.CloseMainWindow();//Закрывает процесс
                        myProcess.Close(); // Освобождает ресурсы занимаемые процессом
                                           //process_list.RemoveAt(process_list.Count - 1);
                                           //lvProcesses.Items[lvProcesses.Items.Count - 1];
                                           //lvProcesses.Items.RemoveByKey(myProcess.Id.ToString());
                        lvProcesses.Items.RemoveAt(lvProcesses.Items.Count - 1);
                        //Info();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //process_list.RemoveAt(process_list.Count - 1);
                        //lvProcesses.Items.RemoveByKey(myProcess.Id.ToString());
                        lvProcesses.Items.RemoveAt(lvProcesses.Items.Count - 1);
                    }
                    //}
                }

            }

        }
        //[System.Runtime.InteropServices.DllImport("USER32.DLL")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (process_list.Count > 0)
            {
                try
                {
                    process_list.First().CloseMainWindow();
                    process_list.First().Close();
                    process_list.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    process_list.RemoveAt(0);
                }
            }
        }
        void Info()
        {
            //myProcess = process_list.First();

            if (process_list.Count > 0)
            {
                myProcess = process_list.First();
                labelProcessInfo.Text = $"Total process count:       {process_list.Count}\n";
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

        private void lvProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvProcesses.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvProcesses.SelectedItems[0];
                int processId = Convert.ToInt32(selectedItem.Text);// Преобразование ID процесса из текста.
                Process process = Process.GetProcessById(processId); // получение id процесса
                string info = $"PID:                {process.Id}\n" +
                        $"BasePriority:             {process.BasePriority}\n" +
                        $"PriorityClass:            {process.PriorityClass}\n" +
                        $"StartTime:                {process.StartTime}\n" +
                        $"UserProcessorTime:        {process.UserProcessorTime}\n" +
                        $"TotalProcessorTime:       {process.TotalProcessorTime}\n" +
                        $"SessionId:                {process.SessionId}\n" +
                        $"ProcessName:              {process.ProcessName}\n" +
                        $"ProcessorAffinity:        {process.ProcessorAffinity}\n" +
                        $"Threads:                  {process.Threads.Count}\n" +
                        $"CPU:                      {process.TotalProcessorTime.TotalSeconds / Environment.ProcessorCount}\n" +
                        $"Memory:                   {process.WorkingSet64 / (1024 * 1024)}\n";

                labelProcessInfo.Text = info;
            }
            else
            {
                labelProcessInfo.Text = "Выберите процесс";
            }
        }

        private void DeliteToIndex()
        {
            if (lvProcesses.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvProcesses.SelectedItems[0];
                int processId = Convert.ToInt32(selectedItem.Text);// Преобразование ID процесса из текста.
                try
                {
                    Process process = Process.GetProcessById(processId);// получение id процесса
                    process.Kill(); // Закрытие процесса.
                    process.WaitForExit(); // Ожидание полного завершения процесса.

                    lvProcesses.Items.Remove(selectedItem); // Удаление элемента из ListView.
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось закрыть процесс: {ex.Message}");
                }
            }
        }
    }
}
