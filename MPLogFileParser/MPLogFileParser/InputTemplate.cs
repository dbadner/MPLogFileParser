using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLogFileParser
{
    class InputTemplate
    {
        //add interface? make more loosely coupled
        public int HostNameInd { get; private set; }
        public int DateTimeInd { get; private set; }
        public int RequestInd { get; private set; }
        public int ReturnInd { get; private set; }
        public int ReturnSize { get; private set; }
        public InputTemplate()
        {
            //No custom import templates in V1; initialize column indices
            HostNameInd = 0;
            DateTimeInd = 1;
            RequestInd = 2;
            ReturnInd = 3;
            ReturnSize = 4;
        }
    }
}
