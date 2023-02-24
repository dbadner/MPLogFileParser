using System;

namespace MPLogFileParser
{
    class ParseParameters
    {
        //Purpose: Class contains properties for the parsing defined by the user in the UI
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public bool FiltDateTime { get; set; }
        public CustomDateTime DateTimeFrom { get; set; } //DD:HH:MM:SS
        public CustomDateTime DateTimeTo { get; set; } //DD:HH:MM:SS

        public ParseParameters()
        {
            InputFile = "";
            OutputFile = "";
            FiltDateTime = false;
            DateTimeFrom = new CustomDateTime();
            DateTimeTo = new CustomDateTime();
        }
    }
}
