﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudDataMar.Model;

namespace CloudDataMar.Web.ViewModel
{
    public class UserViewModel : user_info
    {
        public string Action { get; set; }

        public string Role { get; set; }
    }
}