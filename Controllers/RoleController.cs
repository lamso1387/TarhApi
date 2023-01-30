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
using System.Reflection;
using System.ComponentModel;  
using SRLCore.Model;

namespace TarhApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : DefaultController
    {
        public RoleController(IDistributedCache distributedCache, ILogger<RoleController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> us) :
            base(distributedCache, logger, dbContext,us)
        {

        }

        [HttpPost("search")]
        [DisplayName("جستجوی نقش")]
        public async Task<IActionResult> SearchRole()
        {
            string method = nameof(SearchRole);
            LogHandler.LogMethod(SRLCore.Model.EventType.Call, Logger, method);
            PagedResponse<object> response = new PagedResponse<object>();

            try
            {

                var query = await Db.GetRoles().Paging(response)//, request.page_start, request.page_size)
                                                                       //  .Include(x=>x.fund)
                    .ToListAsync();
                var entity_list = query
                    .Select(x => new
                    {
                        x.id,
                        x.name
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
        [DisplayName("افزودن نقش")]
        public async Task<IActionResult> AddRole([FromBody] Models.AddRoleRequest request)
        {
            string method = nameof(AddRole);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);

            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.CheckValidation();

                var entiry =  request.ToEntity(user_session_id);

                var existingEntity = await Db.GetRole(entiry);
                if (existingEntity != null)
                {
                    response.ErrorCode = (int)ErrorCode.AddRepeatedEntity;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                foreach (var userId in request.user_ids)
                {
                    var user = await Db.GetUser(new User { id = userId });
                    var user_role = new UserRole
                    {
                        create_date = DateTime.Now,
                        creator_id = user_session_id,
                        role = entiry,
                        user = user
                    };
                    await Db.AddAsync(user_role);
                }
                 

                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                var entity_list = new List<Role> { entiry }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        x.name,
                        x.status,
                        x.accesses,
                       // request.user_ids
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
         
        [HttpDelete("delete/{id}")]
        [DisplayName("حذف نقش")]
        public async Task<IActionResult> DeleteRole(long id)
        {
            string method = nameof(DeleteRole);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {
                var existingEntity = await Db.GetRole(new Role { id = id });
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }

                var existingUserRoles = Db.GetUserRoles(existingEntity.id);

                Db.RemoveRange(existingUserRoles);
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

        [HttpGet("{id}")]
        [DisplayName("مشاهده نقش")]
        public async Task<IActionResult> GetRole(long id)
        {
            string method = nameof(GetRole);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            { 

                var existingEntity = await Db.GetRole(new Role { id = id });
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                Db.Entry(existingEntity).Collection(a => a.user_roles).Query().Include(b => b.user).Load();
                
                var entity = new List<Role> { existingEntity }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        accesses=x.accesses.Split(","),
                        x.name,  
                        users = x.user_roles.Select(y=>new {y.user.first_name,y.user.last_name, y.user.full_name,id= y.user_id }).ToArray(), 
                        user_ids=x.user_roles.Select(y=>y.user_id),
                        x.status
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

        [HttpPut("edit")]
        [DisplayName("ویرایش نقش")]
        public async Task<IActionResult> EditRole([FromBody] Models.AddRoleRequest request)
        {
            string method = nameof(EditRole);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);

            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.CheckValidation();

                var entiry = request.ToEntity(user_session_id);
                entiry.id = (long)request.id;

                var existingEntity = await Db.GetRole(new Role { id=entiry.id});
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                if (existingEntity.name != entiry.name)
                {
                    var existingName = await Db.GetRole(new Role { name = entiry.name });
                    if (existingName != null)
                    {
                        response.ErrorCode = (int)ErrorCode.AddRepeatedEntity;
                        return response.ToHttpResponse(Logger,Request.HttpContext);
                    }
                }
                existingEntity.name = entiry.name;
                existingEntity.modifier_id = user_session_id;
                existingEntity.modify_date = DateTime.Now;
                existingEntity.accesses = entiry.accesses;
                
                var users_old = Db.UserRoles.Where(x => x.role_id == existingEntity.id).Select(x => x.user);
                var user_ids_to_delet = users_old.Select(x => x.id).Where(x => !request.user_ids.Select(y => y).Contains(x));
                var user_ids_new = request.user_ids.Select(x => x).Where(x => !users_old.Select(y => y.id).Contains(x));

                foreach (var user_id_to_delet in user_ids_to_delet)
                {
                    var user_role_delet = await Db.GetUserRole (new UserRole {user_id=user_id_to_delet,role_id=existingEntity.id });
                    Db.Remove(user_role_delet);
                }
                    foreach (var user_new_id in user_ids_new)
                    {
                        var new_user = await Db.GetUser(new User { id = user_new_id });
                        var user_role = new UserRole
                        {
                            create_date = DateTime.Now,
                            creator_id = user_session_id,
                            role = existingEntity,
                            user = new_user
                        };

                        await Db.AddAsync(user_role);
                    }
                


                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger,Request.HttpContext);
                }
                var entity_list = new List<Role> { entiry }
                    .Select(x => new
                    {
                        x.id,
                        x.create_date,
                        x.creator_id,
                        x.name,
                        x.status,
                        x.accesses
                        //userids
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
         
        [HttpGet("accesses")]
        [DisplayName("مشاهده دسترسی ها")]
        public async Task<IActionResult> GetAccesses()
        {
            string method = nameof(GetAccesses);
            LogHandler.LogMethod(EventType.Call, Logger, method);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {
                IEnumerable<Type> all_controller_types = SRL.ChildParent.GetAllChildrenClasses<DefaultController>(Assembly.GetAssembly(typeof(DefaultController)) );
                List<object> action_titles = new List<object>();
                foreach (var controller_type in all_controller_types)
                {

                    MethodInfo[] actions = SRL.ActionManagement.Method.GetPublicMethods(controller_type);
                    var titles = actions.Select(x => new
                    {
                        name = x.Name,
                        title = SRL.ActionManagement.Method.GetDisplayName(x)
                    });
                    action_titles.AddRange(titles);
                }

                response.Model = action_titles;
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