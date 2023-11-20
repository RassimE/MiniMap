using System;

namespace MiniMap
{
	partial class MapToolStrip
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnZoomToAll = new System.Windows.Forms.ToolStripButton();
			this.btnZoomToPrev = new System.Windows.Forms.ToolStripButton();
			this.btnZoomToNext = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.modeToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.measureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SuspendLayout();
			// 
			// btnZoomToAll
			// 
			this.btnZoomToAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomToAll.Image = global::MiniMap.Properties.Resources.Globe;
			this.btnZoomToAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomToAll.Name = "btnZoomToAll";
			this.btnZoomToAll.Size = new System.Drawing.Size(23, 22);
			this.btnZoomToAll.Text = "Zoom to all";
			this.btnZoomToAll.Click += new System.EventHandler(this.btnZoomToAll_Click);
			// 
			// btnZoomToPrev
			// 
			this.btnZoomToPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomToPrev.Image = global::MiniMap.Properties.Resources.left;
			this.btnZoomToPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomToPrev.Name = "btnZoomToPrev";
			this.btnZoomToPrev.Size = new System.Drawing.Size(23, 22);
			this.btnZoomToPrev.Text = "Zoom to prev ";
			this.btnZoomToPrev.ToolTipText = "Zoom to prev view";
			this.btnZoomToPrev.Click += new System.EventHandler(this.btnZoomToPrev_Click);
			// 
			// btnZoomToNext
			// 
			this.btnZoomToNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomToNext.Image = global::MiniMap.Properties.Resources.right;
			this.btnZoomToNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomToNext.Name = "btnZoomToNext";
			this.btnZoomToNext.Size = new System.Drawing.Size(23, 22);
			this.btnZoomToNext.Text = "Zoom to next";
			this.btnZoomToNext.ToolTipText = "Zoom to next view";
			this.btnZoomToNext.Click += new System.EventHandler(this.btnZoomToNext_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// modeToolStripSplitButton
			// 
			this.modeToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.modeToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.measureToolStripMenuItem,
            this.panToolStripMenuItem,
            this.noneToolStripMenuItem});
			this.modeToolStripSplitButton.Image = global::MiniMap.Properties.Resources.bmpHandFlat;
			this.modeToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.modeToolStripSplitButton.Name = "modeToolStripSplitButton";
			this.modeToolStripSplitButton.Size = new System.Drawing.Size(32, 20);
			this.modeToolStripSplitButton.Text = "Mode - Pan";
			// 
			// zoomInToolStripMenuItem
			// 
			this.zoomInToolStripMenuItem.AutoSize = false;
			this.zoomInToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.zoomInToolStripMenuItem.Image = global::MiniMap.Properties.Resources.bmpZoomIn;
			this.zoomInToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.zoomInToolStripMenuItem.Text = "Zoom in";
			this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
			// 
			// zoomOutToolStripMenuItem
			// 
			this.zoomOutToolStripMenuItem.AutoSize = false;
			this.zoomOutToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.zoomOutToolStripMenuItem.Image = global::MiniMap.Properties.Resources.bmpZoomOut;
			this.zoomOutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.zoomOutToolStripMenuItem.Text = "Zoom out";
			this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
			// 
			// measureToolStripMenuItem
			// 
			this.measureToolStripMenuItem.AutoSize = false;
			this.measureToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.measureToolStripMenuItem.Image = global::MiniMap.Properties.Resources.bmpMeasure;
			this.measureToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.measureToolStripMenuItem.Name = "measureToolStripMenuItem";
			this.measureToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.measureToolStripMenuItem.Text = "Measure";
			this.measureToolStripMenuItem.Click += new System.EventHandler(this.measureToolStripMenuItem_Click);
			// 
			// panToolStripMenuItem
			// 
			this.panToolStripMenuItem.AutoSize = false;
			this.panToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.panToolStripMenuItem.Image = global::MiniMap.Properties.Resources.bmpHandFlat;
			this.panToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.panToolStripMenuItem.Name = "panToolStripMenuItem";
			this.panToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.panToolStripMenuItem.Text = "Pan";
			this.panToolStripMenuItem.Click += new System.EventHandler(this.panToolStripMenuItem_Click);
			// 
			// noneToolStripMenuItem
			// 
			this.noneToolStripMenuItem.AutoSize = false;
			this.noneToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.noneToolStripMenuItem.Image = global::MiniMap.Properties.Resources.bmpNone;
			this.noneToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
			this.noneToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.noneToolStripMenuItem.Text = "None";
			this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
			// 
			// MapToolStrip
			// 
			this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomToAll,
            this.btnZoomToPrev,
            this.btnZoomToNext,
            this.toolStripSeparator1,
            this.modeToolStripSplitButton});
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripButton btnZoomToAll;
		private System.Windows.Forms.ToolStripButton btnZoomToPrev;
		private System.Windows.Forms.ToolStripButton btnZoomToNext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSplitButton modeToolStripSplitButton;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem measureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem panToolStripMenuItem;
	}
}
