namespace Bayesian
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.expressionsTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DataBaseName = new System.Windows.Forms.TextBox();
            this.Load = new System.Windows.Forms.Button();
            this.expressionsList = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.solutionTitle = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(89, 527);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 93);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // expressionsTitle
            // 
            this.expressionsTitle.AutoSize = true;
            this.expressionsTitle.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.expressionsTitle.Location = new System.Drawing.Point(156, 21);
            this.expressionsTitle.Name = "expressionsTitle";
            this.expressionsTitle.Size = new System.Drawing.Size(96, 16);
            this.expressionsTitle.TabIndex = 1;
            this.expressionsTitle.Text = "Expressions list";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Load);
            this.panel1.Controls.Add(this.DataBaseName);
            this.panel1.Location = new System.Drawing.Point(12, 336);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 68);
            this.panel1.TabIndex = 2;
            // 
            // DataBaseName
            // 
            this.DataBaseName.Location = new System.Drawing.Point(3, 3);
            this.DataBaseName.Name = "DataBaseName";
            this.DataBaseName.Size = new System.Drawing.Size(404, 22);
            this.DataBaseName.TabIndex = 0;
            this.DataBaseName.Text = "AI Class Data Base";
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(3, 31);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(155, 31);
            this.Load.TabIndex = 1;
            this.Load.Text = "Load DB";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // expressionsList
            // 
            this.expressionsList.AcceptsReturn = true;
            this.expressionsList.AcceptsTab = true;
            this.expressionsList.AllowDrop = true;
            this.expressionsList.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.expressionsList.Location = new System.Drawing.Point(12, 40);
            this.expressionsList.Multiline = true;
            this.expressionsList.Name = "expressionsList";
            this.expressionsList.ReadOnly = true;
            this.expressionsList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.expressionsList.Size = new System.Drawing.Size(410, 290);
            this.expressionsList.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.AllowDrop = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Location = new System.Drawing.Point(457, 40);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(780, 597);
            this.textBox1.TabIndex = 4;
            // 
            // solutionTitle
            // 
            this.solutionTitle.AutoSize = true;
            this.solutionTitle.Location = new System.Drawing.Point(826, 20);
            this.solutionTitle.Name = "solutionTitle";
            this.solutionTitle.Size = new System.Drawing.Size(59, 17);
            this.solutionTitle.TabIndex = 5;
            this.solutionTitle.Text = "Solution";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 415);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(410, 22);
            this.textBox2.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1249, 649);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.solutionTitle);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.expressionsList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.expressionsTitle);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Bayesian expert system";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label expressionsTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Load;
        private System.Windows.Forms.TextBox DataBaseName;
        private System.Windows.Forms.TextBox expressionsList;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label solutionTitle;
        private System.Windows.Forms.TextBox textBox2;
    }
}

