using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MPLogFileParser
{
    class Reader
    {
        //class properties
        private readonly string _inputFile;
        private readonly char _parser;
        private readonly InputTemplate _inputTemplate;
        private Dictionary<string, int> _logHost;
        private Dictionary<string, int> _logURI;
           
        public Reader(string inputFile)
        {
            _inputFile = inputFile;
            _parser = ' ';
            _inputTemplate = new InputTemplate();
            _logHost = new Dictionary<string, int>();
            _logURI = new Dictionary<string, int>();
        }

        public void Read()
        {
            //method variables
            const int bufferSize = 1024;
            const int maxLines = 10000000; //Set maximum # of lines allowed by the program

            using (var fileStream = File.OpenRead(_inputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
            {
                string line;
                int numLine = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    numLine++;
                    if (numLine > maxLines)
                        throw new IndexOutOfRangeException("Maximum number of lines in input file surpassed.");

                    List<string> words = new List<string>();
                    words = ParseLine(line);
                    UpdateDictionaries(words, numLine);

                }
            }
        }

        private List<string> ParseLine(string line)
        {
            // string[] words = line.Split(_parser);
            var words = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            words[_inputTemplate.RequestInd] = words[_inputTemplate.RequestInd]
                .Remove(0, 1)
                .Remove(words[_inputTemplate.RequestInd].Length - 2, 1);

            return words;
        }
        private void UpdateDictionaries(List<string> words, int numLine)
        {
            string hostName;
            if (words.Count <= _inputTemplate.HostNameInd)
                throw new IndexOutOfRangeException("Could not find host name on line " + numLine + ".");
            else
                hostName = words[_inputTemplate.HostNameInd];

            if (_logHost.ContainsKey(hostName))
                _logHost[hostName] += 1;
            else
            {
                _logHost.Add(hostName, 1);
            }
        }

    }
}
