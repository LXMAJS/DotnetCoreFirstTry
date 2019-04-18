using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Entities.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class Book 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Author_Id { get; set; }
        public int Star { get; set; }
        public string Category { get; set; }
        public DateTime Create_Time { get; set; }
    }
}
