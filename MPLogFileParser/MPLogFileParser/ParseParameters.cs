namespace MPLogFileParser
{
    class ParseParameters
    {
        //Purpose: Class contains properties for the parsing defined by the user in the UI
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public bool FiltDateTime { get; set; }
        public int[] DateTimeFrom { get; set; } //DD:HH:MM:SS
        public int[] DateTimeTo { get; set; } //DD:HH:MM:SS

        public ParseParameters()
        {
            InputFile = "";
            OutputFile = "";
            FiltDateTime = false;
            DateTimeFrom = new int[4];
            DateTimeTo = new int[4];
        }
    }
}
