using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class StudentRepo : IStudentRepo
    {
        private readonly CollegeDBContext _context;
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly IMapper _mapper;

        public StudentRepo(CollegeDBContext context, UtilitiesRepo utilitiesRepo, IMapper mapper)
        {
            _context = context;
            _utilitiesRepo = utilitiesRepo;
            _mapper = mapper;
        }
        public async Task<CustomResponse<StudentDTO>> Add(StudentInputModel model)
        {
            string firstName = model.FirstName.Trim();
            string fatherName = model.FathertName.Trim();
            string grandFatherName = model.GrandfatherName.Trim();
            string lastName = model.LastName.Trim();
            string email = model.Email.Trim();
            string phone = model.Phone.Trim();
            string nationalNumber = model.NationalNumber.Trim();


            if (firstName == "" || fatherName == "" || grandFatherName == "" || lastName == "")
                return new CustomResponse<StudentDTO>(400, "Full name must be specefied");

            if (!_utilitiesRepo.IsValidEmail(email))
                return new CustomResponse<StudentDTO>(400, "Email is not valid");

            if (!_utilitiesRepo.IsValidNationalId(nationalNumber))
                return new CustomResponse<StudentDTO>(400, "NationalId is not valid");

            if (!_utilitiesRepo.IsValidPhoneNumber(phone))
                return new CustomResponse<StudentDTO>(400, "phone number is not valid");

            Account account = await _context.Accounts.FirstOrDefaultAsync(A => A.Email == email);

            if (account == null)
                return new CustomResponse<StudentDTO>(404, "No account associated with this email was found");

            if (account.Role != "student")
                return new CustomResponse<StudentDTO>(403, "the account associated with this email is not a student type");

            Student student = new Student()
            {
                FirstName = firstName,
                FathertName = fatherName,
                GrandfatherName = grandFatherName,
                LastName = lastName,
                NationalNumber = nationalNumber,
                Phone = phone,
                AccountId = account.AccountId,
            };
            if (model.EnrollmentDate != null)
                student.EnrollmentDate = (DateTime)model.EnrollmentDate;

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(201, "Student added successfully", studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Intenral server error");
            }
        }

        public async Task<CustomResponse<bool>> Delete(int studentId)
        {
            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);

            if (student == null)
                return new CustomResponse<bool>(404, "Student not found");

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Student deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Intenral server error");
            }
        }

        public async Task<CustomResponse<StudentDTO>> EditName(int studentId, FullNameInputModel model)
        {
            string firstName = model.FirstName.Trim();
            string fatherName = model.FathertName.Trim();
            string grandFatherName = model.GrandfatherName.Trim();
            string lastName = model.LastName.Trim();

            if (firstName == "" || fatherName == "" || grandFatherName == "" || lastName == "")
                return new CustomResponse<StudentDTO>(400, "Full name must be specefied");

            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);

            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student not found");

            student.FirstName = firstName;
            student.FathertName = fatherName;
            student.GrandfatherName = grandFatherName;
            student.LastName = lastName;
            try
            {
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(200, "Name edited successfully", studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Intenral server error");
            }
        }

        public async Task<CustomResponse<StudentDTO>> EditPhone(int studentId, string phoneNumber)
        {
            if (!_utilitiesRepo.IsValidPhoneNumber(phoneNumber))
                return new CustomResponse<StudentDTO>(400, "phone number is not valid");


            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);

            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student not found");

            if (student.Phone == phoneNumber)
                return new CustomResponse<StudentDTO>(409, $"Phone number is already: {phoneNumber}");

            student.Phone = phoneNumber;

            try
            {
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(200, "Phone number edited successfully", studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Intenral server error");
            }
        }

        public async Task<CustomResponse<StudentDTO>> GetStudentByEmail(string email)
        {

            Student student = await _context.Students.Join(_context.Accounts, s => s.AccountId, a => a.AccountId, (s, a) => new { Student = s, Account = a })
                                .Where(sa => sa.Account.Email == email)
                                .Select(sa => sa.Student)
                                .FirstOrDefaultAsync();


            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student not found");

            StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
            return new CustomResponse<StudentDTO>(200, "Student retreived", studentDTO);
        }

        public async Task<CustomResponse<List<StudentDTO>>> GetStudentSByYear(int year, TakeSkipModel model)
        {
            if (model.skip < 0 || model.take < 1)
                return new CustomResponse<List<StudentDTO>>(400, "Take must be more than 0 and skip must be bigger than or equal to 0");

            List<Student> students = await _context.Students.Where(S => S.EnrollmentDate.Year == year).OrderBy(S=>S.StudentId).Skip(model.skip).Take(model.take).ToListAsync();

            if (!students.Any())
                return new CustomResponse<List<StudentDTO>>(404, "No students were found");

            List<StudentDTO> studentsDTO = _mapper.Map<List<StudentDTO>>(students);

            return new CustomResponse<List<StudentDTO>>(200, "Students retreived", studentsDTO);
        }

        public async Task<CustomResponse<List<StudentDTO>>> SearchStudentsByName(string searchQuery, TakeSkipModel model)
        {
            if (searchQuery.Trim() == "")
                return new CustomResponse<List<StudentDTO>>(400, "Search query must be specefied");

            if (model.skip < 0 || model.take < 1)
                return new CustomResponse<List<StudentDTO>>(400, "Take must be more than 0 and skip must be bigger than or equal to 0");


            List<Student> students = await _context.Students.Where(S => (S.FirstName +" "+ S.FathertName + " " + S.GrandfatherName + " " + S.LastName).StartsWith(searchQuery)).OrderBy(S =>S.StudentId).Skip(model.skip).Take(model.take).ToListAsync();

            if (!students.Any())
                return new CustomResponse<List<StudentDTO>>(404, "No students were found");

            List<StudentDTO> studentsDTO = _mapper.Map<List<StudentDTO>>(students);

            return new CustomResponse<List<StudentDTO>>(200, "Students retreived", studentsDTO);

        }

        public async Task<CustomResponse<bool>> calculateALLStudentsCGPA_totalHours()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(GetUpdateQuery());
                return new CustomResponse<bool>(200, "CGPA and total hours updated successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

            
        }

        public async Task<CustomResponse<StudentDTO>> calculateStudentCGPA_totalHours(int studentId)
        {
            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);
            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student does not exsit");

            var result = await (from SC in _context.StudentCourses
                                where SC.StudentId == studentId && SC.Status == "completed"
                                join CS in _context.Coursesemesters on SC.CourseSemesterId equals CS.CourseSemesterId
                                join C in _context.Courses on CS.CourseId equals C.CourseId
                                select new
                                {
                                    grade = SC.Grade,
                                    totalHours = C.Credits
                                    
                                }


                         ).ToListAsync();
           
            if (!result.Any())
            {
                student.Cgpa = 0;
                student.TotalHours = 0;
            }
            else
            {
                student.TotalHours = result.Sum(R => R.totalHours);
                student.Cgpa = result.Sum(R => R.totalHours * GetGradePoints(R.grade)) / student.TotalHours;
            }
            try
            {
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(200, "CGPA and total credit horus calcualted successfully", studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Internal server error");
            }
            

        }

        private string GetUpdateQuery()
        {
            return @"UPDATE s
                    SET 
                        s.CGPA = COALESCE(cgpa_results.CGPA, 0),
                        s.totalHours = COALESCE(cgpa_results.TotalCreditHours, 0)
                    FROM 
                        Students s
                    LEFT JOIN 
                        (SELECT 
                            sc.StudentID,
                            SUM(c.Credits) AS TotalCreditHours,
                            SUM(CASE 
                                    WHEN sc.Grade = 'A' THEN 4 * c.Credits
                                    WHEN sc.Grade = 'A-' THEN 3.7 * c.Credits
                                    WHEN sc.Grade = 'B+' THEN 3.3 * c.Credits
                                    WHEN sc.Grade = 'B' THEN 3 * c.Credits
                                    WHEN sc.Grade = 'B-' THEN 2.7 * c.Credits
                                    WHEN sc.Grade = 'C+' THEN 2.3 * c.Credits
                                    WHEN sc.Grade = 'C' THEN 2 * c.Credits
                                    WHEN sc.Grade = 'C-' THEN 1.7 * c.Credits
                                    WHEN sc.Grade = 'D+' THEN 1.3 * c.Credits
                                    WHEN sc.Grade = 'D' THEN 1 * c.Credits
                                    WHEN sc.Grade = 'D-' THEN 0.7 * c.Credits
                                    ELSE 0
                                END) / CAST(SUM(c.Credits) AS FLOAT) AS CGPA
                        FROM 
                            StudentCourses sc
                        INNER JOIN 
                            Coursesemesters cs ON sc.CourseSemesterID = cs.CourseSemesterID
                        INNER JOIN 
                            Courses c ON cs.CourseID = c.CourseID
                        WHERE 
                            sc.Status = 'completed'
                        GROUP BY 
                            sc.StudentID
                        ) AS cgpa_results 
                        ON s.StudentID = cgpa_results.StudentID;
                    ";
        }

        private float GetGradePoints(string grade)
        {
            return grade switch
            {
                "A" => 4,
                "A-" => 3.7f,
                "B+" => 3.3f,
                "B" => 3,
                "B-" => 2.7f,
                "C+" => 2.3f,
                "C" => 2,
                "C-" => 1.7f,
                "D+" => 1.3f,
                "D" => 1,
                "D-" => 0.7f,
                _ => 0,
            };
        }
    }
}
