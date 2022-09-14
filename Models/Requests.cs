using Microsoft.EntityFrameworkCore.Internal;
using SRLCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;  

namespace TarhApi.Models
{
#pragma warning disable CS1591
    
    public abstract class AppRequest : SRLCore.Model.WebRequest { }
    
    public class SearchPlanRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? applicant_id { get; set; }
    }
    public class SearchApplicantRequest : PagedRequest
    {
        public long? id { get; set; }
    }
    public class SearchProvinceRequest : PagedRequest
    {
        public long? id { get; set; }
    }
    public class SearchLevelRequest : PagedRequest
    {
        public long? id { get; set; }
    }
    
    public class SearchCityRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? province_id { get; set; }
    }
    public class SearchLevelEventRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? level_id { get; set; }
    }
    public class SearchEventRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? plan_id { get; set; }
    }
    public class SearchExpertRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? user_id { get; set; }
    }
    public class SearchBaseInfoRequest : PagedRequest
    {
        public long? id { get; set; }

        public BaseKind? kind { get; set; }
    }
    public class SearchEvidenceRequest : PagedRequest
    {
        public long? id { get; set; }
        public long? doc_type_id { get; set; }
        public long? sub_company_id { get; set; }

    }
    public class AddRoleRequest : AppRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نام")]
        public string name { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("دسترسی ها")]
        public List<string> accesses { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کاربران")]
        public List<long> user_ids { get; set; }
        protected override bool CheckPropertyValidation()
        {
            bool is_valid = true;
            is_valid = accesses == null ? false : accesses.Count > 0;
            if (is_valid == false)
                validation_errors.Add(Constants.MessageText.RoleAccessNotDefinedError);
            else
            {
                is_valid = user_ids == null ? false : user_ids.Count > 0;
                if (is_valid == false)
                    validation_errors.Add(Constants.MessageText.RoleUsersNotDefinedError);
            }
            return is_valid;
        }


    }
     
    public class AddPlanRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("نوع طرح")]
        public long type_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("حوزه فناوری")]
        public long technology_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("چرخه عمر")]
        public long lifecycle_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("نوع دانش بنیانی")]
        public long knowledge_based_company_type_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("سمت ارزیاب")]
        public long assessor_position_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("واحد ارزیاب")]
        public long evaluation_unit_id { get; set; }
        [DisplayName("شرکت متقاضی")]
        public long? applicant_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("کارشناس")]
        public long expert_id { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("موضوع")]
        public string subject { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("شناسه طرح")]
        public string shenase { get; set; }
        [DisplayName("براورد ریالی")]
        public decimal estimate_riali { get; set; }
        [RegularExpression(@"^1[34][0-9][0-9]\/((1[0-2])|([1-9]))\/(([12][0-9])|(3[01])|[1-9])$"), DisplayName("تاریخ ارجاع")]
        public string referral_pdate { get; set; }


    }
    public class AddApplicantRequest : AppRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نام متقاضی")]
        public string name { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("نوع متقاضی")]
        public long type_id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("شهر")]
        public long city_id { get; set; }
        [DisplayName("نماینده")]
        public string representative { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("رده مرتبط")]
        public long related_category_id { get; set; }
        [DisplayName("ادرس")]
        public string address { get; set; }
        [ DisplayName("مدیرعامل")]
        public string ceo { get; set; } 
        [ DisplayName("تلفن")]
        public string phone { get; set; }

        public override T ToEntity2<T>( long? edit_id = null)
        {
            var entity = new Applicant
            {
                address =address,
                 name =name, 
                phone =phone, 
                related_category_id =related_category_id,
                representative =representative, 
                ceo=ceo,
                city_id=city_id,
                type_id=type_id,
                
            };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity as T;
        }

    }
    public class AddProvinceRequest : AppRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("عنوان")]
        public string title { get; set; }

    }
    public class AddLevelRequest : AppRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("عنوان")]
        public string title { get; set; }

    }
    public class AddCityRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("استان")]
        public long province_id { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("عنوان")]
        public string title { get; set; }

    }
    public class AddLevelEventRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("مرحله")]
        public long level_id { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("عنوان")]
        public string title { get; set; }

    }
    public class AddEventRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("مرحله")]
        public long plan_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("نوع رویداد")]
        public long level_event_id { get; set; }

        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("شرح")]
        public string description { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("تاریخ رویداد")]
        public string pdate { get; set; }
        [DisplayName("انجام دهنده")]
        public string doer { get; set; }
        [ DisplayName("توضیحات ارزیاب")]
        public string assessor_description { get; set; }
        [ DisplayName("شماره نامه")]
        public string letter_number { get; set; }
        [ DisplayName("تاریخ نامه")]
        public string letter_pdate { get; set; }

    }
    public class AddExpertRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("کاربر")]
        public long user_id { get; set; } 

    }
    public class AddBaseInfoRequest : AppRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نوع اطلاعات پایه")]
        public BaseKind kind { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("عنوان")]
        public string title { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("پیش فرض")]
        public bool? is_default { get; set; }
    }


    public class SrlFile
    {
        public string name { get; set; }
        public string base64_string { get; set; }
    }
    public class AddEvidenceRequest : AppRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("نوع سند")]
        public long doc_type_id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("شرکت زیرمجموعه")]
        public long sub_company_id { get; set; }
        [ DisplayName("برچسب")]
        public string tag { get; set; }
        [DisplayName("توضیحات")]
        public string description { get; set; } 
        [RegularExpression(@"^1[34][0-9][0-9]\/((1[0-2])|([1-9]))\/(([12][0-9])|(3[01])|[1-9])$"), DisplayName("تاریخ سند")]
        public string evidence_pdate { get; set; }
        public SrlFile pdf_file { get; set; }
        [DisplayName("شرح")]
        public string explain { get; set; }

    }

    public static class RequestConvertor
    {
        public static Plan ToEntity(this AddPlanRequest request, long? edit_id = null)
        {
            var entity = new Plan
            {
                //address = request.address,
               // applicant = request.applicant,
                assessor_position_id = request.assessor_position_id,
                applicant_id = request.applicant_id,
                estimate_riali = request.estimate_riali,
                evaluation_unit_id = request.evaluation_unit_id,
                expert_id = request.expert_id,
                knowledge_based_company_type_id = request.knowledge_based_company_type_id,
                lifecycle_id = request.lifecycle_id,
                //phone = request.phone,
                referral_pdate = request.referral_pdate,
                //related_category = request.related_category,
               // representative = request.representative,
                subject = request.subject,
                technology_id = request.technology_id,
                type_id = request.type_id,
                shenase = request.shenase

            };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static BaseInfo ToEntity(this AddBaseInfoRequest request, long? edit_id = null)
        {
            var entity = new BaseInfo
            {
                kind = request.kind,
                title = request.title,
                is_default=request.is_default
            };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static Province ToEntity(this AddProvinceRequest request,long user_id, long? edit_id = null)
        {
            var entity = new Province
            {
                title = request.title,
                creator_id=user_id
            };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static Level ToEntity(this AddLevelRequest request, long? edit_id = null)
        {
            var entity = new Level
            {
                title = request.title
            };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static City ToEntity(this AddCityRequest request, long? edit_id = null)
        {
            var entity = new City
            {
                province_id=request.province_id,
                title = request.title,
              };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }  
        public static LevelEvent ToEntity(this AddLevelEventRequest request, long? edit_id = null)
        {
            var entity = new LevelEvent
            {
                level_id=request.level_id,
                title = request.title,
              };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static PlanEvent ToEntity(this AddEventRequest request, long? edit_id = null)
        {
            var entity = new PlanEvent
            {
                assessor_description = request.assessor_description,
                description = request.description,
                letter_number=request.letter_number,
                letter_pdate=request.letter_pdate,
                level_event_id=request.level_event_id,
                pdate=request.pdate,
                plan_id=request.plan_id,
                doer=request.doer,
                };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
        public static Expert ToEntity(this AddExpertRequest request, long? edit_id = null)
        {
            var entity = new Expert
            {
                user_id=request.user_id, 
              };
            if (edit_id != null) entity.id = (long)edit_id;
            return entity;
        }
       

        public static Role ToEntity(this AddRoleRequest request, long user_id)
   => new Role
   {
       create_date = DateTime.Now,
       creator_id = user_id,
       accesses = request.accesses.Join(","),
       name = request.name,
       status = SRLCore.Model.EntityStatus.active.ToString()
   };
    }
#pragma warning restore CS1591
}