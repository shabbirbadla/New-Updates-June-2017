namespace udEmpPayHeadMaster
{
    partial class frmFormula
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
            this.btnNotEqual = new System.Windows.Forms.Button();
            this.btnCloseBrac = new System.Windows.Forms.Button();
            this.btnLessEqual = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenBrac = new System.Windows.Forms.Button();
            this.btnGraterEqual = new System.Windows.Forms.Button();
            this.btnEqual = new System.Windows.Forms.Button();
            this.btnbtnDevision = new System.Windows.Forms.Button();
            this.btnLess = new System.Windows.Forms.Button();
            this.btnMulti = new System.Windows.Forms.Button();
            this.btnGrater = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstFldList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblExpression = new System.Windows.Forms.Label();
            this.lstVeriable = new System.Windows.Forms.ListBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.rtxtExpression = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNotEqual
            // 
            this.btnNotEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotEqual.Location = new System.Drawing.Point(36, 81);
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.Size = new System.Drawing.Size(31, 25);
            this.btnNotEqual.TabIndex = 55;
            this.btnNotEqual.Text = "<>";
            this.btnNotEqual.UseVisualStyleBackColor = true;
            this.btnNotEqual.Click += new System.EventHandler(this.btnNotEqual_Click);
            // 
            // btnCloseBrac
            // 
            this.btnCloseBrac.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseBrac.Location = new System.Drawing.Point(66, 112);
            this.btnCloseBrac.Name = "btnCloseBrac";
            this.btnCloseBrac.Size = new System.Drawing.Size(31, 25);
            this.btnCloseBrac.TabIndex = 54;
            this.btnCloseBrac.Text = ")";
            this.btnCloseBrac.UseVisualStyleBackColor = true;
            this.btnCloseBrac.Click += new System.EventHandler(this.btnCloseBrac_Click);
            // 
            // btnLessEqual
            // 
            this.btnLessEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLessEqual.Location = new System.Drawing.Point(66, 80);
            this.btnLessEqual.Name = "btnLessEqual";
            this.btnLessEqual.Size = new System.Drawing.Size(31, 25);
            this.btnLessEqual.TabIndex = 53;
            this.btnLessEqual.Text = "<=";
            this.btnLessEqual.UseVisualStyleBackColor = true;
            this.btnLessEqual.Click += new System.EventHandler(this.btnLessEqual_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNotEqual);
            this.groupBox1.Controls.Add(this.btnCloseBrac);
            this.groupBox1.Controls.Add(this.btnLessEqual);
            this.groupBox1.Controls.Add(this.btnOpenBrac);
            this.groupBox1.Controls.Add(this.btnGraterEqual);
            this.groupBox1.Controls.Add(this.btnEqual);
            this.groupBox1.Controls.Add(this.btnbtnDevision);
            this.groupBox1.Controls.Add(this.btnLess);
            this.groupBox1.Controls.Add(this.btnMulti);
            this.groupBox1.Controls.Add(this.btnGrater);
            this.groupBox1.Controls.Add(this.btnMinus);
            this.groupBox1.Controls.Add(this.btnPlus);
            this.groupBox1.Location = new System.Drawing.Point(210, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(105, 152);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operator";
            // 
            // btnOpenBrac
            // 
            this.btnOpenBrac.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenBrac.Location = new System.Drawing.Point(36, 112);
            this.btnOpenBrac.Name = "btnOpenBrac";
            this.btnOpenBrac.Size = new System.Drawing.Size(31, 25);
            this.btnOpenBrac.TabIndex = 52;
            this.btnOpenBrac.Text = "(";
            this.btnOpenBrac.UseVisualStyleBackColor = true;
            this.btnOpenBrac.Click += new System.EventHandler(this.btnOpenBrac_Click);
            // 
            // btnGraterEqual
            // 
            this.btnGraterEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraterEqual.Location = new System.Drawing.Point(6, 112);
            this.btnGraterEqual.Name = "btnGraterEqual";
            this.btnGraterEqual.Size = new System.Drawing.Size(31, 25);
            this.btnGraterEqual.TabIndex = 51;
            this.btnGraterEqual.Text = ">=";
            this.btnGraterEqual.UseVisualStyleBackColor = true;
            this.btnGraterEqual.Click += new System.EventHandler(this.btnGraterEqual_Click);
            // 
            // btnEqual
            // 
            this.btnEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEqual.Location = new System.Drawing.Point(6, 81);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(31, 25);
            this.btnEqual.TabIndex = 50;
            this.btnEqual.Text = "=";
            this.btnEqual.UseVisualStyleBackColor = true;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btnbtnDevision
            // 
            this.btnbtnDevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbtnDevision.Location = new System.Drawing.Point(6, 50);
            this.btnbtnDevision.Name = "btnbtnDevision";
            this.btnbtnDevision.Size = new System.Drawing.Size(31, 25);
            this.btnbtnDevision.TabIndex = 49;
            this.btnbtnDevision.Text = "/";
            this.btnbtnDevision.UseVisualStyleBackColor = true;
            this.btnbtnDevision.Click += new System.EventHandler(this.btnbtnDevision_Click);
            // 
            // btnLess
            // 
            this.btnLess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLess.Location = new System.Drawing.Point(66, 49);
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(31, 25);
            this.btnLess.TabIndex = 48;
            this.btnLess.Text = "<";
            this.btnLess.UseVisualStyleBackColor = true;
            this.btnLess.Click += new System.EventHandler(this.btnLess_Click);
            // 
            // btnMulti
            // 
            this.btnMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMulti.Location = new System.Drawing.Point(66, 18);
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(31, 25);
            this.btnMulti.TabIndex = 47;
            this.btnMulti.Text = "*";
            this.btnMulti.UseVisualStyleBackColor = true;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // btnGrater
            // 
            this.btnGrater.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrater.Location = new System.Drawing.Point(36, 50);
            this.btnGrater.Name = "btnGrater";
            this.btnGrater.Size = new System.Drawing.Size(31, 25);
            this.btnGrater.TabIndex = 46;
            this.btnGrater.Text = ">";
            this.btnGrater.UseVisualStyleBackColor = true;
            this.btnGrater.Click += new System.EventHandler(this.btnGrater_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.Location = new System.Drawing.Point(36, 19);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(31, 25);
            this.btnMinus.TabIndex = 45;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.Location = new System.Drawing.Point(6, 19);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(31, 25);
            this.btnPlus.TabIndex = 44;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 51;
            this.label1.Text = "Field List";
            // 
            // lstFldList
            // 
            this.lstFldList.FormattingEnabled = true;
            this.lstFldList.Location = new System.Drawing.Point(12, 194);
            this.lstFldList.Name = "lstFldList";
            this.lstFldList.Size = new System.Drawing.Size(183, 199);
            this.lstFldList.TabIndex = 50;
            this.lstFldList.DoubleClick += new System.EventHandler(this.lstFldList_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(321, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Veriables";
            this.label3.Visible = false;
            // 
            // lblExpression
            // 
            this.lblExpression.AutoSize = true;
            this.lblExpression.Location = new System.Drawing.Point(12, 17);
            this.lblExpression.Name = "lblExpression";
            this.lblExpression.Size = new System.Drawing.Size(58, 13);
            this.lblExpression.TabIndex = 56;
            this.lblExpression.Text = "Expression";
            // 
            // lstVeriable
            // 
            this.lstVeriable.FormattingEnabled = true;
            this.lstVeriable.Location = new System.Drawing.Point(324, 207);
            this.lstVeriable.Name = "lstVeriable";
            this.lstVeriable.Size = new System.Drawing.Size(149, 147);
            this.lstVeriable.TabIndex = 57;
            this.lstVeriable.Visible = false;
            this.lstVeriable.SelectedIndexChanged += new System.EventHandler(this.lstVeriable_SelectedIndexChanged);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(509, 371);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 59;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // rtxtExpression
            // 
            this.rtxtExpression.Location = new System.Drawing.Point(15, 33);
            this.rtxtExpression.MaxLength = 300;
            this.rtxtExpression.Name = "rtxtExpression";
            this.rtxtExpression.Size = new System.Drawing.Size(565, 123);
            this.rtxtExpression.TabIndex = 60;
            this.rtxtExpression.Text = "";
            // 
            // frmFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 401);
            this.Controls.Add(this.rtxtExpression);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstFldList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblExpression);
            this.Controls.Add(this.lstVeriable);
            this.Name = "frmFormula";
            this.Text = "frmFormula";
            this.Load += new System.EventHandler(this.frmFormula_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNotEqual;
        private System.Windows.Forms.Button btnCloseBrac;
        private System.Windows.Forms.Button btnLessEqual;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenBrac;
        private System.Windows.Forms.Button btnGraterEqual;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.Button btnbtnDevision;
        private System.Windows.Forms.Button btnLess;
        private System.Windows.Forms.Button btnMulti;
        private System.Windows.Forms.Button btnGrater;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstFldList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblExpression;
        private System.Windows.Forms.ListBox lstVeriable;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.RichTextBox rtxtExpression;
    }
}