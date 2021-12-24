using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using TarhApi.Models;  
using SRLCore.Model;

namespace TarhApi.Controllers
{

    public class DefaultController: SRLCore.Controllers.CommonController<TarhDb,User,Role,UserRole>
    {
        public DefaultController(IDistributedCache distributedCache,
           ILogger<DefaultController> logger, TarhDb dbContext,
           SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService)
            : base(distributedCache,logger,  dbContext, userService)
        { 

        }

    } 

}