using DeleteFilesWorkerService.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFilesWorkerService.Services
{
    public class SaveFileDeleteService : IService
    {
        private readonly ILogBuffuer _logBuffer;

        public SaveFileDeleteService(ILogBuffuer loggerBuffer)
        {
            _logBuffer = loggerBuffer;

            var Paths = System.Configuration.ConfigurationManager.AppSettings.Get("SurveillancePaths");
            if (string.IsNullOrEmpty(Paths)) throw new Exception("監視パスの設定値が無効です");
            SurveillancePaths = Paths.Split(',').Select(s => s.Trim()).Where(w => !string.IsNullOrEmpty(w)).ToList();
        }
        public List<string> SurveillancePaths { get; set; }

        void IService.Execute()
        {
            SurveillancePaths.ForEach(d =>
            {
                if (System.IO.Directory.Exists(d)) 
                {
                    var directory = new System.IO.DirectoryInfo(d);
                    Console.WriteLine($"start delete files in {directory.FullName}");
                    System.IO.Directory.GetFiles(d).ToList().ForEach(f =>
                    {
                        try
                        {
                            var file = new System.IO.FileInfo(f);
                            file.Delete();
                            Console.WriteLine($"    {file.Name} was deleted");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine($"{e.Message}{System.Environment.NewLine}{e.StackTrace}");
                            _logBuffer.Append($"{e.Message}{System.Environment.NewLine}{e.StackTrace}");
                        }
                    });
                    Console.WriteLine($"end delete files in {directory.Name}");
                }
            });
        }

        public void Flush() => _logBuffer.Flush();
    }
}
