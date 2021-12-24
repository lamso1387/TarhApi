using TarhApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarhApi.Models;

namespace TarhApi
{
    public class Constants
    {
        public class MessageText: SRLCore.Constants.MessageText
        { 
        }
        public class Actions
        {
            public static string[] NoAuth = { nameof(UserController.AuthenticatePost) };
        }
    }
}
