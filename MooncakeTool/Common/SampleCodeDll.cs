using MooncakeTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class SampleCodeDll
    {


        /// <summary>
        /// insert one sample code entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool InsertSampleCode(SampleCode entity)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            try
            {
                var result = dbContext.SampleCodes.Where(c => c.Description == entity.Description);
                if (result != null) return false;
                dbContext.SampleCodes.Add(entity);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("insert to sample code error");
            }

        }

        /// <summary>
        /// batch insert sample code
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static bool BatchInsertSampleCode(List<SampleCode> entities)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            try
            {
                foreach (var entity in entities)
                {
                    if (dbContext.SampleCodes.Any(c => c.Description == entity.Description))
                    {
                        //update
                        var original = dbContext.SampleCodes.Where(c => c.Description == entity.Description).FirstOrDefault();
                        if (original != null)
                        {
                            entity.Id = original.Id;
                            //update gipull table if exist update, else add
                            AddOrUpdateGitPull(entity, original, dbContext);
                            AddOrUpdateGitIssue(entity, original, dbContext);
                            AddOrUpdateGitCommit(entity, original, dbContext);
                            dbContext.Entry(original).CurrentValues.SetValues(entity);
                        }
                    }
                    else {
                        //add               
                        dbContext.SampleCodes.Add(entity);
                        AddOrUpdateGitPull(entity, null, dbContext);
                        AddOrUpdateGitIssue(entity, null, dbContext);
                        AddOrUpdateGitCommit(entity, null, dbContext);
                    }
                    dbContext.SaveChanges();
                   //' CommitDll.FindAllIsNewEntity();
                }

                return true;
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        /// <summary>
        /// if gitpull table has item update it, if not exist add
        /// </summary>
        /// <param name="entity">new sample code object</param>
        /// <param name="original">database sample code object, if no original add directory</param>
        /// <param name="dbContext"></param>
        public static void AddOrUpdateGitPull(SampleCode entity, SampleCode original, AzureReportEntities dbContext)
        {
            entity.GitHubPullRequests.ToList<GitHubPullRequest>().ForEach((p) =>
            {
                if (original != null)
                {
                    p.GitCodeId = original.Id;
                    if (original.GitHubPullRequests.Any(c => c.Title == p.Title))
                    {
                        var updatepull = original.GitHubPullRequests.Where(c => c.Title == p.Title).FirstOrDefault();
                        p.Id = updatepull.Id;
                        dbContext.Entry(updatepull).CurrentValues.SetValues(p);
                    }
                    else {
                        p.IsNew = true;
                        // log new git pull to history table
                        dbContext.Entry(p).State = System.Data.EntityState.Added;
                        
                    }
                }
                else {
                    //p.IsNew = true;
                    dbContext.Entry(p).State = System.Data.EntityState.Added;
                }
            });
        }

        public static void AddOrUpdateGitIssue(SampleCode entity, SampleCode original, AzureReportEntities dbContext)
        {
            entity.GitHubIssues.ToList<GitHubIssue>().ForEach((p) =>
            {
                if (original != null)
                {
                    p.GitCodeId = original.Id;
                    if (original.GitHubIssues.Any(c => c.Title == p.Title))
                    {
                        var updatepull = original.GitHubIssues.Where(c => c.Title == p.Title).FirstOrDefault();
                        p.Id = updatepull.Id;
                        dbContext.Entry(updatepull).CurrentValues.SetValues(p);
                    }
                    else {
                        p.IsNew = true;
                        dbContext.Entry(p).State = System.Data.EntityState.Added;
                    }
                }
                else {
                    dbContext.Entry(p).State = System.Data.EntityState.Added;
                }
            });
        }
        public static void AddOrUpdateGitCommit(SampleCode entity, SampleCode original, AzureReportEntities dbContext)
        {
            entity.GitHubCommits.ToList<GitHubCommit>().ForEach((p) =>
            {
                if (original != null)
                {

                    p.GitCodeId = original.Id;
                    if (original.GitHubCommits.Any(c => c.Sha == p.Sha))
                    {
                        var updatepull = original.GitHubCommits.Where(c => c.Sha == p.Sha).FirstOrDefault();
                        p.Id = updatepull.Id;
                        dbContext.Entry(updatepull).CurrentValues.SetValues(p);
                    }
                    else {
                        p.IsNew = true;
                        dbContext.Entry(p).State = System.Data.EntityState.Added;
                    }
                }
                else {
                    dbContext.Entry(p).State = System.Data.EntityState.Added;
                }
            });
        }

        public static List<CardModel> GetCardInfo(int page)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = dbContext.SampleCodes.OrderBy(c => c.Id).Skip(page - 1).Take(numberEachPage);
            List<CardModel> cards = new List<CardModel>();
            foreach (var item in result)
            {
                CardModel card = new CardModel();
                card.Id = item.Id;
                card.Title = item.Title;
                card.NewCommitsHistory = HistoryDll.NewCommitNumber(item.Id);

                card.Description = item.Description;
                foreach (var c in item.SamplePlatforms)
                {
                    card.Platforms = new List<string>();
                    card.Platforms.Add(PlatformDll.FindPlatformNamebyId(c.PlatformId));
                }
                foreach (var c in item.SampleProducts)
                {
                    card.Products = new List<string>();
                    card.Products.Add(ProductDll.FindProductNamebyId(c.ProductId));
                }
                card.Author = item.Author;
                card.Link = item.GitResourceUrl;
                cards.Add(card);
            }
            return cards;
        }

        /// <summary>
        /// total page
        /// </summary>
        /// <returns></returns>
        public static int[] GetTotalSampleNumber()
        {
            AzureReportEntities dbContext = new AzureReportEntities();

            var numbers = dbContext.SampleCodes.Count();
            var total = Convert.ToInt16(Math.Ceiling(((decimal)numbers / (decimal)numberEachPage)));

            int[] array = new int[total];
            for (int i = 0; i < total; i++)
            {
                array[i] = i;
            }
            return array;
        }
        private static int numberEachPage = 18;
    }
}