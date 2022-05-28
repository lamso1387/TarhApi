using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Web;
using System.ComponentModel;
using Task = System.Threading.Tasks.Task;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Globalization;
using SRLCore.Model;
using TarhApi.Models;
using SRLCore.Services;
using System.Text.Json;
using TarhApi.Controllers;

namespace TarhApi.Middleware
{
    public class AppHandlerMiddleware  :SRLCore.Middleware.HandlerMiddleware<TarhDb,User,Role,UserRole>
         
    {
        public override string[] no_auth_actions => new string[] { nameof(UserController.AuthenticatePost) }; 

        public AppHandlerMiddleware(RequestDelegate next): base(next)
        { 

        }
        

     


    } 
}
