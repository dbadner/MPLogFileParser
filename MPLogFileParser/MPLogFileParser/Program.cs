namespace MPLogFileParser
{
    class Program
    {
        //Purpose: Main start point for the back end of the application after the UI is closed
        //class properties
        private readonly ParseParameters _parseParam = new ParseParameters();

        public Program(ParseParameters parseParam) => _parseParam = parseParam;
        public void ProgramMain()
        {
            Parser parser = new Parser(_parseParam);
            parser.Read();
            parser.SortOutput();
        }

        //merge parser into Program for simplicity?

    }
}
