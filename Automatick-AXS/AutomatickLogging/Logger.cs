using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;


namespace Automatick.Logging
{
    [Serializable]
    public class Logger : List<Log>
    {
        //----for singleton
        static Logger _logger = null;      
        private String FileLocation;
        private static List<Log> _logList;        
        public static String LicenceID=null;
        public static Logger LoggerInstance
        {
            get
            {
                if (_logger == null)
                {                 
                   _logger = new Logger();
                   _logList = new List<Log>();
                }
                return _logger;
            }
        }

        public new void Add(Log obj)
        {
            if (!ExistsInList(obj))
            {
                _logList.Add((Log)obj);               
            }           
        }

        public void DuplicateErrors(Log logEntry)
        {
            try
            {
                if (_logList.Any(p => p.ErrorMessage == logEntry.ErrorMessage))
                {
                 Log duplicateLog= _logList.SingleOrDefault(p => p.ErrorMessage == logEntry.ErrorMessage);
                 logEntry.LogId = duplicateLog.LogId;
                 logEntry.ErrorMessage = String.Empty;                
                }
            }
            catch
            { } 
        }

        public bool ExistsInList(Log logEntry)
        {
            bool ifExists = false;
            try
            {
                DuplicateErrors(logEntry);
                if (_logList.Any(p => p.TicketID == logEntry.TicketID && p.TicketURL == logEntry.TicketURL && p.ErrorMessage == logEntry.ErrorMessage))
                {
                    return ifExists = true;
                }
                else
                {
                   return ifExists = false;
                }
            }
            catch
            {
                return ifExists;
            }
 
        }            

        #region For saving log file       

        public void CreateLogFolder(String _filelocation)
        {
            try
            {
                string LogPath = _filelocation + @"\Logs";
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileLocation = _filelocation;
               
            }
            catch { }
        }

        public void CreateLogFolder()
        {
            try
            {
                string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TB";
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                LogPath = LogPath + "\\Automatick-AXS";
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileLocation = LogPath;
            }
            catch { }
        }

        public void Delete()
        { 
            if (File.Exists(FileLocation + @"\Logs\" + MakeValidFileName() + ".dat"))
            {
                File.Delete(FileLocation + @"\Logs\" + MakeValidFileName() + ".dat");
            }
        }

        public  bool SaveLogFile()
        {
            try
            {
                lock (_logList)
                {
                    if (_logList.Count > 0)
                    {
                        int iretryAttempt = 0;
                    Retry:
                        if (String.IsNullOrEmpty(FileLocation))
                        {
                            CreateLogFolder();
                        }
                        if (!Directory.Exists(FileLocation + @"\Logs\"))
                        {
                            Directory.CreateDirectory(FileLocation + @"\Logs\");
                        }
                        try
                        {
                            Delete();
                            Save(FileLocation + @"\Logs\" + MakeValidFileName() + ".dat");
                        }
                        catch (IOException ioex)
                        {
                            if (iretryAttempt < 5)
                            {
                                iretryAttempt++;
                                Thread.Sleep(10);
                                goto Retry;
                            }
                        }
                        catch
                        {
                            if (iretryAttempt < 5)
                            {
                                iretryAttempt++;
                                Thread.Sleep(10);
                                goto Retry;
                            }
                        }

                        if (!verifyTicketFile())
                        {
                            if (iretryAttempt < 10)
                            {
                                iretryAttempt++;
                                Thread.Sleep(10);
                                goto Retry;
                            }
                        }
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Save(String filename)
        {
            lock (_logList)
            {
                if (_logList.Count > 0)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        lock (stream)
                        {
                            formatter.Serialize(stream, (List<Log>)_logList);
                            stream.Close();
                        }
                    }
                }
            }
        }

        public void Load(String filename)
        {
            try
            {
                lock (_logList)
                {
                    _logList.Clear();
                    if (File.Exists(filename))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            // Deserialize data list items
                            ((List<Log>)_logList).AddRange((IEnumerable<Log>)formatter.Deserialize(stream));
                            stream.Close();

                        }
                    }
                }
            }
            catch
            { }
        }

        public static String MakeValidFileName()
        {
            String file;
            return file= DateTime.Now.ToString("MM-dd-yyyy");
        }

        private Boolean verifyTicketFile()
        {
            Boolean result = false;
            try
            {
                try
                {
                    String fileName = FileLocation + @"\Logs\" + MakeValidFileName() + ".dat";
                    if (File.Exists(fileName))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
                finally
                {
                    //GC.Collect();
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }      

        #endregion        

        ~Logger()
        {
            this.CloseFile();
        }

        public void CloseFile()
        {
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        
        #region Delete log files if more than seven

        public bool DeleteLogFile()
        {
            try
            {
                String[] logfiles = Directory.GetFiles(FileLocation + @"\logs\");
                //--skip latest seven files
                foreach (String oldlogfile in logfiles.OrderByDescending(x => x).Skip(7))
                    File.Delete(oldlogfile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }       

        #endregion
       
    }
}
