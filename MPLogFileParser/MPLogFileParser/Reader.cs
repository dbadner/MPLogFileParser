using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;

namespace MPLogFileParser
{
    class Reader
    {
        //class properties
        private readonly string _inputFile;
        private readonly string[] _delimeters;
        private readonly InputTemplate _inputTemplate;
        private Dictionary<string, int> _logHost;
        private Dictionary<string, int> _logURI;
           
        public Reader(string inputFile)
        {
            _inputFile = inputFile;
            _delimeters = new string[] {" "};
            _inputTemplate = new InputTemplate();
            _logHost = new Dictionary<string, int>();
            _logURI = new Dictionary<string, int>();
        }

        public void Read()
        {
            //method variables
            //const int bufferSize = 1024;
            const int maxLines = 10000000; //Set maximum # of lines allowed by the program

            //using (var fileStream = File.OpenRead(_inputFile))
            //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
            //{
            //    string line;
            //    int numLine = 0;
            //    while ((line = streamReader.ReadLine()) != null)
            //    {
            //        numLine++;
            //        if (numLine > maxLines)
            //            throw new IndexOutOfRangeException("Maximum number of lines in input file surpassed.");

            //        List<string> words = new List<string>();
            //        words = ParseLine(line);
            //        UpdateDictionaries(words, numLine);

            //    }
            //}

            using (TextFieldParser parser = new TextFieldParser(@_inputFile))
            {
                int numLine = 0;
                parser.TextFieldType = FieldType.Delimited;
                //string[] delimeters = { " " };
                parser.SetDelimiters(_delimeters);
                parser.HasFieldsEnclosedInQuotes = true;
                while (!parser.EndOfData)
                {
                    numLine++;
                    if (numLine > maxLines)
                        throw new IndexOutOfRangeException("Maximum number of lines in input file surpassed.");
                    //Processing row
                    string[] fields;
                    try
                    {
                        fields = parser.ReadFields();
                        if (fields == null || fields.Length != 5)
                            throw new MalformedLineException();

                        UpdateHostLog(fields, numLine);
                        UpdateURILog(fields, numLine);
                    }
                    catch (MalformedLineException)
                    {
                        Console.WriteLine("Line " + numLine + " contains invalid data. Skipping line.");
                    }
                }
            }
        }

        private void UpdateHostLog(string[] fields, int numLine)
        {
            //Purpose: Method adds current parsed line from the input file to _logHost dictionary
            //Args:
            //fields: array of fields parsed from current line from input file
            //numLine: current line # from input file

            string hostName = fields[_inputTemplate.HostNameInd];

            if (_logHost.ContainsKey(hostName))
                _logHost[hostName] += 1;
            else
            {
                _logHost.Add(hostName, 1);
            }
        }

        private void UpdateURILog(string[] fields, int numLine)
        {
            //Purpose: Method adds current parsed line from the input file to _logURI dictionary
            //Args:
            //fields: array of fields parsed from current line from input file
            //numLine: current line # from input file

            string requestURI = fields[_inputTemplate.RequestInd];
            string returnCode = fields[_inputTemplate.ReturnCodeInd];

            string[] arrRequestURI = requestURI.Split(' '); //parse request URI into 3 strings

            if (arrRequestURI == null || arrRequestURI.Length != 3)
                throw new MalformedLineException();

            if ((arrRequestURI[0] == "GET") && (returnCode == "200"))
            {
                if (_logURI.ContainsKey(arrRequestURI[1]))
                    _logURI[arrRequestURI[1]] += 1;
                else
                {
                    _logURI.Add(arrRequestURI[1], 1);
                }
            }
        }

        public void SortOutput(string outputFile)
        {
            SortAndOutput(_logHost, outputFile, false, "Number of accesses to webserver per host:");
            SortAndOutput(_logURI, outputFile, true, "Number of successful resource accesses per URI:");
        }

        private void SortAndOutput(Dictionary<string, int> dict, string outputFile, bool append, string sectionHeader)
        {
            StreamWriter writer = new StreamWriter(@outputFile, append);
            if (append) { writer.WriteLine("") ; }
            writer.WriteLine(sectionHeader);
            foreach (KeyValuePair<string, int> entry in dict.OrderByDescending(key => key.Value))
            {
                writer.WriteLine("{0} {1}", entry.Key, entry.Value);
            }
            writer.Close();               
        }



        //private List<string> ParseLine(string line)
        //{

        //    //string[] words = line.Split(_parser);
        //    var words = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
        //        .Cast<Match>()
        //        .Select(m => m.Value)
        //        .ToList();
        //    words[_inputTemplate.RequestInd] = words[_inputTemplate.RequestInd]
        //        .Remove(0, 1)
        //        .Remove(words[_inputTemplate.RequestInd].Length - 2, 1);



        //    return words;
        //}

        //public void SortOutput()
        //{

        //    IOrderedEnumerable<KeyValuePair<string, int>> sortedURI = SortDictionaries(_logURI);
        //    IOrderedEnumerable<KeyValuePair<string, int>> sortedHost = SortDictionaries(_logHost);
        //}

        //private IOrderedEnumerable<KeyValuePair<string,int>> SortDictionaries(Dictionary<string, int> dict)
        //{
        //    //Purpose: Method sorts the dictionary passed to it by reference
        //    //Args:
        //    //dict: dictionary passed to the method
        //    IOrderedEnumerable<KeyValuePair<string, int>> sortedEnum = from entry in dict orderby entry.Value descending select entry;
        //    return sortedEnum;
        //}

    }
}
