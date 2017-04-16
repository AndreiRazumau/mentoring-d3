using FileManagementConsole.Flags;
using System;

namespace FileManagementConsole
{
    public class FileManagementWrapper
    {
        #region [Static methods]

        public static void CopyFile(string copyFrom, string copyTo)
        {
            FileManagementInterop.CopyFileEx(copyFrom,
                                             copyTo,
                                             new FileManagementInterop.ProgressCallback(CopingPrrogressCallback),
                                             IntPtr.Zero,
                                             false,
                                             CopyFileFlags.COPY_FILE_RESTARTABLE);
        }

        public static void MoveFile(string fileName, string destinationPath)
        {
            FileManagementInterop.MoveFileWithProgress(fileName,
                                                       destinationPath,
                                                       new FileManagementInterop.ProgressCallback(MovingProgressCallback),
                                                       IntPtr.Zero,
                                                       MoveFileFlags.MOVE_FILE_REPLACE_EXISTSING | MoveFileFlags.MOVE_FILE_WRITE_THROUGH | MoveFileFlags.MOVE_FILE_COPY_ALLOWED);
        }

        #endregion

        #region [Private methods]

        private static ProgressResult CopingPrrogressCallback(long totalSize,
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
            return ProgressResult.PROGRESS_CONTINUE;
        }

        private static ProgressResult MovingProgressCallback(long totalSize,
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
