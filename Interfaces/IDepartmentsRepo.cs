using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.models;

namespace College_managemnt_system.Interfaces
{
    public interface IDepartmentsRepo
    {
        public Task<CustomResponse<IEnumerable<DepartmentsDTO>>> GetDepartments();

        //public Task<CustomResponse<Department>> GetDepartmentById(int departmentId); 
        public Task<CustomResponse<DepartmentsDTO>> AddDepartment(string departmnetName);

        public Task<CustomResponse<DepartmentsDTO>> EditDepartmentName(EditDepartmentNameModel editDepartmentNameModel);

        public Task<CustomResponse<bool>> DeleteDepartment(int departmentId);
    }
}
