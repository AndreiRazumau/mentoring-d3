using System;

namespace FileManagementConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileManagementWrapper.CopyFile("D:\\test.txt", "D:\\b.txt");
            FileManagementWrapper.MoveFile("D:\\b.txt", "D:\\Sandbox\\b.txt");
            Console.ReadLine();
        }
    }
}
