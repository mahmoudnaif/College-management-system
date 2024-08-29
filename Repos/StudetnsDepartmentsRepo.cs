using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.Interfaces;

namespace College_managemnt_system.Repos
{
    public class StudetnsDepartmentsRepo : IStudetnsDepartmentsRepo
    {
        public CustomResponse<bool> AddStudentDepartment()
        {
            throw new NotImplementedException();
        }

        public CustomResponse<bool> change()
        {
            throw new NotImplementedException();
        }

        public CustomResponse<StudentDTO> ViewStudentsByDepartment(int departmentId, TakeSkipModel model)
        {
            throw new NotImplementedException();
        }
    }
}
