namespace Automatick
{
    partial class frmCaptcha
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCaptcha));
            this.pbCaptcha = new System.Windows.Forms.PictureBox();
            this.btnEnter = new C1.Win.C1Input.C1Button();
            this.txtCaptchaText = new System.Windows.Forms.TextBox();
            this.lblRemainingCaptcha = new System.Windows.Forms.Label();
            this.tMSearchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptcha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMSearchBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCaptcha
            // 
            this.pbCaptcha.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCaptcha.Location = new System.Drawing.Point(2, 22);
            this.pbCaptcha.Name = "pbCaptcha";
            this.pbCaptcha.Size = new System.Drawing.Size(300, 150);
            this.pbCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCaptcha.TabIndex = 1;
            this.pbCaptcha.TabStop = false;
            // 
            // btnEnter
            // 
            this.btnEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnter.Location = new System.Drawing.Point(232, 182);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 20);
            this.btnEnter.TabIndex = 2;
            this.btnEnter.Text = "Enter";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnEnter.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // txtCaptchaText
            // 
            this.txtCaptchaText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaptchaText.BackColor = System.Drawing.Color.GhostWhite;
            this.txtCaptchaText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCaptchaText.Location = new System.Drawing.Point(2, 182);
            this.txtCaptchaText.Name = "txtCaptchaText";
            this.txtCaptchaText.Size = new System.Drawing.Size(227, 20);
            this.txtCaptchaText.TabIndex = 3;
            // 
            // lblRemainingCaptcha
            // 
            this.lblRemainingCaptcha.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRemainingCaptcha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainingCaptcha.Location = new System.Drawing.Point(0, 0);
            this.lblRemainingCaptcha.Name = "lblRemainingCaptcha";
            this.lblRemainingCaptcha.Size = new System.Drawing.Size(312, 19);
            this.lblRemainingCaptcha.TabIndex = 4;
            this.lblRemainingCaptcha.Text = "Remaining captcha : ";
            this.lblRemainingCaptcha.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tMSearchBindingSource
            // 
            this.tMSearchBindingSource.AllowNew = true;
            this.tMSearchBindingSource.DataSource = typeof(Automatick.Core.AXSSearch);
            this.tMSearchBindingSource.CurrentChanged += new System.EventHandler(this.tMSearchBindingSource_CurrentChanged);
            this.tMSearchBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.tMSearchBindingSource_ListChanged);
            // 
            // frmCaptcha
            // 
            this.AcceptButton = this.btnEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(312, 204);
            this.ControlBox = false;
            this.Controls.Add(this.lblRemainingCaptcha);
            this.Controls.Add(this.txtCaptchaText);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.pbCaptcha);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimumSize = new System.Drawing.Size(320, 235);
            this.Name = "frmCaptcha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Enter Captcha";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCaptcha_FormClosing);
            this.Load += new System.EventHandler(this.frmCaptcha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptcha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tMSearchBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pbCaptcha;
        private C1.Win.C1Input.C1Button btnEnter;
        private System.Windows.Forms.TextBox txtCaptchaText;
        private System.Windows.Forms.BindingSource tMSearchBindingSource;
        private System.Windows.Forms.Label lblRemainingCaptcha;
    }
}