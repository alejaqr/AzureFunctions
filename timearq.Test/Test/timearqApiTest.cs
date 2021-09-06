using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using timearq.Common.Models;
using timearq.Functions.Entities;
using timearq.Functions.Functions;
using timearq.Test.Helpers;
using Xunit;



namespace timearq.Test.Test
{
    public class timearqApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateRegisterShouldReturn200()
        {
            //Arrange 

            Time time = TestFactory.GetTimeRequest();
            MockCloudTableTime mockCloudTableTime = new MockCloudTableTime
                (new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(time);


            //Act

            IActionResult response = await timearqApi.CreateRegister(request, mockCloudTableTime, logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void UpdateRegisterShouldReturn200()
        {
            //Arrange 

            Time time = TestFactory.GetTimeRequest();
            MockCloudTableTime mockCloudTableTime = new MockCloudTableTime
                (new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Guid Id = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(Id, time);


            //Act

            IActionResult response = await timearqApi.UpdateRegister
                (request, mockCloudTableTime, Id.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void DeleteRegisterShouldReturn200()
        {
            //Arrange 

            Time time = TestFactory.GetTimeRequest();
            MockCloudTableTime mockCloudTableTime = new MockCloudTableTime
                (new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Guid Id = Guid.NewGuid();
            timearqEntity timearqEntity = TestFactory.GettimeEntity();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(Id, time);


            //Act

            IActionResult response = await timearqApi.DeleteRegister
                (request, timearqEntity, mockCloudTableTime, Id.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public void GetIdRegisterShouldReturn200()
        {
            //Arrange 

            Time time = TestFactory.GetTimeRequest();
            Guid Id = Guid.NewGuid();
            timearqEntity timearqEntity = TestFactory.GettimeEntity();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(Id, time);


            //Act

            IActionResult response = timearqApi.GetRegister
                (request, timearqEntity, Id.ToString(), logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void GetAllRegisterShouldReturn200()
        {
            //Arrange 

            MockCloudTableTime mockCloudTableTime = new MockCloudTableTime
           (new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act

            IActionResult response = await timearqApi.GetAllRegister
                (request, mockCloudTableTime, logger);

            //Assert

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

    }
}
