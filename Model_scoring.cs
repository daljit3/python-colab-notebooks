// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Newtonsoft.Json

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CallRequestResponseService
{
    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        static async Task InvokeRequestResponseService()
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            using (var client = new HttpClient(handler))
            {
                // Request data goes here
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                    {
                        {
                            "WebServiceInput0",
                            new List<Dictionary<string, string>>()
                            {
                                new Dictionary<string, string>()
                                {
                                    {
                                        "symboling", "3"
                                    },
                                    {
                                        "normalized-losses", "1"
                                    },
                                    {
                                        "make", "alfa-romero"
                                    },
                                    {
                                        "fuel-type", "gas"
                                    },
                                    {
                                        "aspiration", "std"
                                    },
                                    {
                                        "num-of-doors", "two"
                                    },
                                    {
                                        "body-style", "convertible"
                                    },
                                    {
                                        "drive-wheels", "rwd"
                                    },
                                    {
                                        "engine-location", "front"
                                    },
                                    {
                                        "wheel-base", "88.6"
                                    },
                                    {
                                        "length", "168.8"
                                    },
                                    {
                                        "width", "64.1"
                                    },
                                    {
                                        "height", "48.8"
                                    },
                                    {
                                        "curb-weight", "2548"
                                    },
                                    {
                                        "engine-type", "dohc"
                                    },
                                    {
                                        "num-of-cylinders", "four"
                                    },
                                    {
                                        "engine-size", "130"
                                    },
                                    {
                                        "fuel-system", "mpfi"
                                    },
                                    {
                                        "bore", "3.47"
                                    },
                                    {
                                        "stroke", "2.68"
                                    },
                                    {
                                        "compression-ratio", "9"
                                    },
                                    {
                                        "horsepower", "111"
                                    },
                                    {
                                        "peak-rpm", "5000"
                                    },
                                    {
                                        "city-mpg", "21"
                                    },
                                    {
                                        "highway-mpg", "27"
                                    },
                                    {
                                        "price", "13495"
                                    },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                

                const string apiKey = "mBD2JVfNf1UD7QS7gLwEHHADg2bQSRbP"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", apiKey);
                client.BaseAddress = new Uri("http://cea90c4e-8de0-4d85-b2c5-ea181497591c.eastus.azurecontainer.io/score");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                var requestString = JsonConvert.SerializeObject(scoreRequest);
                var content = new StringContent(requestString);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}
