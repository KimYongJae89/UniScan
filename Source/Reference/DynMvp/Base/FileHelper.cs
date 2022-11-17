using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace DynMvp.Base
{
    public class FileHelper
    {
        public static void SafeSave(string srcFileName, string bakFileName, string destFileName)
        {
            if (File.Exists(bakFileName))
            {
                File.SetAttributes(bakFileName, FileAttributes.Normal);
                File.Delete(bakFileName);
            }

            if (File.Exists(destFileName))
            {
                File.Move(destFileName, bakFileName);
                File.SetAttributes(bakFileName, FileAttributes.Normal);
            }

            if (File.Exists(srcFileName) == true)
            {
                File.Move(srcFileName, destFileName);
                File.SetAttributes(destFileName, FileAttributes.Normal);
            }
        }

        public static void Move(string srcFileName, string destFileName)
        {
            if (File.Exists(destFileName) == true)
                File.Delete(destFileName);

            if (File.Exists(srcFileName) == true)
                File.Move(srcFileName, destFileName);
        }

        public static void ClearFolder(string folderName, string searchPattern, bool includeSubFolder)
        {
            if (Directory.Exists(folderName) == false)
                return;

            DirectoryInfo dir = new DirectoryInfo(folderName);

            FileInfo[] fileInfos = dir.GetFiles(searchPattern);
            foreach (FileInfo fi in fileInfos)
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }

            if (includeSubFolder)
            {
                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ClearFolder(di.FullName, searchPattern, includeSubFolder);
                    if (di.GetFiles().Count() == 0)
                        di.Delete();
                }
            }
        }

        public static void ClearFolder(string folderName, params string[] excludeFileNames)
        {
            if (Directory.Exists(folderName) == false)
                return;

            DirectoryInfo dir = new DirectoryInfo(folderName);

            FileInfo[] fileInfos = dir.GetFiles();
            foreach (FileInfo fi in fileInfos)
            {
                bool exist = excludeFileNames != null && Array.Exists(excludeFileNames, f => f == fi.Name);
                if (exist == false)
                {
                    fi.IsReadOnly = false;
                    fi.Delete();
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName, excludeFileNames);
                if (di.GetFiles().Count() == 0)
                    di.Delete();
            }
        }

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs, bool overrite, CancellationToken? cancellationToken = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            //if (!dir.Exists)
            //{
            //    throw new DirectoryNotFoundException(
            //        "Source directory does not exist or could not be found: "
            //        + sourceDirName);
            //}

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                if (File.Exists(tempPath) == true && overrite == false)
                    continue;

                file.CopyTo(tempPath, true);
                cancellationToken?.ThrowIfCancellationRequested();
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs, overrite, cancellationToken);
                }
            }
        }

        public static bool CopyFile(string srcCommonFile, string dstCommonFile, bool overWrite)
        {
            if (File.Exists(srcCommonFile) == false)
                return false;

            try
            {
                File.Copy(srcCommonFile, dstCommonFile, overWrite);
                return true;
            }
            catch (IOException)
            { return false; }
        }


        public static void CompressZip(DirectoryInfo path, FileInfo zip, CancellationTokenSource cancellationTokenSource)
        {
            string zipFileName = zip.FullName;
            using (FileStream fileStream = new FileStream(zipFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, false))
                {
                    CompressZip(zipArchive, "", path, cancellationTokenSource?.Token);
                }
            }
        }

        private static void CompressZip(ZipArchive zipArchive, string entryName, DirectoryInfo path, CancellationToken? cancellationToken)
        {
            FileInfo[] fileInfos = path.GetFiles();
            Array.ForEach(fileInfos, f =>
            {
                cancellationToken?.ThrowIfCancellationRequested();
                zipArchive.CreateEntryFromFile(f.FullName, Path.Combine(entryName, f.Name), CompressionLevel.Optimal);
            });

            DirectoryInfo[] dInfos = path.GetDirectories();
            Array.ForEach(dInfos, f => CompressZip(zipArchive, Path.Combine(entryName, f.Name), f, cancellationToken));
        }

        public static void DecompressZip(FileInfo zip, DirectoryInfo path)
        {
            throw new NotImplementedException();
        }

    }
}
