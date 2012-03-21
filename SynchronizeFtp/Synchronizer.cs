using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace SynchronizeFtp
{
	public delegate void ProgressEventHandler(string msg, int currentStep, int totalSteps);

	public class Synchronizer
	{
		#region Events
		/// <summary>
		/// Reports the status of the synchronization process.
		/// Also reports the current and total amount of files to synchronize per folder.
		/// </summary>
		public event ProgressEventHandler Progress;

		public void OnProgress(string msg, int currentStep, int totalSteps)
		{
			if (Progress != null)
			{
				Progress(msg, currentStep, totalSteps);
			}
		}
		int _currentFile = 0;
		int _totalFiles = 0;
		#endregion

		#region Properties
		/// <summary>
		/// Starting with ftp://
		/// </summary>
		public string FtpUrl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		#endregion

		#region Constructor
		/// <param name="ftpUrl">Starting with ftp://</param>
		public Synchronizer(string ftpUrl, string username, string password)
		{
			FtpUrl = ftpUrl;
			Username = username;
			Password = password;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Copies all files from the local <code>sourceFolder</code> to the <code>destinationFolder</code> on the ftp server.
		/// </summary>
		/// <param name="sourceFolder">The complete path of the sourcefolder.</param>
		/// <param name="destinationFolder">The complete path of the destination folder.</param>
		public void SynchronizeFolders(string sourceFolder, string destinationFolder)
		{
			SynchronizeFolders(sourceFolder, destinationFolder, true, true, true, false);
		}

		/// <summary>
		/// Copies all files from the local <code>sourceFolder</code> to the <code>destinationFolder</code> on the ftp server.
		/// </summary>
		/// <param name="sourceFolder">The complete path of the sourcefolder.</param>
		/// <param name="destinationFolder">The complete path of the destination folder.</param>
		/// <param name="deleteMissingFiles">Delete files in the ftp folder that do not exist on the source folder. Default true.</param>
		public void SynchronizeFolders(string sourceFolder, string destinationFolder, bool deleteMissingFiles)
		{
			SynchronizeFolders(sourceFolder, destinationFolder, deleteMissingFiles, true, true, false);
		}

		/// <summary>
		/// Copies all files from the local <code>sourceFolder</code> to the <code>destinationFolder</code> on the ftp server.
		/// </summary>
		/// <param name="sourceFolder">The complete path of the sourcefolder.</param>
		/// <param name="destinationFolder">The complete path of the destination folder.</param>
		/// <param name="deleteMissingFiles">Delete files in the ftp folder that do not exist on the source folder. Default true.</param>
		/// <param name="recursive">Also synchronize subdirectories. Default true.</param>
		/// <param name="extensionsToLowercase">Converts all file extensions to lower case. Default true.</param>
		/// <param name="ignoreFilesStartingWithDot">Ignore files starting with a . Default is false (allow files starting with .)</param>
		/// <param name="fileTypes">A list of all file extentions (lowercase, including dot, for example ".jpg") to synchronize. Leave blank for all filetypes.</param>
		public void SynchronizeFolders(string sourceFolder, string destinationFolder, bool deleteMissingFiles, bool recursive, bool extensionsToLowercase, bool ignoreFilesStartingWithDot, params string[] fileTypes)
		{
			_currentFile = 0;
			_totalFiles = 0;

			if (!Directory.Exists(sourceFolder))
			{
				OnProgress("Source folder does not exist: " + sourceFolder + ".", _currentFile, _totalFiles);
				return;
			}
			else
			{
				_totalFiles = Directory.GetFiles(sourceFolder).Length;
			}

			OnProgress("SYNCHRONIZING FOLDER: " + destinationFolder + ".", _currentFile, _totalFiles);

			// Try to create the directory.
			CreateDirectoryIfNotExists(destinationFolder);

			// Iterate through all files in the sourceFolder
			OnProgress("Checking for new files...", _currentFile, _totalFiles);
			foreach (string file in Directory.GetFiles(sourceFolder))
			{
				_currentFile++;

				// Set extension to lowercase
				string sourceFile = file;
				if (extensionsToLowercase)
					sourceFile = Path.ChangeExtension(file, Path.GetExtension(file).ToLower());

				// Upload the file if it does not exists or is newer than the one in the destination folder
				if (fileTypes.Length == 0 || fileTypes.Contains(Path.GetExtension(sourceFile).ToLower()))
					if (!ignoreFilesStartingWithDot || !Path.GetFileName(sourceFile).StartsWith("."))
						UploadFile(sourceFile, destinationFolder + "/" + Path.GetFileName(sourceFile));
			}
			
			if (deleteMissingFiles)
			{
				OnProgress("Checking for missing files...", _currentFile, _totalFiles);
				// Delete files from the destinationfolder that don't exist on the source folder
				foreach (string filename in GetFilenames(destinationFolder))
				{
					if (!File.Exists(Path.Combine(sourceFolder, filename)))
						DeleteFile(destinationFolder + "/" + filename);
				}

				OnProgress("Checking for missing folders...", _currentFile, _totalFiles);
				// Delete folders from the destinationfolder that don't exist on the source folder
				foreach (string foldername in GetFoldernames(destinationFolder))
				{
					if (!Directory.Exists(Path.Combine(sourceFolder, foldername)))
						DeleteFolderRecursively(destinationFolder + "/" + foldername);
				}
			}

			// If recursive: Iterate through all folders in the source folder and call method recursively.
			if (recursive)
				foreach (string subFolder in Directory.GetDirectories(sourceFolder))
					SynchronizeFolders(subFolder, destinationFolder + "/" + Path.GetFileName(subFolder), deleteMissingFiles, recursive, extensionsToLowercase, ignoreFilesStartingWithDot, fileTypes);

			OnProgress("FOLDER SYNCHRONIZED: " + destinationFolder + ".", _currentFile, _totalFiles);
		}
		#endregion

		#region Private Methods
		private void CreateDirectoryIfNotExists(string destinationFolder)
		{
			FtpWebRequest request = SetupFtpWebRequest(destinationFolder, WebRequestMethods.Ftp.MakeDirectory);

			try
			{
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				OnProgress("Folder " + destinationFolder + " did not yet exist, created.", _currentFile, _totalFiles);
			}
			catch (WebException ex)
			{
				FtpWebResponse response = (FtpWebResponse)ex.Response;
				if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
				{
					// Already exists, ignore...
				}
			}
		}

		/// <summary>
		/// Uploads a file if it does not yet exist, or if it is newer than the <code>destinationFile</code>.
		/// </summary>
		private void UploadFile(string sourceFile, string destinationFile)
		{
			bool write = false;
			DateTime destinationFileLastModified = DateTime.Today;
			DateTime sourceFileLastModified = DateTime.Today;
			long destinationSize = 0;
			long sourceSize = 0;

			// Get the DateTimeStamp of the destinationFile.
			FtpWebRequest request = SetupFtpWebRequest(destinationFile, WebRequestMethods.Ftp.GetDateTimestamp);
			try
			{
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				destinationFileLastModified = response.LastModified;
			}
			catch (WebException ex)
			{
				FtpWebResponse response = (FtpWebResponse)ex.Response;
				// File does not exist: create
				if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
				{
					OnProgress("File " + destinationFile + " does not yet exist. Adding...", _currentFile, _totalFiles);
					write = true;
				}
			}

			if (!write)
			{
				// Get the DateTimeStamp of the sourceFile.
				FileInfo fi = new FileInfo(sourceFile);
				sourceFileLastModified = fi.LastWriteTime;

				if (sourceFileLastModified > destinationFileLastModified)
				{
					OnProgress("File " + destinationFile + " already exists, but newer version found... Replacing...", _currentFile, _totalFiles);
					write = true;
				}
				else
				{
					// Compare size
					// Destination file
					request = SetupFtpWebRequest(destinationFile, WebRequestMethods.Ftp.GetFileSize);
					FtpWebResponse response = (FtpWebResponse)request.GetResponse();
					destinationSize = response.ContentLength;

					// Source file
					sourceSize = fi.Length;

					if (sourceSize != destinationSize)
					{
						OnProgress("File " + destinationFile + " already exists, but of different size... Replacing...", _currentFile, _totalFiles);
						write = true;
					}
				}
			}

			if (write)
			{
				request = SetupFtpWebRequest(destinationFile, WebRequestMethods.Ftp.UploadFile);

				FileStream stream = File.OpenRead(sourceFile);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				stream.Close();

				Stream reqStream = request.GetRequestStream();
				try
				{
					reqStream.Write(buffer, 0, buffer.Length);
				}
				finally
				{
					reqStream.Close();
				}
			}
			else
			{
				OnProgress("File " + destinationFile + " already exists. Skipping...", _currentFile, _totalFiles);
			}
		}

		/// <summary>
		/// Returns list of all filenames in the <source>destinationPath</source>.
		/// </summary>
		/// <param name="destinationFolder">The folder to check</param>
		/// <param name="extentions">A list of all fileTypes to check. When none are specified, all files are returned.</param>
		private List<string> GetFilenames(string destinationFolder, params string[] fileTypes)
		{
			if (!destinationFolder.EndsWith("/")) destinationFolder += "/";
			List<string> result = new List<string>();
			FtpWebRequest request = SetupFtpWebRequest(destinationFolder, WebRequestMethods.Ftp.ListDirectory);

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				string line = reader.ReadLine();
				while (line != null)
				{
					if (line != "." && line != ".." && Path.HasExtension(line))
						if (fileTypes.Length == 0 || fileTypes.Contains(Path.GetExtension(line).ToLower()))
							result.Add(line);
					line = reader.ReadLine();
				}
			}
			return result;
		}

		/// <summary>
		/// Returns list of all foldernames in the <source>destinationPath</source>.
		/// </summary>
		/// <param name="destinationFolder">The folder to check</param>
		private List<string> GetFoldernames(string destinationFolder)
		{
			if (!destinationFolder.EndsWith("/")) destinationFolder += "/";
			List<string> result = new List<string>();
			FtpWebRequest request = SetupFtpWebRequest(destinationFolder, WebRequestMethods.Ftp.ListDirectory);

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				string line = reader.ReadLine();
				while (line != null)
				{
					if (line != "." && line != ".." && !Path.HasExtension(line))
						result.Add(line);
					line = reader.ReadLine();
				}
			}
			return result;
		}

		private void DeleteFile(string destinationFile)
		{
			OnProgress("Deleting file: " + destinationFile + ".", _currentFile, _totalFiles);
			FtpWebRequest request = SetupFtpWebRequest(destinationFile, WebRequestMethods.Ftp.DeleteFile);
			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
		}

		/// <summary>
		/// Deletes the <code>destinationFolder</code> and all subfolders.
		/// </summary>
		private void DeleteFolderRecursively(string destinationFolder)
		{
			// First, call this method for every subfolder.
			foreach (string subfolder in GetFoldernames(destinationFolder))
				DeleteFolderRecursively(destinationFolder + "/" + subfolder);

			// No more subfolders in this folder: Delete all the files in the folder.
			foreach (string file in GetFilenames(destinationFolder))
				DeleteFile(destinationFolder + "/" + file);

			// Lastly, delete this folder - it is empty.
			OnProgress("Deleting folder: " + destinationFolder + ".", _currentFile, _totalFiles);
			FtpWebRequest request = SetupFtpWebRequest(destinationFolder, WebRequestMethods.Ftp.RemoveDirectory);
			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
		}

		/// <summary>
		/// Create a ready-made <code>FtpWebRequest</code> instance configured for the specified action.
		/// </summary>
		/// <param name="destination">The full path of the file or folder.</param>
		/// <param name="method">A string from the<code>WebRequestMethods.Ftp</code> class.</param>
		private FtpWebRequest SetupFtpWebRequest(string destination, string method)
		{
			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(destination);
			request.Method = method;
			request.Credentials = new NetworkCredential(Username, Password);
			request.UseBinary = true;
			return request;
		}
		#endregion
	}
}
