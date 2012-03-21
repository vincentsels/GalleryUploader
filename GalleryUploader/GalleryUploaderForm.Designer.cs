namespace GalleryUploader
{
	partial class GalleryUploaderForm
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
			this.txtStartingDirectory = new System.Windows.Forms.TextBox();
			this.lblStartingDirectory = new System.Windows.Forms.Label();
			this.grpProgress = new System.Windows.Forms.GroupBox();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.txtProgress = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnUpload = new System.Windows.Forms.Button();
			this.chkDeleteUnexisting = new System.Windows.Forms.CheckBox();
			this.grpInput = new System.Windows.Forms.GroupBox();
			this.grpProgress.SuspendLayout();
			this.grpInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtStartingDirectory
			// 
			this.txtStartingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtStartingDirectory.Location = new System.Drawing.Point(104, 19);
			this.txtStartingDirectory.Name = "txtStartingDirectory";
			this.txtStartingDirectory.Size = new System.Drawing.Size(349, 20);
			this.txtStartingDirectory.TabIndex = 1;
			// 
			// lblStartingDirectory
			// 
			this.lblStartingDirectory.AutoSize = true;
			this.lblStartingDirectory.Location = new System.Drawing.Point(6, 22);
			this.lblStartingDirectory.Name = "lblStartingDirectory";
			this.lblStartingDirectory.Size = new System.Drawing.Size(92, 13);
			this.lblStartingDirectory.TabIndex = 2;
			this.lblStartingDirectory.Text = "Website directory:";
			// 
			// grpProgress
			// 
			this.grpProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpProgress.Controls.Add(this.pbProgress);
			this.grpProgress.Controls.Add(this.txtProgress);
			this.grpProgress.Location = new System.Drawing.Point(12, 95);
			this.grpProgress.Name = "grpProgress";
			this.grpProgress.Size = new System.Drawing.Size(549, 228);
			this.grpProgress.TabIndex = 5;
			this.grpProgress.TabStop = false;
			this.grpProgress.Text = "Vooruitgang";
			// 
			// pbProgress
			// 
			this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgress.Location = new System.Drawing.Point(6, 19);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(537, 23);
			this.pbProgress.TabIndex = 6;
			// 
			// txtProgress
			// 
			this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProgress.Location = new System.Drawing.Point(6, 48);
			this.txtProgress.Multiline = true;
			this.txtProgress.Name = "txtProgress";
			this.txtProgress.ReadOnly = true;
			this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtProgress.Size = new System.Drawing.Size(535, 174);
			this.txtProgress.TabIndex = 5;
			this.txtProgress.WordWrap = false;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(459, 17);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(82, 23);
			this.btnBrowse.TabIndex = 6;
			this.btnBrowse.Text = "Bladeren";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnUpload
			// 
			this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpload.Location = new System.Drawing.Point(9, 45);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new System.Drawing.Size(339, 23);
			this.btnUpload.TabIndex = 7;
			this.btnUpload.Text = "Genereer xml file en upload gallerijen";
			this.btnUpload.UseVisualStyleBackColor = true;
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			// 
			// chkDeleteUnexisting
			// 
			this.chkDeleteUnexisting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkDeleteUnexisting.AutoSize = true;
			this.chkDeleteUnexisting.Checked = true;
			this.chkDeleteUnexisting.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDeleteUnexisting.Location = new System.Drawing.Point(352, 49);
			this.chkDeleteUnexisting.Name = "chkDeleteUnexisting";
			this.chkDeleteUnexisting.Size = new System.Drawing.Size(191, 17);
			this.chkDeleteUnexisting.TabIndex = 8;
			this.chkDeleteUnexisting.Text = "Onbestaande gallerijen verwijderen";
			this.chkDeleteUnexisting.UseVisualStyleBackColor = true;
			// 
			// grpInput
			// 
			this.grpInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpInput.Controls.Add(this.lblStartingDirectory);
			this.grpInput.Controls.Add(this.chkDeleteUnexisting);
			this.grpInput.Controls.Add(this.btnUpload);
			this.grpInput.Controls.Add(this.txtStartingDirectory);
			this.grpInput.Controls.Add(this.btnBrowse);
			this.grpInput.Location = new System.Drawing.Point(12, 12);
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size(549, 77);
			this.grpInput.TabIndex = 9;
			this.grpInput.TabStop = false;
			this.grpInput.Text = "Opdracht";
			// 
			// GalleryUploaderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(576, 335);
			this.Controls.Add(this.grpInput);
			this.Controls.Add(this.grpProgress);
			this.Name = "GalleryUploaderForm";
			this.Text = "Upload gallerijen";
			this.grpProgress.ResumeLayout(false);
			this.grpProgress.PerformLayout();
			this.grpInput.ResumeLayout(false);
			this.grpInput.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtStartingDirectory;
		private System.Windows.Forms.Label lblStartingDirectory;
		private System.Windows.Forms.GroupBox grpProgress;
		private System.Windows.Forms.TextBox txtProgress;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.CheckBox chkDeleteUnexisting;
		private System.Windows.Forms.GroupBox grpInput;
		private System.Windows.Forms.ProgressBar pbProgress;
	}
}

