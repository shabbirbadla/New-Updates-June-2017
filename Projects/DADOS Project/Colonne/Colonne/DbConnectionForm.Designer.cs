namespace Colonne
{
    partial class DbConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbConnectionForm));
            this.serverLabel = new System.Windows.Forms.Label();
            this.windowsAuthenticationRadioButton = new System.Windows.Forms.RadioButton();
            this.sqlAuthenticationRadioButton = new System.Windows.Forms.RadioButton();
            this.databaseComboBox = new System.Windows.Forms.ComboBox();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.userIdTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userIdLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.btnRetry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(21, 31);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(38, 13);
            this.serverLabel.TabIndex = 1;
            this.serverLabel.Text = "Server";
            // 
            // windowsAuthenticationRadioButton
            // 
            this.windowsAuthenticationRadioButton.AutoSize = true;
            this.windowsAuthenticationRadioButton.Checked = true;
            this.windowsAuthenticationRadioButton.Location = new System.Drawing.Point(24, 53);
            this.windowsAuthenticationRadioButton.Name = "windowsAuthenticationRadioButton";
            this.windowsAuthenticationRadioButton.Size = new System.Drawing.Size(140, 17);
            this.windowsAuthenticationRadioButton.TabIndex = 3;
            this.windowsAuthenticationRadioButton.TabStop = true;
            this.windowsAuthenticationRadioButton.Text = "Windows Authentication";
            this.windowsAuthenticationRadioButton.UseVisualStyleBackColor = true;
            this.windowsAuthenticationRadioButton.CheckedChanged += new System.EventHandler(this.sqlAuthenticationRadioButton_CheckedChanged);
            // 
            // sqlAuthenticationRadioButton
            // 
            this.sqlAuthenticationRadioButton.AutoSize = true;
            this.sqlAuthenticationRadioButton.Location = new System.Drawing.Point(170, 53);
            this.sqlAuthenticationRadioButton.Name = "sqlAuthenticationRadioButton";
            this.sqlAuthenticationRadioButton.Size = new System.Drawing.Size(151, 17);
            this.sqlAuthenticationRadioButton.TabIndex = 4;
            this.sqlAuthenticationRadioButton.Text = "SQL Server Authentication";
            this.sqlAuthenticationRadioButton.UseVisualStyleBackColor = true;
            this.sqlAuthenticationRadioButton.CheckedChanged += new System.EventHandler(this.sqlAuthenticationRadioButton_CheckedChanged);
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.FormattingEnabled = true;
            this.databaseComboBox.Location = new System.Drawing.Point(81, 128);
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(209, 21);
            this.databaseComboBox.TabIndex = 8;
            this.databaseComboBox.Enter += new System.EventHandler(this.databaseComboBox_Enter);
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Location = new System.Drawing.Point(24, 155);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(104, 23);
            this.testConnectionButton.TabIndex = 7;
            this.testConnectionButton.Text = "Test Connection";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler(this.testConnectionButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(156, 185);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(68, 23);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(230, 185);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(59, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(21, 131);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(53, 13);
            this.databaseLabel.TabIndex = 8;
            this.databaseLabel.Text = "Database";
            // 
            // userIdTextBox
            // 
            this.userIdTextBox.AccessibleName = "";
            this.userIdTextBox.Enabled = false;
            this.userIdTextBox.Location = new System.Drawing.Point(81, 76);
            this.userIdTextBox.Name = "userIdTextBox";
            this.userIdTextBox.Size = new System.Drawing.Size(209, 20);
            this.userIdTextBox.TabIndex = 5;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.AccessibleName = "";
            this.passwordTextBox.Enabled = false;
            this.passwordTextBox.Location = new System.Drawing.Point(81, 102);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(209, 20);
            this.passwordTextBox.TabIndex = 6;
            // 
            // userIdLabel
            // 
            this.userIdLabel.AutoSize = true;
            this.userIdLabel.Location = new System.Drawing.Point(21, 79);
            this.userIdLabel.Name = "userIdLabel";
            this.userIdLabel.Size = new System.Drawing.Size(41, 13);
            this.userIdLabel.TabIndex = 11;
            this.userIdLabel.Text = "User Id";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(21, 105);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 12;
            this.passwordLabel.Text = "Password";
            // 
            // serverComboBox
            // 
            this.serverComboBox.FormattingEnabled = true;
            this.serverComboBox.Location = new System.Drawing.Point(81, 28);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(209, 21);
            this.serverComboBox.TabIndex = 1;
            // 
            // btnRetry
            // 
            this.btnRetry.Location = new System.Drawing.Point(294, 26);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(46, 23);
            this.btnRetry.TabIndex = 2;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // DbConnectionForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(341, 220);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.serverComboBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userIdLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.userIdTextBox);
            this.Controls.Add(this.databaseLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.testConnectionButton);
            this.Controls.Add(this.databaseComboBox);
            this.Controls.Add(this.sqlAuthenticationRadioButton);
            this.Controls.Add(this.windowsAuthenticationRadioButton);
            this.Controls.Add(this.serverLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Connections";
            this.Load += new System.EventHandler(this.DbConnectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.RadioButton windowsAuthenticationRadioButton;
        private System.Windows.Forms.RadioButton sqlAuthenticationRadioButton;
        private System.Windows.Forms.ComboBox databaseComboBox;
        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.TextBox userIdTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label userIdLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.ComboBox serverComboBox;
        private System.Windows.Forms.Button btnRetry;
    }
}

