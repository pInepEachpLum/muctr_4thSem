using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace OSLab_second.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }
        public long Size { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }

        public FileItem(string fullPath, bool isFolder)
        {
            FullPath = fullPath;
            IsFolder = isFolder;
            Name = Path.GetFileName(fullPath);

            try
            {
                if (isFolder)
                {
                    var info = new DirectoryInfo(fullPath);
                    CreationTime = info.CreationTime;
                    LastWriteTime = info.LastWriteTime;
                    Size = 0;
                }
                else
                {
                    var info = new FileInfo(fullPath);
                    CreationTime = info.CreationTime;
                    LastWriteTime = info.LastWriteTime;
                    Size = info.Length;
                }
            }
            catch (Exception)
            {
                CreationTime = DateTime.MinValue;
                LastWriteTime = DateTime.MinValue;
                Size = -1;
            }
        }
    }
}
