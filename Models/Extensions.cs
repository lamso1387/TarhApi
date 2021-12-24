﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core; 
using SRLCore.Model;
using SRLCore.Middleware;

namespace TarhApi.Models
{
#pragma warning disable CS1591

     

    public static class TarhDbExtensions
    {
        public static IQueryable<Applicant> GetApplicants(this TarhDb db, SearchApplicantRequest request)
        {
            return GetApplicants(db, request.id);

        }
        public static IQueryable<Applicant> GetApplicants(this TarhDb db, long? id = null)
        {
            var query = db.Applicants.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);

            return query;
        }
        public static async Task<Applicant> GetApplicant(this TarhDb db, Applicant entity)
      => await db.Applicants.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<Plan> GetPlans(this TarhDb db, SearchPlanRequest request)
        {
            return GetPlans(db, request.id,request.applicant_id);

        }
        public static IQueryable<Plan> GetPlans(this TarhDb db, long? id = null,long? applicant_id=null)
        {
            var query = db.Plans.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (applicant_id.HasValue)
                query = query.Where(item => item.applicant_id == applicant_id);

            //  query = query.FilterNonActionAccess(nameof(User.id), null);

            return query;
        }
        public static async Task<Plan> GetPlan(this TarhDb db, Plan entity)
      => await db.Plans.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<BaseInfo> GetBaseInfos(this TarhDb db, SearchBaseInfoRequest request)
        {
            return GetBaseInfos(db, request.id, request.kind);

        }
        public static IQueryable<BaseInfo> GetBaseInfos(this TarhDb db, long? id = null, BaseKind? kind = null)
        {
            var query = db.BaseInfos.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (kind.HasValue)
                query = query.Where(item => item.kind == kind);

            return query;
        }
        public static async Task<BaseInfo> GetBaseInfo(this TarhDb db, BaseInfo entity)
      => await db.BaseInfos.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<Province> GetProvinces(this TarhDb db, SearchProvinceRequest request)
        {
            return GetProvinces(db, request.id);

        }
        public static IQueryable<Province> GetProvinces(this TarhDb db, long? id = null)
        {
            var query = db.Provinces.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);

            return query;
        }
        public static async Task<Province> GetProvince(this TarhDb db, Province entity)
      => await db.Provinces.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<Level> GetLevels(this TarhDb db, SearchLevelRequest request)
        {
            return GetLevels(db, request.id);

        }
        public static IQueryable<Level> GetLevels(this TarhDb db, long? id = null)
        {
            var query = db.Levels.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);

            return query;
        }
        public static async Task<Level> GetLevel(this TarhDb db, Level entity)
      => await db.Levels.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<City> GetCities(this TarhDb db, SearchCityRequest request)
        {
            return GetCities(db, request.id, request.province_id);

        }
        public static IQueryable<City> GetCities(this TarhDb db, long? id = null, long? province_id = null)
        {
            var query = db.Cities.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (province_id.HasValue)
                query = query.Where(item => item.province_id == province_id);

            return query;
        }
        public static async Task<City> GetCity(this TarhDb db, City entity)
      => await db.Cities.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<LevelEvent> GetLevelEvents(this TarhDb db, SearchLevelEventRequest request)
        {
            return GetLevelEvents(db, request.id, request.level_id);

        }
        public static IQueryable<LevelEvent> GetLevelEvents(this TarhDb db, long? id = null, long? level_id = null)
        {
            var query = db.LevelEvents.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (level_id.HasValue)
                query = query.Where(item => item.level_id == level_id);

            return query;
        }
        public static async Task<LevelEvent> GetLevelEvent(this TarhDb db, LevelEvent entity)
      => await db.LevelEvents.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<PlanEvent> GetEvents(this TarhDb db, SearchEventRequest request)
        {
            return GetEvents(db, request.id, request.plan_id);

        }
        public static IQueryable<PlanEvent> GetEvents(this TarhDb db, long? id = null, long? plan_id = null)
        {
            var query = db.PlanEvents.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (plan_id.HasValue)
                query = query.Where(item => item.plan_id == plan_id);

            return query;
        }
        public static async Task<PlanEvent> GetEvent(this TarhDb db, PlanEvent entity)
      => await db.PlanEvents.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<Expert> GetExperts(this TarhDb db, SearchExpertRequest request)
        {
            return GetExperts(db, request.id, request.user_id);

        }
        public static IQueryable<Expert> GetExperts(this TarhDb db, long? id = null, long? user_id = null)
        {
            var query = db.Experts.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (user_id.HasValue)
                query = query.Where(item => item.user_id == user_id);

            return query;
        }
        public static async Task<Expert> GetExpert(this TarhDb db, Expert entity)
      => await db.Experts.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id);
        public static IQueryable<User> GetUsers(this TarhDb db, long? id = null)
        {
            var query = db.Users.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);

            query = query.FilterNonActionAccess(nameof(User.id), null);

            return query;
        }
        public static IQueryable<User> GetUsers(this TarhDb db, SearchUserRequest request)
        {
            return GetUsers(db, request.id);

        }
        public static async Task<User> GetUser(this TarhDb db, User entity)
        => await db.Users.FilterNonActionAccess(nameof(User.id), null).FirstOrDefaultAsync(item => item.id == entity.id || (item.national_code == entity.national_code));

        public static async Task<Role> GetRole(this TarhDb db, Role entity)
    => await db.Roles.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id || item.name == entity.name);
        public static IQueryable<Role> GetRoles(this TarhDb db, long? id = null, string name = null)
        {
            var query = db.Roles.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(item => item.name == name);
            query = query.FilterNonActionAccess(null, null);
            return query;
        }
        public static async Task<UserRole> GetUserRole(this TarhDb db, UserRole entity)
=> await db.UserRoles.FilterNonActionAccess(null, null).FirstOrDefaultAsync(item => item.id == entity.id || (item.user_id == entity.user_id && item.role_id == entity.role_id));

        public static IQueryable<UserRole> GetUserRoles(this TarhDb db, long role_id)
=> db.UserRoles.FilterNonActionAccess(null, null).Where(item => item.role_id == role_id).AsQueryable();


        public static async Task AddSave<T>(this TarhDb db, T entity)
        {
            await db.AddAsync(entity);
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        public static async Task RemoveSave<T>(this TarhDb db, T entity)
        {
            db.Remove(entity);
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        public static async Task UpdateSave(this TarhDb db)
        {
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }


    }

    public static class UserExtensions
    {
        public static User UpdatePasswordHash(this User user, TarhDb db)
        {
            if (!string.IsNullOrWhiteSpace(user.password))
            {
                new SRLCore.Services.UserService<TarhDb, User, Role, UserRole>(db).CreatePasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
                user.password_hash = passwordHash;
                user.password_salt = passwordSalt;
            }
            return user;
        }
    }

    
   
    public static class PlanExtensions
    {
        public static IQueryable<Plan> IncludeLastLevel(this IQueryable<Plan> plans,TarhDb Db)
        {
            return plans.Include(x => x.events).IncludeJustLastLevel(Db);

        }
        public static IQueryable<Plan> IncludeLastLevelEvent(this IQueryable<Plan> plans, TarhDb Db)
        {
            var plan_events= plans.Include(x => x.events)
                .IncludeJustLastLevel(Db) ;
            foreach (var plan in plan_events)
            {
                plan.last_event.level_event = Db.GetLevelEvent(new LevelEvent { id = plan.last_event.level_event_id }).Result;
            }
            return plan_events.OrderByDescending(x=>x.last_event.pdate);
        }

        private static IQueryable<Plan> IncludeJustLastLevel(this IQueryable<Plan> plans, TarhDb Db)
        {
            return plans
                .SelectMany(plan => plan.events.OrderByDescending(z => z.pdate).ThenByDescending(x => x.create_date).Take(1),
      (plan, ev) => new { plan, last_event = ev })
  .Join(Db.GetLevelEvents(),
    plan => plan.last_event.level_event_id,
    level_event => level_event.id,
    (plan, level_event) => new { plan.plan, plan.last_event, level_event.level_id })
  .Join(Db.GetLevels(),
    plan => plan.level_id,
    level => level.id,
    (plan, level) => new Plan
    {
        applicant_company = plan.plan.applicant_company,
        applicant_id = plan.plan.applicant_id,
        assessor_position_id = plan.plan.assessor_position_id,
        create_date = plan.plan.create_date,
        creator_id = plan.plan.creator_id,
        estimate_riali = plan.plan.estimate_riali,
        evaluation_unit_id = plan.plan.evaluation_unit_id,
        events = plan.plan.events,
        expert_id = plan.plan.expert_id,
        id = plan.plan.id,
        knowledge_based_company_type_id = plan.plan.knowledge_based_company_type_id,
        lifecycle_id = plan.plan.lifecycle_id,
        referral_pdate = plan.plan.referral_pdate,
        shenase = plan.plan.shenase,
        subject = plan.plan.subject,
        technology_id = plan.plan.technology_id,
        type = plan.plan.type,
        type_id = plan.plan.type_id,

        last_level  = level ,
        last_event = plan.last_event
    });

        }

    }


#pragma warning restore CS1591
}