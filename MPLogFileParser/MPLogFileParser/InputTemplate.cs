
namespace MPLogFileParser
{
    class InputTemplate
    {
        //Purpose: Class defined for hypothetical input template future extensibility of solution (i.e. import field mapping). 
        //class properties
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
