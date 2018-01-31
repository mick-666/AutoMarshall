namespace AutoMarshal
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tbBaseURI = new System.Windows.Forms.TextBox();
            this.tbVehiclesURI = new System.Windows.Forms.TextBox();
            this.tbImageURI = new System.Windows.Forms.TextBox();
            this.updownPeriod = new System.Windows.Forms.NumericUpDown();
            this.updownRowLimit = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.updownPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownRowLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // tbBaseURI
            // 
            this.tbBaseURI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBaseURI.Location = new System.Drawing.Point(216, 31);
            this.tbBaseURI.Name = "tbBaseURI";
            this.tbBaseURI.Size = new System.Drawing.Size(496, 20);
            this.tbBaseURI.TabIndex = 0;
            // 
            // tbVehiclesURI
            // 
            this.tbVehiclesURI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVehiclesURI.Location = new System.Drawing.Point(216, 57);
            this.tbVehiclesURI.Name = "tbVehiclesURI";
            this.tbVehiclesURI.Size = new System.Drawing.Size(496, 20);
            this.tbVehiclesURI.TabIndex = 1;
            // 
            // tbImageURI
            // 
            this.tbImageURI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImageURI.Location = new System.Drawing.Point(216, 84);
            this.tbImageURI.Name = "tbImageURI";
            this.tbImageURI.Size = new System.Drawing.Size(496, 20);
            this.tbImageURI.TabIndex = 2;
            // 
            // updownPeriod
            // 
            this.updownPeriod.Location = new System.Drawing.Point(216, 111);
            this.updownPeriod.Name = "updownPeriod";
            this.updownPeriod.Size = new System.Drawing.Size(100, 20);
            this.updownPeriod.TabIndex = 3;
            // 
            // updownRowLimit
            // 
            this.updownRowLimit.Location = new System.Drawing.Point(216, 137);
            this.updownRowLimit.Name = "updownRowLimit";
            this.updownRowLimit.Size = new System.Drawing.Size(100, 20);
            this.updownRowLimit.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Адрес web сервиса";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "URI запроса журнала (шаблон)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "URI изображений (шаблон)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Запрашиваемый период (дней)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Количество строк журнала в запросе";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(559, 158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(640, 158);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&Сохранить";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 193);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.updownRowLimit);
            this.Controls.Add(this.updownPeriod);
            this.Controls.Add(this.tbImageURI);
            this.Controls.Add(this.tbVehiclesURI);
            this.Controls.Add(this.tbBaseURI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(254, 38);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.updownPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownRowLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBaseURI;
        private System.Windows.Forms.TextBox tbVehiclesURI;
        private System.Windows.Forms.TextBox tbImageURI;
        private System.Windows.Forms.NumericUpDown updownPeriod;
        private System.Windows.Forms.NumericUpDown updownRowLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}