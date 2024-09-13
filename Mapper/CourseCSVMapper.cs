using College_managemnt_system.ClientModels;
using CsvHelper.Configuration;

namespace College_managemnt_system.Mapper
{
    public class CourseCSVMapper : ClassMap<CoursesInputModelCSV>
    {
        public CourseCSVMapper()
        {
            Map(C => C.CourseName).TypeConverter<TrimmedStringConverter>();
            Map(C => C.CourseCode).TypeConverter<TrimmedStringConverter>();
            Map(C => C.Credits);
            Map(C => C.DepartmentId);
            Map(C => C.DepartmentName).TypeConverter<TrimmedStringConverter>();
            Map(C => C.PrereqsCoursesCodes).Convert(R =>
            {
                string prereqs = R.Row.GetField("Prereqs");

                if (prereqs == null || prereqs == "")
                    return [];

                return prereqs.Split(";").Select(P => P.Trim()).ToList();

            });
        }
    }
}
