using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Utils
{
    /**
     * General purpose file utilities.
     */
    public static class FileUtils
    {
        public const string BACKUP_SUFFIX = "_backup";
        private const string NEWFILE_SUFFIX = "_newfile";
        
        public static byte[] ReadReliableFile(string path, out string error)
        {
            try
            {
                byte[] result;
                var newFile = path + NEWFILE_SUFFIX;
                var backupFile = path + BACKUP_SUFFIX;
                if (File.Exists(newFile))
                {
                    result = File.ReadAllBytes(newFile);
                    Debug.LogWarning($"[File] Reading from newFile for {path}");
                }
                else if (File.Exists(path))
                {
                    result = File.ReadAllBytes(path);
                }
                else if (File.Exists(backupFile))
                {
                    result = File.ReadAllBytes(backupFile);
                    Debug.LogWarning($"[File] Reading from backup for {path}");
                }
                else
                {
                    result = null;
                }

                error = "";
                return result;
            }
            catch (IOException e)
            {
                int win32ErrorCode = Marshal.GetHRForException(e) & 0xFFFF;
                error = $"{e}, code {win32ErrorCode}";
                return null;
            }
        }
        
        public static bool ReliablyWriteFile(string path, byte[] data, out string error)
        {
            try
            {
                var newFile = path + NEWFILE_SUFFIX;
                if (File.Exists(newFile))
                {
                    ShiftReliableFiles(path);
                }
                File.WriteAllBytes(newFile, data);
                Debug.Log($"[File] Written {data.Length} bytes into {path}");
                ShiftReliableFiles(path);

                error = "";
                return true;
            }
            catch (IOException e)
            {
                // from https://forum.unity.com/threads/catch-device-out-of-space-exception.393791/
                const int ERROR_HANDLE_DISK_FULL = 0x27;
                const int ERROR_DISK_FULL = 0x70;
     
                int win32ErrorCode = Marshal.GetHRForException(e) & 0xFFFF;
                error = win32ErrorCode == ERROR_HANDLE_DISK_FULL || win32ErrorCode == ERROR_DISK_FULL ? 
                    "device out of space" :
                    $"{e}, code {win32ErrorCode}";

                return false;
            }
        }
        
        public static bool PurgeLatestReliableVersion(string path, out string error)
        {
            try
            {
                var newFile = path + NEWFILE_SUFFIX;
                var backupFile = path + BACKUP_SUFFIX;
                var result = true;
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (File.Exists(backupFile))
                {
                    File.Delete(backupFile);
                }
                else
                {
                    result = false;
                }
                
                error = "";
                return result;
            }
            catch (IOException e)
            {
                int win32ErrorCode = Marshal.GetHRForException(e) & 0xFFFF;
                error = $"{e}, code {win32ErrorCode}";

                return false;
            }
        }

        private static void ShiftReliableFiles(string path)
        {
            var newFile = new FileInfo(path + NEWFILE_SUFFIX);
            var backupFile = new FileInfo(path + BACKUP_SUFFIX);
            var theFile = new FileInfo(path);

            if (newFile.Exists)
            {
                if (theFile.Exists)
                {
                    if (backupFile.Exists)
                        backupFile.Delete();

                    theFile.MoveTo(backupFile.FullName);    // changes own FullName
                    newFile.MoveTo(path);
                }
                else
                {
                    newFile.MoveTo(path);
                }
            }
            else if (theFile.Exists)
            {
                if (backupFile.Exists)
                    backupFile.Delete();
                
                theFile.MoveTo(backupFile.FullName);
            }
        }
    }
}