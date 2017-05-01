using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ConveyorProcessing
{
    public class Program
    {
        private struct GroupInfo
        {
            public char GroupName { get; set; }

            public int GroupNumber { get; set; }

            public GroupInfo(char name, int number)
            {
                this.GroupName = name;
                this.GroupNumber = number;
            }
        }

        private static BlockingCollection<string> _rowsToProcessCollection;
        private static BlockingCollection<GroupInfo> _structuresToWriteCollection;

        private static ManualResetEvent _readFileEvent;
        private static ManualResetEvent _processFileFileEvent;

        static void Main(string[] args)
        {
            Console.WriteLine("Start processing...");
            _rowsToProcessCollection = new BlockingCollection<string>();
            _structuresToWriteCollection = new BlockingCollection<GroupInfo>();
            _readFileEvent = new ManualResetEvent(false);
            _processFileFileEvent = new ManualResetEvent(false);

            try
            {
                var readFileThread = new Thread(ReadFile);
                var processFileRowThread = new Thread(ProcessFileRow);
                var writeGroupsThread = new Thread(WriteGroups);
                readFileThread.Start(@"..\Debug\Resources\source.txt");
                processFileRowThread.Start();
                writeGroupsThread.Start();
                writeGroupsThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Processing finished.");
            Console.ReadKey();
        }

        public static void ReadFile(object filePath)
        {
            string row;
            using (var file = new StreamReader((string)filePath))
            {
                while ((row = file.ReadLine()) != null)
                {
                    _rowsToProcessCollection.Add(row);
                }
            }

            _readFileEvent.Set();
        }

        public static void ProcessFileRow()
        {
            for (;;)
            {
                if (_readFileEvent.WaitOne(0) && _rowsToProcessCollection.Count == 0)
                {
                    _processFileFileEvent.Set();
                    break;
                }
                if (_rowsToProcessCollection.TryTake(out string fileRow, 1))
                {
                    char groupName;
                    for (int i = 0; i < fileRow.Length;)
                    {
                        if (fileRow[i] >= 'A' && fileRow[i] <= 'z')
                        {
                            int numberLength = 0;
                            groupName = fileRow[i];
                            for (int j = i + 1; j < fileRow.Length; j++)
                            {
                                if (fileRow[j] >= '0' && fileRow[j] <= '9')
                                {
                                    numberLength++;
                                }
                                else
                                {
                                    if (numberLength == 0)
                                    {
                                        throw new InvalidDataException("Invalid file content.");
                                    }

                                    break;
                                }
                            }
                            var groupNumber = fileRow.Substring(i + 1, numberLength);
                            _structuresToWriteCollection.Add(new GroupInfo(groupName, int.Parse(groupNumber)));
                            i = numberLength + i + 1;
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid file content.");
                        }
                    }
                }
            }
        }

        public static void WriteGroups()
        {
            var line = 1;
            for (;;)
            {
                if (_processFileFileEvent.WaitOne(0) && _structuresToWriteCollection.Count == 0)
                {
                    break;
                }
                if (_structuresToWriteCollection.TryTake(out GroupInfo group, 1))
                {
                    using (var file = File.AppendText($@"..\Debug\Resources\{group.GroupName}-Group.txt"))
                    {
                        file.WriteLine($"Line: {line}; Number: {group.GroupNumber};");
                    }
                }
            }
        }
    }
}
