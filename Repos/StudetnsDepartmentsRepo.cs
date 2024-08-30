using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class StudetnsDepartmentsRepo : IStudetnsDepartmentsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public StudetnsDepartmentsRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomResponse<StudentDTO>> AddStudentDepartment(int studentId, int departmentId)
        {
            Department department = await _context.Departments.FirstOrDefaultAsync(D =>D.DepartmentId == departmentId);
            if (department == null)
                return new CustomResponse<StudentDTO>(404, "Department does not exist");

            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);
            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student does not exist");

            if (student.TotalHours < 81)
                return new CustomResponse<StudentDTO>(403, "Can't assign department untill sutdent finishes 81 hours or more");

            if (student.DepartmentId != null)
                return new CustomResponse<StudentDTO>(409, "Student department already specefied");

            student.DepartmentId = departmentId;
            try
            {
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(201, "Department added successfully",studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Internal server errro");
            }

        }

        public async Task<CustomResponse<StudentDTO>> changeStudentDepartment(int studentId, int departmentId)
        {
            Department department = await _context.Departments.FirstOrDefaultAsync(D => D.DepartmentId == departmentId);
            if (department == null)
                return new CustomResponse<StudentDTO>(404, "Department does not exist");

            Student student = await _context.Students.FirstOrDefaultAsync(S => S.StudentId == studentId);
            if (student == null)
                return new CustomResponse<StudentDTO>(404, "Student does not exist");

            if (student.DepartmentId == null)
                return new CustomResponse<StudentDTO>(403, "there is no old department specefied, please add a department instead of editing");

            if (student.DepartmentId == departmentId)
                return new CustomResponse<StudentDTO>(409, $"Department is already set to: {department.DepartmentName}");

            try
            {
                await _context.SaveChangesAsync();
                StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);
                return new CustomResponse<StudentDTO>(200, "Department edited successfully", studentDTO);
            }
            catch
            {
                return new CustomResponse<StudentDTO>(500, "Internal server errro");
            }
        }

        public async Task<CustomResponse<List<StudentDTO>>> ViewStudentsByDepartment(int departmentId, TakeSkipModel model)
        {
            if (model.take < 1 || model.skip < 0)
                return new CustomResponse<List<StudentDTO>>(400, "Take must be more than 0 and skip must be more than or equal to 0");

            List<Student> students = await _context.Students.Where(S=> S.DepartmentId == departmentId).OrderBy(S => S.StudentId).Skip(model.skip).Take(model.take).ToListAsync();

            if (!students.Any())
                return new CustomResponse<List<StudentDTO>>(404, "No students found in this department");

            List<StudentDTO> studentsDTO = _mapper.Map<List<StudentDTO>>(students);

            return new CustomResponse<List<StudentDTO>>(200, "Students retreived successfully", studentsDTO);
        }
    }
}
