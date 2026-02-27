using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OSLab_second.Models;

namespace OSLab_second.Core
{
    public static class FileManager
    {
        public static List<FileItem> GetDirectoryContent(string path)
        {
            var items = new List<FileItem>();
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    items.Add(new FileItem(dir, true));
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    items.Add(new FileItem(file, false));
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception) { }
            return items;
        }

        public static void Rename(string oldPath, string newName)
        {
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);
            if (File.Exists(oldPath)) File.Move(oldPath, newPath);
            else if (Directory.Exists(oldPath)) Directory.Move(oldPath, newPath);
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), true);
            foreach (var dir in Directory.GetDirectories(sourceDir))
                CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }
        public static void Copy(string sourcePath, string destPath)
        {
            if (File.Exists(sourcePath)) File.Copy(sourcePath, destPath, true);
            else if (Directory.Exists(sourcePath)) CopyDirectory(sourcePath, destPath);
        }

        public static void Move(string sourcePath, string destPath)
        {
            if (File.Exists(sourcePath)) File.Move(sourcePath, destPath);
            else if (Directory.Exists(sourcePath)) Directory.Move(sourcePath, destPath);
        }   
    }
}
