using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Mapper;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using System.Globalization;

namespace College_managemnt_system.Repos
{
    public class CSVParser : ICSVParser
    {
        private readonly CollegeDBContext _context;
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public CSVParser(CollegeDBContext context,UtilitiesRepo utilitiesRepo,IMapper mapper, IPasswordService passwordService)
        {
            _context = context;
            _utilitiesRepo = utilitiesRepo;
            _mapper = mapper;
            _passwordService = passwordService;
        }
        public async Task<CustomResponse<List<StudentErrorSheet>>> AddStudents(IFormFile file) //Code can be simplified by distributing the steps on mutliple functions
        {

            if (file == null || file.Length == 0)
                return new CustomResponse<List<StudentErrorSheet>>(400,"File must be specefied");
            

            string? fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (fileExtension == null)
                return new CustomResponse<List<StudentErrorSheet>>(400,"File does not have an extension");

            if (fileExtension != ".csv")
                return new CustomResponse<List<StudentErrorSheet>>(400, "The file specefied must be a csv file");

            using var reader = new StreamReader(file.OpenReadStream());

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<StudentCSVMapper>(); //used to trim the strings coming from the csv file

            var studentsInputModel = new List<StudentsInputModelCSV>();

            var errorSheetList = new List<StudentErrorSheet>();

            int rowNumber = 2;
            await foreach (var student in csv.GetRecordsAsync<StudentsInputModelCSV>())
            {
                 StudentErrorSheet studentErrorSheet = new StudentErrorSheet(rowNumber);

                if (!_utilitiesRepo.IsValidEmail(student.Email))
                {
                    studentErrorSheet.Email = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (!_utilitiesRepo.IsValidPhoneNumber(student.Phone))
                {
                    studentErrorSheet.Phone = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (!_utilitiesRepo.IsValidNationalId(student.NationalNumber))
                {
                    studentErrorSheet.NationalNumber = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.FirstName == "") {
                    studentErrorSheet.FirstName = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.FathertName == "") {
                    studentErrorSheet.FathertName = false;
                    studentErrorSheet.numberOfErrors++;
                }
                if (student.GrandfatherName == "") {
                    studentErrorSheet.GrandfatherName = false;
                    studentErrorSheet.numberOfErrors++;
                } 
                if (student.LastName == "") {
                    studentErrorSheet.LastName = false;
                    studentErrorSheet.numberOfErrors++;
                }


                if(studentErrorSheet.numberOfErrors != 0)
                {
                    errorSheetList.Add(studentErrorSheet);
                }
                else
                {
                    student.Password = _passwordService.HashPassword(new Account(),student.PasswordAsString);
                    studentsInputModel.Add(student);
                }
                
                 rowNumber++; 
                Console.WriteLine($"{student.Email} {student.FirstName} {student.FathertName} {student.GrandfatherName} {student.LastName} {student.Phone} {student.NationalNumber} {student.PasswordAsString}");
            }

            if (errorSheetList.Any())
                return new CustomResponse<List<StudentErrorSheet>>(400, "Invalid data please check the errorsheet for more info", errorSheetList);

            if (!studentsInputModel.Any())
                return new CustomResponse<List<StudentErrorSheet>>(404, "NO studetns were found in the csv file");

            using var transaction = await _context.Database.BeginTransactionAsync();

        
            try
            {
                List<Account> accounts = _mapper.Map<List<Account>>(studentsInputModel);

                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();

                var students = _mapper.Map<List<Student>>(studentsInputModel);

                for (int i = 0; i < students.Count; i++)
                {
                    students[i].AccountId = accounts[i].AccountId;
                }

                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();

                //TODO. send emails to the students of their password.

                await transaction.CommitAsync();
                return new CustomResponse<List<StudentErrorSheet>>(201, "Students added successfully");
            }
            catch
            {
                await transaction.RollbackAsync();
                return new CustomResponse<List<StudentErrorSheet>>(500, "Internal server error");
            }
        } 
        public Task<CustomResponse<bool>> AddTeachingAssistances(IFormFile file)
        {
            throw new NotImplementedException();
        }
        public async Task<CustomResponse<List<ProfErrorSheet>>> AddProfessors(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new CustomResponse<List<ProfErrorSheet>>(400, "File must be specefied");


            string? fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (fileExtension == null)
                return new CustomResponse<List<ProfErrorSheet>>(400, "File does not have an extension");

            if (fileExtension != ".csv")
                return new CustomResponse<List<ProfErrorSheet>>(400, "The file specefied must be a csv file");

            using var reader = new StreamReader(file.OpenReadStream());

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<ProfCSVMapper>();

            var profInputModel = new List<ProfessorInputModelCSV>();

            var profErrorSheetList = new List<ProfErrorSheet>();

            int rowNumber = 2;
            var departmentsCache = new Dictionary<string, int>(); //Cache like variable to prevent multiple requests for the same department.

            await foreach (var prof in csv.GetRecordsAsync<ProfessorInputModelCSV>())
            {
                Console.WriteLine($"{prof.email} {prof.FirstName} {prof.LastName} {prof.NationalNumber} {prof.Phone} {prof.HiringDate} {prof.DepartmentName} {prof.DepartmentId}");

                var profErrorSheet = new ProfErrorSheet(rowNumber);
                if(prof.FirstName == "")
                {
                    profErrorSheet.FirstName = false;
                    profErrorSheet.numberOfErrors++;
                }
                if(prof.LastName == "")
                {
                    profErrorSheet.LastName= false;
                    profErrorSheet.numberOfErrors++;
                }
                if(!_utilitiesRepo.IsValidPhoneNumber(prof.Phone))
                {
                    profErrorSheet.Phone = false;
                    profErrorSheet.numberOfErrors++;
                }
                if(!_utilitiesRepo.IsValidEmail(prof.email))
                {
                    profErrorSheet.email = false;
                    profErrorSheet.numberOfErrors++;
                }
                if(!_utilitiesRepo.IsValidNationalId(prof.NationalNumber))
                {
                    profErrorSheet.NationalNumber = false;
                    profErrorSheet.numberOfErrors++;
                }

                if (prof.DepartmentId < 0) // nested if else statements is ugly and needs to be cleaned.
                {
                    profErrorSheet.DepartmentId = false;
                    profErrorSheet.numberOfErrors++;
                }
                else if (prof.DepartmentId == 0)
                {

                    if (prof.DepartmentName == "")
                    {
                        profErrorSheet.DepartmentName = false;
                        profErrorSheet.numberOfErrors++;
                    }
                    else 
                    {


                        if (departmentsCache.TryGetValue(prof.DepartmentName, out var departmentId))
                        {
                            prof.DepartmentId = departmentId;
                        }
                        else
                        {
                            Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentName == prof.DepartmentName);

                            if (department != null)
                            {
                                prof.DepartmentId = department.DepartmentId;
                                departmentsCache.Add(prof.DepartmentName, department.DepartmentId);
                            }
                            else
                            {
                                profErrorSheet.DepartmentId = false;
                                profErrorSheet.numberOfErrors++;
                            }
                        }

                    }




                }

                if(profErrorSheet.numberOfErrors > 0)
                {
                    profErrorSheetList.Add(profErrorSheet);
                }
                else
                {
                    prof.Password = _passwordService.HashPassword(new Account(), prof.PasswordAsString);
                    profInputModel.Add(prof);
                }

                rowNumber++;
            }
            departmentsCache = null;

            if (profErrorSheetList.Any())
                return new CustomResponse<List<ProfErrorSheet>>(400, "Invalid data please check the errorsheet for more info", profErrorSheetList);

            if (!profInputModel.Any())
                return new CustomResponse<List<ProfErrorSheet>>(400, "No courses found in the csv file");


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                List<Account> accounts = _mapper.Map<List<Account>>(profInputModel);

                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();

                List<Professor> professors = _mapper.Map<List<Professor>>(profInputModel);

                for (int i = 0; i < professors.Count; i++)
                {
                    professors[i].AccountId = accounts[i].AccountId;
                }


                await _context.Professors.AddRangeAsync(professors);
                await _context.SaveChangesAsync();

                //TODO. send emails to the professors of their password.

                await transaction.CommitAsync();

                return new CustomResponse<List<ProfErrorSheet>>(201, "Professors added succesfully");

            }
            catch
            {
                await transaction.RollbackAsync();
                return new CustomResponse<List<ProfErrorSheet>>(500, "Internal server error");
            }
            

        }
        public async Task<CustomResponse<List<CourseErrorSheet>>> AddCourses(IFormFile file)
        {
          

            if (file == null || file.Length == 0)
                return new CustomResponse<List<CourseErrorSheet>>(400, "File must be specefied");


            string? fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (fileExtension == null)
                return new CustomResponse<List<CourseErrorSheet>>(400, "File does not have an extension");

            if (fileExtension != ".csv")
                return new CustomResponse<List<CourseErrorSheet>>(400, "The file specefied must be a csv file");

            using var reader = new StreamReader(file.OpenReadStream());

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<CourseCSVMapper>();

            var coursesInputModel = new List<CoursesInputModelCSV>();

            var coursesErrorSheetList = new List<CourseErrorSheet>();

            int rowNumber = 2;
            var departmentsCache = new Dictionary<string, int>(); //Cache like variable to prevent multiple requests for the same department.
            
            await foreach (var course in csv.GetRecordsAsync<CoursesInputModelCSV>())
            {
                var courseErrorSheet = new CourseErrorSheet(rowNumber, course.PrereqsCoursesCodes.Count);

                if(course.CourseName == "")
                {
                    courseErrorSheet.CourseName = false;
                    courseErrorSheet.numberOfErrors++;
                } 
                if(course.CourseCode == "")
                {
                    courseErrorSheet.CourseCode = false;
                    courseErrorSheet.numberOfErrors++;
                }
                if(course.Credits < 0)
                {
                    courseErrorSheet.Credits = false;
                    courseErrorSheet.numberOfErrors++;
                }
                if(course.DepartmentId < 0)
                {
                    courseErrorSheet.DepartmentId = false;
                    courseErrorSheet.numberOfErrors++;
                }
                else if(course.DepartmentId == 0)
                {

                    if (course.DepartmentName == "")
                    {
                        courseErrorSheet.DepartmentName = false;
                        courseErrorSheet.numberOfErrors++;
                    }
                    else // nested if else statements is ugly and needs to be cleaned.
                    {
                       

                            if (departmentsCache.TryGetValue(course.DepartmentName, out var departmentId))
                            {
                                course.DepartmentId = departmentId;
                                Console.WriteLine("I'm a good cache and i'm working HEHE.");
                            }
                            else
                            {
                                Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentName == course.DepartmentName);

                                if (department != null)
                                {
                                    course.DepartmentId = department.DepartmentId;
                                    departmentsCache.Add(course.DepartmentName, department.DepartmentId);
                                }
                                else
                                {
                                    courseErrorSheet.DepartmentId = false;
                                    courseErrorSheet.numberOfErrors++;
                                }
                            }

                    }


                }


                for(int i = 0; i < course.PrereqsCoursesCodes.Count; i++)
                {
                    if (course.PrereqsCoursesCodes[i] == "")
                    {
                        courseErrorSheet.PrereqsCoursesCodes[i] = false;
                        courseErrorSheet.numberOfErrors++;
                    }
                }

                if (courseErrorSheet.numberOfErrors != 0) {
                    coursesErrorSheetList.Add(courseErrorSheet);
                }
                else
                {
                    coursesInputModel.Add(course);
                }
                rowNumber++;

                Console.WriteLine($"{course.CourseName} {course.CourseCode} {course.DepartmentName} {course.Credits} {string.Join(";", course.PrereqsCoursesCodes)} {course.DepartmentId}");

            }
            departmentsCache = null; //Letting the garbage collector reclaim the memory for efficiency :>.

            if (coursesErrorSheetList.Any())
                return new CustomResponse<List<CourseErrorSheet>>(400, "Invalid data please check the errorsheet for more info", coursesErrorSheetList);

            if (!coursesInputModel.Any())
                return new CustomResponse<List<CourseErrorSheet>>(400, "No courses found in the csv file");

           using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                List<Course> courses = _mapper.Map<List<Course>>(coursesInputModel);

                await _context.Courses.AddRangeAsync(courses);
                await _context.SaveChangesAsync();


                var CoursesCache = new Dictionary<string, int>(); //Another caching mechanism.
                foreach (Course course in courses) {
                    CoursesCache.Add(course.CourseCode, course.CourseId);
                }

                List<Prereq> prereqsCourses = [];
                rowNumber = 2;
                for(int i = 0; i< courses.Count; i++)
                {
                    CourseErrorSheet courseErrorSheet = new CourseErrorSheet(rowNumber, coursesInputModel[i].PrereqsCoursesCodes.Count);
                    int PrereqsNum = 0;
                    foreach (var prereqsCourseCode in coursesInputModel[i].PrereqsCoursesCodes)
                    {
                        Prereq prereq = new Prereq() { CourseId = courses[i].CourseId };

                        if (CoursesCache.TryGetValue(prereqsCourseCode,out var courseId))
                        {
                            prereq.PrereqCourseId = courseId;
                            prereqsCourses.Add(prereq);
                        }
                        else
                        {
                            Course prereqCourse = await _context.Courses.FirstOrDefaultAsync(C => C.CourseCode == prereqsCourseCode);
                            if(prereqCourse != null)
                            {
                                prereq.PrereqCourseId = prereqCourse.CourseId;
                                prereqsCourses.Add(prereq);
                            }
                            else
                            {
                                courseErrorSheet.PrereqsCoursesCodes[PrereqsNum] = false;
                                courseErrorSheet.numberOfErrors++;
                              
                            }
                        }
                        PrereqsNum++;
                    }

                    if (courseErrorSheet.numberOfErrors !=0)
                        coursesErrorSheetList.Add(courseErrorSheet);


                }

                if (coursesErrorSheetList.Any())
                    return new CustomResponse<List<CourseErrorSheet>>(400, "Some Preqsuits courses do not exist please check the error sheet", coursesErrorSheetList);

                if (prereqsCourses.Any())
                {
                    await _context.Prereqs.AddRangeAsync(prereqsCourses);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new CustomResponse<List<CourseErrorSheet>>(201, "Courses and prequisits added successfully");

            }
            catch
            {
                await  transaction.RollbackAsync();
                return new CustomResponse<List<CourseErrorSheet>>(500, "Internal server error");
            }
           
        }
    }
}
