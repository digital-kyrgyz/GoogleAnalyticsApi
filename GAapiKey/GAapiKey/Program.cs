using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GAapiKey
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var credential = GetCredential().Result;
                using (var svc = new AnalyticsReportingService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google Analytics API Console"
                    }))
                {
                    var dateRange = new DateRange
                    {
                        StartDate = "2021-07-01",
                        EndDate = "2021-08-31"
                    };
                    var dateRange2 = new DateRange()
                    {
                        StartDate = "2021-08-01",
                        EndDate = "2021-08-30",
                    };

                    var users = new Metric
                    {
                        Expression = "ga:users",
                        Alias = "Users Visit"
                    };
                    var sessions = new Metric
                    {
                        Expression = "ga:sessions",
                        Alias = "Session"
                    };
                    var date = new Dimension
                    {
                        Name = "ga:date" 
                    };
                    var device = new Dimension
                    {
                        Name = "ga:deviceCategory"
                    };
                    var country = new Dimension
                    {
                        Name = "ga:country"
                    };
                    var city = new Dimension
                    {
                        Name = "ga:city"
                    };
                  
                    var metrics = new Metric
                    {
                        Expression = "ga:users"
                        
                    };
                    var reportRequest = new ReportRequest
                    {

                        DateRanges = new List<DateRange> { dateRange, dateRange2 },
                        Dimensions = new List<Dimension> { country, device, city},
                        Metrics = new List<Metric> { users , metrics},
                        ViewId = "284356779"//"bl284356779"//"my249390995"
                    };
                    var getReportsRequest = new GetReportsRequest
                    {
                        ReportRequests = new List<ReportRequest> { reportRequest }
                    };
                    var batchRequest = svc.Reports.BatchGet(getReportsRequest);
                    var response = batchRequest.Execute();
                    //List<Report> responseFromRequest = response.Reports.ToList();
                    //printResults(responseFromRequest);
                    foreach (var x in response.Reports.First().Data.Rows)
                    {
                        Console.WriteLine(string.Join(", ", x.Metrics.First().Values));
                    }
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static async Task<UserCredential> GetCredential()
        {
            using (var stream = new FileStream(@"C:\Users\Melis\source\repos\GAapiKey\GAapiKey\Folder\client_secret_2020.json",
                 FileMode.Open, FileAccess.Read))
            {
                const string loginEmailAddress = "BookinglisLane@gmail.com";
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { AnalyticsReportingService.Scope.Analytics },
                    loginEmailAddress, CancellationToken.None,
                    new FileDataStore("GoogleAnalyticsApiConsole"));
            }
        }
        public  static void printResults(List<Report> reports)
        {
            foreach(Report report in reports)
            {
                ColumnHeader header = report.ColumnHeader;
                List<string> dimensionHeaders = (List<string>)header.Dimensions;

                List<MetricHeaderEntry> metricHeaders = (List<MetricHeaderEntry>)header.MetricHeader.MetricHeaderEntries;
                List<ReportRow> rows = (List<ReportRow>)report.Data.Rows;

                foreach(ReportRow row in rows)
                {
                    List<string> dimensions = (List<string>)row.Dimensions;
                    List<DateRangeValues> metrics = (List<DateRangeValues>)row.Metrics;

                    for (int i = 0; i < dimensionHeaders.Count() && i < dimensions.Count(); i++)
                    {
                        Console.WriteLine(dimensionHeaders[i] + ": " + dimensions[i]);
                    }
                    
                    for(int j = 0; j < metrics.Count(); j++)
                    {
                        Console.WriteLine("Date Range (" + j + "): ");
                        DateRangeValues values = metrics[j];
                        for (int k = 0; k < values.Values.Count() && k < metricHeaders.Count(); k++)
                        {
                            Console.WriteLine(metricHeaders[k].Name = ": " + values.Values[k]);
                        }
                    }
                }
            }
        }
    }
 }


//string vmcApiKey = "AIzaSyAh4HcO2oZ9n_eKdOgMXRp2aJ85e5FwbMA";
//AnalyticsReportingService ars = GetService(vmcApiKey);

//// Create the DateRange object.
//DateRange dateRange = new DateRange() { StartDate = "2017-01-01", EndDate = "2017-04-28" };

//// Create the Metrics object.
//Metric sessions = new Metric { Expression = "ga:sessions", Alias = "Sessions" };

////Create the Dimensions object.
//Dimension browser = new Dimension { Name = "ga:browser" };

//// Create the ReportRequest object.
//// Create the ReportRequest object.
//ReportRequest reportRequest = new ReportRequest
//{
//    ViewId = "249390995",
//    DateRanges = new List<DateRange>() { dateRange },
//    Dimensions = new List<Dimension>() { browser },
//    Metrics = new List<Metric>() { sessions }
//};

//List<ReportRequest> requests = new List<ReportRequest>();
//requests.Add(reportRequest);

//// Create the GetReportsRequest object.
//GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };

//// Call the batchGet method.
//GetReportsResponse response = ars.Reports.BatchGet(getReport).Execute();
//        }

//            public static AnalyticsReportingService GetService(string apiKey)
//{
//    try
//    {
//        if (string.IsNullOrEmpty(apiKey))
//            throw new ArgumentNullException("AIzaSyAh4HcO2oZ9n_eKdOgMXRp2aJ85e5FwbMA");

//        return new AnalyticsReportingService(new BaseClientService.Initializer()
//        {
//            ApiKey = apiKey,
//            ApplicationName = "AnalyticsReporting API key example",
//        });
//    }
//    catch (Exception ex)
//    {
//        throw new Exception("Failed to create new AnalyticsReporting Service", ex);
//    }
//}