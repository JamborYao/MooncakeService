using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class ProductDll
    {
        /// <summary>
        /// find product id by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindProductIDbyName(string name)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var product = dbContext.Products.Where(x => x.Name == name).FirstOrDefault();
            if (product == null)
            {
                InsertProduct(name);
                product = dbContext.Products.Where(x => x.Name == name).FirstOrDefault();
            }
            return product.Id;
        }

        public static string FindProductNamebyId(int? id)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var product = dbContext.Products.Where(x => x.Id == id).FirstOrDefault();
            if (product == null) throw new Exception("did not find platform from Platfrom table!");
            return product.Name;
        }
        public static bool InsertProduct(string name)
        {
            try
            {
                AzureReportEntities dbContext = new AzureReportEntities();
                Product product = new Product();
                product.Value = name.ToLower();
                product.Name = name;
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<Product> FindAllProduct()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = from r in dbContext.Products select r;
            if (result != null)
            {
                return result.ToList<Product>();
            }
            else
            {
                return null;
            }

        }
    }
}