////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Main.cs
//
// summary:	Implements the main class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FileLocker
{
    using System.Timers;

    using Mastercam.App;
    using Mastercam.App.Types;
    using Mastercam.IO;

    using Services;

    using Views;

    /// <summary> A main. </summary>
    public class Main : NetHook3App
    {
        /// <summary> The automatic save timer. </summary>
        private Timer _autoSave;

        #region Public Override Methods

        /// <summary> Overrideable. This method is executed when a NET-Hook add-on is first loaded. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn return type code. </returns>
        public override MCamReturn Init(int param)
        {
            // define a new timer instance
            this._autoSave = new Timer();

            // subscribe to the elapsed event
            this._autoSave.Elapsed += OnAutoSave;

            return MCamReturn.NoErrors;
        }

        /// <summary> Overrideable. This method is executed just before a NET-Hook add-on is to be unloaded, and can be used to
        ///           perform app-specific cleanup. </summary>
        ///
        /// <param name="param"> The parameter (optional). </param>
        ///
        /// <returns> A MCamReturn return type code. </returns>
        public override MCamReturn Close(int param)
        {
            // unsubscribe from the elapsed event
            this._autoSave.Elapsed -= OnAutoSave;

            return MCamReturn.NoErrors;
        }

        /// <summary> Overrideable. This method is executed after a NET-Hook add-on has been loaded and serves as the main
        ///           execution loop for NET-Hook apps. </summary>
        ///
        /// <param name="param"> The parameter (optional). </param>
        ///
        /// <returns> A MCamReturn return type code. </returns>
        public override MCamReturn Run(int param)
        {
            if (MastercamFileService.Instance.IsFileOnDisk())
            {
                var result = MastercamFileService.Instance.LockFile();
                if (result.IsFailure)
                {
                    DialogManager.Error(result.Error, ApplicationStrings.Title);
                    MastercamFileService.Instance.LogError(result.Error);

                    return MCamReturn.ErrorOccurred;
                }
            }
            else
            {
                MastercamFileService.Instance.GenerateFileError();
            }

            return MCamReturn.NoErrors;
        }

        #endregion

        #region Public User Defined Methods

        /// <summary> Automatic save options. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn. </returns>
        public MCamReturn AutoSaveOptions(int param)
        {
            using (var view = new AutoSaveView())
            {
                view.ShowDialog();
            }

            return MCamReturn.NoErrors;
        }

        /// <summary> Automatic save on. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn. </returns>
        public MCamReturn AutoSaveOn(int param)
        {
            try
            {
                if (this._autoSave.Enabled)
                {
                    DialogManager.OK(ApplicationStrings.AutoSaveIsEnabled, ApplicationStrings.Title);
                    return MCamReturn.NoErrors;
                }

                PromptManager.WriteString(ApplicationStrings.LockFileAutoSaveEnabled);

                this._autoSave.Interval = Properties.Settings.Default.Interval * 60000;
                this._autoSave.Enabled = true;
                this._autoSave.Start();

                MastercamFileService.Instance.LogInformation(ApplicationStrings.LockFileAutoSaveEnabled);

                DialogManager.OK(ApplicationStrings.LockFileAutoSaveEnabled, ApplicationStrings.Title);
            }
            finally
            {
                PromptManager.Clear();
            }

            return MCamReturn.NoErrors;
        }

        /// <summary> Automatic save off. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn. </returns>
        public MCamReturn AutoSaveOff(int param)
        {
            try
            {
                if (!this._autoSave.Enabled)
                {
                    return MCamReturn.NoErrors;
                }

                PromptManager.WriteString(ApplicationStrings.LockFileAutoSaveDisabled);

                this._autoSave.Stop();
                this._autoSave.Enabled = false;

                MastercamFileService.Instance.LogInformation(ApplicationStrings.LockFileAutoSaveDisabled);

                DialogManager.OK(ApplicationStrings.LockFileAutoSaveDisabled, ApplicationStrings.Title);
            }
            finally
            {
                PromptManager.Clear();
            }

            return MCamReturn.NoErrors;
        }

        /// <summary> Executes the unlock operation. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn. </returns>
        public MCamReturn RunUnlock(int param)
        {
            if (MastercamFileService.Instance.IsFileOnDisk())
            {
                var result = MastercamFileService.Instance.UnlockFile();
                if (result.IsFailure)
                {
                    DialogManager.Error(result.Error, ApplicationStrings.Title);
                    MastercamFileService.Instance.LogError(result.Error);

                    return MCamReturn.ErrorOccurred;
                }
            }
            else
            {
                MastercamFileService.Instance.GenerateFileError();
            }

            return MCamReturn.NoErrors;
        }

        /// <summary> Executes the save operation. </summary>
        ///
        /// <param name="param"> The parameter (optional) </param>
        ///
        /// <returns> A MCamReturn. </returns>
        public MCamReturn RunSave(int param)
        {
            var result = MastercamFileService.Instance.SaveFile();
            if (result.IsFailure)
            {
                DialogManager.Error(result.Error, ApplicationStrings.Title);
                MastercamFileService.Instance.LogError(result.Error);

                return MCamReturn.ErrorOccurred;
            }

            return MCamReturn.NoErrors;
        }

        #endregion

        #region Private Methods

        /// <summary> Raises the elapsed event. </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnAutoSave(object sender, ElapsedEventArgs e)
        {
            try
            {
                // PromptManager.WriteString(ApplicationStrings.LockFileAutoSaving);
                RunSave(0);
            }
            catch (Exception ex)
            {
                DialogManager.Error(ex.Message, ApplicationStrings.Title);
            }
            finally
            {
               // PromptManager.Clear();
            }
        }

        #endregion
    }
}
