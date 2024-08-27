using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.models;

namespace College_managemnt_system.Interfaces
{
    public interface IDepartmentsRepo
    {
        public Task<CustomResponse<List<DepartmentDTO>>> GetDepartments();

        //public Task<CustomResponse<Department>> GetDepartmentById(int departmentId); 
        public Task<CustomResponse<DepartmentDTO>> AddDepartment(string departmnetName);

        public Task<CustomResponse<DepartmentDTO>> EditDepartmentName(EditDepartmentNameModel editDepartmentNameModel);

        public Task<CustomResponse<bool>> DeleteDepartment(int departmentId);
    }
}
