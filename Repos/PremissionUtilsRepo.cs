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

        //Start of Dropping Courses
        public async Task<CustomResponse<bool>> EnableDroppingCourses(TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionManagerRepo.Enable("DropCourses_admin", timeCachedInputModel);

            return response;
        }


        public async Task<CustomResponse<bool>> EnableDroppingCourses(DateTime expirationDate)
        {
            var response = await _premissionManagerRepo.Enable("DropCourses_admin", expirationDate);

            return response;
        }

        public async Task<CustomResponse<bool>> DisableDroppingCourses()
        {
            var response = await _premissionManagerRepo.Disable("DropCourses_admin");

            return response;
        }


        public async Task<CustomResponse<bool>> CheckDroppingCoursesEndPoint()
        {
            var response = await _premissionManagerRepo.CheckForEndPoint("DropCourses_admin");

            return response;
        }

        public async Task<bool> CheckDroppingCourses()
        {
            return await _premissionManagerRepo.Check("DropCourses_admin");
        }

        //End of Dropping Courses


        //Start of withdrawing Courses
        public async Task<CustomResponse<bool>> EnableWithdrawingCourses(TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionManagerRepo.Enable("WithdrawCourses_admin", timeCachedInputModel);

            return response;
        }


        public async Task<CustomResponse<bool>> EnableWithdrawingCourses(DateTime expirationDate)
        {
            var response = await _premissionManagerRepo.Enable("WithdrawCourses_admin", expirationDate);

            return response;
        }

        public async Task<CustomResponse<bool>> DisableWithdrawingCourses()
        {
            var response = await _premissionManagerRepo.Disable("WithdrawCourses_admin");

            return response;
        }


        public async Task<CustomResponse<bool>> CheckWithdrawingCoursesEndPoint()
        {
            var response = await _premissionManagerRepo.CheckForEndPoint("WithdrawCourses_admin");

            return response;
        }

        public async Task<bool> CheckWithdrawingCourses()
        {
            return await _premissionManagerRepo.Check("WithdrawCourses_admin");
        }

        //End of WithDrawing Courses



        //Start of grade Courses
        public async Task<CustomResponse<bool>> EnableGradingCourses(TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionManagerRepo.Enable("GradingCourses_admin", timeCachedInputModel);

            return response;
        }


        public async Task<CustomResponse<bool>> EnableGradingCourses(DateTime expirationDate)
        {
            var response = await _premissionManagerRepo.Enable("GradingCourses_admin", expirationDate);

            return response;
        }

        public async Task<CustomResponse<bool>> DisableGradingCourses()
        {
            var response = await _premissionManagerRepo.Disable("GradingCourses_admin");

            return response;
        }


        public async Task<CustomResponse<bool>> CheckGradingCoursesEndPoint()
        {
            var response = await _premissionManagerRepo.CheckForEndPoint("GradingCourses_admin");

            return response;
        }

        public async Task<bool> CheckGradingCourses()
        {
            return await _premissionManagerRepo.Check("GradingCourses_admin");
        }

        //End of grade Courses
    }
}
