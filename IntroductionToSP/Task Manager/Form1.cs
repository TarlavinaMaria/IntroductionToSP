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
using System.Management;
using Microsoft.VisualBasic;

namespace Task_Manager
{
    public partial class Form1 : Form
    {
        private List<Process> processes = null; //список процессов
        public Form1()
        {
            InitializeComponent();
        }
        private void GetProcesses()
        {
            processes.Clear();// очищаем список

            processes = Process.GetProcesses().ToList<Process>(); // заполнение списка системными процессами, в формате ToList<Process>()
        }
        private void RefreshProcessesList()
        {
            listView1.Items.Clear();// получение list

            double memSize = 0; // переменная памяти
            foreach (Process p in processes)
            {
                memSize = 0;

                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Process";
                pc.CounterName = "Working Set - Private";
                pc.InstanceName = p.ProcessName;

                memSize = (double)pc.NextValue() / (1000 * 1000); // в мегобайтах

                string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString() }; // массив сток в котором хранятся колонки 

                listView1.Items.Add(new ListViewItem(row)); // добавление в List

                pc.Close();
                pc.Dispose();
            }
            // в шапке формы процессы
            Text = "Запущено процессов: " + processes.Count.ToString();
        }
        private void RefreshProcessesList(List<Process> processes, string keyword)
        {
            try
            {
                listView1.Items.Clear();// получение list

                double memSize = 0; // переменная памяти
                foreach (Process p in processes)
                {
                    if (p != null)
                    {
                        memSize = 0;

                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = p.ProcessName;

                        memSize = (double)pc.NextValue() / (1000 * 1000); // в мегобайтах

                        string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString() }; // массив сток в котором хранятся колонки 

                        listView1.Items.Add(new ListViewItem(row)); // добавление в List

                        pc.Close();
                        pc.Dispose();
                    }
                }
                // в шапке формы процессы
                Text = $"Запущено процессов: '{keyword}'" + processes.Count.ToString();
            }
            catch (Exception) { }
        }

        private void KillProcess(Process process)
        {
            process.Kill();

            process.WaitForExit();
        }
        private void KillProcessAndChildren(int pid)//Завершение дочерних процессов
        {
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID =" + pid);

            ManagementObjectCollection objectsCollection = searcher.Get();
            foreach (ManagementObject obj in objectsCollection)
            {
                KillProcessAndChildren(Convert.ToInt32(obj["ProcessID"]));
            }
            try
            {
                Process p = Process.GetProcessById(pid);

                p.Kill();
                p.WaitForExit();
            }
            catch (ArgumentException) { }
        }
        private int GetParentProcessId(Process p)
        {
            int parentID = 0;
            try
            {
                ManagementObject managementObject = new ManagementObject("win32_process.handle='" + p.Id + "'");
                managementObject.Get();

                parentID = Convert.ToInt32(managementObject["ParentProcessId"]);
            }
            catch (Exception) { }
            return parentID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            processes = new List<Process>();
            //заполнение список с процессами
            GetProcesses();
            //заполнение ListView
            RefreshProcessesList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetProcesses();

            RefreshProcessesList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    //where -получим процесс, сравниваем имя процесса с текстом в крайней колонке
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                     listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcess(processToKill);
                    GetProcesses();
                    RefreshProcessesList();
                }
            }
            catch (Exception) { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    //where -получим процесс, сравниваем имя процесса с текстом в крайней колонке
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                     listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcessAndChildren(GetParentProcessId(processToKill));
                    GetProcesses();
                    RefreshProcessesList();
                }
            }
            catch (Exception) { }
        }

        private void завершитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    //where -получим процесс, сравниваем имя процесса с текстом в крайней колонке
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                     listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcessAndChildren(GetParentProcessId(processToKill));
                    GetProcesses();
                    RefreshProcessesList();
                }
            }
            catch (Exception) { }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string path = Interaction.InputBox("Введите имя программы", "Запуск новой задачи");
            try
            {
                Process.Start(path);
            }
            catch (Exception) { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetProcesses();

            List<Process> filteredprocesses = processes.Where((x) =>
            x.ProcessName.ToLower().Contains(textBox1.Text.ToLower())).ToList<Process>();

            RefreshProcessesList(filteredprocesses, textBox1.Text);
        }
    }
}
