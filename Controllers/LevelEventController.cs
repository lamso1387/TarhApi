using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarhApi.Models;
using Microsoft.Extensions.Caching.Distributed; 
using Microsoft.AspNetCore.Routing; 
using System.ComponentModel; 
using SRLCore.Model;

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelEventController : DefaultController
    {

        public LevelEventController(IDistributedCache distributedCache, ILogger<LevelEventController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن رویداد مرحله")]
        public async Task<IActionResult> AddLevelEvent([FromBody] AddLevelEventRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation();

            var entity = request.ToEntity();

            await Db.AddSave(entity); 

            return response.ToResponse<LevelEvent>(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date, 
                x.title,
                x.level_id
            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی رویداد مرحله")]
        public async Task<IActionResult> SearchLevelEvent([FromBody] SearchLevelEventRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetLevelEvents(request).Paging(response, request.page_start, request.page_size) 
                .Include(x=>x.level)
                 .ToListAsync(); 
            return response.ToResponse<LevelEvent>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.title, 
                x.level_id,
                x.level_title
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده رویداد مرحله")]
        public async Task<IActionResult> GetLevelEvent(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetLevelEvent(new LevelEvent { id = id });
            existingEntity.ThrowIfNotExist();
             

            return response.ToResponse<LevelEvent>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
                x.level_id
            });
        }

        [DisplayName("ویرایش رویداد مرحله")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditLevelEvent([FromBody] AddLevelEventRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation();

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetLevelEvent(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.title = entity.title; 

            await Db.UpdateSave(); 

            return response.ToResponse<LevelEvent>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
                x.level_id
            });
        }

        [DisplayName("حذف رویداد مرحله")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLevelEvent(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetLevelEvent(new LevelEvent { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}