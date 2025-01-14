using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Dtos
{
   
    public class UserDto
    {
        public string Name { get; set; } // שם המשתמש
        public string Email { get; set; } // כתובת האימייל
        public string Password { get; set; } // סיסמה
    }
}
