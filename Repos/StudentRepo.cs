using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace College_managemnt_system.Repos
{
    public class StudentRepo : IStudentRepo
    {
        private readonly CollegeDBContext _context;
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly IMapper _mapper;

        public StudentRepo(CollegeDBContext context,UtilitiesRepo utilitiesRepo,IMapper mapper)
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
                return new CustomResponse<StudentDTO>(201, "Student added successfully",studentDTO);
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
            student.FathertName= fatherName;
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

        public async  Task<CustomResponse<StudentDTO>> GetStudentByEmail(string email)
        {

            Student student = await _context.Students.Join(_context.Accounts,s => s.AccountId,a => a.AccountId,(s, a) => new { Student = s, Account = a })
                                .Where(sa => sa.Account.Email == email) 
                                .Select(sa => sa.Student)              
                                .FirstOrDefaultAsync();


            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student not found");

            StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
            return new CustomResponse<StudentDTO>(200, "Student retreived", studentDTO);
        }

        public async Task<CustomResponse<IEnumerable<StudentDTO>>> GetStudentSByYear(int year, TakeSkipModel model)
        {
            if (model.skip < 0 || model.take < 1)
                return new CustomResponse<IEnumerable<StudentDTO>>(400, "Take must be more than 0 and skip must be bigger than or equal to 0");

            IEnumerable<Student> students = _context.Students.Where(S => S.EnrollmentDate.Year == year).Skip(model.skip).Take(model.take);

            if (students.Count() == 0)
                return new CustomResponse<IEnumerable<StudentDTO>>(404, "No students were found");

            IEnumerable<StudentDTO> studentsDTO = _mapper.Map<IEnumerable<StudentDTO>>(students);

            return new CustomResponse<IEnumerable<StudentDTO>>(200, "Students retreived", studentsDTO);
        }

        public async Task<CustomResponse<IEnumerable<StudentDTO>>> SearchStudentsByName(string searchQuery, TakeSkipModel model)
        {
            if (searchQuery.Trim() == "")
                return new CustomResponse<IEnumerable<StudentDTO>>(400, "Search query must be specefied");

            if (model.skip < 0 || model.take < 1)
                return new CustomResponse<IEnumerable<StudentDTO>>(400, "Take must be more than 0 and skip must be bigger than or equal to 0");


            IEnumerable<Student> students = _context.Students.Where(S => $"{S.FirstName} {S.FathertName} {S.GrandfatherName} {S.LastName}".StartsWith(searchQuery)).Skip(model.skip).Take(model.take);

            if (students.Count() == 0)
                return new CustomResponse<IEnumerable<StudentDTO>>(404, "No students were found");

            IEnumerable<StudentDTO> studentsDTO = _mapper.Map<IEnumerable<StudentDTO>>(students);

            return new CustomResponse<IEnumerable<StudentDTO>>(200, "Students retreived", studentsDTO);

        }
    }
}
