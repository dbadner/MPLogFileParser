namespace MPLogFileParser
{
    class ValidationProperties
    {
        //Purpose: Class contains readonly validation properties for DateTime validation checks for UI
        public readonly int[] minDateTime = { 0, 0, 0, 0 };
        public readonly int[] maxDateTime = { 31, 23, 59, 59 };
        public readonly int numVal = 4; //DD:HH:MM:SS
    }
}
