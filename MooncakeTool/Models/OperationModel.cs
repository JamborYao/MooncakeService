using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Models
{
    public class OperationModel
    {
        public int Id { get; set; }
        public int SampleCodeId { get; set; }
        public Nullable<float> Labor { get; set; }
        public string LaborDetail { get; set; }
        public string Title { get; set; }
        public string CurrentProgress { get; set; }
        public string Log { get; set; }
        public int? StateValue { get; set; }
        public DateTime? LogAt { get; set; }
        public string GitHubRepro { get; set; }
    }
}