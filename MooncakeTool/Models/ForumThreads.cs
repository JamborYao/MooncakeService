using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Models
{
    public class ForumThreads
    {
        public int Rank { get; set; }
        public double WeightImpact { get; set; }
        public string TechCategory { get; set; }
        public string IssueType { get; set; }
        public string ThreadTitle { get; set; }
        public string ThreadUrl { get; set; }
        public double AvgPageView { get; set; }
        public int Views { get; set; }
        public int Replies { get; set; }
        public int UniquePoster  { get; set; }
        public int Subscriber { get; set; }
        public int Age { get; set; }
        public DateTime? CreateOn { get; set; }
        public double IRT { get; set; }
        public DateTime? LastReply { get; set; }
        public bool Answered { get; set; }
        public string AnsweredBy { get; set; }
        public string ThreadType { get; set; }
        public string Trend { get; set; }
        public string  TrendUrl { get; set; }
        public string CSSDone { get; set; }
        public string CustomerNeeded { get; set; }
        public string Difficult { get; set; }
    }
}