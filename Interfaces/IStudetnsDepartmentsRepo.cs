using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace College_managemnt_system.Interfaces
{
    public interface IStudetnsDepartmentsRepo
    {
        public Task<CustomResponse<StudentDTO>> AddStudentDepartment(int studentId,int departmentId);
        public Task<CustomResponse<StudentDTO>> changeStudentDepartment(int studentId, int departmentId);
        public Task<CustomResponse<List<StudentDTO>>> ViewStudentsByDepartment(int departmentId, TakeSkipModel model);
    }
}
