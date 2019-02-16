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
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new TraceLogger();

            DoWork(logger);

            Console.WriteLine("Hit a key to exit");
            Console.ReadKey();
        }

        private static void DoWork(TraceLogger logger)
        {
            var fixture = new Fixture();

            var repeatCount = 3;

            for (int i = 0; i < repeatCount; i++)
            {
                logger.Info($"Processing loop {i} : START ==============================");

                var dto = fixture.Create<MyDto>();
                var dtoSerialised = XmlSerializeObject(dto);
                var dtoSerialisedJson =
                    JsonConvert
                    .SerializeObject(
                        dto,
                        Formatting.Indented);

                logger.Info($"MyDto - Xml - Start\n{dtoSerialised}");
                logger.Info($"MyDto - Xml - End\n");

                // logger.Info($"MyDto - Json - Start\n{dtoSerialisedJson}\nMyApp: MyDto - Json - End\n");
                logger.Info($"MyDto - Json - Start\n{dtoSerialisedJson}");
                logger.Info($"MyDto - Json - End\n");

                try
                {
                    var x = 0;

                    var y = 9 / x;
                }
                catch (Exception ex)
                {
                    logger.Error($"{ex}");
                    //throw;
                }

                logger.Info($"Processing loop {i} : END ==============================");
                logger.Info($"");
            }
        }

        public static string XmlSerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }

    public interface ILogger
    {
        void Error(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);
        void Info(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);
    }

    public class TraceLogger : ILogger
    {
        public void Error(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            TraceMessage($"[ERROR] : {sourceFilePath} : Method {memberName.ToString()} : Line {sourceLineNumber.ToString()}");
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
        //}

        public void Info(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            TraceMessage($"[INFO] : {sourceFilePath} : Method {memberName.ToString()} : Line {sourceLineNumber.ToString()}");
            TraceMessage(message);
        }

        public void TraceMessage(string message)
        {
            Trace.WriteLine(
                string.IsNullOrWhiteSpace(message) ?
                message :
                $"{System.AppDomain.CurrentDomain.FriendlyName}: {message}");
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
