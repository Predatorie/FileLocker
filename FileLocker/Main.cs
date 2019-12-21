namespace FileLocker
{
    using Mastercam.App;
    using Mastercam.App.Types;

    using FileLocker.Services;

    public class Main : NetHook3App
    {
        #region Public Override Methods

        public override MCamReturn Run(int param)
        {
            var fileLocker = new MastercamFileService();

            if (fileLocker.IsFileOnDisk())
            {
                fileLocker.LockFile();
            }
            else
            {
                fileLocker.GenerateFileError();
            }

            return MCamReturn.NoErrors;
        }

        #endregion

        #region Public User Defined Methods

        public MCamReturn RunUnlock(int param)
        {
            var fileLocker = new MastercamFileService();

            if (fileLocker.IsFileOnDisk())
            {
                fileLocker.UnlockFile();
            }
            else
            {
                fileLocker.GenerateFileError();
            }

            return MCamReturn.NoErrors;
        }

        public MCamReturn RunSave(int param)
        {
            var fileLocker = new MastercamFileService();

            if (fileLocker.IsFileOnDisk())
            {
                fileLocker.SaveFile();
            }
            else
            {
                fileLocker.GenerateFileError();
            }

            return MCamReturn.NoErrors;
        }

        #endregion

    }
}
