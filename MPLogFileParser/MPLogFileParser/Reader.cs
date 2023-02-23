using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


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
            TextFieldParser parser = new TextFieldParser(@_inputFile);
            int numLine = 0;
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_delimeters);
            parser.HasFieldsEnclosedInQuotes = true;
            while (!parser.EndOfData)
            {
                numLine++;
                //if (numLine > maxLines)
                    //throw new IndexOutOfRangeException("Maximum number of lines in input file surpassed.");
                //Processing row
                string[] fields;
                try
                {
                    fields = parser.ReadFields();
                    if (fields == null)
                        throw new MalformedLineException();

                    UpdateHostLog(fields);
                    UpdateURILog(fields);
                }
                catch (MalformedLineException)
                {
                    Console.WriteLine("Line " + numLine + " contains invalid data. Skipping line.");
                }
            }  
        }

        private void UpdateHostLog(string[] fields)
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

        private void UpdateURILog(string[] fields)
        {
            //Purpose: Method adds current parsed line from the input file to _logURI dictionary
            //Args:
            //fields: array of fields parsed from current line from input file
            //numLine: current line # from input file

            string requestURI = fields[_inputTemplate.RequestInd];
            string returnCode = fields[_inputTemplate.ReturnCodeInd];

            if (returnCode != "200")
                return;

            //regular expression to parse Request URI
            var matches = Regex.Matches(requestURI, @"^(GET)\s(.+?)(\sHTTP\S+)?$");
            if (matches.Count() > 0)
            {
                string httpMethod = matches[0].Groups[1].Value;
                string URI = matches[0].Groups[2].Value;

                if (_logURI.ContainsKey(URI))
                    _logURI[URI] += 1;
                else
                {
                    _logURI.Add(URI, 1);
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
