using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
namespace UdyogZipUnzip
{
    public class UdyoyZipUnZipUtility
    {
        public void UdyogUnzip(string ZipFile, string TargetPath, string UnzipPassword)
        {
            
            string[] file = ZipFile.Split('\\');
            string filename = file[file.Length - 1].ToString();
            filename = filename.Replace(".zip", "");
            ZipInputStream s = new ZipInputStream(File.OpenRead(ZipFile));
            if (UnzipPassword != null && UnzipPassword != String.Empty)
                s.Password = UnzipPassword;
            ZipEntry theEntry;
            string tmpEntry = String.Empty;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = TargetPath;
                string fileName = Path.GetFileName(theEntry.Name);
                // create directory 
                if (directoryName != "")
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (fileName != String.Empty)
                {
                    if (theEntry.Name.IndexOf(".ini") < 0) 
                    {
                        string fullPath = directoryName + "\\" + filename + "\\" + theEntry.Name;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                        FileStream streamWriter = File.Create(fullPath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
            }
         
            s.Close();
        }
        public void UdyogZip(string FilePath, string TargetPath)
        {
            // the directory you need to zip
            string[] filenames = Directory.GetFiles(FilePath);
            // path which the zip file built in
            ZipOutputStream s = new ZipOutputStream(File.Create(TargetPath));
            foreach (string filename in filenames)
            {
                FileStream fs = File.OpenRead(filename);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(filename);
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);
                fs.Close();

            }
            s.SetLevel(5);
            s.Finish();
            s.Close();

        }
        public void UdyogZip(string inputFolderPath, string outputPathAndFile, string password)
        {
            ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
            int TrimLength = inputFolderPath.Length;
            // find number of chars to remove     // from orginal file path
            //TrimLength +=4; //remove '\'
            FileStream ostream;
            byte[] obuffer;
            string outPath = outputPathAndFile;
            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
            if (password != null && password != String.Empty)
                oZipStream.Password = password;
            oZipStream.SetLevel(9); // maximum compression
            ZipEntry oZipEntry;
            foreach (string Fil in ar) // for each file, generate a zipentry
            {
                oZipEntry = new ZipEntry((Fil.Remove(0, TrimLength)));
                oZipStream.PutNextEntry(oZipEntry);

                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    ostream = File.OpenRead(Fil);
                    obuffer = new byte[ostream.Length];
                    ostream.Read(obuffer, 0, obuffer.Length);
                    oZipStream.Write(obuffer, 0, obuffer.Length);
                    ostream.Close();
                }
            }
            //oZipEntry.c
            oZipStream.Finish();
            oZipStream.Close();
            oZipStream.Dispose();
        }


        private static ArrayList GenerateFileList(string Dir)
        {
            ArrayList fils = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                fils.Add(file);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                // if directory is completely empty, add it
                {
                    fils.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    fils.Add(obj);
                }
            }
            return fils; // return file list
        }

    }
          
}
