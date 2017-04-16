using System;

namespace FileManagementConsole.Flags
{
    [Flags]
    public enum MoveFileFlags : uint
    {
        MOVE_FILE_REPLACE_EXISTSING = 0x00000001,
        MOVE_FILE_COPY_ALLOWED = 0x00000002,
        MOVE_FILE_DELAY_UNTIL_REBOOT = 0x00000004,
        MOVE_FILE_WRITE_THROUGH = 0x00000008,
        MOVE_FILE_CREATE_HARDLINK = 0x00000010,
        MOVE_FILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
    }
}
