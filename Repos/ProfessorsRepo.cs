using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
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

            Department department = _context.Departments.FirstOrDefault(D => D.DepartmentId == departmentId);
            if (department == null)
                return new CustomResponse<ProfessorDTO>(404, "Department does not exist");

            Account account = _context.Accounts.FirstOrDefault(A => A.Email == email);
            if (account == null)
                return new CustomResponse<ProfessorDTO>(404, "there is no account associated with this email");

            Professor professorDuplicate = _context.Professors.FirstOrDefault(P => P.AccountId == account.AccountId);
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
            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == profId);
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

        public async Task<CustomResponse<ProfessorDTO>> EditDepartment(EditDepartmentInputModle model)
        {
            Department department = _context.Departments.FirstOrDefault(D =>D.DepartmentId == model.departmentId);
            if (department == null)
                return new CustomResponse<ProfessorDTO>(404, "Department does not exist");

            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == model.id);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.DepartmentId == department.DepartmentId)
                return new CustomResponse<ProfessorDTO>(409, $"Department is already set to: {department.DepartmentName}.");

            professor.DepartmentId = model.departmentId;

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

        public async Task<CustomResponse<ProfessorDTO>> EditHiringDate(EditDateInputModel model)
        {
            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == model.id);
            if (professor == null)
                return new CustomResponse<ProfessorDTO>(404, "Professor does not exist");

            if (professor.HiringDate.Date == model.date.Date)
                return new CustomResponse<ProfessorDTO>(409, $"Date is already set to: {model.date.Date}");

            professor.HiringDate = model.date;

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

        public async Task<CustomResponse<ProfessorDTO>> EditName(EditNameInputModel model)
        {
            string firstName = model.firstName.Trim();
            string lastName = model.lastName.Trim();

            if (firstName== "" || lastName== "")
                return new CustomResponse<ProfessorDTO>(400, "Name must be specefied");

            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == model.id);
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

        public async Task<CustomResponse<ProfessorDTO>> EditPhoneNumber(EditPhoneNumberInputModel model)
        {
            string phoneNumber = model.phoneNumber.Trim();

            if (!_utilities.IsValidPhoneNumber(phoneNumber))
                return new CustomResponse<ProfessorDTO>(400, "Invalid phone number");

            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == model.id);
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
            /*Department department = _context.Departments.FirstOrDefault(D => D.DepartmentId == departmentId);

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
