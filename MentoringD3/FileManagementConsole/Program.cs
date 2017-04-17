using System;
using System.Threading;

namespace FileManagementConsole
{
    public class Program
    {
        private static ManualResetEvent _closeConsoleEvent = new ManualResetEvent(false);

        private static UserActions _userActions;

        static void Main(string[] args)
        {
            var userInputThread = new Thread(GetUserInput);
            userInputThread.IsBackground = true;
            userInputThread.Start();

            var operation = new Operation(FileManagementMode.COPY, "D:\\test.txt", "D:\\b.txt");
            operation.Start();

            _closeConsoleEvent.WaitOne();
        }

        static void GetUserInput()
        {
            for (;;)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 13)
                {
                    _closeConsoleEvent.Set();
                    break;
                }

                switch (key.KeyChar)
                {
                    case 'c':
                        _userActions.AddUserAction(UserActionType.CANCEL);
                        break;
                    case 'p':
                        _userActions.AddUserAction(UserActionType.PAUSE);
                        break;
                    case 'r':
                        _userActions.AddUserAction(UserActionType.RESUME);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
