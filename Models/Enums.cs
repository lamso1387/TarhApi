using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TarhApi
{ 
  
    public enum BaseKind
    {
        [Description("نوع طرح")]
        PlanType = 0,
        [Description("حوزه فناوری طرح")]
        Technology = 1,
        [Description("چرخه عمر")]
        LifeCycle = 2,
        [Description("نوع دانش بنیانی")]
        KnowledgeBasedCompanyType = 3,
        [Description("سمت شغلی")]
        JobPosition = 4,
        [Description("واحد ارزیابی")]
        EvaluationUnit = 5,
        [Description("نوع متقاضی")]
        ApplicantType = 6,
        [Description("رده مرتبط")]
        RelatedCategory = 7
    }
     
}
