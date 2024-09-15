using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace College_managemnt_system.Repos
{
    public class TeachingAssistanceRepo : ITeachingAssistanceRepo
    {
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public TeachingAssistanceRepo(UtilitiesRepo utilitiesRepo,CollegeDBContext context,IMapper mapper)
        {
            _utilitiesRepo = utilitiesRepo;
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<TeachingAssistanceDTO>> AddTa(TeachingAssistanceInputModel model)
        {
            string firstName = model.FirstName.Trim();
            string lastName = model.LastName.Trim();
            string nationalNumber = model.NationalNumber.Trim();
            string phone = model.Phone.Trim();
            string email = model.email.Trim();
            DateTime? hiringDate = model.HiringDate;

            if (!_utilitiesRepo.IsValidEmail(email))
                return new CustomResponse<TeachingAssistanceDTO>(400, "Invalid email format");

            if (!_utilitiesRepo.IsValidNationalId(nationalNumber))
                return new CustomResponse<TeachingAssistanceDTO>(400, "National number is not valid");

            if (!_utilitiesRepo.IsValidPhoneNumber(phone))
                return new CustomResponse<TeachingAssistanceDTO>(400, "Invalid phone number");

            if (firstName == "" || lastName == "")
                return new CustomResponse<TeachingAssistanceDTO>(400, "Name must be specefied");

            Account account = await _context.Accounts.FirstOrDefaultAsync(A => A.Email == email);
            if (account == null)
                return new CustomResponse<TeachingAssistanceDTO>(404, "there is no account associated with this email");

            if (account.Role != "ta")
                return new CustomResponse<TeachingAssistanceDTO>(403, "Account associated must be a ta account");

            TeachingAssistance teachingAssistanceDuplicate = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AccountId == account.AccountId);
            if (teachingAssistanceDuplicate != null)
                return new CustomResponse<TeachingAssistanceDTO>(409, "Account already associated with another Ta");

            teachingAssistanceDuplicate = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.NationalNumber == nationalNumber);
            if (teachingAssistanceDuplicate != null)
                return new CustomResponse<TeachingAssistanceDTO>(409, "National ID already associated with another Ta");

            TeachingAssistance teachingAssistance = new TeachingAssistance()
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                NationalNumber = nationalNumber,
                AccountId = account.AccountId,
                HiringDate = hiringDate == null ? DateTime.Now : (DateTime)hiringDate

            };

            try
            {
                await _context.TeachingAssistances.AddAsync(teachingAssistance);
                await _context.SaveChangesAsync();
                TeachingAssistanceDTO teachingAssistanceDTO = _mapper.Map<TeachingAssistanceDTO>(teachingAssistance);
                return new CustomResponse<TeachingAssistanceDTO>(201, "Professor added succesfuuly",teachingAssistanceDTO);
            }
            catch
            {
                return new CustomResponse<TeachingAssistanceDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<TeachingAssistanceDTO>> EditHiringDate(int taId, DateTime hiringDate)
        {
            TeachingAssistance teachingAssistance= await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);
            if (teachingAssistance == null)
                return new CustomResponse<TeachingAssistanceDTO>(404, "Ta does not exist");

            if (teachingAssistance.HiringDate.Date == hiringDate.Date)
                return new CustomResponse<TeachingAssistanceDTO>(409, $"Date is already set to: {hiringDate.Date}");

            teachingAssistance.HiringDate = hiringDate;

            try
            {
                await _context.SaveChangesAsync();
                TeachingAssistanceDTO teachingAssistanceDTO = _mapper.Map<TeachingAssistanceDTO>(teachingAssistance);
                return new CustomResponse<TeachingAssistanceDTO>(200, "Hiring date edited succesfuuly", teachingAssistanceDTO);
            }
            catch
            {
                return new CustomResponse<TeachingAssistanceDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<TeachingAssistanceDTO>> EditName(int taId, NameInputModel model)
        {
            string firstName = model.firstName.Trim();
            string lastName = model.lastName.Trim();

            if (firstName == "" || lastName == "")
                return new CustomResponse<TeachingAssistanceDTO>(400, "Name must be specefied");

            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);

            if (teachingAssistance == null)
                return new CustomResponse<TeachingAssistanceDTO>(404, "Professor does not exist");

            if (teachingAssistance.FirstName == firstName && teachingAssistance.LastName == lastName)
                return new CustomResponse<TeachingAssistanceDTO>(409, $"Name is already set to: {firstName} {lastName}");

            teachingAssistance.FirstName = firstName;
            teachingAssistance.LastName = lastName;

            try
            {
                await _context.SaveChangesAsync();
                TeachingAssistanceDTO teachingAssistanceDTO = _mapper.Map<TeachingAssistanceDTO>(teachingAssistance);
                return new CustomResponse<TeachingAssistanceDTO>(200, "Name edited succesfuuly", teachingAssistanceDTO);
            }
            catch
            {
                return new CustomResponse<TeachingAssistanceDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<TeachingAssistanceDTO>> EditPhone(int taId, string newPhoneNumber)
        {
            string phoneNumber = newPhoneNumber.Trim();

            if (!_utilitiesRepo.IsValidPhoneNumber(phoneNumber))
                return new CustomResponse<TeachingAssistanceDTO>(400, "Invalid phone number");

            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);
            if (teachingAssistance == null)
                return new CustomResponse<TeachingAssistanceDTO>(404, "Professor does not exist");

            if (teachingAssistance.Phone == phoneNumber)
                return new CustomResponse<TeachingAssistanceDTO>(409, $"Phone number is already {phoneNumber}");

            teachingAssistance.Phone = phoneNumber;

            try
            {
                await _context.SaveChangesAsync();
                TeachingAssistanceDTO teachingAssistanceDTO = _mapper.Map<TeachingAssistanceDTO>(teachingAssistance);
                return new CustomResponse<TeachingAssistanceDTO>(200, "Name edited succesfuuly", teachingAssistanceDTO);
            }
            catch
            {
                return new CustomResponse<TeachingAssistanceDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<List<TeachingAssistanceDTO>>> GetAllTas(TakeSkipModel takeSkipModel)
        {
            if (takeSkipModel.take <= 0 || takeSkipModel.skip < 0)
                return new CustomResponse<List<TeachingAssistanceDTO>>(400, "Take must be more than 0 and skip must be more than or equal 0");


            List<TeachingAssistance> teachingAssistances = await _context.TeachingAssistances.OrderBy(T => T.AssistantId).Skip(takeSkipModel.skip).Take(takeSkipModel.take).ToListAsync();

            if (!teachingAssistances.Any())
                return new CustomResponse<List<TeachingAssistanceDTO>>(404, "Not found");

            List<TeachingAssistanceDTO> teachingAssistancesDTO = _mapper.Map<List<TeachingAssistanceDTO>>(teachingAssistances);

            return new CustomResponse<List<TeachingAssistanceDTO>>(200, "Professors retreived succesfully", teachingAssistancesDTO);
        
    }

        public async Task<CustomResponse<TeachingAssistanceDTO>> GetTaByNationalId(string nationalId)
        {
            if (!_utilitiesRepo.IsValidNationalId(nationalId))
                return new CustomResponse<TeachingAssistanceDTO>(400, "National id is not valid");

            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.NationalNumber == nationalId);

            if (teachingAssistance == null)
                return new CustomResponse<TeachingAssistanceDTO>(400, "Not found");

            TeachingAssistanceDTO teachingAssistanceDTO = _mapper.Map<TeachingAssistanceDTO>(teachingAssistance);

            return new CustomResponse<TeachingAssistanceDTO>(200, "Prof retreived successfully",teachingAssistanceDTO);
        }

        public async Task<CustomResponse<bool>> RemoveTa(int taId)
        {
            TeachingAssistance teachingAssistance = await _context.TeachingAssistances.FirstOrDefaultAsync(T => T.AssistantId == taId);

            if (teachingAssistance == null)
                return new CustomResponse<bool>(404,"Ta not found");

            try
            {
                 _context.TeachingAssistances.Remove(teachingAssistance);
                 await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Ta removed successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<List<TeachingAssistanceDTO>>> SearchTas(string searchQuery, TakeSkipModel takeSkipModel)
        {
            if (searchQuery.Trim() == "")
                return new CustomResponse<List<TeachingAssistanceDTO>>(400, "Search query must be specefied");


            if (takeSkipModel.take < 0 || takeSkipModel.skip < 0)
                return new CustomResponse<List<TeachingAssistanceDTO>>(400, "Take and skip must more than or equal 0");


            List<TeachingAssistance> teachingAssistances = [];

            if (searchQuery[0] == '+')
                teachingAssistances = await _context.TeachingAssistances.Where(T => T.Phone.StartsWith(searchQuery)).OrderBy(T => T.Phone).Skip(takeSkipModel.skip).Take(takeSkipModel.take).ToListAsync();

            else
                teachingAssistances = await _context.TeachingAssistances.Where(T => (T.FirstName + " " + T.LastName).StartsWith(searchQuery)).OrderBy(T => (T.FirstName + " " + T.LastName).Length - searchQuery.Length).ThenBy(T => T.FirstName + " " + T.LastName).Skip(takeSkipModel.skip).Take(takeSkipModel.take).ToListAsync();

            if (!teachingAssistances.Any())
                return new CustomResponse<List<TeachingAssistanceDTO>>(404, "Not found");

            List<TeachingAssistanceDTO> teachingAssistancesDTO= _mapper.Map<List<TeachingAssistanceDTO>>(teachingAssistances);
            return new CustomResponse<List<TeachingAssistanceDTO>>(200, "Professors retrieved successfully", teachingAssistancesDTO);
        }
    }
}
