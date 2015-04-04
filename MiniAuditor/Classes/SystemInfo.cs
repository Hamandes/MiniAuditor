using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniAuditor.Classes
{
    class SystemInfo
    {
        string processor = null;

        public string HostName { get; set; }

        public string SystemManufacturer { get; set; }

        public string SystemModel { get; set; }

        public string TotalPhysicalMemory { get; set; }

        public string Processor 
        { get
            {
                return processor;
            }
            set
            {
                if (processor == null)
                {
                    processor = value;
                }
            }
        }
    }
}
