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
            var result = from c in dbContext.CodeStates orderby c.Num  select c;
            return result.ToList<CodeState>();

        }
        /// <summary>
        /// find sample code operation log show in modal
        /// </summary>
        /// <param name="sampleCodeID"></param>
        /// <returns></returns>
        public static Models.OperationModel GetCodeOperation(int sampleCodeID)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = from c in dbContext.CodeOperations where c.SampleCodeId == sampleCodeID orderby c.LogAt descending select c;


            Models.OperationModel model = new Models.OperationModel();

            var getEntity = dbContext.SampleCodes.Where(c => c.Id == sampleCodeID).FirstOrDefault();
            model.Title = getEntity.Title;
            model.SampleCodeId = getEntity.Id;
            if (result != null)
            {
                CodeOperation operation = result.ToList<CodeOperation>().FirstOrDefault();
                if (operation != null)
                {

                    model.Log = operation.LogInfo;
                    model.LogAt = operation.LogAt;
                    model.StateValue = operation.State;
                    model.GitHubRepro = operation.GitHubRepro;
                    model.Id = operation.Id;
                    //model.Labor = operation.Labor;
                    //model.LaborDetail = operation.LaborDetail;
                    var state = dbContext.CodeStates.Where(c => c.Id == operation.State);
                    if (state != null && state.Count() >= 1)
                    {
                        model.CurrentProgress = state.FirstOrDefault().State;
                    }
                }
                return model;


            }
            else return null;
        }

        public static void AddOperation(Models.OperationModel model)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            CodeOperation operation = new CodeOperation();
            //var entity= dbContext.CodeOperations.Where(c => c.Id == model.Id);
            // if (entity != null)
            // {
            //     if (entity.Count() >= 1)
            //     {
            //         operation = entity.FirstOrDefault();
            //         operation.State = model.StateValue;
            //         operation.LogAt = DateTime.Now;
            //         operation.LogInfo = model.Log;

            //         dbContext.SaveChanges();
            //     }
            //     else {
            operation.State = model.StateValue;
            operation.GitHubRepro = model.GitHubRepro;
            operation.LogAt = DateTime.Now;
            operation.LogInfo = model.Log;
            operation.SampleCodeId = model.SampleCodeId;
            operation.Labor = model.Labor;
            operation.LaborDetail = model.LaborDetail;
            dbContext.CodeOperations.Add(operation);
            dbContext.SaveChanges();
        }
        //}
    }

}