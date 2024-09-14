using College_managemnt_system.ClientModels;
using CsvHelper.Configuration;

namespace College_managemnt_system.Mapper
{
    public class ProfCSVMapper : ClassMap<ProfessorInputModelCSV>
    {
        public ProfCSVMapper()
        {
            Map(P => P.FirstName).TypeConverter<TrimmedStringConverter>();
            Map(P => P.LastName).TypeConverter<TrimmedStringConverter>();
            Map(P => P.NationalNumber).TypeConverter<TrimmedStringConverter>();
            Map(P => P.Phone).TypeConverter<TrimmedStringConverter>();
            Map(P => P.email).TypeConverter<TrimmedStringConverter>();
            Map(P => P.DepartmentName).TypeConverter<TrimmedStringConverter>();
            Map(P => P.DepartmentId);
            Map(P => P.HiringDate);
        }
    }
}
