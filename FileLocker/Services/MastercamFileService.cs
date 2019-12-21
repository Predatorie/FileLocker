using System;
using System.IO;

using Mastercam.IO;
using Mastercam.IO.Types;

namespace FileLocker.Services
{
    class MastercamFileService
    {
        public void LockFile(bool displayDialog = true)
        {
            File.SetAttributes(FileManager.CurrentFileName, FileAttributes.ReadOnly);

            EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                                  FileManager.CurrentFileName,
                                  "File Locked - Read-only attribute applied");

            if (displayDialog)
            {
                DialogManager.OK($"Read-only attribute applied to {Environment.NewLine}" +
                                 $"{FileManager.CurrentFileName}",
                                 "File Locked!");
            }

        }

        public void UnlockFile(bool displayDialog = true)
        {
            File.SetAttributes(FileManager.CurrentFileName,
                               File.GetAttributes(FileManager.CurrentFileName) & ~FileAttributes.ReadOnly);

            EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                                  FileManager.CurrentFileName,
                                  "File Unlocked - Read-only attribute removed");
            if (displayDialog)
            {
                DialogManager.OK($"Read-only attribute removed from {Environment.NewLine}" +
                                 $"{FileManager.CurrentFileName}",
                                 "File Unlocked!");
            }

        }

        public void SaveFile()
        {
            if (IsFileOnDisk())
            {
                UnlockFile(false);   
            }

            if (FileManager.Save(true))
            {
                EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                                      FileManager.CurrentFileName,
                                      "File Saved");

                LockFile(false);
            }
            else
            {
                return;
            }
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
