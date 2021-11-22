using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarhApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Microsoft.Build.Execution;
using System.ComponentModel;
using SRL;
using System.Net.Http;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using System.Text;
using TarhApi.Services;
using TarhApi.Middleware; 

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : DefaultController
    { 

        public UserController(IDistributedCache distributedCache, ILogger<UserController> logger, TarhDb dbContext, UserService userService) :
            base(distributedCache, logger, dbContext, userService)
        { 
             

        }
         
        [HttpPost("authenticatepost")]
        [DisplayName("احراز هویت")]
        public async Task<IActionResult> AuthenticatePost([FromBody] User user)
        {
            string method = nameof(AuthenticatePost);
            LogHandler.LogMethod(EventType.Call, Logger, method, user);
            SingleResponse<object> response = new SingleResponse<object>();
            try
            {
                user = await _userService.Authenticate(user?.national_code, user?.password);

                if (user == null)
                {
                    response.ErrorCode = (int)ErrorCode.Unauthorized;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                
                UserSession.SetAccesses(Db);

                response.Model = UserSession.Session;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }

            return response.ToHttpResponse(Logger,Request.HttpContext);
        }
         
        [HttpGet("authenticate")]
        [DisplayName("احراز هویت")]
        public async Task<IActionResult> Authenticate()
        {
            string method = nameof(Authenticate);
            LogHandler.LogMethod(EventType.Call, Logger, method);
            PagedResponse<object> response = new PagedResponse<object>();
            try
            {
                response.Model = UserSession.Accesses;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }

            return response.ToHttpResponse(Logger,Request.HttpContext);

        }
         
        [HttpPost("search")]
        [DisplayName("جستجوی کاربر")]
        public async Task<IActionResult> SearchUser([FromBody] SearchUserRequest request)
        {
            string method = nameof(SearchUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);
            PagedResponse<object> response = new PagedResponse<object>();

            try
            {

                var query = await Db.GetUsers(request).Paging(response, request.page_start, request.page_size)
                    //  .Include(x=>x.fund)
                    .ToListAsync();
                var entity_list = query
                    .Select(x => new
                    {
                        x.id,
                        x.first_name,
                        x.last_name,
                        x.full_name,
                        x.mobile,
                        x.national_code
                    });
                response.Model = entity_list;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }

            return response.ToHttpResponse(Logger,Request.HttpContext);

        }
         
        [HttpPost("add")]
        [DisplayName("افزودن کاربر")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            string method = nameof(AddUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);

            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.pass_mode = PassMode.add;
                request.CheckValidation(response);

                var user = request.ToEntity();

                var existingEntity = await Db.GetUser(user);
                if (existingEntity != null)
                {
                    response.ErrorCode = (int)ErrorCode.AddRepeatedEntity;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }

                user.UpdatePasswordHash(Db);

                await Db.AddAsync(user);
                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                var entity_list = new List<User> { user }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        x.first_name,
                        x.last_name,
                        x.mobile, 
                        x.national_code
                    }).First();
                response.Model = entity_list;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger,Request.HttpContext);
        }
         
        [DisplayName("حذف کاربر")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            string method = nameof(DeleteUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {
                var existingEntity = await Db.GetUser(new User { id = id });
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }

                Db.Remove(existingEntity);
                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                response.Model = true;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger,Request.HttpContext);
        }
         
        [DisplayName("ویرایش کاربر")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditUser([FromBody] AddUserRequest request)
        {
            string method = nameof(EditUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.CheckValidation(response);

                var entity = request.ToEntity();
                entity.id = request.id;

                var existingEntity = await Db.GetUser(entity);
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }

                existingEntity.first_name = entity.first_name;
                existingEntity.last_name = entity.last_name;
                existingEntity.mobile = entity.mobile;
                existingEntity.national_code = entity.national_code;
                existingEntity.password = entity.password; 

                existingEntity.UpdatePasswordHash(Db);

                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                var entity_list = new List<User> { entity }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        x.first_name,
                        x.last_name,
                        x.mobile,
                        x.national_code
                    }).First();
                response.Model = entity_list;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger,Request.HttpContext);
        }
         
        [HttpGet("{id}")]
        [DisplayName("مشاهده کاربر")]
        public async Task<IActionResult> GetUser(long id)
        {
            string method = nameof(GetUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();


            try
            { 

                var existingEntity = await Db.GetUser(new User { id = id });
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                var entity = new List<User> { existingEntity }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        x.first_name,
                        x.last_name,
                        x.mobile,
                        x.national_code
                    }).First();
                response.Model = entity;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger,Request.HttpContext);
        }
          
    }
}