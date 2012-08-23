namespace pushtotalk_gui
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CaptureDevice_cBox = new System.Windows.Forms.ComboBox();
            this.CaptureDevice_lbl = new System.Windows.Forms.Label();
            this.MyNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HotKey_lbl = new System.Windows.Forms.Label();
            this.HotKey_btn = new System.Windows.Forms.Button();
            this.Mute_tmr = new System.Windows.Forms.Timer(this.components);
            this.About_btn = new System.Windows.Forms.Button();
            this.pttButton = new System.Windows.Forms.RadioButton();
            this.toggleButton = new System.Windows.Forms.RadioButton();
            this.DeviceState_lbl = new System.Windows.Forms.Label();
            this.DeviceStateValue_lbl = new System.Windows.Forms.Label();
            this.NotifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // CaptureDevice_cBox
            // 
            this.CaptureDevice_cBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureDevice_cBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CaptureDevice_cBox.FormattingEnabled = true;
            this.CaptureDevice_cBox.Location = new System.Drawing.Point(90, 1);
            this.CaptureDevice_cBox.Name = "CaptureDevice_cBox";
            this.CaptureDevice_cBox.Size = new System.Drawing.Size(284, 21);
            this.CaptureDevice_cBox.TabIndex = 0;
            this.CaptureDevice_cBox.SelectedIndexChanged += new System.EventHandler(this.CaptureDevice_cBox_SelectedIndexChanged);
            // 
            // CaptureDevice_lbl
            // 
            this.CaptureDevice_lbl.AutoSize = true;
            this.CaptureDevice_lbl.Location = new System.Drawing.Point(2, 4);
            this.CaptureDevice_lbl.Name = "CaptureDevice_lbl";
            this.CaptureDevice_lbl.Size = new System.Drawing.Size(82, 13);
            this.CaptureDevice_lbl.TabIndex = 1;
            this.CaptureDevice_lbl.Text = "Capture device:";
            // 
            // MyNotifyIcon
            // 
            this.MyNotifyIcon.BalloonTipText = "The mic is muted";
            this.MyNotifyIcon.BalloonTipTitle = "HPTT";
            this.MyNotifyIcon.ContextMenuStrip = this.NotifyIconContextMenu;
            this.MyNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("MyNotifyIcon.Icon")));
            this.MyNotifyIcon.Text = "Hardings Global Push-To-Talk";
            this.MyNotifyIcon.Visible = true;
            this.MyNotifyIcon.DoubleClick += new System.EventHandler(this.MyNotifyIcon_DoubleClick);
            // 
            // NotifyIconContextMenu
            // 
            this.NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.NotifyIconContextMenu.Name = "NotifyIconContextMenu";
            this.NotifyIconContextMenu.Size = new System.Drawing.Size(126, 76);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.showToolStripMenuItem.Text = "Show/Hide";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // HotKey_lbl
            // 
            this.HotKey_lbl.AutoSize = true;
            this.HotKey_lbl.Location = new System.Drawing.Point(40, 34);
            this.HotKey_lbl.Name = "HotKey_lbl";
            this.HotKey_lbl.Size = new System.Drawing.Size(44, 13);
            this.HotKey_lbl.TabIndex = 2;
            this.HotKey_lbl.Text = "Hotkey:";
            // 
            // HotKey_btn
            // 
            this.HotKey_btn.AutoSize = true;
            this.HotKey_btn.Location = new System.Drawing.Point(90, 28);
            this.HotKey_btn.Name = "HotKey_btn";
            this.HotKey_btn.Size = new System.Drawing.Size(81, 25);
            this.HotKey_btn.TabIndex = 3;
            this.HotKey_btn.Text = "<none>";
            this.HotKey_btn.UseVisualStyleBackColor = true;
            this.HotKey_btn.Click += new System.EventHandler(this.HotKey_btn_Click);
            // 
            // Mute_tmr
            // 
            this.Mute_tmr.Interval = 500;
            this.Mute_tmr.Tick += new System.EventHandler(this.Mute_tmr_Tick);
            // 
            // About_btn
            // 
            this.About_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.About_btn.Location = new System.Drawing.Point(285, 57);
            this.About_btn.Name = "About_btn";
            this.About_btn.Size = new System.Drawing.Size(89, 22);
            this.About_btn.TabIndex = 4;
            this.About_btn.Text = "About";
            this.About_btn.UseVisualStyleBackColor = true;
            this.About_btn.Click += new System.EventHandler(this.About_btn_Click);
            // 
            // pttButton
            // 
            this.pttButton.AutoSize = true;
            this.pttButton.Checked = true;
            this.pttButton.Location = new System.Drawing.Point(209, 34);
            this.pttButton.Name = "pttButton";
            this.pttButton.Size = new System.Drawing.Size(81, 17);
            this.pttButton.TabIndex = 5;
            this.pttButton.TabStop = true;
            this.pttButton.Text = "Push to talk";
            this.pttButton.UseVisualStyleBackColor = true;
            // 
            // toggleButton
            // 
            this.toggleButton.AutoSize = true;
            this.toggleButton.Location = new System.Drawing.Point(300, 34);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(58, 17);
            this.toggleButton.TabIndex = 6;
            this.toggleButton.Text = "Toggle";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.CheckedChanged += new System.EventHandler(this.pttButtonCheckedChanged);
            // 
            // DeviceState_lbl
            // 
            this.DeviceState_lbl.AutoSize = true;
            this.DeviceState_lbl.Location = new System.Drawing.Point(14, 62);
            this.DeviceState_lbl.Name = "DeviceState_lbl";
            this.DeviceState_lbl.Size = new System.Drawing.Size(70, 13);
            this.DeviceState_lbl.TabIndex = 7;
            this.DeviceState_lbl.Text = "Device state:";
            // 
            // DeviceStateValue_lbl
            // 
            this.DeviceStateValue_lbl.AutoSize = true;
            this.DeviceStateValue_lbl.Location = new System.Drawing.Point(90, 62);
            this.DeviceStateValue_lbl.Name = "DeviceStateValue_lbl";
            this.DeviceStateValue_lbl.Size = new System.Drawing.Size(0, 13);
            this.DeviceStateValue_lbl.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 80);
            this.Controls.Add(this.DeviceStateValue_lbl);
            this.Controls.Add(this.DeviceState_lbl);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.pttButton);
            this.Controls.Add(this.About_btn);
            this.Controls.Add(this.CaptureDevice_lbl);
            this.Controls.Add(this.HotKey_btn);
            this.Controls.Add(this.HotKey_lbl);
            this.Controls.Add(this.CaptureDevice_cBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Hardings Global Push-To-Talk";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.NotifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CaptureDevice_cBox;
        private System.Windows.Forms.Label CaptureDevice_lbl;
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip NotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label HotKey_lbl;
        private System.Windows.Forms.Button HotKey_btn;
        private System.Windows.Forms.Timer Mute_tmr;
        private System.Windows.Forms.Button About_btn;
        private System.Windows.Forms.RadioButton pttButton;
        private System.Windows.Forms.RadioButton toggleButton;
        private System.Windows.Forms.Label DeviceState_lbl;
        private System.Windows.Forms.Label DeviceStateValue_lbl;
    }
}

