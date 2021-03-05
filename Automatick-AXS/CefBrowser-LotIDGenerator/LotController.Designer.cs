namespace CefBrowser_LotIDGenerator
{
    partial class LotController
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
            this.btnOK = new System.Windows.Forms.Button();
            this.nudNoOfBrowsers = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfBrowsers)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(7, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 45);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Start";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // nudNoOfBrowsers
            // 
            this.nudNoOfBrowsers.Location = new System.Drawing.Point(121, 7);
            this.nudNoOfBrowsers.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudNoOfBrowsers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNoOfBrowsers.Name = "nudNoOfBrowsers";
            this.nudNoOfBrowsers.Size = new System.Drawing.Size(102, 20);
            this.nudNoOfBrowsers.TabIndex = 4;
            this.nudNoOfBrowsers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNoOfBrowsers.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "No. of Browsers";
            this.label2.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(120, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 45);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // LotController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 72);
            this.ControlBox = false;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.nudNoOfBrowsers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LotController";
            this.Text = "LotController 2.7";
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfBrowsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown nudNoOfBrowsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStop;
    }
}