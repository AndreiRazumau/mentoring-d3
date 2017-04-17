using FileManagementConsole.Flags;
using System;
using System.ComponentModel;
using System.Threading;

namespace FileManagementConsole
{
    public class FileManagementWrapper
    {
        #region [Construction]

        public FileManagementWrapper()
        {
        }

        #endregion

        #region [Static methods]

        public void CopyFile(string copyFrom, string copyTo)
        {
            var result = FileManagementInterop.CopyFileEx(copyFrom,
                                             copyTo,
                                             new FileManagementInterop.ProgressCallback(CopingProgressCallback),
                                             IntPtr.Zero,
                                             false,
                                             CopyFileFlags.COPY_FILE_RESTARTABLE);
        }

        public void MoveFile(string fileName, string destinationPath)
        {
            if (!FileManagementInterop.MoveFileWithProgress(fileName,
                                                       destinationPath,
                                                       new FileManagementInterop.ProgressCallback(MovingProgressCallback),
                                                       IntPtr.Zero,
                                                       MoveFileFlags.MOVE_FILE_REPLACE_EXISTSING | MoveFileFlags.MOVE_FILE_WRITE_THROUGH | MoveFileFlags.MOVE_FILE_COPY_ALLOWED))
            {
                throw new Win32Exception();
            }
        }

        #endregion

        #region [Private methods]

        private ProgressResult CopingProgressCallback(long totalSize,
                                                      long totalBytesTransferred,
                                                      long streamSize,
                                                      long streamBytesTransferred,
                                                      uint dwStreamNumber,
                                                      uint dwCallbackReason,
                                                      IntPtr hSourceFile,
                                                      IntPtr hDestinationFile,
                                                      IntPtr lpData)
        {
            Console.WriteLine($"Copy in progress: {totalBytesTransferred * 100 / totalSize}% done.");
            Thread.Sleep(300);
            return ProgressResult.PROGRESS_CONTINUE;
        }

        private ProgressResult MovingProgressCallback(long totalSize,
                                                             long totalBytesTransferred,
                                                             long streamSize,
                                                             long streamBytesTransferred,
                                                             uint dwStreamNumber,
                                                             uint dwCallbackReason,
                                                             IntPtr hSourceFile,
                                                             IntPtr hDestinationFile,
                                                             IntPtr lpData)
        {
            Console.WriteLine($"Moving in progress: {totalBytesTransferred * 100 / totalSize}% done.");
            return ProgressResult.PROGRESS_CONTINUE;
        }

        #endregion
    }
}
