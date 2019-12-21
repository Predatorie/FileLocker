using System;
using System.IO;

using Mastercam.IO;
using Mastercam.IO.Types;

namespace FileLocker.Services
{
    class MastercamFileService
    {
        public void LockFile()
        {
            File.SetAttributes(FileManager.CurrentFileName, FileAttributes.ReadOnly);

            EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                                  FileManager.CurrentFileName,
                                  "File Locked - Read-only attribute applied");

            DialogManager.OK($"Read-only attribute applied to {Environment.NewLine}" +
                             $"{FileManager.CurrentFileName}",
                              "File Locked!");
        }

        public void UnlockFile()
        {
            File.SetAttributes(FileManager.CurrentFileName,
                               File.GetAttributes(FileManager.CurrentFileName) & ~FileAttributes.ReadOnly);

            EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                                 FileManager.CurrentFileName,
                                 "File Unlocked - Read-only attribute removed");

            DialogManager.OK($"Read-only attribute removed from {Environment.NewLine}" +
                             $"{FileManager.CurrentFileName}",
                              "File Unlocked!");
        }

        public bool IsFileOnDisk()
        {
            return File.Exists(FileManager.CurrentFileName);
        }

        public void GenerateFileError()
        {
            DialogManager.Error($"The current file doesn't exist {Environment.NewLine}" +
                                $"{FileManager.CurrentFileName}",
                                "File Not Saved!");
        }

    }
}
