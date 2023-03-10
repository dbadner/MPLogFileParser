using System.Windows;

namespace MPLogFileParser
{
    public class Program
    {
        //Purpose: Main start point for the back end of the application after the UI is closed
        //private readonly ParseParameters _parseParam = new ParseParameters();

        //public Program(ParseParameters parseParam) => _parseParam = parseParam;
        public static void ProgramMain(ParseParameters _parseParam)
        {
            IO io = new IO(_parseParam);
            io.Read();
            if (io.SortOutput())
                MessageBox.Show("Results successfully saved to "+_parseParam.OutputFile, "Notification", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }
    }
}
