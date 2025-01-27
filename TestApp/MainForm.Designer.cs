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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAsBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            drawGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zoomToExtendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openMapData = new System.Windows.Forms.OpenFileDialog();
            saveMapData = new System.Windows.Forms.SaveFileDialog();
            saveAsBitmap = new System.Windows.Forms.SaveFileDialog();
            mapToolStrip1 = new MiniMap.MapToolStrip();
            mapControl1 = new MiniMap.MapControl();
            mapStatus1 = new MiniMap.MapStatusStrip();
            scaleToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            mapStatus1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, toolStripMenuItem2, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1260, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsBitmapToolStripMenuItem, toolStripMenuItem1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            openToolStripMenuItem.Text = "&Open ...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            saveToolStripMenuItem.Text = "&Save...";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsBitmapToolStripMenuItem
            // 
            saveAsBitmapToolStripMenuItem.Name = "saveAsBitmapToolStripMenuItem";
            saveAsBitmapToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            saveAsBitmapToolStripMenuItem.Text = "Save as bitmap...";
            saveAsBitmapToolStripMenuItem.Click += saveAsBitmapToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(199, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { drawGridToolStripMenuItem, zoomToExtendToolStripMenuItem });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(55, 24);
            toolStripMenuItem2.Text = "View";
            // 
            // drawGridToolStripMenuItem
            // 
            drawGridToolStripMenuItem.Name = "drawGridToolStripMenuItem";
            drawGridToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            drawGridToolStripMenuItem.Text = "Draw grid";
            drawGridToolStripMenuItem.Click += drawGridToolStripMenuItem_Click;
            // 
            // zoomToExtendToolStripMenuItem
            // 
            zoomToExtendToolStripMenuItem.Name = "zoomToExtendToolStripMenuItem";
            zoomToExtendToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            zoomToExtendToolStripMenuItem.Text = "Zoom to full extend";
            zoomToExtendToolStripMenuItem.Click += zoomToExtendToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // openMapData
            // 
            openMapData.Filter = "Text files (*.txt)|*.txt|GeoJson files (*.GeoJson)|*.GeoJson|All files (*.*)|*.*";
            // 
            // saveMapData
            // 
            saveMapData.Filter = "Text files (*.txt)|*.txt|GeoJson files (*.GeoJson)|*.GeoJson|All files (*.*)|*.*";
            // 
            // saveAsBitmap
            // 
            saveAsBitmap.Filter = "Bimap files (*.bmp)|*.bmp|Gif files (*.gif)|*.gif|Png files (*.png)|*.png|Jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            // 
            // mapToolStrip1
            // 
            mapToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            mapToolStrip1.Location = new System.Drawing.Point(0, 28);
            mapToolStrip1.mapControl = mapControl1;
            mapToolStrip1.Name = "mapToolStrip1";
            mapToolStrip1.Size = new System.Drawing.Size(1260, 25);
            mapToolStrip1.TabIndex = 3;
            mapToolStrip1.Text = "mapToolStrip1";
            // 
            // mapControl1
            // 
            mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            mapControl1.DrawGrid = false;
            mapControl1.Location = new System.Drawing.Point(0, 28);
            mapControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            mapControl1.Mode = MiniMap.ViewerMode.Pan;
            mapControl1.Name = "mapControl1";
            mapControl1.Size = new System.Drawing.Size(1260, 906);
            mapControl1.Step = -1D;
            mapControl1.TabIndex = 0;
            mapControl1.Units = MiniMap.MapUnits.Meters;
            mapControl1.ScaleChanged += mapControl1_ScaleChanged;
            // 
            // mapStatus1
            // 
            mapStatus1.ImageScalingSize = new System.Drawing.Size(20, 20);
            mapStatus1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { scaleToolStripStatusLabel });
            mapStatus1.Location = new System.Drawing.Point(0, 904);
            mapStatus1.mapControl = mapControl1;
            mapStatus1.Name = "mapStatus1";
            mapStatus1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            mapStatus1.Size = new System.Drawing.Size(1260, 30);
            mapStatus1.TabIndex = 1;
            mapStatus1.Text = "mapStatus1";
            mapStatus1.UserFields = false;
            // 
            // scaleToolStripStatusLabel
            // 
            scaleToolStripStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            scaleToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            scaleToolStripStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            scaleToolStripStatusLabel.Name = "scaleToolStripStatusLabel";
            scaleToolStripStatusLabel.Size = new System.Drawing.Size(51, 24);
            scaleToolStripStatusLabel.Text = "Scale:";
            scaleToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1260, 934);
            Controls.Add(mapToolStrip1);
            Controls.Add(mapStatus1);
            Controls.Add(mapControl1);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "Test Application";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            mapStatus1.ResumeLayout(false);
            mapStatus1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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

