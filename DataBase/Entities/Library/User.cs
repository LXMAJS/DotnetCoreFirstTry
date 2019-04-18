using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Entities.Library
{
    public class User
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Wechat_Id { get; set; }
        public string Email { get; set; }
        public string Email_Authorization_Code { get; set; }
        public int Mobile { get; set; }
        public string Nickname { get; set; }
        public DateTime Create_Time { get; set; }
    }
}
