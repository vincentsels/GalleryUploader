using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GalleryUploader.Properties;
using System.DirectoryServices;

namespace GalleryUploader
{
	public partial class GalleryUploaderForm : Form
	{
		#region Variables
		GalleryUploader gu;
		BackgroundWorker bgWorker;

		delegate void InvokeSetProgress(string msg, int currentStep, int totalSteps);
		delegate void InvokeToggleButtons(bool enable);
		#endregion

		#region Constructor
		public GalleryUploaderForm()
		{
			InitializeComponent();

			gu = new GalleryUploader();
			gu.Progress += new ProgressEventHandler(gu_Progress);
			txtStartingDirectory.Text = gu.CurrentDirectoryName;

			bgWorker = new BackgroundWorker();
			bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
			bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
		}
		#endregion

		#region Event Handlers
		#region Button Events
		private void btnUpload_Click(object sender, EventArgs e)
		{
			gu.CurrentDirectoryName = txtStartingDirectory.Text;
			txtProgress.Clear();
			bgWorker.RunWorkerAsync();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fb = new FolderBrowserDialog();
			try
			{
				fb.SelectedPath = txtStartingDirectory.Text;

				DialogResult dr = fb.ShowDialog();

				if (dr == DialogResult.OK)
					txtStartingDirectory.Text = fb.SelectedPath;
			}
			catch
			{
				//
			}
		}
		#endregion

		#region Backgroundworker
		void bgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ToggleButtons(false);
			gu.CreateXmlFile();
			gu.UploadToFtp(chkDeleteUnexisting.Checked);
		}

		void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ToggleButtons(true);
		}
		#endregion

		#region Custom Events
		void gu_Progress(string msg, int currentStep, int totalSteps)
		{
			SetProgress(msg, currentStep, totalSteps);
		}
		#endregion
		#endregion

		#region UI Methods
		private void ToggleButtons(bool enable)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new InvokeToggleButtons(ToggleButtons), enable);
				return;
			}
			grpInput.Enabled = enable;
		}

		private void SetProgress(string msg, int currentStep, int totalSteps)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new InvokeSetProgress(SetProgress), msg, currentStep, totalSteps);
				return;
			}
			txtProgress.AppendText((txtProgress.Text.Length > 0 ? System.Environment.NewLine : "") + msg);
			pbProgress.Minimum = 0;
			pbProgress.Maximum = totalSteps;
			pbProgress.Value = currentStep;
		}
		#endregion
	}
}
