using AutoFixture;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DebugViewConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var traceLogger = new TraceLogger();
            traceLogger.Info($"START ==============================");

            var workService = new WorkService(traceLogger);
            workService.DoWork();

            traceLogger.Info($"END ==============================");

            Console.WriteLine("Hit a key to exit");
            Console.ReadKey();
        }
    }

    public class WorkService
    {
        private readonly ILogger _logger;

        public WorkService(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void DoWork()
        {
            var fixture = new Fixture();

            var repeatCount = 3;

            for (int i = 0; i < repeatCount; i++)
            {
                _logger.Info($"Processing loop {i} : START ==============================");

                var dto = fixture.Create<MyDto>();
                var dtoSerialised = XmlSerializeObject(dto);
                var dtoSerialisedJson =
                    JsonConvert
                    .SerializeObject(
                        dto,
                        Formatting.Indented);

                _logger.Info($"MyDto - Xml - Start\n{dtoSerialised}");
                _logger.Info($"MyDto - Xml - End\n");

                _logger.Info($"MyDto - Json - Start\n{dtoSerialisedJson}");
                _logger.Info($"MyDto - Json - End\n");

                try
                {
                    var x = 0;

                    var y = 9 / x;
                }
                catch (Exception ex)
                {
                    _logger.Error($"{ex}");
                    //throw;
                }

                _logger.Info($"Processing loop {i} : END ==============================");
                _logger.Info($"");
            }
        }

        public string XmlSerializeObject<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, value);
                return textWriter.ToString();
            }
        }
    }

    public interface ILogger
    {
        void Error(string message, string memberName = null, string sourceFilePath = null, int sourceLineNumber = 0);
        void Info(string message, string memberName = null, string sourceFilePath = null, int sourceLineNumber = 0);
    }

    public class TraceLogger : ILogger
    {
        public void Error(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            TraceMessage($"[ERROR] : Method [{memberName}] : File [{sourceFilePath}] : Line [{sourceLineNumber.ToString()}]");
            TraceMessage(message);
        }

        //public void Info(string message)
        //{
        //    Trace.WriteLine(
        //        string.IsNullOrWhiteSpace(message) ? 
        //        message : 
        //        $"{System.AppDomain.CurrentDomain.FriendlyName}: {message}");
        //}

        //public void TraceMessage(string message,
        //[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        //[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        //[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    System.Diagnostics.Trace.WriteLine("message: " + message);
        //    System.Diagnostics.Trace.WriteLine("member name: " + memberName);
        //    System.Diagnostics.Trace.WriteLine("source file path: " + sourceFilePath);
        //    System.Diagnostics.Trace.WriteLine("source line number: " + sourceLineNumber);
        //}h

        public void Info(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            TraceMessage($"[INFO] : Method [{memberName}] : File [{sourceFilePath}] : Line [{sourceLineNumber.ToString()}]");
            TraceMessage(message);
        }

        public void TraceMessage(string message)
        {
            Trace.WriteLine(
                string.IsNullOrWhiteSpace(message) ?
                message :
                $"{AppDomain.CurrentDomain.FriendlyName}: {message}");
        }
    }

    public class MyDto
    {
        public List<MyChildDto> Items { get; set; } = new List<MyChildDto>();
    }

    public class MyChildDto
    {
        public int Id { get; set; }

        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyProperty3 { get; set; }
    }
}
