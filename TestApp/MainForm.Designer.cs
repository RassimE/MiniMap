namespace TestApp
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.drawGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToExtendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openMapData = new System.Windows.Forms.OpenFileDialog();
			this.saveMapData = new System.Windows.Forms.SaveFileDialog();
			this.saveAsBitmap = new System.Windows.Forms.SaveFileDialog();
			this.mapToolStrip1 = new MiniMap.MapToolStrip();
			this.mapControl1 = new MiniMap.MapControl();
			this.mapStatus1 = new MiniMap.MapStatusStrip();
			this.scaleToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1.SuspendLayout();
			this.mapStatus1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem2,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(945, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsBitmapToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openToolStripMenuItem.Text = "&Open ...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.saveToolStripMenuItem.Text = "&Save...";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsBitmapToolStripMenuItem
			// 
			this.saveAsBitmapToolStripMenuItem.Name = "saveAsBitmapToolStripMenuItem";
			this.saveAsBitmapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.saveAsBitmapToolStripMenuItem.Text = "Save as bitmap...";
			this.saveAsBitmapToolStripMenuItem.Click += new System.EventHandler(this.saveAsBitmapToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawGridToolStripMenuItem,
            this.zoomToExtendToolStripMenuItem});
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(44, 20);
			this.toolStripMenuItem2.Text = "View";
			// 
			// drawGridToolStripMenuItem
			// 
			this.drawGridToolStripMenuItem.Name = "drawGridToolStripMenuItem";
			this.drawGridToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.drawGridToolStripMenuItem.Text = "Draw grid";
			this.drawGridToolStripMenuItem.Click += new System.EventHandler(this.drawGridToolStripMenuItem_Click);
			// 
			// zoomToExtendToolStripMenuItem
			// 
			this.zoomToExtendToolStripMenuItem.Name = "zoomToExtendToolStripMenuItem";
			this.zoomToExtendToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.zoomToExtendToolStripMenuItem.Text = "Zoom to full extend";
			this.zoomToExtendToolStripMenuItem.Click += new System.EventHandler(this.zoomToExtendToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// openMapData
			// 
			this.openMapData.Filter = "Text files (*.txt)|*.txt|GeoJson files (*.GeoJson)|*.GeoJson|All files (*.*)|*.*";
			// 
			// saveMapData
			// 
			this.saveMapData.Filter = "Text files (*.txt)|*.txt|GeoJson files (*.GeoJson)|*.GeoJson|All files (*.*)|*.*";
			// 
			// saveAsBitmap
			// 
			this.saveAsBitmap.Filter = "Bimap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif|Png files (*.png)|*.png|Jpeg fi" +
    "les (*.jpg)|*.jpg|All files (*.*)|*.*";
			// 
			// mapToolStrip1
			// 
			this.mapToolStrip1.Location = new System.Drawing.Point(0, 24);
			this.mapToolStrip1.mapControl = this.mapControl1;
			this.mapToolStrip1.Name = "mapToolStrip1";
			this.mapToolStrip1.Size = new System.Drawing.Size(945, 25);
			this.mapToolStrip1.TabIndex = 3;
			this.mapToolStrip1.Text = "mapToolStrip1";
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.DrawGrid = false;
			this.mapControl1.Location = new System.Drawing.Point(0, 24);
			this.mapControl1.Mode = MiniMap.ViewerMode.Pan;
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(945, 583);
			this.mapControl1.Step = -1D;
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Units = MiniMap.MapUnits.Meters;
			this.mapControl1.ScaleChanged += new System.EventHandler(this.mapControl1_ScaleChanged);
			// 
			// mapStatus1
			// 
			this.mapStatus1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scaleToolStripStatusLabel});
			this.mapStatus1.Location = new System.Drawing.Point(0, 583);
			this.mapStatus1.mapControl = this.mapControl1;
			this.mapStatus1.Name = "mapStatus1";
			this.mapStatus1.Size = new System.Drawing.Size(945, 24);
			this.mapStatus1.TabIndex = 1;
			this.mapStatus1.Text = "mapStatus1";
			this.mapStatus1.UserFields = false;
			// 
			// scaleToolStripStatusLabel
			// 
			this.scaleToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.scaleToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.scaleToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.scaleToolStripStatusLabel.Name = "scaleToolStripStatusLabel";
			this.scaleToolStripStatusLabel.Size = new System.Drawing.Size(41, 19);
			this.scaleToolStripStatusLabel.Text = "Scale:";
			this.scaleToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 607);
			this.Controls.Add(this.mapToolStrip1);
			this.Controls.Add(this.mapStatus1);
			this.Controls.Add(this.mapControl1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Test Application";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.mapStatus1.ResumeLayout(false);
			this.mapStatus1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MiniMap.MapControl mapControl1;
		private MiniMap.MapStatusStrip mapStatus1;
		private System.Windows.Forms.ToolStripStatusLabel scaleToolStripStatusLabel;
		private System.Windows.Forms.OpenFileDialog openMapData;
		private System.Windows.Forms.SaveFileDialog saveMapData;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem zoomToExtendToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawGridToolStripMenuItem;
		private MiniMap.MapToolStrip mapToolStrip1;
		private System.Windows.Forms.ToolStripMenuItem saveAsBitmapToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveAsBitmap;
	}
}

