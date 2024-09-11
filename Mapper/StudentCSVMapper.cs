using College_managemnt_system.ClientModels;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace College_managemnt_system.Mapper
{
    public class StudentCSVMapper : ClassMap<StudentsInputModelCSV>
    {
        public StudentCSVMapper() {
            Map(S => S.Email).TypeConverter<TrimmedStringConverter>();
            Map(S => S.FirstName).TypeConverter<TrimmedStringConverter>();
            Map(S => S.FathertName).TypeConverter<TrimmedStringConverter>();
            Map(S => S.GrandfatherName).TypeConverter<TrimmedStringConverter>();
            Map(S => S.LastName).TypeConverter<TrimmedStringConverter>();
            Map(S => S.Phone).TypeConverter<TrimmedStringConverter>();
            Map(S => S.NationalNumber).TypeConverter<TrimmedStringConverter>();
        }
    }
}

public class TrimmedStringConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text?.Trim();
    }

}
