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
    public class EvidenceController : DefaultController
    {

        public EvidenceController(IDistributedCache distributedCache, ILogger<LevelController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن مدرک")]
        public async Task<IActionResult> AddEvidence([FromBody] AddLevelRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();

            await Db.AddSave(entity); 

            return response.ToResponse<Level>(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date, 
                x.title,
            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی مدرک")]
        public async Task<IActionResult> SearchEvidence([FromBody] SearchLevelRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetLevels(request).Paging(response, request.page_start, request.page_size)
                
                 .ToListAsync(); 
            return response.ToResponse<Level>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.title, 
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده مدرک")]
        public async Task<IActionResult> GetEvidence(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetLevel(new Level { id = id });
            existingEntity.ThrowIfNotExist();
             

            return response.ToResponse<Level>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
            });
        }

        [DisplayName("ویرایش مدرک")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditEvidence([FromBody] AddLevelRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetLevel(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.title = entity.title; 

            await Db.UpdateSave(); 

            return response.ToResponse<Level>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.title, 
            });
        }

        [DisplayName("حذف مدرک")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvidence(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetLevel(new Level { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}