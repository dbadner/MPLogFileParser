using System;

namespace MPLogFileParser
{
    class CustomDateTime
    {
        //Purpose: Class for storing and setting custom datetime format dd:HH:mm:ss
        public DateTime CustDateTime { get; private set; } //dd:HH:mm:ss

        public bool SetDateTime(string dateTimeStr)
        {
            try { CustDateTime = DateTime.ParseExact(dateTimeStr, "dd:HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture); }
            catch (FormatException) { return false; }
            return true;
        }
    }
}
