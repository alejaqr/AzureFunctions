using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using timearq.Common.Models;
using timearq.Functions.Entities;

namespace timearq.Test.Helpers
{
    public class TestFactory
    {
        public static timearqEntity GettimeEntity()
        {
            return new timearqEntity
            {
                ETag = "*",
                PartitionKey = "time",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployee = 1,
                Register = DateTime.UtcNow,
                Type = 0,
                Consolidated = false
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid timeId, Time timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{timeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid timeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{timeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Time timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {

            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Time GetTimeRequest()
        {
            return new Time
            {
                IdEmployee = 1,
                Register = DateTime.UtcNow,
                Type = 0,
                Consolidated = false
            };

        }

        private static Stream GenerateStreamFromString(string request)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(request);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }

        public static List<timearqEntity> GetListEntities()
        {
            return new List<timearqEntity>();
        }
    }
}
