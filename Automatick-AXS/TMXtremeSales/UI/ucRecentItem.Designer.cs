namespace Automatick
{
    partial class ucRecentItem
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (System.Exception)
            {

            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            C1.Win.C1Gauge.C1GaugeSegment c1GaugeSegment13 = new C1.Win.C1Gauge.C1GaugeSegment();
            C1.Win.C1Gauge.C1GaugeRange c1GaugeRange9 = new C1.Win.C1Gauge.C1GaugeRange();
            C1.Win.C1Gauge.C1GaugeEllipse c1GaugeEllipse17 = new C1.Win.C1Gauge.C1GaugeEllipse();
            C1.Win.C1Gauge.C1GaugeEllipse c1GaugeEllipse18 = new C1.Win.C1Gauge.C1GaugeEllipse();
            C1.Win.C1Gauge.C1GaugeCaption c1GaugeCaption17 = new C1.Win.C1Gauge.C1GaugeCaption();
            txtBuyRate = new C1.Win.C1Gauge.C1GaugeCaption();
            C1.Win.C1Gauge.C1GaugeSegment c1GaugeSegment14 = new C1.Win.C1Gauge.C1GaugeSegment();
            C1.Win.C1Gauge.C1GaugeSegment c1GaugeSegment15 = new C1.Win.C1Gauge.C1GaugeSegment();
            C1.Win.C1Gauge.C1GaugeRange c1GaugeRange10 = new C1.Win.C1Gauge.C1GaugeRange();
            C1.Win.C1Gauge.C1GaugeEllipse c1GaugeEllipse19 = new C1.Win.C1Gauge.C1GaugeEllipse();
            C1.Win.C1Gauge.C1GaugeEllipse c1GaugeEllipse20 = new C1.Win.C1Gauge.C1GaugeEllipse();
            C1.Win.C1Gauge.C1GaugeCaption c1GaugeCaption19 = new C1.Win.C1Gauge.C1GaugeCaption();
            txtFoundRate = new C1.Win.C1Gauge.C1GaugeCaption();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucRecentItem));
            this.gaugeMain = new C1.Win.C1Gauge.C1Gauge();
            this.rgBuyRate = new C1.Win.C1Gauge.C1RadialGauge();
            this.rgFoundRate = new C1.Win.C1Gauge.C1RadialGauge();
            this.lblTicketName = new System.Windows.Forms.LinkLabel();
            this.lblEdit = new System.Windows.Forms.LinkLabel();
            this.lblDelete = new System.Windows.Forms.LinkLabel();
            this.lblStop = new System.Windows.Forms.LinkLabel();
            this.lblDuplicate = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeMain)).BeginInit();
            this.SuspendLayout();
            // 
            // gaugeMain
            // 
            this.gaugeMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gaugeMain.Gauges.AddRange(new C1.Win.C1Gauge.C1GaugeBase[] {
            this.rgBuyRate,
            this.rgFoundRate});
            this.gaugeMain.Location = new System.Drawing.Point(0, 0);
            this.gaugeMain.Name = "gaugeMain";
            this.gaugeMain.Size = new System.Drawing.Size(300, 175);
            this.gaugeMain.TabIndex = 39;
            this.gaugeMain.Viewport.AspectRatio = 2.1D;
            this.gaugeMain.ViewTag = ((long)(1119465662117722821));
            // 
            // rgBuyRate
            // 
            this.rgBuyRate.Cap.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            this.rgBuyRate.Cap.Filling.Color = System.Drawing.Color.DarkSlateGray;
            this.rgBuyRate.Cap.Radius = 7D;
            this.rgBuyRate.Cap.Shadow.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.rgBuyRate.Cap.Shadow.Opacity = 0.15D;
            this.rgBuyRate.Cap.Shadow.Visible = true;
            c1GaugeSegment13.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeSegment13.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeSegment13.Filling.Color = System.Drawing.Color.White;
            c1GaugeSegment13.Filling.Color2 = System.Drawing.Color.White;
            c1GaugeSegment13.Filling.Opacity = 0.1D;
            c1GaugeSegment13.Filling.Opacity2 = 0.2D;
            c1GaugeSegment13.Gradient.Direction = C1.Win.C1Gauge.C1GaugeGradientDirection.RadialOuter;
            c1GaugeSegment13.InnerRadius = 120D;
            c1GaugeSegment13.OuterRadius = 98D;
            c1GaugeSegment13.StartAngle = -35D;
            c1GaugeSegment13.SweepAngle = 195D;
            this.rgBuyRate.CoverShapes.AddRange(new C1.Win.C1Gauge.C1GaugeBaseShape[] {
            c1GaugeSegment13});
            c1GaugeRange9.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeRange9.Filling.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            c1GaugeRange9.Location = 85D;
            c1GaugeRange9.ViewTag = ((long)(719900497179979479));
            c1GaugeRange9.Width = 1D;
            c1GaugeRange9.Width2 = 20D;
            this.rgBuyRate.Decorators.AddRange(new C1.Win.C1Gauge.C1GaugeDecorator[] {
            c1GaugeRange9});
            c1GaugeEllipse17.Border.Color = System.Drawing.Color.LightGray;
            c1GaugeEllipse17.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeEllipse17.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeEllipse17.Filling.Color = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(244)))), ((int)(((byte)(251)))));
            c1GaugeEllipse17.Filling.Color2 = System.Drawing.Color.DarkSlateGray;
            c1GaugeEllipse17.Height = -1.08D;
            c1GaugeEllipse17.HitTestable = false;
            c1GaugeEllipse17.Width = -1.08D;
            c1GaugeEllipse18.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeEllipse18.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeEllipse18.Filling.Color = System.Drawing.Color.WhiteSmoke;
            c1GaugeEllipse18.Filling.Color2 = System.Drawing.Color.CadetBlue;
            c1GaugeEllipse18.Gradient.CenterPointX = 0.8D;
            c1GaugeEllipse18.Gradient.CenterPointY = 0.8D;
            c1GaugeEllipse18.Gradient.Direction = C1.Win.C1Gauge.C1GaugeGradientDirection.RadialOuter;
            c1GaugeEllipse18.Height = -0.98D;
            c1GaugeEllipse18.HitTestable = false;
            c1GaugeEllipse18.Width = -0.98D;
            c1GaugeCaption17.CenterPointY = 0.85D;
            c1GaugeCaption17.Color = System.Drawing.Color.DarkSlateGray;
            c1GaugeCaption17.FontSize = 15D;
            c1GaugeCaption17.Text = "Buy rate";
            txtBuyRate.CenterPointY = 0.75D;
            txtBuyRate.Color = System.Drawing.Color.DarkSlateGray;
            txtBuyRate.FontSize = 13D;
            txtBuyRate.Name = "txtBuyRate";
            txtBuyRate.Text = "Very low";
            this.rgBuyRate.FaceShapes.AddRange(new C1.Win.C1Gauge.C1GaugeBaseShape[] {
            c1GaugeEllipse17,
            c1GaugeEllipse18,
            c1GaugeCaption17,
            txtBuyRate});
            this.rgBuyRate.Name = "rgBuyRate";
            this.rgBuyRate.Pointer.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            this.rgBuyRate.Pointer.Filling.Color = System.Drawing.Color.DarkSlateGray;
            this.rgBuyRate.Pointer.Length = 73D;
            this.rgBuyRate.Pointer.Offset = 0D;
            this.rgBuyRate.Pointer.Shadow.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.rgBuyRate.Pointer.Shadow.Opacity = 0.15D;
            this.rgBuyRate.Pointer.Shadow.Visible = true;
            this.rgBuyRate.Pointer.Shape = C1.Win.C1Gauge.C1GaugePointerShape.Rectangle;
            this.rgBuyRate.Pointer.SweepTime = 1D;
            this.rgBuyRate.Pointer.Width = 5D;
            this.rgBuyRate.PointerOriginX = 0.65D;
            this.rgBuyRate.Radius = 0.4D;
            this.rgBuyRate.StartAngle = -30D;
            this.rgBuyRate.SweepAngle = 170D;
            this.rgBuyRate.ViewTag = ((long)(639930353501108013));
            // 
            // rgFoundRate
            // 
            this.rgFoundRate.Cap.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            this.rgFoundRate.Cap.Filling.Color = System.Drawing.Color.DarkSlateGray;
            this.rgFoundRate.Cap.Radius = 7D;
            this.rgFoundRate.Cap.Shadow.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.rgFoundRate.Cap.Shadow.Opacity = 0.15D;
            this.rgFoundRate.Cap.Shadow.Visible = true;
            c1GaugeSegment14.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeSegment14.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeSegment14.Filling.Color = System.Drawing.Color.White;
            c1GaugeSegment14.Filling.Color2 = System.Drawing.Color.White;
            c1GaugeSegment14.Filling.Opacity = 0.1D;
            c1GaugeSegment14.Filling.Opacity2 = 0.2D;
            c1GaugeSegment14.Gradient.Direction = C1.Win.C1Gauge.C1GaugeGradientDirection.RadialOuter;
            c1GaugeSegment14.InnerRadius = -150D;
            c1GaugeSegment14.OuterRadius = 98D;
            c1GaugeSegment14.StartAngle = 250D;
            c1GaugeSegment14.SweepAngle = 359D;
            c1GaugeSegment15.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeSegment15.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeSegment15.Filling.Color = System.Drawing.Color.White;
            c1GaugeSegment15.Filling.Color2 = System.Drawing.Color.White;
            c1GaugeSegment15.Filling.Opacity = 0.1D;
            c1GaugeSegment15.Filling.Opacity2 = 0.2D;
            c1GaugeSegment15.Gradient.Direction = C1.Win.C1Gauge.C1GaugeGradientDirection.RadialOuter;
            c1GaugeSegment15.InnerRadius = 125D;
            c1GaugeSegment15.OuterRadius = 98D;
            c1GaugeSegment15.SweepAngle = 190D;
            this.rgFoundRate.CoverShapes.AddRange(new C1.Win.C1Gauge.C1GaugeBaseShape[] {
            c1GaugeSegment14,
            c1GaugeSegment15});
            c1GaugeRange10.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeRange10.Filling.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            c1GaugeRange10.Location = 85D;
            c1GaugeRange10.ViewTag = ((long)(719900497179979479));
            c1GaugeRange10.Width = 1D;
            c1GaugeRange10.Width2 = 20D;
            this.rgFoundRate.Decorators.AddRange(new C1.Win.C1Gauge.C1GaugeDecorator[] {
            c1GaugeRange10});
            c1GaugeEllipse19.Border.Color = System.Drawing.Color.LightGray;
            c1GaugeEllipse19.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeEllipse19.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeEllipse19.Filling.Color = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(244)))), ((int)(((byte)(251)))));
            c1GaugeEllipse19.Filling.Color2 = System.Drawing.Color.DarkSlateGray;
            c1GaugeEllipse19.Height = -1.08D;
            c1GaugeEllipse19.HitTestable = false;
            c1GaugeEllipse19.Width = -1.08D;
            c1GaugeEllipse20.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            c1GaugeEllipse20.Filling.BrushType = C1.Win.C1Gauge.C1GaugeBrushType.Gradient;
            c1GaugeEllipse20.Filling.Color = System.Drawing.Color.WhiteSmoke;
            c1GaugeEllipse20.Filling.Color2 = System.Drawing.Color.CadetBlue;
            c1GaugeEllipse20.Gradient.CenterPointX = 0.8D;
            c1GaugeEllipse20.Gradient.CenterPointY = 0.8D;
            c1GaugeEllipse20.Gradient.Direction = C1.Win.C1Gauge.C1GaugeGradientDirection.RadialOuter;
            c1GaugeEllipse20.Height = -0.98D;
            c1GaugeEllipse20.HitTestable = false;
            c1GaugeEllipse20.Width = -0.98D;
            c1GaugeCaption19.CenterPointY = 0.85D;
            c1GaugeCaption19.Color = System.Drawing.Color.DarkSlateGray;
            c1GaugeCaption19.FontSize = 15D;
            c1GaugeCaption19.Text = "Found rate";
            txtFoundRate.CenterPointY = 0.75D;
            txtFoundRate.Color = System.Drawing.Color.DarkSlateGray;
            txtFoundRate.FontSize = 13D;
            txtFoundRate.Name = "txtFoundRate";
            txtFoundRate.Text = "Very high";
            this.rgFoundRate.FaceShapes.AddRange(new C1.Win.C1Gauge.C1GaugeBaseShape[] {
            c1GaugeEllipse19,
            c1GaugeEllipse20,
            c1GaugeCaption19,
            txtFoundRate});
            this.rgFoundRate.Name = "rgFoundRate";
            this.rgFoundRate.Pointer.Border.LineStyle = C1.Win.C1Gauge.C1GaugeBorderStyle.None;
            this.rgFoundRate.Pointer.Filling.Color = System.Drawing.Color.DarkSlateGray;
            this.rgFoundRate.Pointer.Length = 73D;
            this.rgFoundRate.Pointer.Offset = 0D;
            this.rgFoundRate.Pointer.Shadow.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.rgFoundRate.Pointer.Shadow.Opacity = 0.15D;
            this.rgFoundRate.Pointer.Shadow.Visible = true;
            this.rgFoundRate.Pointer.Shape = C1.Win.C1Gauge.C1GaugePointerShape.Rectangle;
            this.rgFoundRate.Pointer.SweepTime = 1D;
            this.rgFoundRate.Pointer.Width = 5D;
            this.rgFoundRate.PointerOriginX = 0.35D;
            this.rgFoundRate.Radius = 0.48D;
            this.rgFoundRate.StartAngle = -90D;
            this.rgFoundRate.SweepAngle = 230D;
            this.rgFoundRate.ViewTag = ((long)(639930353501108013));
            // 
            // lblTicketName
            // 
            this.lblTicketName.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.lblTicketName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblTicketName.LinkColor = System.Drawing.Color.White;
            this.lblTicketName.Location = new System.Drawing.Point(0, 201);
            this.lblTicketName.Name = "lblTicketName";
            this.lblTicketName.Size = new System.Drawing.Size(300, 20);
            this.lblTicketName.TabIndex = 40;
            this.lblTicketName.TabStop = true;
            this.lblTicketName.Text = "Ticket Name";
            this.lblTicketName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTicketName.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.lblTicketName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblTicketName_LinkClicked);
            // 
            // lblEdit
            // 
            this.lblEdit.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.lblEdit.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdit.Image = ((System.Drawing.Image)(resources.GetObject("lblEdit.Image")));
            this.lblEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblEdit.LinkColor = System.Drawing.Color.White;
            this.lblEdit.Location = new System.Drawing.Point(34, 177);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(70, 14);
            this.lblEdit.TabIndex = 41;
            this.lblEdit.TabStop = true;
            this.lblEdit.Text = "Edit";
            this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEdit.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.lblEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEdit_LinkClicked);
            // 
            // lblDelete
            // 
            this.lblDelete.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.lblDelete.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelete.Image = ((System.Drawing.Image)(resources.GetObject("lblDelete.Image")));
            this.lblDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblDelete.LinkColor = System.Drawing.Color.White;
            this.lblDelete.Location = new System.Drawing.Point(112, 177);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(85, 14);
            this.lblDelete.TabIndex = 42;
            this.lblDelete.TabStop = true;
            this.lblDelete.Text = "Delete";
            this.lblDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDelete.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.lblDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDelete_LinkClicked);
            // 
            // lblStop
            // 
            this.lblStop.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.lblStop.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStop.Image = global::Automatick.Properties.Resources.stop16;
            this.lblStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStop.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblStop.LinkColor = System.Drawing.Color.White;
            this.lblStop.Location = new System.Drawing.Point(115, 177);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(70, 14);
            this.lblStop.TabIndex = 43;
            this.lblStop.TabStop = true;
            this.lblStop.Text = "Stop";
            this.lblStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStop.Visible = false;
            this.lblStop.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.lblStop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblStop_LinkClicked);
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.lblDuplicate.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuplicate.Image = global::Automatick.Properties.Resources.if_page_copy_36245;
            this.lblDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDuplicate.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblDuplicate.LinkColor = System.Drawing.Color.White;
            this.lblDuplicate.Location = new System.Drawing.Point(195, 177);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(85, 14);
            this.lblDuplicate.TabIndex = 44;
            this.lblDuplicate.TabStop = true;
            this.lblDuplicate.Text = "    Duplicate";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDuplicate.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.lblDuplicate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDuplicate_LinkClicked);
            // 
            // ucRecentItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDuplicate);
            this.Controls.Add(this.lblStop);
            this.Controls.Add(this.lblDelete);
            this.Controls.Add(this.lblEdit);
            this.Controls.Add(this.lblTicketName);
            this.Controls.Add(this.gaugeMain);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Margin = new System.Windows.Forms.Padding(15, 30, 10, 5);
            this.Name = "ucRecentItem";
            this.Size = new System.Drawing.Size(300, 225);
            this.Load += new System.EventHandler(this.ucRecentItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gaugeMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Gauge.C1Gauge gaugeMain;
        private C1.Win.C1Gauge.C1RadialGauge rgBuyRate;
        private C1.Win.C1Gauge.C1RadialGauge rgFoundRate;
        private C1.Win.C1Gauge.C1GaugeCaption txtBuyRate;
        private C1.Win.C1Gauge.C1GaugeCaption txtFoundRate;
        private System.Windows.Forms.LinkLabel lblTicketName;
        public System.Windows.Forms.LinkLabel lblEdit;
        public System.Windows.Forms.LinkLabel lblDelete;
        public System.Windows.Forms.LinkLabel lblStop;
        public System.Windows.Forms.LinkLabel lblDuplicate;
    }
}