// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="CNC Software, Inc.">
//   Copyright (c) 2019 CNC Software, Inc.
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileLocker
{
    using Mastercam.App;
    using Mastercam.App.Types;

    using FileLocker.Services;

    public class Main : NetHook3App
    {
        #region Public Override Methods

        /// <summary> The main entry point for your FileLocker. </summary>
        ///
        /// <param name="param"> System parameter. </param>
        ///
        /// <returns> A <c>MCamReturn</c> return type representing the outcome of your NetHook application. </returns>
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

        /// <summary> The custom user function entry point for your FileLocker. </summary>
        ///
        /// <param name="param"> System parameter. </param>
        ///
        /// <returns> A <c>MCamReturn</c> return type representing the outcome of your NetHook application. </returns>
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

        #endregion

    }
}
