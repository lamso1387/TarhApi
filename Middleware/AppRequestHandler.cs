﻿using System;
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
using TarhApi.Models;
using TarhApi.Services;
using System.Text.Json;
using System.IO;
using System.Globalization;

namespace TarhApi.Middleware
{
    public class CustomHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomHandlerMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public async Task Invoke(HttpContext context, UserService _userService, ILogger Logger)
        {
            Stream response_body = context.Response.Body;
            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;

                string action = null;
                try
                {


                    bool need_auth = context.NeedAuth(ref action);
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(
                        context.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: 1000000,
                        leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        LogHandler.LogMethod(EventType.Call, Logger, action,Newtonsoft.Json.JsonConvert.DeserializeObject(body));
                        context.Request.Body.Position = 0;
                    }

                    if (need_auth)
                    {
                        if (!context.Request.Headers.ContainsKey("Authorization"))
                            throw new GlobalException(ErrorCode.Unauthorized);
                        User user = null;
                        try
                        {
                            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                            var username = credentials[0];
                            var password = credentials[1];
                            user = await _userService.Authenticate(username, password);
                        }
                        catch
                        {
                            throw new GlobalException(ErrorCode.Unauthorized);
                        }
                        if (user == null) throw new GlobalException(ErrorCode.Unauthorized);
                        bool has_authority = false;

                        if (action != "authenticate") has_authority = _userService.Authorization(action);

                        if (has_authority == false) throw new GlobalException(ErrorCode.NoDataAccess);

                        var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.full_name)};
                        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;
                    }

                    await _next.Invoke(context);


                }

                catch (Exception error)
                {
                    await HandleExceptionAsync(context, error);
                }
                try
                {
                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream, 
                        encoding: Encoding.UTF8 
                        ,detectEncodingFromByteOrderMarks: false).ReadToEnd(); 
                    LogHandler.LogMethod(EventType.Return, Logger, action,context.Response.StatusCode,
                        Newtonsoft.Json.JsonConvert.DeserializeObject(SRL.Convertor.StringToRegx(responseBody)));
                    memStream.Position = 0;
                    await memStream.CopyToAsync(response_body);
                }
                finally
                {
                    context.Response.Body = response_body;
                }

            }


        }
        private Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            string output = string.Empty;
            if (!context.Response.HasStarted)
            {
                MessageResponse mes_res = new MessageResponse();
                string message = error.Message;
                int code = -1;
                switch (error.GetType().Name)
                {
                    case nameof(GlobalException):
                        var error_code = EnumConvert.StringToEnum<ErrorCode>(error.Message);
                        var error_prop = ErrorProp.GetError(error_code);
                        context.Response.StatusCode = (int)error_prop.status;
                        message = error_prop.message;
                        code = (int)error_code;
                        if (error.InnerException?.Message != null) message = error.InnerException.Message;
                        break; 
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        if (error.InnerException?.Message != null) mes_res.ErrorDetail = error.InnerException.Message; 
                        break;
                }
                context.Response.ContentType = "application/json";
                mes_res.ErrorMessage = message;
                mes_res.ErrorCode = code;
                output = JsonSerializer.Serialize(mes_res);
            }
            return context.Response.WriteAsync(output);
        }


    }
    public class GlobalException : Exception
    {
        public GlobalException(ErrorCode error_code) : base(error_code.ToString())
        {
        }
        public GlobalException(ErrorCode error_code,string message) : 
            base(error_code.ToString(), new Exception(message))
        {
        }
    } 
     
    public class ErrorProp
    {
        public string message { get; set; } = Constants.MessageText.ErrorNotSet;
        //  public int code { get; set; } = -1;
        public HttpStatusCode status { get; set; } = HttpStatusCode.Unused;

        public static ErrorProp GetError(ErrorCode key, string message = null)
        {
            string enum_des_str = SRL.ClassManagement.GetEnumDescription(key);
            ErrorProp enum_des = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorProp>(enum_des_str);
            if (message != null) enum_des.message = message;
            return enum_des;
        }
    }
}
