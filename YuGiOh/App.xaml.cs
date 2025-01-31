﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.Web;

namespace YuGiOh_DeckBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        #region Constants
        /// <summary>
        /// Uri to the the .txt file contains the last update time on github
        /// </summary>
        private const string lastUpdateUri = "https://github.com//MarkHerdt/YuGiOh-DeckBuilder/releases/download/YGO-DB/Last-Update.txt";
        /// <summary>
        /// Name + file extension of the build .rar file 
        /// </summary>
        public const string RarFile = "YuGiOh-DeckBuilder.rar";
        /// <summary>
        /// Name + file extension of the last update .txt file
        /// </summary>
        public const string LastUpdateFile = "Last-Update.txt";
        #endregion

        #region Properties
        /// <summary>
        /// Indicates if a new app version is available on github
        /// </summary>
        public static bool UpdatesAvailable { get; }
        #endregion
        
        #region Constrcutor
        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += LogException;
            
            UpdatesAvailable = AreUpdatesAvailable().Result;
            FinishUpdate();
        }
        
        /// <summary>
        /// Logs every unhandled <see cref="Exception"/> to the console
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="exceptionEventArgs"><see cref="UnhandledExceptionEventArgs"/></param>
        private static void LogException(object sender, UnhandledExceptionEventArgs exceptionEventArgs)
        {
            var exception = (Exception)exceptionEventArgs.ExceptionObject;
            
            PrintException(exception);
            
            Console.Read();
        }

        /// <summary>
        /// Prints the given <see cref="Exception"/> and every <see cref="Exception.InnerException"/> to the console
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to print to the console</param>
        private static void PrintException(Exception? exception)
        {
            while (true)
            {
                if (exception != null)
                {
                    Console.WriteLine(exception.ToString());

                    exception = exception.InnerException;
                    continue;
                }

                break;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns true, when a new app version is available on github, otherwise false
        /// </summary>
        /// <returns>True, when a new app version is available on github, otherwise false</returns>
        private static async Task<bool> AreUpdatesAvailable()
        {
            var remoteLastUpdate = await WebClient.DownloadStringAsync(lastUpdateUri);

            var folderPath = AppContext.BaseDirectory;
            var filePath = Path.Combine(folderPath, LastUpdateFile);
            var localLastUpdate = await File.ReadAllTextAsync(filePath);
            
            if (DateTime.TryParse(remoteLastUpdate, out var remoteDate) && DateTime.TryParse(localLastUpdate, out var localDate))
            {
                if (remoteDate > localDate)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Replaces the old installer with the newly downloaded one
        /// </summary>
        private static void FinishUpdate()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                var tempFolder = Environment.GetCommandLineArgs()[1];
                
                var tempInstallerDirectory = new DirectoryInfo(Structure.BuildPath(Folder.TempInstaller));
                var installerDirectory = new DirectoryInfo(Structure.BuildPath(Folder.Installer));
            
                tempInstallerDirectory.DeepCopy(installerDirectory);

                var tempDirectoryPath = Path.Combine(AppContext.BaseDirectory, tempFolder);
            
                Directory.Delete(tempDirectoryPath, true);
            }
        }
        #endregion
    }
}
