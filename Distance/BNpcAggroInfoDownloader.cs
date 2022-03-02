﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Dalamud.Logging;
using CheapLoc;

namespace Distance
{
	internal static class BNpcAggroInfoDownloader
	{
		public static async Task<BNpcAggroInfoFile> DownloadUpdatedAggroDataAsync( string filePath )
		{
			//	Don't do anything if we're already running the task.
			if( CurrentDownloadStatus == DownloadStatus.Downloading ) return null;

			BNpcAggroInfoFile downloadedDataFile = new();

			//string url = "https://punishedpineapple.github.io/DalamudPlugins/Distance/Support/AggroDistances.dat";
			string url = "https://raw.githubusercontent.com/PunishedPineapple/PunishedPineapple.github.io/master/DalamudPlugins/Distance/Support/AggroDistances.dat";
			await Task.Run( async () =>
			{
				DownloadStatus status = DownloadStatus.Downloading;
				CurrentDownloadStatus = status;
				try
				{
					string responseBody = await LocalHttpClient.GetStringAsync( url );

					if( downloadedDataFile.ReadFromString( responseBody ) )
					{
						PluginLog.LogInformation( $"Downloaded BNpc aggro range data version {downloadedDataFile.GetFileVersionAsString()} ({downloadedDataFile.FileVersion})" );
						if( downloadedDataFile.FileVersion > BNpcAggroInfo.GetCurrentFileVersion() )
						{
							status = DownloadStatus.FailedFileWrite;
							downloadedDataFile.WriteFile( filePath );
							PluginLog.LogInformation( $"Wrote BNpc aggro range data to disk: Version {downloadedDataFile.GetFileVersionAsString()} ({downloadedDataFile.FileVersion})" );
							status = DownloadStatus.Completed;
						}
						else
						{
							status = DownloadStatus.OutOfDateFile;
							downloadedDataFile = null;
						}
					}
					else
					{
						status = DownloadStatus.FailedFileLoad;
						downloadedDataFile = null;
					}
				}
				catch( HttpRequestException e )
				{
					PluginLog.LogWarning( $"Exception occurred while trying to update aggro distance data: {e}" );
					status = DownloadStatus.FailedDownload;
					downloadedDataFile = null;
				}
				catch( TaskCanceledException )
				{
					PluginLog.LogInformation( "Aggro distance data update http request was canceled." );
					status = DownloadStatus.Canceled;
					downloadedDataFile = null;
				}
				catch( Exception e )
				{
					PluginLog.LogWarning( $"Unkown exception occurred while trying to update aggro distance data: {e}" );
					status = DownloadStatus.FailedDownload;
					downloadedDataFile = null;
				}
				finally
				{
					CurrentDownloadStatus = status;
				}
			} );

			return downloadedDataFile;
		}

		public static string GetDownloadStatusMessage( DownloadStatus status )
		{
			string str = "You shouldn't ever see this!";

			switch( status )
			{
				case DownloadStatus.None:
					str = Loc.Localize( "Download Status Message: None", "Ready" );
					break;

				case DownloadStatus.Downloading:
					str = Loc.Localize( "Download Status Message: Downloading", "Downloading..." );
					break;

				case DownloadStatus.FailedDownload:
					str = Loc.Localize( "Download Status Message: Failed Download", "Download failed!" );
					break;

				case DownloadStatus.FailedFileLoad:
					str = Loc.Localize( "Download Status Message: Failed File Load", "The downloaded file was invalid!" );
					break;

				case DownloadStatus.FailedFileWrite:
					str = Loc.Localize( "Download Status Message: Failed File Write", "The downloaded file could not be saved to disk; any updates will be lost upon reloading." );
					break;

				case DownloadStatus.OutOfDateFile:
					str = Loc.Localize( "Download Status Message: Out of Date File", "The downloaded file was older than the current data, and has been discarded." );
					break;

				case DownloadStatus.Completed:
					str = Loc.Localize( "Download Status Message: Completed", "Update Completed" );
					break;

				case DownloadStatus.Canceled:
					str = Loc.Localize( "Download Status Message: Canceled", "The update operation was canceled!" );
					break;

				default:
					str = "You shouldn't ever see this!";
					break;
			}

			return str;
		}

		public static string GetCurrentDownloadStatusMessage()
		{
			return GetDownloadStatusMessage( CurrentDownloadStatus );
		}

		public static void TryResetStatusMessage()
		{
			if( CurrentDownloadStatus != DownloadStatus.Downloading )
			{
				CurrentDownloadStatus = DownloadStatus.None;
			}
		}

		public static void CancelAllDownloads()
		{
			LocalHttpClient.CancelPendingRequests();
		}

		private static readonly HttpClient LocalHttpClient = new();

		public static DownloadStatus CurrentDownloadStatus { get; private set; } = DownloadStatus.None;

		public enum DownloadStatus
		{
			None,
			Downloading,
			FailedDownload,
			FailedFileLoad,
			FailedFileWrite,
			OutOfDateFile,
			Completed,
			Canceled
		}
	}
}
