using College_managemnt_system.ClientModels;
using CsvHelper.Configuration;

namespace College_managemnt_system.Mapper
{
    public class TaCSVMapper : ClassMap<TeachingAssistanceInputModelCSV>
    {
        public TaCSVMapper()
        {
            Map(T => T.FirstName).TypeConverter<TrimmedStringConverter>();
            Map(T => T.LastName).TypeConverter<TrimmedStringConverter>();
            Map(T => T.email).TypeConverter<TrimmedStringConverter>();
            Map(T => T.Phone).TypeConverter<TrimmedStringConverter>();
            Map(T => T.NationalNumber).TypeConverter<TrimmedStringConverter>();
            Map(T => T.HiringDate).TypeConverter<StringDateConverter>();
        }
    }
}
