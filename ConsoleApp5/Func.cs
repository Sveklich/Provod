using ConsoleApp4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Func
    {
        public static class FileExplorer
        {
            private static string currentPath = "";
            private static Stack<string> previousPaths = new Stack<string>();

            public static void Start()
            {
                Console.WriteLine("Проводник");

                Console.WriteLine();
                Console.WriteLine("Выберите диск:");
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo disk in drives)
                {
                    Console.WriteLine($"  {disk.Name}  Свободно {Math.Round(Convert.ToDecimal(disk.AvailableFreeSpace / 1073741824))} ГБ из {Math.Round(Convert.ToDecimal(disk.TotalSize / 1073741824))} ГБ");
                }

                int choice = Menu.Show(drives.Length + 2);
                string selectedDrive = drives[choice].RootDirectory.FullName;
                previousPaths.Clear();
                Console.Clear();
                currentPath = selectedDrive;

                ExploreDirectory(currentPath);
            }


            private static void ExploreDirectory(string path)
            {
                while (true)
                {
                    Console.WriteLine("Текущая папка: " + path);
                    Console.WriteLine();
                    Console.WriteLine("Выберите папку или файл:");
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    int directoriesCount = 0;
                    foreach (var directory in directoryInfo.GetDirectories())
                    {
                        Console.WriteLine($"  {directoriesCount + 1}. {directory.Name}");
                        directoriesCount++;
                    }

                    int filesCount = 0;
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        Console.WriteLine($"  {filesCount + 1 + directoriesCount}. {file.Name}");
                        filesCount++;
                    }


                    int choice = Menu.Show(directoriesCount + filesCount +2);
                    Console.Clear();

                    if (choice == -1)
                    {
                        if (previousPaths.Count > 0)
                        {
                            path = previousPaths.Pop();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (choice <= directoriesCount)
                    {
                        DirectoryInfo selectedDir = directoryInfo.GetDirectories()[choice];
                        previousPaths.Push(path);
                        path = selectedDir.FullName;
                    }
                    else
                    {
                        FileInfo selectedFile = directoryInfo.GetFiles()[choice - directoriesCount];
                        OpenFile(selectedFile.FullName);
                    }
                }
            }

            private static void OpenFile(string filePath)
            {
                Console.Clear();
                Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
            }
        }
    }
}
