using Google.Apis.AnalyticsReporting.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAccoutnGoogle
{
    class Program
    {
        static void Main(string[] args)
        {
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(@"C:\Users\Melis\source\repos\ServiceAccoutnGoogle\ServiceAccoutnGoogle\Folder\bookinglane-279405-651f633b7746.json")
                .CreateScoped(new[] { Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService.Scope.AnalyticsReadonly });
            using(var analytics=new Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer=credential
            }))
            {
                var request = analytics.Reports.BatchGet(new GetReportsRequest
                {
                    ReportRequests = new[]
                    {
                        new ReportRequest
                        {
                            DateRanges=new[]{new DateRange { StartDate="2021-08-01", EndDate= "2021-08-31" } },
                            Dimensions=new[]{new Dimension { Name="ga:date"} },
                            Metrics=new []{new Metric { Expression="ga:sessions", Alias="Sessions"} },
                            ViewId="284356779"
                        }
                    }
                });
                var response = request.Execute();
                foreach(var row in response.Reports[0].Data.Rows)
                {
                    Console.Write(string.Join(", ", row.Dimensions) + ": ");
                    foreach (var metric in row.Metrics) Console.WriteLine(string.Join(", ", metric.Values));
                }
            }
        }
    }
}
