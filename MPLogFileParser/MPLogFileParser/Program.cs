

namespace MPLogFileParser
{
    public class Parser
    {
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }

        public Parser(string inputfile, string outputfile) 
        {
            InputFile = inputfile;
            OutputFile = outputfile;
        }
        public void ParserMain()
        {
            Reader reader = new Reader(InputFile);
            reader.Read();
            reader.SortOutput(OutputFile);
        }

    }
}
