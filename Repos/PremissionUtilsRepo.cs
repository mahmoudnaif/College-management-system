using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces;

namespace College_managemnt_system.Repos
{
    public class PremissionUtilsRepo
    {
        private readonly IPremissionManagerRepo _premissionManagerRepo;

        public PremissionUtilsRepo(IPremissionManagerRepo premissionManagerRepo)
        {
            _premissionManagerRepo = premissionManagerRepo;
        }

        //Start of Register students.



        public async Task<CustomResponse<bool>> EnableRegestringStudetns(TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionManagerRepo.Enable("registerStudetns", timeCachedInputModel);

            return response;
        }


        public async Task<CustomResponse<bool>> EnableRegestringStudetns(DateTime expirationDate)
        {
            var response = await _premissionManagerRepo.Enable("registerStudetns", expirationDate);

            return response;
        }

        public async Task<CustomResponse<bool>> DisableRegestringStudetns()
        {
            var response = await _premissionManagerRepo.Disable("registerStudetns");

            return response;
        }

        public async Task<CustomResponse<bool>> CheckRegestringStudetnsEndPoint()
        {
            var response = await _premissionManagerRepo.CheckForEndPoint("registerStudetns");

            return response;
        }

        public async Task<bool> CheckRegestringStudetns()
        {
            return await _premissionManagerRepo.Check("registerStudetns");
        }



        //End of register students


        //start of register courses

        public async Task<CustomResponse<bool>> EnableRegestringCourses(TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionManagerRepo.Enable("registerCourses_admin", timeCachedInputModel);

            return response;
        }

  
        public async Task<CustomResponse<bool>> EnableRegestringCourses( DateTime expirationDate)
        {
            var response = await _premissionManagerRepo.Enable("registerCourses_admin", expirationDate);

            return response;
        }

        public async Task<CustomResponse<bool>> DisableRegestringCourses()
        {
            var response = await _premissionManagerRepo.Disable("registerCourses_admin");

            return response;
        }

     
        public async Task<CustomResponse<bool>> CheckRegestringCoursesEndPoint()
        {
            var response = await _premissionManagerRepo.CheckForEndPoint("registerCourses_admin");

            return response;
        }

        public async Task<bool> CheckRegestringCourses()
        {
            return await _premissionManagerRepo.Check("registerCourses_admin");
        }

        //end of register courses
    }
}
