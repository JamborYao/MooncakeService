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
    
    public partial class Platform
    {
        public Platform()
        {
            this.SamplePlatforms = new HashSet<SamplePlatform>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string CName { get; set; }
    [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<SamplePlatform> SamplePlatforms { get; set; }
    }
}
