using Ecommerce.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Shared
{
    public static class LoggerException
    {
        private static ConcurrentQueue<LogException> _tempLogs = new ConcurrentQueue<LogException>();
        static Task _tLogTimelineBatchProcess = Task.Run(Loop_To_BatchProcess);
        static DateTime _lastCleanLogFiles = DateTime.MinValue;
        //public LoggerTimeLine()
        //{
        //    _tLogBatchProcess = Task.Run(Loop_To_BatchProcess);
        //}

        static async Task Loop_To_BatchProcess()
        {

            var rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            while (true)
            {
                try
                {
                    var fileLogError = Path.Combine(rootDir, DateTime.Now.ToString("yyyyMMddHH") + "_error.csv");

                    var serror = string.Empty;
                    for (int i = 0; i < 100; i++)
                    {
                        if (!_tempLogs.TryDequeue(out LogException u))
                        {
                            break;
                        }

                        if (u != null)
                        {
                            serror += JsonConvert.SerializeObject(u);

                        }

                        if (string.IsNullOrEmpty(serror)) continue;

                        using (var sw = new StreamWriter(fileLogError, true, System.Text.Encoding.UTF8))
                        {
                            await sw.WriteLineAsync(serror);
                            await sw.FlushAsync();
                        }

                        if (_lastCleanLogFiles <= DateTime.Now.AddDays(-3))
                        {
                            _lastCleanLogFiles = DateTime.Now;

                            var _30DayAgo = DateTime.Now.Date.AddDays(-10);
                            var _60DayAgo = DateTime.Now.Date.AddDays(-60);

                            for (var d = _60DayAgo; d <= _30DayAgo; d = d.AddHours(1))
                            {
                                if (File.Exists(fileLogError)) File.Delete(fileLogError);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await Task.Delay(5000);
                }
            }
        }

        public static async Task LogError<H>(Exception ex, string functionName, H data)
        {
            _tempLogs.Enqueue(new LogException
            {
                Exception = ex.Message,
                Type = 1,
                InnerException = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                FunctionName = functionName,
                DataJson = JsonConvert.SerializeObject(data),
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
            });
        }
    }
}
