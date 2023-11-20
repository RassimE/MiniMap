
namespace MiniMap
{
	partial class MapControl
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
			this.MapPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.MapPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// MapPicture
			// 
			this.MapPicture.Cursor = System.Windows.Forms.Cursors.NoMove2D;
			this.MapPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MapPicture.Location = new System.Drawing.Point(0, 0);
			this.MapPicture.Name = "MapPicture";
			this.MapPicture.Size = new System.Drawing.Size(231, 216);
			this.MapPicture.TabIndex = 0;
			this.MapPicture.TabStop = false;
			this.MapPicture.SizeChanged += new System.EventHandler(this.MapPicture_SizeChanged);
			this.MapPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseDown);
			this.MapPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseMove);
			this.MapPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseUp);
			this.MapPicture.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseWheel);
			// 
			// MapControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MapPicture);
			this.Name = "MapControl";
			this.Size = new System.Drawing.Size(231, 216);
			((System.ComponentModel.ISupportInitialize)(this.MapPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox MapPicture;
	}
}
