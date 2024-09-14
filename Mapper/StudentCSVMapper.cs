using College_managemnt_system.ClientModels;
using CsvHelper.Configuration;

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


