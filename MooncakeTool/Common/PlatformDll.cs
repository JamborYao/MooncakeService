using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class PlatformDll
    {
        /// <summary>
        /// find platform id from name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindPlatformIDbyName(string name)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var platform = dbContext.Platforms.Where(x => x.Name == name).FirstOrDefault();
            if (platform == null)
            {
                InsertPlatform(name);
                platform = dbContext.Platforms.Where(x => x.Name == name).FirstOrDefault();
            };
            return platform.Id;
        }

        public static string FindPlatformNamebyId(int? id)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var platform = dbContext.Platforms.Where(x => x.Id == id).FirstOrDefault();
            if (platform == null) throw new Exception("did not find platform from Platfrom table!");
            return platform.Name;
        }

        public static bool InsertPlatform(string name)
        {
            try
            {
                AzureReportEntities dbContext = new AzureReportEntities();
                Platform platForm = new Platform();
                platForm.Value = name.ToLower();
                platForm.Name = name;
                dbContext.Platforms.Add(platForm);

                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Platform> FindAllPlatform()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = from r in dbContext.Platforms select r;
            if (result != null)
            {
                return result.ToList<Platform>();
            }
            else
            {
                return null;
            }

        }
    }
}