using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

//todo LOG最好寫進資料庫
namespace HolyShong.Services
{
    public static class OperationResultHelper
    {
        public static string WriteLog(this OperationResult value)
        {
            if (value.Exception != null)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
                path = path + ".txt";
                File.WriteAllText(path, value.Exception.ToString());
                return path;
            }
            else { return "沒有存檔"; }
        }
    }
}