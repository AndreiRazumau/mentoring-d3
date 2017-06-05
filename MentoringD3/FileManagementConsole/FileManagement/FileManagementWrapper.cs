using FileManagementConsole.Flags;
using System;
using System.ComponentModel;

namespace FileManagementConsole.FileManagement
{
    public class FileManagementWrapper
    {
        #region [Static methods]

        public static void CopyFile(string copyFrom, string copyTo, FileManagementInterop.ProgressCallback callback)
        {
            if (!FileManagementInterop.CopyFileEx(copyFrom,
                                                          copyTo,
                                                          callback,
                                                          IntPtr.Zero,
                                                          false,
                                                          CopyFileFlags.COPY_FILE_RESTARTABLE | CopyFileFlags.COPY_FILE_ALLOW_DECRYPTED_DESTINATION))
            {
                throw new Win32Exception();
            }
        }

        public static void MoveFile(string fileName, string destinationPath, FileManagementInterop.ProgressCallback callback)
        {
            if (!FileManagementInterop.MoveFileWithProgress(fileName,
                                                            destinationPath,
                                                            callback,
                                                            IntPtr.Zero,
                                                            MoveFileFlags.MOVE_FILE_REPLACE_EXISTSING | MoveFileFlags.MOVE_FILE_WRITE_THROUGH | MoveFileFlags.MOVE_FILE_COPY_ALLOWED))
            {
                throw new Win32Exception();
            }
        }

        #endregion
    }
}
