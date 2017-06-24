namespace WindowsFormsApplication5
{
    partial class frmRegister_dll
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
            this.btnregister_dll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnregister_dll
            // 
            this.btnregister_dll.Location = new System.Drawing.Point(35, 17);
            this.btnregister_dll.Name = "btnregister_dll";
            this.btnregister_dll.Size = new System.Drawing.Size(151, 23);
            this.btnregister_dll.TabIndex = 0;
            this.btnregister_dll.Text = "Register DLL";
            this.btnregister_dll.UseVisualStyleBackColor = true;
            this.btnregister_dll.Click += new System.EventHandler(this.btnregister_dll_Click);
            // 
            // frmRegister_dll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 57);
            this.Controls.Add(this.btnregister_dll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRegister_dll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmRegister_dll_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnregister_dll;
    }
}

