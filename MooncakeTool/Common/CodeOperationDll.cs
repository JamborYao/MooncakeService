using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class CodeOperationDll
    {
        public static List<CodeState> GetAllCodeState()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = from c in dbContext.CodeStates select c;
            return result.ToList<CodeState>();

        }
        /// <summary>
        /// find sample code operation log show in modal
        /// </summary>
        /// <param name="sampleCodeID"></param>
        /// <returns></returns>
        public static CodeOperation GetCodeOperation(int sampleCodeID)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = from c in dbContext.CodeOperations where c.SampleCodeId == sampleCodeID select c;
            if (result != null)
            {
                return result.ToList<CodeOperation>().FirstOrDefault();
            }
            else return null;
        }
    }
}