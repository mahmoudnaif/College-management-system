using College_managemnt_system.CustomResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace College_managemnt_system.Interfaces
{
    public interface IStudetnsDepartmentsRepo
    {
        public CustomResponse<bool> AddStudentDepartment();
        public CustomResponse<bool> change();
    }
}
