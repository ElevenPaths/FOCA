//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Diagnostics;
using MS.WindowsAPICodePack.Internal;
//using System.Windows.Interop;
//using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
    /// <summary>
    /// Represents an instance of the Windows taskbar
    /// </summary>
    public class TaskbarManager
    {

        private IntPtr ownerHandle;

        // Hide the default constructor
        private TaskbarManager()
        {

        }

        // Best practice recommends defining a private object to lock on
        private static Object syncLock = new Object();

        private static volatile TaskbarManager instance;
        /// <summary>
        /// Represents an instance of the Windows Taskbar
        /// </summary>
        public static TaskbarManager Instance
        {
            get
            {
                CoreHelpers.ThrowIfNotWin7();

                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                            instance = new TaskbarManager();
                    }
                }

                return instance;
            }
        }

        // Internal implemenation of ITaskbarList4 interface
        private ITaskbarList4 taskbarList;
        internal ITaskbarList4 TaskbarList
        {
            get
            {
                if (taskbarList == null)
                {
                    // Create a new instance of ITaskbarList3
                    lock (syncLock)
                    {
                        if (taskbarList == null)
                        {
                            taskbarList = (ITaskbarList4)new CTaskbarList();
                            taskbarList.HrInit();
                        }
                    }
                }

                return taskbarList;
            }
        }

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button of the main application window
        /// to show the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
        /// <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
        public void SetProgressValue(int currentValue, int maximumValue)
        {
            CoreHelpers.ThrowIfNotWin7();

            TaskbarList.SetProgressValue(OwnerHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
        }

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on a taskbar button of the main application window.
        /// </summary>
        /// <param name="state">Progress state of the progress button</param>
        public void SetProgressState(TaskbarProgressBarState state)
        {
            CoreHelpers.ThrowIfNotWin7();

            TaskbarList.SetProgressState(OwnerHandle, (TBPFLAG)state);
        }

        /// <summary>
        /// Sets the handle of the window whose taskbar button will be used
        /// to display progress.
        /// </summary>
        internal IntPtr OwnerHandle
        {
            get
            {
                if (ownerHandle == IntPtr.Zero)
                {
                    Process currentProcess = Process.GetCurrentProcess();

                    if (currentProcess != null && currentProcess.MainWindowHandle != IntPtr.Zero)
                        ownerHandle = currentProcess.MainWindowHandle;
                    else
                        throw new InvalidOperationException("A valid active Window is needed to update the Taskbar");
                }

                return ownerHandle;
            }
        }

        /// <summary>
        /// Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get
            {
                // We need Windows 7 onwards ...
                return CoreHelpers.RunningOnWin7;
            }
        }

    }
}
