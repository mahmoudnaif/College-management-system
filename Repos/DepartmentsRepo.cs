using AutoMapper;
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;

namespace College_managemnt_system.Repos
{
    public class DepartmentsRepo : IDepartmentsRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public DepartmentsRepo(CollegeDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomResponse<DepartmentsDTO>> AddDepartment(string departmnetName)
        {
            if(departmnetName.Trim().Length == 0)
                return new CustomResponse<DepartmentsDTO>(400,"Department name must be specefied");

            Department departmentExists = _context.Departments.SingleOrDefault(D => D.DepartmentName == departmnetName);

            if(departmentExists != null)
                return  new CustomResponse<DepartmentsDTO>(409, "Department already exists");

            Department department = new Department() {DepartmentName = departmnetName};

            try
            {
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                DepartmentsDTO departmentDTO =  _mapper.Map<DepartmentsDTO>(department);
                return new CustomResponse<DepartmentsDTO>(201, "Deparment added successfully", departmentDTO);
            }
            catch
            {
                return new CustomResponse<DepartmentsDTO>(500, "Internal server error");
            }


        }

        public async Task<CustomResponse<bool>> DeleteDepartment(int departmentId)
        {
            Department department = _context.Departments.SingleOrDefault(D => D.DepartmentId == departmentId);

            if (department == null)
                return new CustomResponse<bool>(409, "Department does not exist");


            try
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(200, "Department removed successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<DepartmentsDTO>> EditDepartmentName(EditDepartmentNameModel editModel )
        {
            if (editModel.newName.Trim().Length == 0)
                return new CustomResponse<DepartmentsDTO>(400, "New name must be specefied");

            Department departmentExists = _context.Departments.SingleOrDefault(D => D.DepartmentName == editModel.newName);

            if (departmentExists != null)
                return new CustomResponse<DepartmentsDTO>(409,"Name already exists");

            Department department = _context.Departments.SingleOrDefault(D => D.DepartmentId == editModel.departmentId);

            if (department == null)
                return new CustomResponse<DepartmentsDTO>(404, "Department doesn't exist");


            department.DepartmentName = editModel.newName;

            try
            {
                await _context.SaveChangesAsync();
                DepartmentsDTO departmentDTO = _mapper.Map<DepartmentsDTO>(department);
                return new CustomResponse<DepartmentsDTO>(200, "Department name edited successfully", departmentDTO);
            }
            catch
            {
                return new CustomResponse<DepartmentsDTO>(500, "Internal server error");
            }

        }

        public async Task<CustomResponse<IEnumerable<DepartmentsDTO>>> GetDepartments()
        {
            IEnumerable<Department> departments = _context.Departments;


            if(departments == null || departments.Count() == 0)
                return new CustomResponse<IEnumerable<DepartmentsDTO>>(404,"No departments found");


            IEnumerable<DepartmentsDTO> departmentsDTO = _mapper.Map<IEnumerable<DepartmentsDTO>>(departments);

            return new CustomResponse<IEnumerable<DepartmentsDTO>>(200, "Departments retreived", departmentsDTO);

        }
    }
}
