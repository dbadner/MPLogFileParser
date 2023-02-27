namespace MPLogFileParser.NUnitTests
{
    public class IOTests
    {
        private ParseParameters parseParam = new ParseParameters
        {
            FiltDateTime = true,
            DateTimeFrom = new CustomDateTime(),
            DateTimeTo = new CustomDateTime(),
        };
        private IO io;

        public IOTests()
        {
            parseParam.DateTimeFrom.SetDateTime("30:14:00:00");
            parseParam.DateTimeTo.SetDateTime("30:21:59:59");
            io = new IO(parseParam);
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void ProcessRow_InvalidChecks()
        {

            //datetime out of range
            string[] fields = { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "GET /Consumer.html HTTP/1.0", "200", "1325" };
            bool ret = io.ProcessRow(fields);
            Assert.IsFalse(ret);

            //extra white space
            fields = new string[] { "query2.lycos.cs.cmu.edu", "", "[30:15:53:36]", "GET /Consumer.html HTTP/1.0", "200", "1325" };
            ret = io.ProcessRow(fields);
            Assert.IsFalse(ret);

        }

        [Test]
        public void ProcessRow_ValidChecks()
        {

            //Valid line
            string[] fields = { "query2.lycos.cs.cmu.edu", "[30:15:53:36]", "GET /Consumer.html HTTP/1.0", "200", "1325" };
            bool ret = io.ProcessRow(fields);
            Assert.IsTrue(ret);

        }

        [Test]
        public void UpdateURILog_SuccessfulChecks()
        {
            //valid return code 200 and valid GET
            string[] fields = { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "GET /Consumer.html HTTP/1.0", "200", "1325" };
            bool ret = io.UpdateURILog(fields);
            Assert.IsTrue(ret);

            //valid but with messy URI, spaces
            fields = new string[] { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "GET /cgi-bin/waisgate?port=210&ip_address=earth1&database_name=/usr1/comwais/indexes/HTDOCS&headline=Query Report for this Search&type=TEXT&docid=\u0002\u001c/usr1/comwais/indexes/HTDOCS\u0003\u00150 0 /tmp/08300405.g3c \u0001 HTTP/1.0", "200", "1325" };
            ret = io.UpdateURILog(fields);
            Assert.IsTrue(ret);

            //valid but with no HTTP
            fields = new string[] { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "GET /OWOW/PubList/", "200", "1325" };
            ret = io.UpdateURILog(fields);
            Assert.IsTrue(ret);
        }

        [Test]
        public void UpdateURILog_UnsuccessfulChecks()
        {
            //invalid return code 400
            string[] fields = { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "GET /Consumer.html HTTP/1.0", "400", "1325" };
            bool ret = io.UpdateURILog(fields);
            Assert.IsFalse(ret);

            //missing GET, test Regex
            fields = new string[] { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "/Consumer.html HTTP/1.0", "200", "1325" };
            ret = io.UpdateURILog(fields);
            Assert.IsFalse(ret);

            //POST instead of GET, test Regex
            fields = new string[] { "query2.lycos.cs.cmu.edu", "[29:23:53:36]", "POST /Consumer.html HTTP/1.0", "200", "1325" };
            ret = io.UpdateURILog(fields);
            Assert.IsFalse(ret);
        }

        [Test]
        public void UpdateDict_CheckValueAccumulation()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            string key1 = "wingerta.mtv.gtegsc.com";
            string key2 = "142.140.32.21";
            string key3 = "r02dg13.r02.epa.gov";

            //initiate dictionary for key1 and key2 at ficticious counts
            dict.Add(key1, 3);
            dict.Add(key2, 5);

            io.UpdateDict(dict, key1);
            Assert.That(dict[key1], Is.EqualTo(4)); //confirm key1 value has increased by 1
            Assert.That(dict[key2], Is.EqualTo(5)); //confirm key2 value has not increased}
            //add key3, recheck all values
            io.UpdateDict(dict, key3);
            Assert.That(dict[key1], Is.EqualTo(4));
            Assert.That(dict[key2], Is.EqualTo(5));
            Assert.That(dict[key3], Is.EqualTo(1));
        }
    }
}
