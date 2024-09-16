using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace College_managemnt_system.Mapper
{
    public class TrimmedStringConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            return text != null ? text.Trim() : string.Empty;
        }

    }

    public class StringDateConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == null || text.Trim() == "")
                return DateTime.Now;

            if (DateTime.TryParse(text, out var date))
                return date;

            return DateTime.MinValue; //this will indicate that there is an error with the datetime parsing (I tried to throw an exception but it doesn't get caught :>)
        }

    }
}
