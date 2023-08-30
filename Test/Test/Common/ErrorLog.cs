using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test.Common
{
    public class ErrorLog
    {
        /// <summary>
        /// エラーフォルダとファイルがない場合は作成し、
        /// エラーフォルダ内のエラーテキストファイルにエラーログを出力するメソッド
        /// </summary>
        /// <param name="ex"></param>
        public void OutPutError(Exception ex, DateTime buttonClickTime)
        {
            Directory.CreateDirectory(PathString.FolderPath);

            using (StreamWriter writer = File.AppendText(PathString.FilePath))
            {
                writer.WriteLine($"[DateTime]: {buttonClickTime}");
                writer.WriteLine($"[Exception Type]: {ex.GetType().FullName}");
                writer.WriteLine($"[Message]: {ex.Message}");
                writer.WriteLine($"[StackTrace]: {ex.StackTrace}");
                writer.WriteLine(new string('-', 50));
            }
        }


    }
}
