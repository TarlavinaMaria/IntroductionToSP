﻿namespace Task_Manager
{
    partial class MainFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.listViewProcess = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCloseAll = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.chartCPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(15, 14);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(88, 25);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "Обновить";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(110, 14);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(170, 25);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Завершить процесс";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listViewProcess
            // 
            this.listViewProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewProcess.FullRowSelect = true;
            this.listViewProcess.GridLines = true;
            this.listViewProcess.HideSelection = false;
            this.listViewProcess.Location = new System.Drawing.Point(13, 64);
            this.listViewProcess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listViewProcess.MultiSelect = false;
            this.listViewProcess.Name = "listViewProcess";
            this.listViewProcess.Size = new System.Drawing.Size(544, 441);
            this.listViewProcess.TabIndex = 2;
            this.listViewProcess.UseCompatibleStateImageBehavior = false;
            this.listViewProcess.View = System.Windows.Forms.View.Details;
            this.listViewProcess.SelectedIndexChanged += new System.EventHandler(this.listViewProcess_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Название процесса";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Используемая память";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "PID";
            this.columnHeader3.Width = 100;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(96, 40);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(244, 22);
            this.textBoxSearch.TabIndex = 3;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(24, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "Поиск";
            // 
            // buttonCloseAll
            // 
            this.buttonCloseAll.Location = new System.Drawing.Point(288, 14);
            this.buttonCloseAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCloseAll.Name = "buttonCloseAll";
            this.buttonCloseAll.Size = new System.Drawing.Size(170, 25);
            this.buttonCloseAll.TabIndex = 5;
            this.buttonCloseAll.Text = "Завершить дерево процессов";
            this.buttonCloseAll.UseVisualStyleBackColor = true;
            this.buttonCloseAll.Click += new System.EventHandler(this.buttonCloseAll_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(466, 13);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(91, 26);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // chartCPU
            // 
            chartArea1.Name = "ChartArea1";
            this.chartCPU.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCPU.Legends.Add(legend1);
            this.chartCPU.Location = new System.Drawing.Point(583, 13);
            this.chartCPU.Name = "chartCPU";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartCPU.Series.Add(series1);
            this.chartCPU.Size = new System.Drawing.Size(516, 197);
            this.chartCPU.TabIndex = 7;
            this.chartCPU.Text = "CPU";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick_1);
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 517);
            this.Controls.Add(this.chartCPU);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonCloseAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.listViewProcess);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRefresh);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainFrom";
            this.Text = "Task Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ListView listViewProcess;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCloseAll;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCPU;
        private System.Windows.Forms.Timer timer;
    }
}

