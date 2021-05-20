////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	ApplicationStrings.cs
//
// summary:	Implements the application strings class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace FileLocker
{
    /// <summary> An application strings. </summary>
    public class ApplicationStrings
    {
        /// <summary> The title. </summary>
        public static string Title = "Lock File";

        /// <summary> The lock file automatic save enabled. </summary>
        public static string LockFileAutoSaveEnabled = "Lock File Auto Save enabled";

        /// <summary> The lock file automatic save disabled. </summary>
        public static string LockFileAutoSaveDisabled = "Lock File Auto Save disabled";

        /// <summary> The lock file automatic saving. </summary>
        public static string LockFileAutoSaving = "Lock File Auto Save...";

        /// <summary> The file saved. </summary>
        public static string FileSaved = "File Saved";

        /// <summary> The file not saved. </summary>
        public static string FileNotSaved = "File Not Saved";

        /// <summary> The failed to save file. </summary>
        public static string FailedToSaveFile = "Failed to save file";
        
        /// <summary> The file locked. </summary>
        public static string FileLocked = "File Locked - Read-only attribute applied";

        /// <summary> The file locked. </summary>
        public static string FileUnLocked = "File Unlocked - Read-only attribute removed";

        /// <summary> The automatic save is enabled. </summary>
        public static string AutoSaveIsEnabled = "Auto Save is already enabled.";
    }
}