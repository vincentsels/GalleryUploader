using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalleryUploader.Properties;
using System.Xml;
using System.IO;
using System.Net;
using System.Windows.Forms;
using SynchronizeFtp;
using System.ComponentModel;

namespace GalleryUploader
{
	public delegate void ProgressEventHandler(string msg, int currentStep, int totalSteps);

	public class GalleryUploader
	{
		#region Events
		public event ProgressEventHandler Progress;

		public void OnProgress(string msg, int currentStep, int totalSteps)
		{
			if (Progress != null)
			{
				Progress(msg, currentStep, totalSteps);
			}
		}
		#endregion

		#region Properties
		private string _currentDirectoryName;
		public string CurrentDirectoryName 
		{ 
			get 
			{ 
				return _currentDirectoryName; 
			}
			set
			{
				_currentDirectoryName = value;
				InitializeDirectories();
			}
		}

		string SourcePicturesDirectoryName { get; set; }
		string SourceIntroPictureGalleryName { get; set; }
		string SourceIntroPictureFileName { get; set; }
		string SourceXmlDirectoryName { get; set; }
		string SourceXmlFileName { get; set; }

		string FtpUrl { get; set; }
		string FtpUserName { get; set; }
		string FtpPassword { get; set; }

		string DestinationWebsiteSubFolderName { get; set; }
		string DestinationThumbsDirectoryName { get; set; }
		string DestinationPicturesDirectoryName { get; set; }
		string DestinationIntroPictureGalleryName { get; set; }
		string DestinationIntroPictureFileName { get; set; }
		string DestinationXmlDirectoryName { get; set; }
		string DestinationXmlFileName { get; set; }
		#endregion

		#region Constructor
		public GalleryUploader()
		{
			FtpUrl = Settings.Default.FtpUrl;
			FtpUserName = Settings.Default.FtpUserName;
			FtpPassword = Settings.Default.FtpPassword;

			CurrentDirectoryName = Settings.Default.DefaultStartupDirectory;
		}

		private void InitializeDirectories()
		{
			SourcePicturesDirectoryName = Path.Combine(CurrentDirectoryName, Settings.Default.PicturesDirectoryName);
			SourceIntroPictureGalleryName = Path.Combine(SourcePicturesDirectoryName, Settings.Default.IntroPictureGalleryName);
			SourceIntroPictureFileName = Path.Combine(SourceIntroPictureGalleryName, Settings.Default.IntroPictureFileName);
			SourceXmlDirectoryName = Path.Combine(CurrentDirectoryName, Settings.Default.XmlDirectoryName);
			SourceXmlFileName = Path.Combine(SourceXmlDirectoryName, Settings.Default.XmlFileName);

			DestinationWebsiteSubFolderName = FtpUrl + "/" + Settings.Default.WebsiteSubfolderName;
			DestinationThumbsDirectoryName = DestinationWebsiteSubFolderName + "/" + Settings.Default.ThumbsDirectoryName;
			DestinationPicturesDirectoryName = DestinationWebsiteSubFolderName + "/" + Settings.Default.PicturesDirectoryName;
			DestinationIntroPictureGalleryName = DestinationPicturesDirectoryName + "/" + Settings.Default.IntroPictureGalleryName;
			DestinationIntroPictureFileName = DestinationIntroPictureGalleryName + "/" + Settings.Default.IntroPictureFileName;
			DestinationXmlDirectoryName = DestinationWebsiteSubFolderName + "/" + Settings.Default.XmlDirectoryName;
			DestinationXmlFileName = DestinationXmlDirectoryName + "/" + Settings.Default.XmlFileName;
		}
		#endregion

		#region Public Methods
		public void CreateXmlFile()
		{
			OnProgress("Xml File creëren...", 0, 0);

			XmlTextWriter tw = new XmlTextWriter(SourceXmlFileName, null);

			tw.WriteStartDocument();

			tw.WriteStartElement("list");

			foreach (string gallery in Directory.GetDirectories(SourcePicturesDirectoryName))
			{
				// Intro gallery niet mee opnemen
				if (gallery.Equals(SourceIntroPictureGalleryName)) continue;
				if (Path.GetFileName(gallery).StartsWith(".")) continue;

				OnProgress("Gallerij opnemen: " + Path.GetFileName(gallery), 0, 0);

				tw.WriteStartElement("gallery");
				tw.WriteAttributeString("name", Path.GetFileName(gallery));

				// Alfabetisch sorteren
				List<String> images = new List<String>(Directory.GetFiles(gallery));
				images.Sort();
				foreach (string image in images)
				{
					if (Path.GetFileName(image).StartsWith(".")) continue;

					OnProgress("Foto opnemen: " + Path.GetFileName(image), 0, 0);

					tw.WriteStartElement("picture");
					tw.WriteAttributeString("name", Path.GetFileName(image));
					tw.WriteEndElement();
				}

				tw.WriteEndElement();
			}

			tw.WriteEndElement();

			tw.WriteEndDocument();

			tw.Close();

			OnProgress("Xml file aangemaakt.", 1, 1);
		}

		public void UploadToFtp(bool deleteUnexistingGalleries)
		{
			OnProgress("Bezig met uploaden...", 0, 0);

			Synchronizer sync = new Synchronizer(FtpUrl, FtpUserName, FtpPassword);
			sync.Progress += new SynchronizeFtp.ProgressEventHandler(_sync_Progress);

			// The galleries
			sync.SynchronizeFolders(SourcePicturesDirectoryName, DestinationPicturesDirectoryName, true, true, true, true, ".jpg");

			// The xml file
			sync.SynchronizeFolders(SourceXmlDirectoryName, DestinationXmlDirectoryName, true, true, true, true, ".xml");

			OnProgress("Upload gedaan.", 1, 1); ;
		}
		#endregion

		#region Event handlers
		void _sync_Progress(string msg, int currentStep, int totalSteps)
		{
			OnProgress(msg, currentStep, totalSteps);
		}
		#endregion
	}
}
