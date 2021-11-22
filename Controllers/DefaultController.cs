using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using TarhApi.Models;
using TarhApi.Services;
using System.Collections.Generic;

namespace TarhApi.Controllers
{

    public class DefaultController : ControllerBase
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger Logger;
        protected readonly TarhDb Db;
        protected UserService _userService;

        public DefaultController(IDistributedCache distributedCache, ILogger<DefaultController> logger, TarhDb dbContext ,UserService userService)  
        {
            _distributedCache = distributedCache;
            Logger = logger;
            Db = dbContext;
            _userService = userService;
        }
         
        protected async Task<IActionResult> Add<RequestT,EntityT>(RequestT request) 
            where RequestT : AppRequest where EntityT : CommonProperty
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity2<EntityT>();

            await Db.AddSave(entity);

            return response.ToResponse(entity, SelectableField.CommonPropertySelector);
        }

        protected IActionResult Get<EntityT>(EntityT existingEntity,Func<EntityT, object> selector)
              where EntityT : CommonProperty

        {
            SingleResponse<object> response = new SingleResponse<object>();
            existingEntity.ThrowIfNotExist();
            return response.ToResponse(existingEntity, selector);
        }

        protected async Task<IActionResult> Edit<RequestT, EntityT>(RequestT request,EntityT existingEntityAfterEdit)
            where RequestT : AppRequest where EntityT : CommonProperty

        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);
            existingEntityAfterEdit.ThrowIfNotExist(); 

            await Db.UpdateSave();

            return response.ToResponse(existingEntityAfterEdit, SelectableField.CommonPropertySelector);
        }

        protected async Task<IActionResult> Delete<EntityT>(EntityT existingEntity)
            where EntityT : CommonProperty
        {
            SingleResponse<object> response = new SingleResponse<object>();  
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }

    }

}