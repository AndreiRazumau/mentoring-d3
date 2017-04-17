namespace FileManagementConsole.Operation
{
    public enum FileOperationState : int
    {
        INITIAL = 0,
        RUNNING = 1,
        PAUSED = 2,
        CANCELED = 3
    }
}
