﻿using System.ComponentModel.DataAnnotations;

namespace TerapiaExam.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
