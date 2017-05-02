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

            public bool IsTerminal { get; set; }

            public GroupInfo(char name, int number, bool isTerminal = false)
            {
                this.GroupName = name;
                this.GroupNumber = number;
                this.IsTerminal = isTerminal;
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
                processFileRowThread.Join();
                readFileThread.Join();
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
            _rowsToProcessCollection.Add(null);
        }

        public static void ProcessFileRow()
        {
            string readValue;
            while ((readValue = _rowsToProcessCollection.Take()) != null)
            {
                if (readValue == null)
                    break;
                char groupName;
                for (int i = 0; i < readValue.Length;)
                {
                    if (readValue[i] >= 'A' && readValue[i] <= 'z')
                    {
                        int numberLength = 0;
                        groupName = readValue[i];
                        for (int j = i + 1; j < readValue.Length; j++)
                        {
                            if (readValue[j] >= '0' && readValue[j] <= '9')
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
                        var groupNumber = readValue.Substring(i + 1, numberLength);
                        _structuresToWriteCollection.Add(new GroupInfo(groupName, int.Parse(groupNumber)));
                        i = numberLength + i + 1;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid file content.");
                    }
                }
            }

            _structuresToWriteCollection.Add(new GroupInfo(default(char), 0, true));
        }

        public static void WriteGroups()
        {
            var line = 1;
            GroupInfo group;
            while (!(group = _structuresToWriteCollection.Take()).IsTerminal)
            {
                using (var file = File.AppendText($@"..\Debug\Resources\{group.GroupName}-Group.txt"))
                {
                    file.WriteLine($"Line: {line}; Number: {group.GroupNumber};");
                }
            }
        }
    }
}
