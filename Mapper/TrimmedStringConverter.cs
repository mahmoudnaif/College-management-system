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
}
