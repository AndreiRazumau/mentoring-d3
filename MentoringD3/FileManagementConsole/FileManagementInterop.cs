using FileManagementConsole.Flags;
using System;
using System.Runtime.InteropServices;

namespace FileManagementConsole
{
    public class FileManagementInterop
    {
        public delegate ProgressResult ProgressCallback(long totalSize,
                                                        long totalBytesTransferred,
                                                        long streamSize,
                                                        long streamBytesTransferred,
                                                        uint dwStreamNumber,
                                                        uint dwCallbackReason,
                                                        IntPtr hSourceFile,
                                                        IntPtr hDestinationFile,
                                                        IntPtr lpData);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static bool CopyFile(string source,
                                           string destination,
                                           bool failIfExists = false);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static bool CopyFileEx(string source,
                                             string destination,
                                             ProgressCallback callback,
                                             IntPtr data,
                                             bool cancel,
                                             CopyFileFlags flags);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public extern static bool MoveFileWithProgress(string source,
                                                       string destination,
                                                       ProgressCallback callback,
                                                       IntPtr data,
                                                       MoveFileFlags flags);
    }
}
