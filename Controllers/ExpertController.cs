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
    public class ExpertController : DefaultController
    {

        public ExpertController(IDistributedCache distributedCache, ILogger<ExpertController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن کارشناس")]
        public async Task<IActionResult> AddExpert([FromBody] AddExpertRequest request)
        {
            
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();
            await Db.AddSave(entity); 
            Db.Entry(entity).Reference(x => x.user).Load(); 
            return response.ToResponse<Expert>(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date, 
                x.user_id, 
                x.full_name
            });
        }

        [HttpPost("search")]
        [DisplayName("جستجوی کارشناس")]
        public async Task<IActionResult> SearchExpert([FromBody] SearchExpertRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetExperts(request).Paging(response, request.page_start, request.page_size) 
                .Include(x=>x.user)
                 .ToListAsync(); 
            return response.ToResponse<Expert>(query, x => new
            {
                x.id, 
                x.create_date,
                x.creator_id,
                x.full_name, 
                x.user_id
            });

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده کارشناس")]
        public async Task<IActionResult> GetExpert(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetExpert(new Expert { id = id });
            existingEntity.ThrowIfNotExist();
            Db.Entry(existingEntity).Reference(x => x.user).Load();

            return response.ToResponse<Expert>(existingEntity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id, 
                x.modifier_id,
                x.modify_date,
                x.user_id, 
                x.full_name
            });
        }
 

        [DisplayName("حذف ارزیاب")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteExpert(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetExpert(new Expert { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}