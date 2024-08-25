using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace College_managemnt_system.Repos
{
    public class CoursesSemestersRepo : ICoursesSemestersRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public CoursesSemestersRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<CourseSemesterDTO>> Add(CourseSemesterInputModel model)
        {
            Professor professor= _context.Professors.FirstOrDefault(P => P.ProfessorId == model.ProfessorId);
            if(professor == null)
                return new CustomResponse<CourseSemesterDTO>(404, "Professor does not exist");

            Course course = _context.Courses.FirstOrDefault(C => C.CourseId == model.CourseId);
            if (course == null)
                return new CustomResponse<CourseSemesterDTO>(404, "Coruse does not exist");

            Semester semester = _context.Semesters.FirstOrDefault(S => S.SemesterId == model.SemesterId);
            if (semester == null)
                return new CustomResponse<CourseSemesterDTO>(200, "Semester does not exist");

            Coursesemester coursesemesterExists = _context.Coursesemesters.SingleOrDefault(CS => CS.SemesterId == model.SemesterId && CS.CourseId == model.CourseId);
            if (coursesemesterExists != null)
                return new CustomResponse<CourseSemesterDTO>(409, "Course already exists for this semester");

            Coursesemester coursesemester = new Coursesemester()
            {
                ProfessorId = model.ProfessorId,
                CourseId = model.CourseId,
                SemesterId = model.SemesterId,
                Isactive = model.Isactive
            };

            try
            {
                _context.Coursesemesters.Add(coursesemester);
                await _context.SaveChangesAsync();
                CourseSemesterDTO courseSemesterDTO = _mapper.Map<CourseSemesterDTO>(coursesemester);
                courseSemesterDTO.CourseName = course.CourseName;
                return new CustomResponse<CourseSemesterDTO>(201, "Course for semester added successfuully", courseSemesterDTO);
            }
            catch
            {
                return new CustomResponse<CourseSemesterDTO>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> Delete(int courseSemesterId)
        {
            Coursesemester coursesemester = _context.Coursesemesters.FirstOrDefault(CS => CS.CourseSemesterId == courseSemesterId);
            if (coursesemester == null)
                return new CustomResponse<bool>(409, "Course Does not exist");

            try
            {
                _context.Coursesemesters.Remove(coursesemester);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Course from this semester deleted successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<CourseSemesterDTO>> ChangeProfessor(int courseSemesterId, int profId)
        {
            Professor professor = _context.Professors.FirstOrDefault(P => P.ProfessorId == profId);
            if (professor == null)
                return new CustomResponse<CourseSemesterDTO>(404, "Professor does not exist");

            Coursesemester coursesemester = _context.Coursesemesters.FirstOrDefault(C => C.CourseSemesterId==courseSemesterId);
            if (coursesemester == null)
                return new CustomResponse<CourseSemesterDTO>(404, "Course does not exist in this semester");

            if (coursesemester.ProfessorId == profId)
                return new CustomResponse<CourseSemesterDTO>(409, $"professor {professor.FirstName} {professor.LastName} is already set for this course");

            coursesemester.ProfessorId = profId;

            try
            {
                await _context.SaveChangesAsync();
                CourseSemesterDTO courseSemesterDTO = _mapper.Map<CourseSemesterDTO>(coursesemester);
                courseSemesterDTO.CourseName = _context.Courses.FirstOrDefault(C => C.CourseId == coursesemester.CourseId)?.CourseName ;
                return new CustomResponse<CourseSemesterDTO>(200, "Professor edited successfully", courseSemesterDTO);
            }
            catch
            {
                return new CustomResponse<CourseSemesterDTO>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<CourseSemesterDTO>> EditActivationStatus(int courseSemesterId, bool isActive)
        {

            Coursesemester coursesemester = _context.Coursesemesters.FirstOrDefault(C => C.CourseSemesterId == courseSemesterId);
            if (coursesemester == null)
                return new CustomResponse<CourseSemesterDTO>(404, "Course does not exist in this semester");

            if (coursesemester.Isactive == isActive)
                return new CustomResponse<CourseSemesterDTO>(409, "Course is already active/notactive");

            if (isActive)
            {
                if (!await CheckActiveSemester(coursesemester.SemesterId))
                    return new CustomResponse<CourseSemesterDTO>(400, "Semester must be active to activate the course");
            }
            else
            {
                if (!await CheckDeActiveStudents(coursesemester.CourseSemesterId))
                    return new CustomResponse<CourseSemesterDTO>(400, "Some students are still active in this course you can't deactivate please assign them their grades");
            }
            

            coursesemester.Isactive = isActive;

            try
            {
                await _context.SaveChangesAsync();
                CourseSemesterDTO courseSemesterDTO = _mapper.Map<CourseSemesterDTO>(coursesemester);
                courseSemesterDTO.CourseName = _context.Courses.FirstOrDefault(C => C.CourseId == coursesemester.CourseId)?.CourseName;
                return new CustomResponse<CourseSemesterDTO>(200, "Activation changed successfully", courseSemesterDTO);
            }
            catch
            {
                return new CustomResponse<CourseSemesterDTO>(500, "Internal server error");
            }
        }
     

        public async Task<CustomResponse<bool>> EditActiveStatusForAllCourses(bool isActive)
        {
            int? semesterIdExists = _context.Semesters.FirstOrDefault(S => S.IsActive == true)?.SemesterId;

            if (semesterIdExists == null)
                return new CustomResponse<bool>(404, "No active semester at the moment");

            int semesterId = (int)semesterIdExists;

            List<Coursesemester> coursesemesters = _context.Coursesemesters.Where(CS => CS.SemesterId == semesterId && CS.Isactive != isActive).ToList();

            if (coursesemesters.Count() == 0)
                return new CustomResponse<bool>(400, "No courses were found");

            foreach (Coursesemester coursesemester in coursesemesters)
            {
               if (!isActive)
                {
                        if (!await CheckDeActiveStudents(coursesemester.CourseSemesterId))
                            return new CustomResponse<bool>(400, "Some students are still active in this course you can't deactivate please assign them their grades");
                    
                }


                coursesemester.Isactive = isActive; 
                
            }

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Courses activated/deactivated successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<IEnumerable<CourseSemesterDTO>>> GetActiveSemesterCourses()
        {
            int? semesterIdExists = _context.Semesters.FirstOrDefault(S => S.IsActive == true)?.SemesterId;

            if (semesterIdExists == null)
                return new CustomResponse<IEnumerable<CourseSemesterDTO>>(404, "No active semester at the moment");

            int semesterId = (int)semesterIdExists;


            IEnumerable<CourseSemesterDTO> coursesemestersDTO = _context.Coursesemesters.Where(CS => CS.SemesterId == semesterId).Join(_context.Courses, CS => CS.CourseId, C => C.CourseId,
              (CS, C) => new CourseSemesterDTO
              {
                  CourseId = CS.CourseId,
                  CourseName = C.CourseName,
                  CourseSemesterId = CS.CourseSemesterId,
                  SemesterId = CS.SemesterId,
                  Isactive = CS.Isactive,
                  ProfessorId = CS.ProfessorId,
              }
              );

            if (coursesemestersDTO.Count() == 0)
                return new CustomResponse<IEnumerable<CourseSemesterDTO>>(400, "No courses were found");

            return new CustomResponse<IEnumerable<CourseSemesterDTO>>(200, "Courses retreived successfully", coursesemestersDTO);
        }

        public async Task<CustomResponse<IEnumerable<CourseSemesterDTO>>> GetCoursesBySemester(int semesterId)
        {

            Semester semester = _context.Semesters.FirstOrDefault(S => S.SemesterId == semesterId);
            if (semester == null)
                return  new CustomResponse<IEnumerable<CourseSemesterDTO>>(404, "Semester does not exist");
            


            IEnumerable<CourseSemesterDTO> coursesemestersDTO = _context.Coursesemesters.Where(CS => CS.SemesterId == semesterId).Join(_context.Courses, CS => CS.CourseId, C => C.CourseId,
              (CS, C) => new CourseSemesterDTO
              {
                  CourseId = CS.CourseId,
                  CourseName = C.CourseName,
                  CourseSemesterId = CS.CourseSemesterId,
                  SemesterId = CS.SemesterId,
                  Isactive = CS.Isactive,
                  ProfessorId = CS.ProfessorId,
              }
              );

            if (coursesemestersDTO.Count() == 0)
                return new CustomResponse<IEnumerable<CourseSemesterDTO>>(400, "No courses were found");

            return new CustomResponse<IEnumerable<CourseSemesterDTO>>(200, "Courses retreived successfully", coursesemestersDTO);
        }






        private async Task<bool> CheckDeActiveStudents(int courseSemesterId)
        {
            StudentCourse studentCourses = _context.StudentCourses.FirstOrDefault(SC => SC.CourseSemesterId == courseSemesterId && SC.IsFinished == false);

            if (studentCourses == null)
                return true;

            return false;
        }

        private async Task<bool> CheckActiveSemester(int semesterId)
        {
            Semester semester = _context.Semesters.FirstOrDefault(S => S.SemesterId == semesterId);

            if (semester == null)
                return false;

            return semester.IsActive;
        }
    }
}