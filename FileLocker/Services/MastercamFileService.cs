////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Services\MastercamFileService.cs
//
// summary:	Implements the mastercam file service class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace FileLocker.Services
{
    using System;
    using System.IO;

    using Mastercam.IO;
    using Mastercam.IO.Types;

    using Models;

    /// <summary> A service for accessing mastercam files information. </summary>
    public class MastercamFileService : SingletonBehaviour<MastercamFileService>
    {
        /// <summary> Locks the file. </summary>
        ///
        /// <param name="displayDialog"> (Optional) True to display dialog. </param>
        ///
        /// <returns> A Result. </returns>
        public Result LockFile(bool displayDialog = true)
        {
            try
            {
                var attrs = File.GetAttributes(FileManager.CurrentFileName);
                if (!attrs.HasFlag(FileAttributes.ReadOnly))
                {
                    File.SetAttributes(FileManager.CurrentFileName, FileAttributes.ReadOnly);
                    LogInformation(ApplicationStrings.FileLocked);

                    if (displayDialog)
                    {
                        DialogManager.OK($"Read-only attribute applied to {Environment.NewLine}{FileManager.CurrentFileName}",
                            ApplicationStrings.Title);
                    }
                }

                return Result.Ok();

            }
            catch (FileNotFoundException e)
            {
                return Result.Fail($"FileNotFound Exception: {e.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                return Result.Fail($"Unauthorized Access Exception: {e.Message}");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        /// <summary> Unlocks the file. </summary>
        ///
        /// <param name="displayDialog"> (Optional) True to display dialog. </param>
        ///
        /// <returns> A Result. </returns>
        public Result UnlockFile(bool displayDialog = true)
        {
            try
            {
                var attrs = File.GetAttributes(FileManager.CurrentFileName);
                if (attrs.HasFlag(FileAttributes.ReadOnly))
                {
                    File.SetAttributes(FileManager.CurrentFileName, attrs & ~FileAttributes.ReadOnly);
                    LogInformation(ApplicationStrings.FileUnLocked);

                    if (displayDialog)
                    {
                        DialogManager.OK($"Read-only attribute removed from {Environment.NewLine}{FileManager.CurrentFileName}",
                            ApplicationStrings.Title);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                LogError(e.Message);
                return Result.Fail($"FileNotFound Exception: {e.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                LogError(e.Message);
                return Result.Fail($"Unauthorized Access Exception: {e.Message}");
            }
            catch (Exception e)
            {
                LogError(e.Message);
                return Result.Fail(e.Message);
            }

            return Result.Ok();
        }

        /// <summary> Saves the file. </summary>
        public Result SaveFile()
        {
            try
            {
                if (IsFileOnDisk())
                {
                    var result = UnlockFile(false);
                    if (result.IsFailure)
                    {
                        return Result.Fail(result.Error);
                    }

                    if (FileManager.Save(true))
                    {
                        LogInformation(ApplicationStrings.FileSaved);

                        result = LockFile(false);
                        if (result.IsFailure)
                        {
                            return Result.Fail(result.Error);
                        }
                    }
                    else
                    {
                        return Result.Fail(ApplicationStrings.FailedToSaveFile);
                    }
                }
            }
            catch (Exception e)
            {
                LogError(e.Message);
                return Result.Fail(e.Message);
            }

            return Result.Ok();
        }

        /// <summary> Query if this object is file on disk. </summary>
        ///
        /// <returns> True if file on disk, false if not. </returns>
        public bool IsFileOnDisk()
        {
            return File.Exists(FileManager.CurrentFileName);
        }

        /// <summary> Generates a file error. </summary>
        public void GenerateFileError()
        {
            DialogManager.Error($"The current file doesn't exist {Environment.NewLine}{FileManager.CurrentFileName}",
                                ApplicationStrings.FileNotSaved);
        }

        /// <summary> Logs an error. </summary>
        ///
        /// <param name="error"> The error. </param>
        public void LogError(string error)
        {
            EventManager.LogEvent(MessageSeverityType.ErrorMessage,
                FileManager.CurrentFileName,
                error);
        }

        /// <summary> Logs an information. </summary>
        ///
        /// <param name="info"> The information. </param>
        public void LogInformation(string info)
        {
            EventManager.LogEvent(MessageSeverityType.InformationalMessage,
                FileManager.CurrentFileName,
                info);
        }
    }
}
