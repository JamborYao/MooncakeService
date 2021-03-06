﻿using MooncakeTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
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

        public static void ReplaceEntity(SampleCode origin, SampleCode newEntity)
        {
            origin.Author = newEntity.Author;
            origin.CodeOperations = newEntity.CodeOperations;
            origin.CreateAt = newEntity.CreateAt;
            origin.CustomAt = newEntity.CustomAt;
            origin.Description = newEntity.Description;
            // origin.GitHubCommits = newEntity.GitHubCommits;
            newEntity.GitHubCommits.ToList<GitHubCommit>().ForEach(c =>
            {
                if (origin.GitHubCommits.Any(d => d.Sha == c.Sha))
                {
                    //origin.GitHubCommits.Add(c);
                }
                else
                {
                    //add new commit to history
                    History history = new History();
                    history.IsHistory = true;
                    history.HistoryType = "Commit";
                    history.IsShow = true;
                    int? finish = null;
                    var flag = SetCommitIsShow(origin.Id,ref finish);

                    if (flag != null && flag == finish)
                    {
                        history.IsShow = true;
                    }
                    else
                    {
                        history.IsShow = false;
                    }
                    
                    history.LogAt = DateTime.Now;
                    origin.Histories.Add(history);

                    origin.GitHubCommits.Add(c);
                }
            });

            // origin.GitHubIssues = newEntity.GitHubIssues;
            // origin.GitHubPullRequests = newEntity.GitHubPullRequests;
            //origin.History = newEntity.History;
            origin.LastUpdate = newEntity.LastUpdate;
            // origin.SamplePlatforms = newEntity.SamplePlatforms;
            // origin.SampleProducts = newEntity.SampleProducts;
            origin.Title = newEntity.Title;
        }

        public static int? SetCommitIsShow(int sampleCodeId,ref int? finish)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var operation = dbContext.CodeOperations.Where(c => c.SampleCodeId == sampleCodeId).OrderByDescending(p => p.LogAt).FirstOrDefault();
            finish = dbContext.CodeStates.OrderByDescending(c => c.Num).FirstOrDefault().Num;
            if (operation != null)
            {
                return dbContext.CodeStates.Where(c => c.Id == operation.State).FirstOrDefault().Num;
            }
            else
            {
                return null;
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
                            //entity.Id = original.Id;
                            ////update gipull table if exist update, else add
                            ////AddOrUpdateGitPull(entity, original, dbContext);
                            ////AddOrUpdateGitIssue(entity, original, dbContext);
                            //AddOrUpdateGitCommit(entity, original, dbContext);
                            ReplaceEntity(original, entity);
                            dbContext.SaveChanges();
                            //dbContext.Entry(original).CurrentValues.SetValues(entity);
                        }
                    }
                    else {
                        //add           
                        History history = new History();
                        history.IsHistory = true;
                        history.HistoryType = "CodeSample";
                        history.IsShow = false;
                        history.LogAt = DateTime.Now;
                        entity.Histories.Add(history);

                        dbContext.SampleCodes.Add(entity);
                        dbContext.SaveChanges();

                        //AddOrUpdateGitPull(entity, null, dbContext);
                        //AddOrUpdateGitIssue(entity, null, dbContext);
                        //AddOrUpdateGitCommit(entity, null, dbContext);
                    }

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

        #region
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
        #endregion
        public static List<CardModel> GetCardInfo(int page, string searchKey, int? product, int? platform, int? state)
        {
            AzureReportEntities dbContext = new AzureReportEntities();


            var result = dbContext.GetSearchSamples(searchKey, state.ToString(), product.ToString(), platform.ToString()).OrderBy(c => c.Id).Skip(page - 1).Take(numberEachPage);

            List<CardModel> cards = new List<CardModel>();
            foreach (var item in result)
            {

                CardModel card = new CardModel();
                card.Id = item.Id;
                card.Title = item.Title;
                card.NewCommitsHistory = HistoryDll.NewCommitNumber(item.Id);

                card.Description = item.Description;
                card.Platforms = new List<string>();
                dbContext.SamplePlatforms.Where(c => c.SampleCodeId == item.Id).ToList<SamplePlatform>().ForEach(p =>
                {
                    card.Platforms.Add(PlatformDll.FindPlatformNamebyId(p.PlatformId));
                });
                card.Products = new List<string>();
                dbContext.SampleProducts.Where(c => c.SampleCodeId == item.Id).ToList<SampleProduct>().ForEach(p =>
                {
                    card.Products.Add(ProductDll.FindProductNamebyId(p.ProductId));
                });

                card.Author = item.Author;
                card.Link = item.GitResourceUrl;
                card.States = new List<CodeState>();
                var operation = dbContext.CodeOperations.Where(c => c.SampleCodeId == item.Id).OrderByDescending(p => p.LogAt);

                int? id, num = null;
                if (operation.Count() >= 1)
                {
                    //find state = 2 show github repro, if not find no result
                    var gitrepro = operation.Where(c => c.State == 2).OrderBy(p => p.LogAt);
                    if (gitrepro.Count() >= 1)
                    {
                        card.GitHubRepro = gitrepro.FirstOrDefault().GitHubRepro;
                    }
                    id = operation.FirstOrDefault().State;
                    num = dbContext.CodeStates.Where(c => c.Id == id).FirstOrDefault().Num;
                }
                if (num > 0 && num != null)
                {
                    card.States = dbContext.CodeStates.Where(c => c.Num <= num).OrderBy(c => c.Num).ToList<CodeState>();
                }
                else
                {
                    card.States = dbContext.CodeStates.Where(c => c.Num == 1).ToList<CodeState>();
                }
                cards.Add(card);
            }
            return cards;
        }



        /// <summary>
        /// total page
        /// </summary>
        /// <returns></returns>
        public static int[] GetTotalSampleNumber(string searchKey, int? product, int? platform, int? state)
        {
            AzureReportEntities dbContext = new AzureReportEntities();


            //  CombineExpress(out express, out productExpress, out platformExpress, out stateExpress, searchKey, product, platform, state, dbContext);

            var numbers = dbContext.GetSearchSamples(searchKey, state.ToString(), product.ToString(), platform.ToString()).ToList<GetSearchSamples_Result>().Count();

            var total = Convert.ToInt16(Math.Ceiling(((decimal)numbers / (decimal)numberEachPage)));

            int[] array = new int[total];
            for (int i = 0; i < total; i++)
            {
                array[i] = i;
            }
            return array;
        }

        /// <summary>
        /// get total search page number
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int[] GetSearchSampleNumber(int numbers)
        {

            var total = Convert.ToInt16(Math.Ceiling(((decimal)numbers / (decimal)numberEachPage)));

            int[] array = new int[total];
            for (int i = 0; i < total; i++)
            {
                array[i] = i;
            }
            return array;
        }
        private static int numberEachPage = 18;

        public static List<CardModel> SearchByTitle(string title)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            List<SampleCode> sample = new List<SampleCode>();
            List<CardModel> cards = new List<CardModel>();
            var result = dbContext.SampleCodes.Where(c => c.Title.IndexOf(title) >= 0);
            if (result != null)
            {
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
            else
            {
                return null;
            }
        }
    }

}