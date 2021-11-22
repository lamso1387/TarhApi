﻿using System;
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
    public class PlanEventController : DefaultController
    {

        public PlanEventController(IDistributedCache distributedCache, ILogger<PlanEventController> logger, TarhDb dbContext, UserService userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن رویداد")]
        public async Task<IActionResult> AddEvent([FromBody] AddEventRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity();

            await Db.AddSave(entity);

            return response.ToResponse<PlanEvent>(entity,  SelectableField.CommonPropertySelector);
        }

        [HttpPost("search")]
        [DisplayName("جستجوی رویداد ")]
        public async Task<IActionResult> SearchEvent([FromBody] SearchEventRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();
             
            var query =await Db.GetEvents(request).Paging(response, request.page_start, request.page_size)
                .Include(x => x.plan).Include(x=>x.level_event).ThenInclude(x=>x.level)
                .ToListAsync();
            return response.ToResponse(query, SelectableField.PlantEventSelector);

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده رویداد ")]
        public async Task<IActionResult> GetEvent(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetEvent(new PlanEvent { id = id });
            existingEntity.ThrowIfNotExist();

            Db.Entry(existingEntity).Reference(x => x.level_event).Load();
            Db.Entry(existingEntity).Reference(x => x.plan).Load();

            return response.ToResponse(existingEntity, SelectableField.PlantEventSelector);
        }

        [DisplayName("ویرایش رویداد ")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditEvent([FromBody] AddEventRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity(request.id);

            var existingEntity = await Db.GetEvent(entity);
            existingEntity.ThrowIfNotExist();

            existingEntity.assessor_description = entity.assessor_description;
            existingEntity.description = entity.description;
            existingEntity.doer = entity.doer;
            existingEntity.letter_number = entity.letter_number;
            existingEntity.letter_pdate = entity.letter_pdate;
            existingEntity.level_event_id = entity.level_event_id;
            existingEntity.pdate = entity.pdate;
            existingEntity.plan_id = entity.plan_id;

            await Db.UpdateSave();

            return response.ToResponse<PlanEvent>(existingEntity, SelectableField.CommonPropertySelector);
        }

        [DisplayName("حذف رویداد")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvent(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetEvent(new PlanEvent { id = id });
            existingEntity.ThrowIfNotExist();

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}