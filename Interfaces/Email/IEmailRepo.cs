﻿using College_managemnt_system.CustomResponse;

namespace College_managemnt_system.Interfaces.Email
{
    public interface IEmailRepo
    {
        public Task<CustomResponse<bool>> SendVerificationEmail(int accountId);
        public Task<CustomResponse<bool>> VerifyAccount(int accountId);
        public Task<CustomResponse<bool>> SendPasswordChangeEmail(string email);

        public Task<CustomResponse<bool>> ChangePassword(int accountId, string newPassword, string repeatNewPassword);
    }
}
