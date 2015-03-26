using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DynamicService.TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Service address: ");
            string apiUrl = Console.ReadLine();
            Console.WriteLine();

            using(HttpClient _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(apiUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine("Running test method");
                var testMethodTask = _httpClient.GetAsync("api/dynamicsql/test").ContinueWith((response) => {
                    Console.WriteLine("Test metod result: {0}", response.Result.StatusCode);
                });
                testMethodTask.Wait();

                Console.Write("SQL query: ");
                string query = Console.ReadLine();
                
                Console.WriteLine("Running query method");
                var queryMethodTask = _httpClient.PostAsJsonAsync<string>("api/dynamicsql/executequery", query).ContinueWith((response) => {
                    Console.WriteLine("Query method result: {0}", response.Result.StatusCode);
                });
                queryMethodTask.Wait();

                Console.WriteLine("\r\nPress ENTER to end...");
                Console.ReadLine();
            }
        }
    }
}
