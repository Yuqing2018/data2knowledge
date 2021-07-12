using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IProcessorFactory
    {
        IProcessorProvider CreateProcessor(string assemblyName, string className);
    }
}
