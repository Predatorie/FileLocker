////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Views\AutoSaveView.cs
//
// summary:	Implements the automatic save view class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace FileLocker.Views
{
    using System;
    using System.Windows.Forms;

    using Properties;

    /// <summary> An automatic save view. </summary>
    public partial class AutoSaveView : Form
    {
        /// <summary> Default constructor. </summary>
        public AutoSaveView()
        {
            InitializeComponent();
        }

        /// <summary> Raises the ok click event. </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnOkClick(object sender, EventArgs e)
        {
            Settings.Default.Interval = (int)this.Interval.Value;
            Settings.Default.Save();

            this.Close();
        }

        /// <summary> Raises the close click event. </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnCloseClick(object sender, EventArgs e) => this.Close();

        /// <summary> Raises the view load event. </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnViewLoad(object sender, EventArgs e) => this.Interval.Value = Settings.Default.Interval;

        /// <summary> Raises the form closing event. </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnViewClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
