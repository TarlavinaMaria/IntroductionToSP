using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Processes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBoxProcessName.Text = "notepad";
            InitProcess();
        }
        void Form1_Closing(object sender, CancelEventArgs e)
        {
            myProcess.CloseMainWindow();
            myProcess.Close();
        }
        void InitProcess()
        {
            AllignText();
            myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(richTextBoxProcessName.Text);

        }
        void AllignText()
        {
            richTextBoxProcessName.SelectAll();
            richTextBoxProcessName.SelectionAlignment = HorizontalAlignment.Center;

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            InitProcess();
            myProcess.Start();
            Info();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            myProcess.CloseMainWindow();//Закрывает процесс
            myProcess.Close(); // Освобождает ресурсы занимаемые процессом
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myProcess.CloseMainWindow();//Закрывает процесс
            myProcess.Close(); // Освобождает ресурсы занимаемые процессом
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
