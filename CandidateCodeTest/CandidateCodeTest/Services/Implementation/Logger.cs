using CandidateCodeTest.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace CandidateCodeTest.Services.Implementation
{
    internal class Logger : ILogger
    {
        public void Log(string message)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string logsDir = Path.Combine(currentDir, "Logs");

            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            Task asyncTask = WriteFileAsync(logsDir, message);
        }

        static async Task WriteFileAsync(string loc, string inform)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(loc)))
            {
                await outputFile.WriteAsync(inform);
            }
        }
    }
}
