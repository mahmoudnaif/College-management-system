using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace College_managemnt_system.Repos
{
    public class SemstersRepo : ISemstersRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public SemstersRepo(CollegeDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<SemesterDTO>> AddSemester(SemesterInputModel semesterInputModel)
        {
            if (semesterInputModel.semesterName != "FALL" && semesterInputModel.semesterName != "SPRING" && semesterInputModel.semesterName != "SUMMER")
                return new CustomResponse<SemesterDTO>(400,"Invalid semester name");


            if (semesterInputModel.semesterYear < DateTime.UtcNow.Year)
                return new CustomResponse<SemesterDTO>(400, "Invalid semester year");

            if(semesterInputModel.startDate < DateTime.UtcNow)
                return new CustomResponse<SemesterDTO>(400, "Invalid start Date");

            if (semesterInputModel.endDate <= DateTime.UtcNow)
                return new CustomResponse<SemesterDTO>(400, "Invalid end Date");


            Semester semester = new Semester();


            semester.SemesterName = semesterInputModel.semesterName;

            semester.SemesterYear = semesterInputModel.semesterYear;

            semester.StartDate = semesterInputModel.startDate;

            semester.EndDate = semesterInputModel.endDate;



            try
            {
                _context.Semesters.Add(semester);
                await _context.SaveChangesAsync();
                SemesterDTO semestersDTO = _mapper.Map<SemesterDTO>(semester);
                return new CustomResponse<SemesterDTO>(201, "Semester Addes succesffuly", semestersDTO);

            }
            catch
            { 
                return new CustomResponse<SemesterDTO>(500, "Internal server error");

            }
        }

        public async Task<CustomResponse<bool>> DeleteSemester(int semesterID) //WARNING DO NOT USE UNLESS NECESSARY
        {
            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == semesterID);

            if (semester == null)
                return new CustomResponse<bool>(404, "SemesterNotFound");

            try
            {
                _context.Semesters.Remove(semester);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Semester Deleted Successfully");
            }
            catch 
            {
                return new CustomResponse<bool>(500,"Internal server error");
            }
        }
        public async Task<CustomResponse<SemesterDTO>> EditStartDateSemester(EditDateModel editDateModel)
        {

            if (editDateModel.newDate< DateTime.UtcNow)
                return new CustomResponse<SemesterDTO>(400, "Can not assign start date to an old one");

            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == editDateModel.semesterId);

            if (semester == null)
                return new CustomResponse<SemesterDTO>(404, "SemesterNotFound");

            semester.StartDate = editDateModel.newDate;

            try
            {
                await _context.SaveChangesAsync();
                SemesterDTO semestersDTO = _mapper.Map<SemesterDTO>(semester);
                return new CustomResponse<SemesterDTO>(200, "Semester start date edited Successfully", semestersDTO);
            }
            catch
            {
                return new CustomResponse<SemesterDTO>(500, "Internal server error");
            }

        }
        public async Task<CustomResponse<SemesterDTO>> EditEndDateSemester(EditDateModel editDateModel)
        {

            if (editDateModel.newDate < DateTime.UtcNow)
                return new CustomResponse<SemesterDTO>(400, "Can not assign start date to an old one");

            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == editDateModel.semesterId);

            if (semester == null)
                return new CustomResponse<SemesterDTO>(404, "SemesterNotFound");

            semester.EndDate = editDateModel.newDate;

            try
            {
                await _context.SaveChangesAsync();
                SemesterDTO semestersDTO = _mapper.Map<SemesterDTO>(semester);
                return new CustomResponse<SemesterDTO>(200, "Semester end date edited Successfully", semestersDTO);
            }
            catch
            {
                return new CustomResponse<SemesterDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<SemesterDTO>> EditActiveStatus(EditIsActiveModel editIsActiveModel)
        {
            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == editIsActiveModel.semesterId);

            if (semester == null)
                return new CustomResponse<SemesterDTO>(404, "SemesterNotFound");

            if (semester.IsActive == editIsActiveModel.isActive)
                return new CustomResponse<SemesterDTO>(409, "Semester already set to: " + editIsActiveModel.isActive);

            semester.IsActive = editIsActiveModel.isActive;

            try
            {
                await _context.SaveChangesAsync();
                SemesterDTO semestersDTO = _mapper.Map<SemesterDTO>(semester);
                return new CustomResponse<SemesterDTO>(200, "Semester set to "+ editIsActiveModel.isActive + " Successfully", semestersDTO);
            }
            catch
            {
                return new CustomResponse<SemesterDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<IEnumerable<SemesterDTO>>> GetSemesters(TakeSkipModel takeSkipModel)
        {
            if (takeSkipModel.take < 0 || takeSkipModel.skip < 0)
                return new CustomResponse<IEnumerable<SemesterDTO>>(400, "Take and skip must more than or equal 0");

            IEnumerable<Semester> semesters = _context.Semesters.OrderByDescending(S => S.SemesterYear).Skip(takeSkipModel.skip).Take(takeSkipModel.take);

            if (semesters.Count() == 0)
                return new CustomResponse<IEnumerable<SemesterDTO>>(404,"Not found");


            IEnumerable<SemesterDTO> semestersDTO = _mapper.Map<IEnumerable<SemesterDTO>>(semesters);

            return new CustomResponse<IEnumerable<SemesterDTO>>(200, "Semesters retreived successfully", semestersDTO);


        }

        public async Task<CustomResponse<SemesterDTO>> GetSingleSemester(int semesterID)
        {
            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterId == semesterID);

            if (semester == null)
                return new CustomResponse<SemesterDTO>(404, "SemesterNotFound");

            SemesterDTO semesterDTO = _mapper.Map<SemesterDTO>(semester);

            return new CustomResponse<SemesterDTO>(200, "Semester found successfully", semesterDTO);

        }

        public async Task<CustomResponse<SemesterDTO>> GetSemesterByNameYear(GetSemesterModel getSemesterModel)
        {
            Semester semester = _context.Semesters.SingleOrDefault(S => S.SemesterName == getSemesterModel.semesterName && 
                                                                    S.SemesterYear == getSemesterModel.SemesterYear);

            if (semester == null)
                return new CustomResponse<SemesterDTO>(404, "Semester Not Found");

            SemesterDTO semesterDTO = _mapper.Map<SemesterDTO>(semester);
            return new CustomResponse<SemesterDTO>(200, "Semester retreived successfully",semesterDTO);
        }
    }
}
