using FileManagementConsole.FileManagement;
using FileManagementConsole.Operation;
using System;
using System.Threading;

namespace FileManagementConsole
{
    public class Program
    {
        private static ManualResetEvent _closeConsoleEvent = new ManualResetEvent(false);
        private static FileOperation _operation;

        static void Main(string[] args)
        {
            var userInputThread = new Thread(GetUserInput);
            userInputThread.IsBackground = true;
            userInputThread.Start();

            _operation = new FileOperation(FileManagementMode.COPY, "D:\\test.exe", "c:\\Users\\Andrei_Razumau\\b.txt");
            _operation.Start();
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
                        _operation.Cancel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
