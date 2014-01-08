﻿using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IAccountingService
    {
        Task<GetAccountResponse> GetAccount(GetAccountRequest request);

        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);

        Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequest request);

        Task<GetAccountLoginResponse> GetAccountLogin(GetAccountLoginRequest request);

        Task<CreateAccountLoginResponse> CreateAccountLogin(CreateAccountLoginRequest request);
    }
}
