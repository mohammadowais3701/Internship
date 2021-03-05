namespace Automatick
{
    partial class frmCaptchaBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCaptchaBrowser));
            this.tMSearchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wbaCaptcha = new Awesomium.Windows.Forms.WebControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tMSearchBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tMSearchBindingSource
            // 
            this.tMSearchBindingSource.DataSource = typeof(Automatick.Core.AXSSearch);
            this.tMSearchBindingSource.CurrentChanged += new System.EventHandler(this.tMSearchBindingSource_CurrentChanged);
            // 
            // wbaCaptcha
            // 
            this.wbaCaptcha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbaCaptcha.Location = new System.Drawing.Point(0, 0);
            this.wbaCaptcha.Size = new System.Drawing.Size(684, 561);
            this.wbaCaptcha.TabIndex = 0;
            this.wbaCaptcha.LoadingFrameComplete += new Awesomium.Core.FrameEventHandler(this.Awesomium_Windows_Forms_WebControl_LoadingFrameComplete);
            // 
            // frmCaptchaBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(684, 561);
            this.ControlBox = false;
            this.Controls.Add(this.wbaCaptcha);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCaptchaBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solve reCaptcha";
            this.Load += new System.EventHandler(this.frmLoginBrowserAwesomium_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tMSearchBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource tMSearchBindingSource;
        private Awesomium.Windows.Forms.WebControl wbaCaptcha;
    }
}