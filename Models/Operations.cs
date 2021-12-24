using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TarhApi.Models
{
    public partial class Role : SRLCore.Model.IRole
    {
        public static SRLCore.Model.GetAllAccess get_all_access = (db, user_id) =>
         {
             List<string> all_access = (db as TarhDb).UserRoles.Where(x => x.user_id == user_id)
      .Include(x => x.role)
      .Select(x => x.role.accesses).ToList();
             return all_access;
         };
    }

     

}
