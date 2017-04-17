using FileManagementConsole.FileManagement;
using System;
using System.Threading;

namespace FileManagementConsole.Operation
{
    public class FileOperation
    {
        private FileManagementMode _operationMode;
        private string _operationSource;
        private string _operationDestination;
        private FileOperationState _state;

        public FileOperation(FileManagementMode mode, string source, string destination)
        {
            this._operationMode = mode;
            this._operationSource = source;
            this._operationDestination = destination;
            this._state = FileOperationState.INITIAL;
        }

        public void Start()
        {
            var operationThread = new Thread(StartOperation);
            operationThread.Start();
        }

        public void Cancel()
        {
            if (this._state == FileOperationState.CANCELED)
            {
                return;
            }

            this._state = FileOperationState.CANCELED;
        }

        #region [Private methods]

        private void StartOperation()
        {
            switch (this._operationMode)
            {
                case FileManagementMode.COPY:
                    this._state = FileOperationState.RUNNING;
                    FileManagementWrapper.CopyFile(this._operationSource, this._operationDestination, new FileManagementInterop.ProgressCallback(CopingProgressCallback));
                    break;
                case FileManagementMode.MOVE:
                    this._state = FileOperationState.RUNNING;
                    FileManagementWrapper.MoveFile(this._operationSource, this._operationDestination, new FileManagementInterop.ProgressCallback(MovingProgressCallback));
                    break;
                default:
                    break;
            }
        }

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
            switch (this._state)
            {
                case FileOperationState.CANCELED:
                    return ProgressResult.PROGRESS_CANCEL;
                default:
                    return ProgressResult.PROGRESS_CONTINUE;
            }
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
