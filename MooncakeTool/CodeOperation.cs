//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MooncakeTool
{
    using System;
    using System.Collections.Generic;
    
    public partial class CodeOperation
    {
        public int Id { get; set; }
        public Nullable<int> SampleCodeId { get; set; }
        public string LogInfo { get; set; }
        public Nullable<System.DateTime> LogAt { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<float> Labor { get; set; }
        public string LaborDetail { get; set; }
        public string GitHubRepro { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual SampleCode SampleCode { get; set; }
    }
}
