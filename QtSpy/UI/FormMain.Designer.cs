namespace QtSpy.UI
{
    partial class FormMain
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
            if (disposing && (components != null)) {
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
            this.label1 = new System.Windows.Forms.Label();
            this.checkOnlyWidgets = new System.Windows.Forms.CheckBox();
            this.checkMinimize = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeWidgets = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dataGridParams = new System.Windows.Forms.DataGridView();
            this.ColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picturePicker = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDump = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExecJs = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePicker)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Drag the Finder Tool over a window\r\nto select it, then release the mouse";
            // 
            // checkOnlyWidgets
            // 
            this.checkOnlyWidgets.AutoSize = true;
            this.checkOnlyWidgets.Checked = true;
            this.checkOnlyWidgets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnlyWidgets.Location = new System.Drawing.Point(271, 10);
            this.checkOnlyWidgets.Name = "checkOnlyWidgets";
            this.checkOnlyWidgets.Size = new System.Drawing.Size(89, 17);
            this.checkOnlyWidgets.TabIndex = 4;
            this.checkOnlyWidgets.Text = "Only Widgets";
            this.checkOnlyWidgets.UseVisualStyleBackColor = true;
            // 
            // checkMinimize
            // 
            this.checkMinimize.AutoSize = true;
            this.checkMinimize.Checked = true;
            this.checkMinimize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMinimize.Location = new System.Drawing.Point(271, 27);
            this.checkMinimize.Name = "checkMinimize";
            this.checkMinimize.Size = new System.Drawing.Size(98, 17);
            this.checkMinimize.TabIndex = 5;
            this.checkMinimize.Text = "Minimize QtSpy";
            this.checkMinimize.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.treeWidgets);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.dataGridParams);
            this.panel1.Location = new System.Drawing.Point(12, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 461);
            this.panel1.TabIndex = 6;
            // 
            // treeWidgets
            // 
            this.treeWidgets.BackColor = System.Drawing.SystemColors.Window;
            this.treeWidgets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeWidgets.DefaultToolTipProvider = null;
            this.treeWidgets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeWidgets.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeWidgets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.treeWidgets.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeWidgets.Location = new System.Drawing.Point(0, 0);
            this.treeWidgets.Model = null;
            this.treeWidgets.Name = "treeWidgets";
            this.treeWidgets.NodeControls.Add(this.nodeIcon1);
            this.treeWidgets.NodeControls.Add(this.nodeTextBox1);
            this.treeWidgets.SelectedNode = null;
            this.treeWidgets.Size = new System.Drawing.Size(414, 278);
            this.treeWidgets.TabIndex = 7;
            this.treeWidgets.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.treeWidgets_NodeMouseClick);
            this.treeWidgets.SelectionChanged += new System.EventHandler(this.treeWidgets_SelectionChanged);
            this.treeWidgets.DrawControl += new System.EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(this.treeWidgets_DrawControl);
            this.treeWidgets.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeWidgets_KeyDown);
            this.treeWidgets.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeWidgets_MouseDoubleClick);
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Icon";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = null;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Description";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            this.nodeTextBox1.UseCompatibleTextRendering = true;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 278);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(414, 3);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // dataGridParams
            // 
            this.dataGridParams.AllowUserToAddRows = false;
            this.dataGridParams.AllowUserToDeleteRows = false;
            this.dataGridParams.AllowUserToResizeRows = false;
            this.dataGridParams.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridParams.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnKey,
            this.ColumnValue});
            this.dataGridParams.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridParams.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridParams.Location = new System.Drawing.Point(0, 281);
            this.dataGridParams.MultiSelect = false;
            this.dataGridParams.Name = "dataGridParams";
            this.dataGridParams.RowHeadersVisible = false;
            this.dataGridParams.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridParams.Size = new System.Drawing.Size(414, 180);
            this.dataGridParams.TabIndex = 5;
            this.dataGridParams.DoubleClick += new System.EventHandler(this.dataGridParams_DoubleClick);
            // 
            // ColumnKey
            // 
            this.ColumnKey.HeaderText = "Key";
            this.ColumnKey.Name = "ColumnKey";
            this.ColumnKey.ReadOnly = true;
            // 
            // ColumnValue
            // 
            this.ColumnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // picturePicker
            // 
            this.picturePicker.Image = global::QtSpy.Properties.Resources.bitmap1;
            this.picturePicker.Location = new System.Drawing.Point(13, 13);
            this.picturePicker.Name = "picturePicker";
            this.picturePicker.Size = new System.Drawing.Size(31, 28);
            this.picturePicker.TabIndex = 0;
            this.picturePicker.TabStop = false;
            this.picturePicker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePicker_MouseDown);
            this.picturePicker.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePicker_MouseMove);
            this.picturePicker.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturePicker_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemVisible,
            this.menuItemEnabled,
            this.toolStripMenuItem1,
            this.menuItemDump,
            this.menuItemExecJs});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(179, 120);
            // 
            // menuItemVisible
            // 
            this.menuItemVisible.Name = "menuItemVisible";
            this.menuItemVisible.Size = new System.Drawing.Size(178, 22);
            this.menuItemVisible.Text = "Visible";
            this.menuItemVisible.Click += new System.EventHandler(this.menuItemVisible_Click);
            // 
            // menuItemEnabled
            // 
            this.menuItemEnabled.Name = "menuItemEnabled";
            this.menuItemEnabled.Size = new System.Drawing.Size(178, 22);
            this.menuItemEnabled.Text = "Enabled";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(175, 6);
            // 
            // menuItemDump
            // 
            this.menuItemDump.Name = "menuItemDump";
            this.menuItemDump.Size = new System.Drawing.Size(178, 22);
            this.menuItemDump.Text = "Dump Content";
            this.menuItemDump.Click += new System.EventHandler(this.menuItemDump_Click);
            // 
            // menuItemExecJs
            // 
            this.menuItemExecJs.Name = "menuItemExecJs";
            this.menuItemExecJs.Size = new System.Drawing.Size(178, 22);
            this.menuItemExecJs.Text = "Execute JavaScript...";
            this.menuItemExecJs.Visible = false;
            this.menuItemExecJs.Click += new System.EventHandler(this.executeJavaScriptToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 520);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkMinimize);
            this.Controls.Add(this.checkOnlyWidgets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picturePicker);
            this.MinimumSize = new System.Drawing.Size(398, 38);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePicker)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picturePicker;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkOnlyWidgets;
        private System.Windows.Forms.CheckBox checkMinimize;
        private System.Windows.Forms.Panel panel1;
        private Aga.Controls.Tree.TreeViewAdv treeWidgets;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dataGridParams;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemVisible;
        private System.Windows.Forms.ToolStripMenuItem menuItemEnabled;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItemDump;
        private System.Windows.Forms.ToolStripMenuItem menuItemExecJs;
    }
}

