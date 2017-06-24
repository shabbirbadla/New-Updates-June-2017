namespace Udyog.Application.License
{
    partial class frmServiceManager
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
            this.m_btnStartService = new System.Windows.Forms.Button();
            this.m_btnStopService = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // m_btnStartService
            // 
            this.m_btnStartService.Enabled = false;
            this.m_btnStartService.Location = new System.Drawing.Point(83, 98);
            this.m_btnStartService.Name = "m_btnStartService";
            this.m_btnStartService.Size = new System.Drawing.Size(119, 23);
            this.m_btnStartService.TabIndex = 0;
            this.m_btnStartService.Text = "Start Service";
            this.m_btnStartService.UseVisualStyleBackColor = true;
            this.m_btnStartService.Click += new System.EventHandler(this.OnStartServiceClick);
            // 
            // m_btnStopService
            // 
            this.m_btnStopService.Enabled = false;
            this.m_btnStopService.Location = new System.Drawing.Point(83, 141);
            this.m_btnStopService.Name = "m_btnStopService";
            this.m_btnStopService.Size = new System.Drawing.Size(119, 23);
            this.m_btnStopService.TabIndex = 0;
            this.m_btnStopService.Text = "Stop Service";
            this.m_btnStopService.UseVisualStyleBackColor = true;
            this.m_btnStopService.Click += new System.EventHandler(this.OnStopService_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.m_btnStopService);
            this.Controls.Add(this.m_btnStartService);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Udyog Application License Service";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_btnStartService;
        private System.Windows.Forms.Button m_btnStopService;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}