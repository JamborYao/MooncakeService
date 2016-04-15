using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class SampleCodeDll
    {
        static AzureReportEntities dbContext = new AzureReportEntities();
        public static int FindPlatformIDbyName(string name)
        {
            var platform = dbContext.Platforms.Where(x => x.Name == name).FirstOrDefault();
            if (platform == null) throw new Exception("did not find platform from Platfrom table!");
            return platform.Id;
        }
        public static int FindProductIDbyName(string name)
        {
            var product = dbContext.Products.Where(x => x.Name == name).FirstOrDefault();
            if (product == null) throw new Exception("did not find platform from Platfrom table!");
            return product.Id;
        }
        public static bool InsertSampleCode(SampleCode entity)
        {
            try
            {
                var result = dbContext.SampleCodes.Find(entity);
                if (result == null) return false;
                dbContext.SampleCodes.Add(entity);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("insert to sample code error");
            }

        }
        public static bool BatchInsertSampleCode(List<SampleCode> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (dbContext.SampleCodes.Any(c => c.Description == entity.Description))
                    {
                        //update
                        var original = dbContext.SampleCodes.Where(c=>c.Description == entity.Description).FirstOrDefault();
                        if (original != null)
                        {
                            entity.Id = original.Id;
                            dbContext.Entry(original).CurrentValues.SetValues(entity);
                        }
                    }
                    else {
                        //add               
                        dbContext.SampleCodes.Add(entity);
                    }                    
                    dbContext.SaveChanges();

                }
                return true;
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}