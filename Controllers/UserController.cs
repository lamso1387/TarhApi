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
using SRLCore.Model;

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : SRLCore.Controllers.UserController<TarhDb, User, Role, UserRole,AddUserRequest> {

        public UserController(IDistributedCache distributedCache, ILogger<UserController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
              base(distributedCache, logger, dbContext, userService)
        {


        }

        protected override void EditUserFieldFromRequest(User existing_entity, User new_entity)
        {
            existing_entity.first_name = new_entity.first_name;
            existing_entity.last_name = new_entity.last_name;
            existing_entity.mobile = new_entity.mobile;
            existing_entity.password = new_entity.password;
        }

        protected override User RequestToEntity(AddUserRequest requst) {
            var entity = new User
            {
                creator_id = 1,// user_session_id,
                first_name=requst.first_name,
                last_name = requst.last_name,
                mobile = requst.mobile,
                national_code = requst.national_code,
                password = requst.password,
                
            };
            //if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }

    }
}