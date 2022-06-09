using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Helpers;
using ClearBank.DeveloperTest.Setttings;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            if (account == null)
            {
                result.Success = false;
                return result;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    result.Success = ValidateForBacs(account);
                    break;

                case PaymentScheme.FasterPayments:
                    result.Success = ValidateFasterPayments(request, account);
                    break;

                case PaymentScheme.Chaps:
                    result.Success = ValidateChaps(account);
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }

        private bool ValidateChaps(Account account)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                && account.Status == AccountStatus.Live;
        }

        private bool ValidateFasterPayments(MakePaymentRequest request, Account account)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                && account.Balance > request.Amount;
        }

        private bool ValidateForBacs(Account account)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}
