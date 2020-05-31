﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities.HealthCheck
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = 1;
            try
            {
                using (var client = new HttpClient())
                {
                    var stopwatch = Stopwatch.StartNew();
                    var task = client.GetAsync("http://localhost/app");
                    Task.WaitAll(task);
                    stopwatch.Stop();
                    Console.WriteLine($"HEALTHCHECK: status {task.Result.StatusCode}, took {stopwatch.ElapsedMilliseconds}ms");
                    if (task.Result.StatusCode == HttpStatusCode.OK && stopwatch.ElapsedMilliseconds < 200)
                    {
                        exitCode = 0;                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HEALTHCHECK: error. Exception {ex.Message}");
            }
            return exitCode;
        }
    }
}