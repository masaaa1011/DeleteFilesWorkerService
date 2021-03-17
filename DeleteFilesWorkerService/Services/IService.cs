using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFilesWorkerService.Services
{
    public interface IService
    {
        void Execute();
        void Flush();
    }
}
