using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using timearq.Common.Models;
using timearq.Common.Responses;
using timearq.Functions.Entities;

namespace timearq.Functions.Functions
{
    public static class timearqApi
    {
        [FunctionName(nameof(CreateRegister))]
        public static async Task<IActionResult> CreateRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "time")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timerTable,
             ILogger log)
        {
            log.LogInformation("Created a new register");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            if (string.IsNullOrEmpty(time?.IdEmployee.ToString()) || time?.IdEmployee <= 0)

            {

                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Error Id can´t be blank"
                });

            }

            if (string.IsNullOrEmpty(time?.Register.ToString()))
            {

                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Error Date can´t be blank"
                });
            }

            if (string.IsNullOrEmpty(time?.Type.ToString()))
            {

                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Error Type can´t be blank"
                });
            }


            timearqEntity timearqEntity = new timearqEntity
            {
                IdEmployee = time.IdEmployee,
                Register = time.Register,
                Type = time.Type,
                Consolidated = false,
                ETag = "*",
                PartitionKey = "time",
                RowKey = Guid.NewGuid().ToString()
            };


            TableOperation createOperation = TableOperation.Insert(timearqEntity);
            await timerTable.ExecuteAsync(createOperation);

            string Message = "New register added";
            log.LogInformation(Message);



            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = Message,
                Result = timearqEntity
            });
        }

        [FunctionName(nameof(UpdateRegister))]
        public static async Task<IActionResult> UpdateRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "time/{Id}")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timerTable,
            string Id,
             ILogger log)
        {
            log.LogInformation("Update a register");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            TableOperation findId = TableOperation.Retrieve<timearqEntity>("time", Id);

            TableResult Result = await timerTable.ExecuteAsync(findId);

            if (Result.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "ID not exist"

                });
            }

            timearqEntity timearq = (timearqEntity)Result.Result;

            if (!string.IsNullOrEmpty(time.Register.ToString()))
            {
                timearq.Register = time.Register;
            }

            TableOperation update = TableOperation.Replace(timearq);
            await timerTable.ExecuteAsync(update);

            string Message = $"Register with ID {Id} was successfully updated";

            log.LogInformation(Message);



            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = Message,
                Result = timearq
            });
        }

        [FunctionName(nameof(DeleteRegister))]
        public static async Task<IActionResult> DeleteRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "time/{Id}")] HttpRequest req,
            [Table("time", "time", "{Id}", Connection = "AzureWebJobsStorage")] timearqEntity timearqEntity,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            string Id,
            ILogger log)
        {
            log.LogInformation("Delete a register");

            if (timearqEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Register not found"

                });
            }

            await timeTable.ExecuteAsync(TableOperation.Delete(timearqEntity));


            string Message = $"Register with ID {Id} was successfully deleted";

            log.LogInformation(Message);

            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = Message,
                Result = timearqEntity
            });
        }

        [FunctionName(nameof(GetRegister))]
        public static ActionResult GetRegister(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time/{Id}")] HttpRequest req,
           [Table("time", "time", "{Id}", Connection = "AzureWebJobsStorage")] timearqEntity timearqEntity,
           string Id,
           ILogger log)
        {
            log.LogInformation("Get a register");

            if (timearqEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Register found"

                });
            }

            string Message = $"Register with ID {Id} was successfully retrieved";

            log.LogInformation(Message);

            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = Message,
                Result = timearqEntity
            });
        }

        [FunctionName(nameof(GetAllRegister))]
        public static async Task<ActionResult> GetAllRegister(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequest req,
           [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
           ILogger log)
        {
            log.LogInformation("Get all register");

            TableQuerySegment<timearqEntity> Registers =
                await timeTable.ExecuteQuerySegmentedAsync(new TableQuery<timearqEntity>(), null);

            string Message = $"Registers was successfully retrieved";

            log.LogInformation(Message);

            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = Message,
                Result = Registers
            });
        }

    }



}
