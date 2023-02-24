using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace MPLogFileParser
{
    class Parser
    {
        //class properties
        private readonly ParseParameters _parseParam = new ParseParameters();
        private readonly string[] _delimeters;
        private readonly InputTemplate _inputTemplate;
        private Dictionary<string, int> _logHost;
        private Dictionary<string, int> _logURI;

        public Parser(ParseParameters parseParam)
        {
            _parseParam = parseParam;
            _delimeters = new string[] { " " };
            _inputTemplate = new InputTemplate();
            _logHost = new Dictionary<string, int>();
            _logURI = new Dictionary<string, int>();
        }

        public void Read()
        {
            //Purpose: Method iteratively reads the specified input file defined in the class property _parseParam and parses valid fields into two class dictionaries

            TextFieldParser parser = new TextFieldParser(@_parseParam.InputFile);
            int numLine = 0;
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_delimeters);
            parser.HasFieldsEnclosedInQuotes = true;
            while (!parser.EndOfData)
            {
                numLine++;
                //Processing row
                string[] fields;
                try { fields = parser.ReadFields(); }
                catch (MalformedLineException) { continue; }
                if (fields == null) { continue; }

                if (_parseParam.FiltDateTime)
                { //if filtering based on dateTime, process the DateTime string and check filters
                    string dateTime = fields[_inputTemplate.DateTimeInd];
                    if (dateTime.Length < 2) { continue; }
                    dateTime = fields[_inputTemplate.DateTimeInd].Substring(1, fields[_inputTemplate.DateTimeInd].Length - 2);
                    if (!CheckIncludeDate(dateTime, _parseParam.DateTimeFrom, _parseParam.DateTimeTo)) { continue; }
                }

                UpdateHostLog(fields);
                UpdateURILog(fields);
            }
        }

        private bool CheckIncludeDate(string dateTime, int[] dateTimeFrom, int[] dateTimeTo)
        {
            //Purpose: Checks if DateTime [as string] from parser is valid and if it falls between the user-specified range
            //Args:
                //dateTime: DateTime as string from current parsed line
                //dateTimeFrom: user specified minimum DateTime
                //dateTimeTo: user specified maximum DateTime
            //Result: true = valid state, false = error

            string[] dateTimeArr = dateTime.Split(":");
            if (dateTimeArr.Length != 4)
                return false;
            for (int i = 0; i < 4; i++)
            {
                int o;
                if (!int.TryParse(dateTimeArr[i], out o))
                    return false;
                if (o < dateTimeFrom[i] || o > dateTimeTo[i])
                    return false;
            }
            return true;
        }

        private void UpdateHostLog(string[] fields)
        {
            //Purpose: Method adds current parsed line from the input file to _logHost dictionary
            //Args:
                //fields: array of fields parsed from current line from input file
                //numLine: current line # from input file

            string hostName = fields[_inputTemplate.HostNameInd];

            UpdateDict(ref _logHost, hostName);
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

                UpdateDict(ref _logURI, URI);
            }
        }

        private void UpdateDict(ref Dictionary<string, int> dict,string key)
        {
            //Purpose: Method updates by-ref dictionary dict, adds +1 if key exists, or adds new key with value = 1 if not

            if (dict.ContainsKey(key))
                dict[key] += 1;
            else
                dict.Add(key, 1);
        }

        public void SortOutput()
        {
            //Purpose: Public method passes dictionaries to function to sort and output them
            bool validProc = SortAndOutput(_logHost, _parseParam.OutputFile, false, "Number of accesses to webserver per host:");
            if (validProc)
                validProc = SortAndOutput(_logURI, _parseParam.OutputFile, true, "Number of successful resource accesses per URI:");
            //if (validProc)
                //launch output file in default program
                //System.Diagnostics.Process.Start(@_parseParam.OutputFile);
        }

        private bool SortAndOutput(Dictionary<string, int> dict, string outputFile, bool append, string sectionHeader)
        {
            //Purpose: Function sorts the provided dictionary and outputs the key and value pair to the specified output file, space delimeted
            //Args: 
                //dict: input dictionary for sorting and output
                //outputFile: output path (incl. file name)
                //append: false to overwrite output file, true to append
                //sectionHeader: header text line for the section in the output file
            //Result: false = error state; true = valid state;

            StreamWriter writer;
            try { writer = new StreamWriter(@outputFile, append); }
            catch (System.IO.IOException) {
                string val = "Specified output file is in use. Please close the file and try again.";
                MessageBox.Show(val, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false; //error state
            }
            if (append) { writer.WriteLine(""); } //blank line between sections
            writer.WriteLine(sectionHeader);
            
            //sort descending and iterate through dictionary, writing to output file
            foreach (KeyValuePair<string, int> entry in dict.OrderByDescending(key => key.Value))
            {
                writer.WriteLine("{0} {1}", entry.Key, entry.Value);
            }
            writer.Close();
            return true; //valid state
        }
    }
}
