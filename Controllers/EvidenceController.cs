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
using System.IO;
using System.Collections.ObjectModel;

namespace TarhApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvidenceController : DefaultController
    {
        private Evidence RequestToEntity(AddEvidenceRequest request, long? edit_id)
        {
            Evidence entity = new Evidence
            {
                description = request.description,
                doc_type_id = request.doc_type_id,
                creator_id = user_session_id,
                evidence_pdate = request.evidence_pdate,
                tag = request.tag,
                sub_company_id = request.sub_company_id,
                explain=request.explain
            };
            if (edit_id != null)
            {
                entity.modifier_id = user_session_id;
                entity.modify_date = DateTime.Now;
                entity.id = (long)edit_id;
            }
            return entity;
        }

        public EvidenceController(IDistributedCache distributedCache, ILogger<LevelController> logger, TarhDb dbContext, SRLCore.Services.UserService<TarhDb, User, Role, UserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {

        }
        [HttpPost("add")]
        [DisplayName("افزودن مدرک")]
        public async Task<IActionResult> AddEvidence([FromBody] AddEvidenceRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            request.CheckValidation(response);

            var entity = RequestToEntity(request, null);
            FileEntity file = null;
            if (!string.IsNullOrWhiteSpace(request.pdf_file?.base64_string))
            {
                file= CreateNewFile(entity, request.pdf_file);
                entity.files = new Collection<FileEntity>();
                entity.files.Add(file);
            }


            await Db.AddSave(entity);

            return response.ToResponse(entity, x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                x.modifier_id,
                x.modify_date,
            });
        }

        private FileEntity CreateNewFile(Evidence entity, SrlFile file)
        {
            string folder = new StreamReader(@"EvidenceFolderPath.txt").ReadLine();
            FileEntity file_entity = new FileEntity(user_session_id, file.name, folder,"pdf");
            file_entity.CreateFile(file.base64_string);
            return file_entity;
        }
        private void DeleteFile(Evidence entity)
        {
          /*  var file = entity.files.ToList()[0];
            string file_guid_name = entity.pdf_guid;
            entity.pdf_file_name = null;
            entity.pdf_guid = null;
            string folder = entity.pdf_folder_path;
            string file_path = Path.Combine(folder, $"{file_guid_name}.pdf");*/
            
            System.IO.File.Delete(entity.files.First().file_full_path); 
            

        }

        [HttpPost("search")]
        [DisplayName("جستجوی مدرک")]
        public async Task<IActionResult> SearchEvidence([FromBody] SearchEvidenceRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetEvidences(request).Paging(response, request.page_start, request.page_size)
                .Include(x => x.doc_type)
                .Include(x => x.sub_company)
                .Include(x=>x.files)
                .ToListAsync();

            return response.ToResponse(query, Models.SelectableField.EvidenceSearchSelector);

        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده مدرک")]
        public async Task<IActionResult> GetEvidence(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetEvidence(new Evidence { id = id });
            existingEntity.ThrowIfNotExist();
            await Db.Entry(existingEntity).Collection(x => x.files).LoadAsync();

            return response.ToResponse(existingEntity, SelectableField.EvidenceSelector);
        }

        [DisplayName("ویرایش مدرک")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditEvidence([FromBody] AddEvidenceRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = RequestToEntity(request, request.id);


            var existingEntity = await Db.GetEvidence(entity);
            existingEntity.ThrowIfNotExist();
            await Db.Entry(existingEntity).Collection(x => x.files).LoadAsync();

            existingEntity.description = entity.description;
            existingEntity.doc_type_id = entity.doc_type_id;
            existingEntity.evidence_pdate = entity.evidence_pdate;
            existingEntity.tag = entity.tag;
            existingEntity.sub_company_id = entity.sub_company_id;
            existingEntity.explain = entity.explain;

            if (existingEntity.files.ToList()[0]?.file_var?.base64_string != request.pdf_file?.base64_string)

                if (string.IsNullOrWhiteSpace(request.pdf_file?.name))
                    DeleteFile(existingEntity);
                else
                {
                    if (!string.IsNullOrWhiteSpace(existingEntity.files.ToList()[0]?.file_name)) DeleteFile(existingEntity);
                    CreateNewFile(existingEntity, request.pdf_file);
                }

            await Db.UpdateSave();

            return response.ToResponse(existingEntity, SelectableField.EvidenceSearchSelector);
        }

        [DisplayName("حذف مدرک")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvidence(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();


            var existingEntity = await Db.GetEvidence(new Evidence { id = id });
            existingEntity.ThrowIfNotExist();
            Db.Entry(existingEntity).Collection(s => s.files).Load();

            if ( existingEntity.files.Any())
                DeleteFile(existingEntity);

            await Db.RemoveSave(existingEntity);

            return response.ToResponse();
        }
    }
}