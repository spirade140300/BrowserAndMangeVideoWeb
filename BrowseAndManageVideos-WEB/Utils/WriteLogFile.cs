using System.IO;

namespace BrowseAndManageVideos_WEB.Utils
{
    public class WriteLogFile
    {
        public static bool WriteLog(string strMessage)  
        {  
            try  
            {  
                FileStream objFilestream = new FileStream("Log/LogFile.txt", FileMode.Append, FileAccess.Write);  
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                objStreamWriter.WriteLine(formattedDateTime + ": " + strMessage);  
                objStreamWriter.Close();  
                objFilestream.Close();  
                return true;  
            }  
            catch(Exception ex)  
            {  
                return false;  
            }  
        }  
    }
}
