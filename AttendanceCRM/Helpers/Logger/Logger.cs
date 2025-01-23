using System.Text;

namespace AttendanceCRM.Helpers.Logger
{
    public class Logger
    {
        public static void WriteErrorFile(Exception ex)
        {
            if (ex != null && !string.IsNullOrEmpty(ex.Message) && !ex.Message.Contains("Thread was being aborted."))
            {
                StringBuilder sb = new();
                sb.Append(string.Format("----------------------------- {0} ------------------------------", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")) + Environment.NewLine);
                sb.Append("Exception Mesage : " + ex.Message + Environment.NewLine);
                if (ex.InnerException != null)
                {
                    sb.Append("Exception Inner Exception : " + ex.InnerException + Environment.NewLine);
                }

                sb.Append("Exception Source : " + ex.Source + Environment.NewLine);
                sb.Append("Exception Stack Trace : " + ex.StackTrace + Environment.NewLine);
                WriteErrorFile(sb.ToString(), true, false);
            }
        }

        public static void WriteErrorFile(Exception ex, string functionName)
        {
            if (ex != null && !string.IsNullOrEmpty(ex.Message) && !ex.Message.Contains("Thread was being aborted."))
            {
                StringBuilder sb = new();
                sb.Append(string.Format("----------------------------- {0} ------------------------------", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")) + Environment.NewLine);
                sb.Append(string.Format("Function Name : {0}", functionName));
                sb.Append(string.Format("Exception Message : {0}", ex.Message) + Environment.NewLine);
                if (ex.InnerException != null)
                {
                    sb.Append("Exception Inner Exception : " + ex.InnerException.ToString() + Environment.NewLine);
                }

                sb.Append("Exception Source : " + ex.Source + Environment.NewLine);
                sb.Append("Exception Stack Trace : " + ex.StackTrace + Environment.NewLine);
                WriteErrorFile(sb.ToString(), true, false);
            }
        }

        public static void WriteErrorFile(string textTowrite, bool isNewLine, bool isDebug = false)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + "_error.txt";
                string folderName = "Error";

                if (isDebug)
                {
                    fileName = DateTime.Now.ToString("yyyyMMdd") + "_debug.txt";
                    folderName = "Debug";
                }
                else
                {
                    fileName = DateTime.Now.ToString("yyyyMMdd") + "_error.txt";
                    folderName = "Error";
                }

                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                if (!Directory.Exists(Path.Combine(basePath, folderName)))
                {
                    Directory.CreateDirectory(Path.Combine(basePath, folderName));
                }

                string txtFolderPath = Path.Combine(basePath, folderName);
                string txtPath = txtFolderPath + "/" + fileName;

                if (!File.Exists(txtPath))
                {
                    File.Create(txtPath).Close();
                }

                if (File.Exists(txtPath))
                {
                    File.AppendAllText(txtPath, Environment.NewLine + textTowrite);

                    if (isNewLine)
                    {
                        File.AppendAllText(txtPath, "---------------------------------------------");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
