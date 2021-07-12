using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.SynchronizedWorkflow
{
    public interface ISyncProcessorFactory
    {
        ISyncProcessorProvider CreateProcessor(string assemblyName, string className);
    }
}
