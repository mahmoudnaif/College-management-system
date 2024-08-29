using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace College_managemnt_system.Interfaces
{
    public interface IStudetnsDepartmentsRepo
    {
        public CustomResponse<bool> AddStudentDepartment();
        public CustomResponse<bool> change();
        public CustomResponse<StudentDTO> ViewStudentsByDepartment(int departmentId, TakeSkipModel model);
    }
}
