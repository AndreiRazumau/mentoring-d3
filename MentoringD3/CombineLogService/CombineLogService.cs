using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace CombineLogService
{
    public partial class CombineLogService : ServiceBase
    {
        private FileSystemWatcher watcher;

        public CombineLogService()
        {
        }

        protected override void OnStart(string[] args)
        {
            this.watcher = new FileSystemWatcher("c:\\test", "*.txt")
            {
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName
            };
            this.watcher.Changed += new FileSystemEventHandler(this.OnChanged);
            this.watcher.Created += new FileSystemEventHandler(this.OnChanged);

            this.watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                        if (new FileInfo(e.FullPath).Name != "alllogs.txt")
                        {
                            using (var writer = File.AppendText(@"c:\\test\alllogs.txt"))
                            {
                                var fileInfo = new FileInfo(e.FullPath);

                                for (int i = 0; i < 6; i++)
                                {
                                    if (!this.IsFileLocked(fileInfo))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (i == 5)
                                        {
                                            throw new IOException("File in use.");
                                        }
                                    }
                                    Thread.Sleep(1000);
                                }

                                using (var reader = File.OpenText(e.FullPath))
                                {
                                    writer.WriteLine(reader.ReadToEnd());
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogEvent(ex.Message);
            }

            LogEvent($"File {e.FullPath} | {e.ChangeType}");
        }

        protected override void OnStop()
        {
            try
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
            }
            catch (Exception ex)
            {
                LogEvent(ex.Message);
            }

            LogEvent("Monitoring Stopped");
        }

        private void LogEvent(string message)
        {
            var eventSource = "CombineLogService";
            message = DateTime.UtcNow.ToLocalTime() + ": " + message;
            EventLog.WriteEntry(eventSource, message);
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                LogEvent($"File {file.Name} locked.");
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
    }
}
