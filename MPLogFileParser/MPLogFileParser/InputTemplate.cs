using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLogFileParser
{
    class InputTemplate
    {
        //add interface?
        //Purpose: Class defined for input template future extensibility of solution (i.e. import field mapping). 
        public int HostNameInd { get; private set; }
        public int DateTimeInd { get; private set; }
        public int RequestInd { get; private set; }
        public int ReturnCodeInd { get; private set; }
        public int ReturnSizeInd { get; private set; }
        public InputTemplate()
        {
            //No custom import templates in V1; initialize column indices
            HostNameInd = 0;
            DateTimeInd = 1;
            RequestInd = 2;
            ReturnCodeInd = 3;
            ReturnSizeInd = 4;
        }
    }
}
