using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace MPLogFileParser
{
    public class IO
    {
        //class properties
        private readonly ParseParameters _parseParam = new ParseParameters();
        private readonly string[] _delimiters;
        private readonly InputTemplate _inputTemplate;
        private Dictionary<string, int> _logHost;
        private Dictionary<string, int> _logURI;

        public IO(ParseParameters parseParam)
        {
            _parseParam = parseParam;
            _delimiters = new string[] { " " };
            _inputTemplate = new InputTemplate();
            _logHost = new Dictionary<string, int>();
            _logURI = new Dictionary<string, int>();
        }

        public void Read()
        {
            //Purpose: Method iteratively reads the specified input file defined in the class property _parseParam and parses valid fields into two class dictionaries

            TextFieldParser parser = new TextFieldParser(@_parseParam.InputFile);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_delimiters);
            parser.HasFieldsEnclosedInQuotes = true;
            while (!parser.EndOfData)
            {
                //Processing row
                string[] fields;
                try {fields = parser.ReadFields(); }
                catch (MalformedLineException) { continue; }
                if (fields == null) { continue; }
                ProcessRow(fields);
            }
            parser.Close();
            
        }

        public bool ProcessRow(string[] fields)
        {
            //Purpose: Method processes a single row of the input file and updates the dictionaries if valid
            //Result: Returns false if error/skipped, or true if successful
            if (_parseParam.FiltDateTime)
            { //if filtering based on dateTime, process the DateTime string and check filters
                string dateTime = fields[_inputTemplate.DateTimeInd];
                if (dateTime.Length < 2) { return false; }
                dateTime = fields[_inputTemplate.DateTimeInd].Substring(1, fields[_inputTemplate.DateTimeInd].Length - 2);
                if (!CheckIncludeDate(dateTime, _parseParam.DateTimeFrom, _parseParam.DateTimeTo)) { return false; }
            }

            UpdateHostLog(fields);
            UpdateURILog(fields);

            return true;
        }

        private bool CheckIncludeDate(string dateTimeStr, CustomDateTime dateTimeFrom, CustomDateTime dateTimeTo)
        {
            //Purpose: Checks if dateTimeStr [as string] from parser is valid and if it falls between the user-specified range
            //Args:
            //dateTimeStr: dateTimeStr as string from current parsed line
            //dateTimeFrom: user specified minimum DateTime
            //dateTimeTo: user specified maximum DateTime
            //Result: true = valid state, false = error

            CustomDateTime dateTime = new CustomDateTime();

            if (!dateTime.SetDateTime(dateTimeStr))
                return false; //error reading in datetime

            if (dateTime.CustDateTime < dateTimeFrom.CustDateTime || dateTime.CustDateTime > dateTimeTo.CustDateTime)
                return false;
            return true;
        }

        private void UpdateHostLog(string[] fields)
        {
            //Purpose: Method adds current parsed line from the input file to _logHost dictionary
            //Args:
            //fields: array of fields parsed from current line from input file

            string hostName = fields[_inputTemplate.HostNameInd];

            UpdateDict(_logHost, hostName);
        }

        public bool UpdateURILog(string[] fields)
        {
            //Purpose: Method adds current parsed line from the input file to _logURI dictionary
            //if the line meets the requirements (returnCode = 200 && GET statement)
            //Args:
            //fields: array of fields parsed from current line from input file
            //Result: false of doesn't 

            string requestURI = fields[_inputTemplate.RequestInd];
            string returnCode = fields[_inputTemplate.ReturnCodeInd];

            if (returnCode != "200")
                return false;

            //regular expression to parse Request URI
            var matches = Regex.Matches(requestURI, @"^(GET)\s(.+?)(\sHTTP\S+)?$");
            if (!(matches.Count() > 0)) {return false;}
            else
            {
                string URI = matches[0].Groups[2].Value;

                UpdateDict(_logURI, URI);
            }
            return true;
        }

        public void UpdateDict(Dictionary<string, int> dict, string key)
        {
            //Purpose: Method updates by-ref dictionary dict, adds +1 if key exists, or adds new key with value = 1 if not

            if (dict.ContainsKey(key))
                dict[key] += 1;
            else
                dict.Add(key, 1);
        }

        public bool SortOutput()
        {
            //Purpose: Public method passes dictionaries to function to sort and output them
            //Result: true if output successfully generated, false if not (file in use)
            bool validProc = SortAndOutput(_logHost, _parseParam.OutputFile, false, "Number of accesses to webserver per host:");
            if (validProc)
                validProc = SortAndOutput(_logURI, _parseParam.OutputFile, true, "Number of successful resource accesses per URI:");
            return validProc;
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

            try 
            {
                StreamWriter writer = new StreamWriter(@outputFile, append);
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
            catch (IOException)
            {
                string val = "Specified output file is in use. Please close the file and try again.";
                MessageBox.Show(val, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false; //error state
            }
        }
    }
}
