﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Dtos
{
    
    public class LoginDto
    {
        public string Email { get; set; } // כתובת האימייל
        public string PasswordHash { get; set; } // סיסמה
    }
}

