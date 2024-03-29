﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

using System.Windows.Forms.DataVisualization.Charting; // для графиков
namespace Task_Manager
{
    public partial class MainFrom : Form
    {
        private List<Process> processes = null; //список процессов
        private PerformanceCounter cpuCounter;//для графиков
        public MainFrom()
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
            listViewProcess.Items.Clear();// очищает текущий список элементов в элементе управления listView1.

            double memSize = 0; // переменная памяти
            foreach (Process p in processes)//  foreach для перебора всех процессов в коллекции processes.
            {
                memSize = 0;//сбрасывается в 0 для каждого нового процесса.

                // Создается объект PerformanceCounter pc для измерения используемой памяти каждого процесса. Он настраивается следующим образом:
                PerformanceCounter pc = new PerformanceCounter();

                // CategoryName устанавливается в "Process", что указывает на категорию счетчиков производительности для процессов
                pc.CategoryName = "Process";

                //CounterName устанавливается в "Working Set - Private", что представляет собой количество памяти, выделенное для частного использования процессом
                pc.CounterName = "Working Set - Private";

                // InstanceName устанавливается равным имени процесса, взятому из коллекции processes
                pc.InstanceName = p.ProcessName;

                //memSize обновляется значением, полученным от pc.NextValue(), которое затем конвертируется в мегабайты (делением на 1000*1000).
                memSize = (double)pc.NextValue() / (1000 * 1000); // в мегобайтах

                //Создается массив строк row с 3 элементами: именем процесса и округленным до одного десятичного знака значением его занимаемой памяти, PID процесса.
                string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString(), p.Id.ToString() };

                //Создается новый ListViewItem, инициализированный массивом row, и добавляется в список listView1.
                listViewProcess.Items.Add(new ListViewItem(row)); // добавление в List

                // После использования счетчика pc, выполняется его закрытие и освобождение ресурсов с помощью методов Close() и Dispose().
                pc.Close();
                pc.Dispose();
            }
            // в шапке обновляет заголовок окна
            Text = "Запущено процессов: " + processes.Count.ToString(); //Значение processes.Count.ToString() преобразует количество процессов в строку.

        }

        private void RefreshProcessesList(List<Process> processes, string keyword)
        {
            //перегрузка RefreshProcessesList для поиска, в котором List<Process> processes - список новый, string keyword - процесс
            try
            {
                listViewProcess.Items.Clear();

                double memSize = 0;
                foreach (Process p in processes)
                {
                    if (p != null)
                    {
                        memSize = 0;

                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = p.ProcessName;

                        memSize = (double)pc.NextValue() / (1000 * 1000);

                        string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString(), p.Id.ToString() };

                        listViewProcess.Items.Add(new ListViewItem(row));

                        pc.Close();
                        pc.Dispose();
                    }
                }
                Text = $"Запущено процессов: '{keyword}'" + processes.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void KillProcess(Process process)//Закрытие 1 процесса
        {
            process.Kill();
            //Это гарантирует, что процесс был полностью завершен перед тем, как продолжить.
            process.WaitForExit();
        }
        private void KillProcessAndChildren(int pid)//предназначен для завершения процесса и всех его дочерних процессов.
        {
            //pid - идентификатор процесса (PID) основного процесса
            if (pid == 0)
            {
                //проверяется, если переданный PID равен нулю (pid == 0), в таком случае метод просто возвращается
                return;
            }
            //создается объект ManagementObjectSearcher, который использует WMI (Windows Management Instrumentation) для получения списка всех процессов,
            //у которых PID родительского процесса равен предоставленному PID (ищет все процессы, которые являются дочерними для процесса с указанным PID)
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID =" + pid);

            // searcher.Get() выполняет запрос и возвращает коллекцию объектов ManagementObjectCollection, представляющих процессы, удовлетворяющие критериям поиска
            ManagementObjectCollection objectsCollection = searcher.Get();
            //перебираем каждый объект в этой коллекции - каждый объект дочерний процесс
            foreach (ManagementObject obj in objectsCollection)
            {
                //вызываем KillProcessAndChildren для каждого идентификатора дочернего процесса (ProcessID), завершая все дочерние процессы перед тем, как убить сам родительский процесс.
                KillProcessAndChildren(Convert.ToInt32(obj["ProcessID"]));
            }
            try
            {
                //После обработки дочерних процессов, получаем экземпляр основного процесса по его PID с помощью Process.GetProcessById.
                Process p = Process.GetProcessById(pid);

                p.Kill();
                //Это гарантирует, что процесс был полностью завершен перед тем, как продолжить.
                p.WaitForExit();
            }
            //исключение ArgumentException. Это исключение будет брошено, если PID не будет соответствовать запущенному процессу (например, если процесс уже завершился).
            //В случае перехвата исключения, метод просто проигнорирует ошибку и продолжит свою работу. 
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private int GetParentProcessId(Process processParent)
        {
            int parentID = 0;//идентификатор родительского процесса
            #region Info
            /*
            Windows Management Instrumentation(WMI) для запроса информации о процессе.WMI — это технология и инфраструктура Microsoft, которая позволяет выполнять запросы к системной информации ОС Windows,
            такой как информация о процессах.
            */
            #endregion
            try
            {
                //Для получения свойств конкретного процесса создается объект ManagementObject,
                //запрос WMI для получения объекта процесса win32_process с идентификатором, равным ID процесса processParent.
                ManagementObject managementObject = new ManagementObject("win32_process.handle='" + processParent.Id + "'");
                //вызываем get для объекта для заполнения данными
                managementObject.Get();

                parentID = Convert.ToInt32(managementObject["ParentProcessId"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return parentID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //инициализация формы
            processes = new List<Process>();
            //заполнение список с процессами
            GetProcesses();
            //заполнение ListView
            RefreshProcessesList();
            //общий график
            ChartAll();
            // Создание и запуск таймера
            Timer timer = new Timer();
            timer.Interval += 1500; // интервал обновления в миллисекундах
            timer.Tick += timer_Tick_1;
            timer.Start();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            GetProcesses();

            RefreshProcessesList();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewProcess.SelectedItems.Count == 1) // выбранный элемент 
                {
                    #region Info
                    /*
                    where - получим процесс, сравниваем имя процесса с текстом в крайней колонке
                    используем LINQ, чтобы найти процесс в предварительно загруженном списке процессов processes.
                    ищем процесс, чье имя(ProcessName) совпадает с текстом в первом подэлементе(SubItems[0].Text) выбранного элемента в listView1. 
                     LINQ - запрос конвертируется в список и берет первый элемент(ToList()[0])
                    */
                    #endregion
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                     listViewProcess.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcess(processToKill);
                    GetProcesses();
                    RefreshProcessesList();
                }
                else
                {
                    MessageBox.Show(this, "Выберите процесс");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCloseAll_Click(object sender, EventArgs e)//завершение дерева процессов
        {
            //??????? Проблема с одним процессом ?????????????
            try
            {
                if (listViewProcess.SelectedItems.Count == 1)
                {

                    Process processToKill = processes.Where((x) => x.ProcessName ==
                     listViewProcess.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcessAndChildren(GetParentProcessId(processToKill));
                    GetProcesses();
                    RefreshProcessesList();
                }
                else
                {
                    MessageBox.Show(this, "Выберите процесс");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            #region Info
            /*
            Microsoft.VisualBasic - нужен для того чтобы не создавать еще одну форму, в просто вывести диалоговое окно
            строка из класса Interaction который находится в Microsoft.VisualBasic которая создает диалоговое окно,
            InputBox(сообщение внутри окна, заголовок)

            Чтобы использовать метод Interaction.InputBox в приложении C#, обычно требуется добавить ссылку на сборку Microsoft.VisualBasic, поскольку она не является частью основных библиотек C#.
            Важно также отметить, что метод InputBox является синхронным методом и приостанавливает выполнение приложения до тех пор, пока пользователь не введет данные или не закроет диалоговое окно.
            Как только пользователь вводит текст и нажимает "ОК" в диалоговом окне, эта строка ввода присваивается переменной path.
            Если пользователь отменит операцию, переменная path по умолчанию будет пустой строкой.
            */
            #endregion
            string path = Interaction.InputBox("Введите имя программы", "Запуск новой задачи");
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show(this, "Введите имя процесса");
                }
                else
                {
                    //запуск процесса
                    Process.Start(path);
                    buttonRefresh_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this, ex.Message, "Что-то пошло не по плану", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            GetProcesses();
            //новый список
            #region Info
            /*
            Where(): Это метод расширения LINQ(Language-Integrated Query), используемый для фильтрации коллекции. 
            Метод принимает функцию - предикат, которая возвращает логическое значение.
            -(x) =>: Это лямбда-выражение, которое служит предикатом для метода Where.
            Для каждого процесса x в processes выполняется эта лямбда - функция, чтобы определить, следует ли включать процесс в результат.
            */
            #endregion
            List<Process> filteredprocesses = processes.Where((x) =>
            x.ProcessName.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList<Process>();
            //вызов обновления списка после поиска
            RefreshProcessesList(filteredprocesses, textBoxSearch.Text);
        }
        private void ChartAll()
        {
            // Инициализация счетчика процессора
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // Настройка графика
            chartCPU.Series.Clear();
            chartCPU.Series.Add("CPU Usage");
            chartCPU.Series["CPU Usage"].ChartType = SeriesChartType.Line;
            chartCPU.Series["CPU Usage"].BorderWidth = 2;
            chartCPU.Series["CPU Usage"].Color = Color.Blue;
            chartCPU.ChartAreas[0].AxisY.Maximum = 100;
            chartCPU.ChartAreas[0].AxisY.Minimum = 0;

            //// Создание и запуск таймера
            //Timer timer = new Timer();
            //timer.Interval += 1500; // интервал обновления в миллисекундах
            //timer.Tick += timer_Tick_1;
            //timer.Start();
        }
        private void timer_Tick_1(object sender, EventArgs e)
        {
            if (listViewProcess.SelectedItems.Count == 0)
            {
                
                // Получение текущей загрузки процессора
                float cpuUsage = cpuCounter.NextValue();

                // Добавление значения в график
                chartCPU.Series["CPU Usage"].Points.AddY(cpuUsage);

            }
            else
            {
                
                // Получение текущей загрузки процессора для заданного процесса
                float processCpuUsage = cpuCounter.NextValue();

                // Добавление значения в график
                chartCPU.Series["Process CPU Usage"].Points.AddY(processCpuUsage);
            }


        }

        private void listViewProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProcess.SelectedItems.Count == 1)
            {
                // Инициализация счетчика процессора для определенного процесса
                Process process = Process.GetProcessesByName(listViewProcess.SelectedItems[0].Text)[0];
                cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);

                // Настройка графика
                chartCPU.Series.Clear();
                chartCPU.Series.Add("Process CPU Usage");
                chartCPU.Series["Process CPU Usage"].ChartType = SeriesChartType.Line;
                chartCPU.Series["Process CPU Usage"].BorderWidth = 2;
                chartCPU.Series["Process CPU Usage"].Color = Color.Red;
                chartCPU.ChartAreas[0].AxisY.Maximum = 100;
                chartCPU.ChartAreas[0].AxisY.Minimum = 0;

                // Создание и запуск таймера
                //Timer timer = new Timer();
                //timer.Interval += 1500; // интервал обновления в миллисекундах
                //timer.Tick += timer_Tick_1;
                //timer.Start();
            }
            else
            {
                ChartAll();
            }

        }

    }
}
