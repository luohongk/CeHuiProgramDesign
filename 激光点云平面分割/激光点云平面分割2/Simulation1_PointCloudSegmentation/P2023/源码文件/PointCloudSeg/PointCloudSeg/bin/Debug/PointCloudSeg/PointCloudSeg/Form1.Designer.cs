namespace PointCloudSeg
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmImportData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveReport = new System.Windows.Forms.ToolStripMenuItem();
            this.计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCalCluster = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSeg = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbImportData = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveReport = new System.Windows.Forms.ToolStripButton();
            this.tsbCalCluster = new System.Windows.Forms.ToolStripButton();
            this.tsbSeg = new System.Windows.Forms.ToolStripButton();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsmViewReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmViewResult = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.计算ToolStripMenuItem,
            this.显示ToolStripMenuItem,
            this.tsmHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(937, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmImportData,
            this.tsmSaveReport});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // tsmImportData
            // 
            this.tsmImportData.Name = "tsmImportData";
            this.tsmImportData.Size = new System.Drawing.Size(180, 22);
            this.tsmImportData.Text = "导入文件";
            this.tsmImportData.Click += new System.EventHandler(this.tsmImportData_Click);
            // 
            // tsmSaveReport
            // 
            this.tsmSaveReport.Name = "tsmSaveReport";
            this.tsmSaveReport.Size = new System.Drawing.Size(180, 22);
            this.tsmSaveReport.Text = "保存报告";
            this.tsmSaveReport.Click += new System.EventHandler(this.tsmSaveReport_Click);
            // 
            // 计算ToolStripMenuItem
            // 
            this.计算ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCalCluster,
            this.tsmSeg});
            this.计算ToolStripMenuItem.Name = "计算ToolStripMenuItem";
            this.计算ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.计算ToolStripMenuItem.Text = "计算";
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmViewReport,
            this.tsmViewResult});
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.显示ToolStripMenuItem.Text = "显示";
            // 
            // tsmHelp
            // 
            this.tsmHelp.Name = "tsmHelp";
            this.tsmHelp.Size = new System.Drawing.Size(44, 21);
            this.tsmHelp.Text = "帮助";
            this.tsmHelp.Click += new System.EventHandler(this.tsmHelp_Click);
            // 
            // tsmCalCluster
            // 
            this.tsmCalCluster.Name = "tsmCalCluster";
            this.tsmCalCluster.Size = new System.Drawing.Size(180, 22);
            this.tsmCalCluster.Text = "栅格化计算";
            this.tsmCalCluster.Click += new System.EventHandler(this.tsmCalCluster_Click);
            // 
            // tsmSeg
            // 
            this.tsmSeg.Name = "tsmSeg";
            this.tsmSeg.Size = new System.Drawing.Size(180, 22);
            this.tsmSeg.Text = "平面分割";
            this.tsmSeg.Click += new System.EventHandler(this.tsmSeg_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbImportData,
            this.tsbSaveReport,
            this.tsbCalCluster,
            this.tsbSeg,
            this.tsbHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(937, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbImportData
            // 
            this.tsbImportData.Image = ((System.Drawing.Image)(resources.GetObject("tsbImportData.Image")));
            this.tsbImportData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportData.Name = "tsbImportData";
            this.tsbImportData.Size = new System.Drawing.Size(52, 22);
            this.tsbImportData.Text = "导入";
            this.tsbImportData.Click += new System.EventHandler(this.tsbImportData_Click);
            // 
            // tsbSaveReport
            // 
            this.tsbSaveReport.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveReport.Image")));
            this.tsbSaveReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveReport.Name = "tsbSaveReport";
            this.tsbSaveReport.Size = new System.Drawing.Size(52, 22);
            this.tsbSaveReport.Text = "保存";
            this.tsbSaveReport.Click += new System.EventHandler(this.tsbSaveReport_Click);
            // 
            // tsbCalCluster
            // 
            this.tsbCalCluster.Image = ((System.Drawing.Image)(resources.GetObject("tsbCalCluster.Image")));
            this.tsbCalCluster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCalCluster.Name = "tsbCalCluster";
            this.tsbCalCluster.Size = new System.Drawing.Size(64, 22);
            this.tsbCalCluster.Text = "栅格化";
            this.tsbCalCluster.Click += new System.EventHandler(this.tsbCalCluster_Click);
            // 
            // tsbSeg
            // 
            this.tsbSeg.Image = ((System.Drawing.Image)(resources.GetObject("tsbSeg.Image")));
            this.tsbSeg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSeg.Name = "tsbSeg";
            this.tsbSeg.Size = new System.Drawing.Size(52, 22);
            this.tsbSeg.Text = "分割";
            this.tsbSeg.Click += new System.EventHandler(this.tsbSeg_Click);
            // 
            // tsbHelp
            // 
            this.tsbHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbHelp.Image")));
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(52, 22);
            this.tsbHelp.Text = "帮助";
            this.tsbHelp.Click += new System.EventHandler(this.tsbHelp_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 54);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(937, 448);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(929, 422);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "报告";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(929, 419);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(-2, -4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(923, 410);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(917, 407);
            this.dataGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(937, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel1.Text = "就绪.";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(176, 17);
            this.toolStripStatusLabel2.Text = "                                          ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel3.Text = "测绘程序设计";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(172, 17);
            this.toolStripStatusLabel4.Text = "                                         ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(74, 17);
            this.toolStripStatusLabel5.Text = "2023-07-29";
            // 
            // tsmViewReport
            // 
            this.tsmViewReport.Name = "tsmViewReport";
            this.tsmViewReport.Size = new System.Drawing.Size(180, 22);
            this.tsmViewReport.Text = "计算报告";
            this.tsmViewReport.Click += new System.EventHandler(this.tsmViewReport_Click);
            // 
            // tsmViewResult
            // 
            this.tsmViewResult.Name = "tsmViewResult";
            this.tsmViewResult.Size = new System.Drawing.Size(180, 22);
            this.tsmViewResult.Text = "结果表格";
            this.tsmViewResult.Click += new System.EventHandler(this.tsmViewResult_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 527);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "激光点云平面分割";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmImportData;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveReport;
        private System.Windows.Forms.ToolStripMenuItem 计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmCalCluster;
        private System.Windows.Forms.ToolStripMenuItem tsmSeg;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmViewReport;
        private System.Windows.Forms.ToolStripMenuItem tsmViewResult;
        private System.Windows.Forms.ToolStripMenuItem tsmHelp;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbImportData;
        private System.Windows.Forms.ToolStripButton tsbSaveReport;
        private System.Windows.Forms.ToolStripButton tsbCalCluster;
        private System.Windows.Forms.ToolStripButton tsbSeg;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
    }
}

