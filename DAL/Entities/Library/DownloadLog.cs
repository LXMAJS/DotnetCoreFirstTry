using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Entities.Library
{
    /// <summary>
    /// 下载记录
    /// </summary>
    public class DownloadLog
    {
        public int Id { get; set; }
        public int Book_Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Create_Time { get; set; }
    }
}
