namespace FileManagementConsole
{
    public class Operation
    {
        private FileManagementMode _operationMode;
        private string _operationSource;
        private string _operationDestination;
        private FileManagementWrapper _fileManagement;

        public Operation(FileManagementMode mode, string source, string destination)
        {
            this._operationMode = mode;
            this._operationSource = source;
            this._operationDestination = destination;
            this._fileManagement = new FileManagementWrapper();
        }

        public void Start()
        {
            switch (this._operationMode)
            {
                case FileManagementMode.COPY:
                    this._fileManagement.CopyFile(this._operationSource, this._operationDestination);
                    break;
                case FileManagementMode.MOVE:
                    this._fileManagement.MoveFile(this._operationSource, this._operationDestination);
                    break;
                default:
                    break;
            }
        }

        public void Resume()
        {
            switch (this._operationMode)
            {
                case FileManagementMode.COPY:
                    this._fileManagement.CopyFile(this._operationSource, this._operationDestination);
                    break;
                case FileManagementMode.MOVE:
                    this._fileManagement.MoveFile(this._operationSource, this._operationDestination);
                    break;
                default:
                    break;
            }
        }
    }
}
