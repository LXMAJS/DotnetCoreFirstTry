using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Entities.System
{
    public class Administrator
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string Nickname { get; set; }

        public int Role_Type { get; set; }

        public DateTime Create_Time { get; set; }
    }
}
