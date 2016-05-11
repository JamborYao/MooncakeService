using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class CommitDll
    {
        public static void FindAllIsNewEntity()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var newCommits = dbContext.GitHubCommits.Where(c => c.IsNew == true);
            if (newCommits != null)
            {
                newCommits.ToList<GitHubCommit>().ForEach((p) =>
                {
                    History history = new History();
                    history.HistoryType = Enum.GetName(typeof(HistoryDll.HistoryType), HistoryDll.HistoryType.commit);
                    history.ForeignId = p.Id;
                    history.GitCodeId = p.GitCodeId;
                    history.IsHistory = true;
                    HistoryDll.InsertHistory(history);
                });
            }

        }

        public static List<Models.CommitModel> FindNewCommitById(int id)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = dbContext.GitHubCommits.Where(c => c.GitCodeId == id && c.IsNew == true);
            List<Models.CommitModel> commits = new List<Models.CommitModel>();
            if (result != null)
            {
              
                result.ToList<GitHubCommit>().ForEach(p =>
                {
                    Models.CommitModel commitModel = new Models.CommitModel();
                    commitModel.Html_Url = p.Html_Url;
                    commitModel.Message = p.Message;
                    var sample= dbContext.SampleCodes.Where(c => c.Id == id).FirstOrDefault();
                    if (sample != null)
                    {
                        commitModel.Title = sample.Title;
                    }
                    commits.Add(commitModel);
                });
            }
            return commits;
        }
    }
}