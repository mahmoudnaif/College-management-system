using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Numerics;

namespace College_managemnt_system.Repos
{
    public class ProfessorsRepo : IProfessorsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly UtilitiesRepo _utilities;
        private readonly IMapper _mapper;

        public ProfessorsRepo(CollegeDBContext context, UtilitiesRepo utilities, IMapper mapper)
        { 
            _context = context;
            _utilities = utilities;
            _mapper = mapper;
        }
        public async Task<CustomResponse<ProfessorDTO>> Add(ProfessorInputModel model)
        {
            string firstName = model.FirstName.Trim();
            string lastName = model.LastName.Trim();
            string phone = model.Phone.Trim();
            string nationalNumber = model.NationalNumber.Trim();
            DateTime hiringDate = model.HiringDate;
            int departmentId = model.DepartmentId;
            string email = model.email.Trim();

            if (!_utilities.IsValidEmail(email))
                return new CustomResponse<ProfessorDTO>(400, "Invalid email format");

            if (!_utilities.IsValidNationalId(nationalNumber))
                return new CustomResponse<ProfessorDTO>(400,"National number is not valid");

            if (!_utilities.IsValidPhoneNumber(phone))
                return new CustomResponse<ProfessorDTO>(400, "Invalid phone number");

            if (firstName == "" || lastName == "")
                return new CustomResponse<ProfessorDTO>(400, "Name must be specefied");

            Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentId == departmentId);
            if (department == null)
                return new CustomResponse<ProfessorDTO>(404, "Department does not exist");

            Account account = await _context.Accounts.FirstOrDefaultAsync(A => A.Email == email);
            if (account == null)
                return new CustomResponse<ProfessorDTO>(404, "there is no account associated with this email");

            if (account.Role != "prof")
                return new CustomResponse<ProfessorDTO>(403, "Account associated must be a prof account");

            Professor professorDuplicate = await _context.Professors.FirstOrDefaultAsync(P => P.AccountId == account.AccountId);
            if (professorDuplicate != null)
                return new CustomResponse<ProfessorDTO>(409, "Email already associated with another professor");

            Professor professor = new Professor()
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                NationalNumber = nationalNumber,
                HiringDate = hiringDate,
                DepartmentId = departmentId,
                AccountId = account.AccountId,
            };

            try
            {
                _context.Professors.Add(professor);
                await _context.SaveChangesAsync();
                ProfessorDTO professorDTO = _mapper.Map<ProfessorDTO>(professor);
                return new CustomResponse<ProfessorDTO>(201, "Professor added succesfuuly");
            }
            catch
            {
                return new CustomResponse<ProfessorDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<bool>> Delete(int profId)
        {
            Professor professor = await _context.Professors.FirstOrDefaultAsync(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<bool>(404, "Professsor is not found");

            try
            {
                _context.Professors.Remove(professor);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Professor deleted succesfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<ProfessorDTO>> EditDepartment(int profId,int departmentId)
        {
            Department department = await _context.Departments.FirstOrDefaultAsync(D =>D.DepartmentId == departmentId);
            if (department == null)
                return new CustomResponse<ProfessorDTO>(404, "Department does not exist");

            Professor professor = await _context.Professors.FirstOrDefaultAsync(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.DepartmentId == departmentId)
                return new CustomResponse<ProfessorDTO>(409, $"Department is already set to: {department.DepartmentName}.");

            professor.DepartmentId = departmentId;

            try
            {
                await _context.SaveChangesAsync();
                ProfessorDTO professorDTO = _mapper.Map<ProfessorDTO>(professor);
                return new CustomResponse<ProfessorDTO>(200, "Department edited succesfuuly",professorDTO);
            }
            catch
            {
                return new CustomResponse<ProfessorDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<ProfessorDTO>> EditHiringDate(int profId,DateTime date)
        {
            Professor professor = await _context.Professors.FirstOrDefaultAsync(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.HiringDate.Date == date.Date)
                return new CustomResponse<ProfessorDTO>(409, $"Date is already set to: {date.Date}");

            professor.HiringDate = date;

            try
            {
                await _context.SaveChangesAsync();
                ProfessorDTO professorDTO = _mapper.Map<ProfessorDTO>(professor);
                return new CustomResponse<ProfessorDTO>(200, "Hiring date edited succesfuuly", professorDTO);
            }
            catch
            {
                return new CustomResponse<ProfessorDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<ProfessorDTO>> EditName(int profId,NameInputModel model)
        {
            string firstName = model.firstName.Trim();
            string lastName = model.lastName.Trim();

            if (firstName== "" || lastName== "")
                return new CustomResponse<ProfessorDTO>(400, "Name must be specefied");

            Professor professor = await _context.Professors.FirstOrDefaultAsync(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.FirstName == firstName && professor.LastName == lastName)
                return new CustomResponse<ProfessorDTO>(409, $"Name is already set to: {firstName} {lastName}");

            professor.FirstName = firstName;
            professor.LastName = lastName;

            try{
                await _context.SaveChangesAsync();
                ProfessorDTO professorDTO = _mapper.Map<ProfessorDTO>(professor);
                return new CustomResponse<ProfessorDTO>(200, "Name edited succesfuuly", professorDTO);
            }
            catch
            {
                return new CustomResponse<ProfessorDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<ProfessorDTO>> EditPhoneNumber(int profId,string newPhoneNumber)
        {
            string phoneNumber = newPhoneNumber.Trim();

            if (!_utilities.IsValidPhoneNumber(phoneNumber))
                return new CustomResponse<ProfessorDTO>(400, "Invalid phone number");

            Professor professor = await _context.Professors.FirstOrDefaultAsync(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.Phone == phoneNumber)
                return new CustomResponse<ProfessorDTO>(409, $"Phone number is already {phoneNumber}");

            professor.Phone = phoneNumber;

            try
            {
                await _context.SaveChangesAsync();
                ProfessorDTO professorDTO = _mapper.Map<ProfessorDTO>(professor);
                return new CustomResponse<ProfessorDTO>(200, "Name edited succesfuuly", professorDTO);
            }
            catch
            {
                return new CustomResponse<ProfessorDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<IEnumerable<ProfessorDTO>>> GetAllProfessors(TakeSkipModel takeSkipModel)
        {
            IEnumerable<Professor> professors = _context.Professors.Skip(takeSkipModel.skip).Take(takeSkipModel.take);

            if(professors.Count() == 0)
                return new CustomResponse<IEnumerable<ProfessorDTO>>(404,"Not found");

            IEnumerable<ProfessorDTO> professorsDTO = _mapper.Map<IEnumerable<ProfessorDTO>>(professors);

            return new CustomResponse<IEnumerable<ProfessorDTO>>(200, "Professors retreived succesfully", professorsDTO);
        }

        public async Task<CustomResponse<IEnumerable<ProfessorDTO>>> GetProfessorsByDepartment(int departmentId, TakeSkipModel takeSkipModel)
        {
            /*Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentId == departmentId);

            if (department == null)
                return new CustomResponse<IEnumerable<ProfessorDTO>>(404, "Department does not exist");*/

            IEnumerable<Professor> professors = _context.Professors.Where(P => P.DepartmentId == departmentId).Skip(takeSkipModel.skip).Take(takeSkipModel.take);

            if (professors.Count() == 0)
                return new CustomResponse<IEnumerable<ProfessorDTO>>(404, "Not found");

            IEnumerable<ProfessorDTO> professorsDTO = _mapper.Map<IEnumerable<ProfessorDTO>>(professors);

            return new CustomResponse<IEnumerable<ProfessorDTO>>(200, "Professors retreived succesfully", professorsDTO);
        }
    }
}
