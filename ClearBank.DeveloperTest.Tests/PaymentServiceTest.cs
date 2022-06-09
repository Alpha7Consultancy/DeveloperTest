using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Helpers;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Setttings;
using ClearBank.DeveloperTest.Tests.TestData;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTest
    {
        readonly Mock<IAccountDataStore> _accountDataStore = new Mock<IAccountDataStore>();
        readonly IPaymentService _sutPaymentService;

        public PaymentServiceTest()
        {
            _sutPaymentService = new PaymentService(_accountDataStore.Object);
        }

        [Theory]
        [ClassData(typeof(MakePaymentRequestTestDataGenerator))]
        public void GivenValidPaymentRequest_AndAccountDoesNotExists_ThenPaymentUnsuccessful(MakePaymentRequest request)
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns<Account>(null);

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
        }

        [Fact]
        public void GivenValidBacsPaymentRequest_AndAccountExists_ThenPaymentSuccessful()
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns(new Account()
                {
                    AccountNumber = "123456",
                    Balance = 1000M,
                    Status = AccountStatus.Live,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
                });

            var request = new MakePaymentRequest()
            {
                CreditorAccountNumber = "CreditorAccountNumber",
                DebtorAccountNumber = "DebtorAccountNumber",
                Amount = 10.0M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            _accountDataStore.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void GivenValidFasterPaymentsPaymentRequest_AndAccountExistsWithSufficientBalance_ThenPaymentSuccessful()
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns(new Account()
                {
                    AccountNumber = "123456",
                    Balance = 1000M,
                    Status = AccountStatus.Live,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
                });

            var request = new MakePaymentRequest()
            {
                CreditorAccountNumber = "CreditorAccountNumber",
                DebtorAccountNumber = "DebtorAccountNumber",
                Amount = 10.0M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            _accountDataStore.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void GivenValidFasterPaymentsPaymentRequest_AndAccountExistsWithInSufficientBalance_ThenPaymentUnSuccessful()
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns(new Account()
                {
                    AccountNumber = "123456",
                    Balance = 5M,
                    Status = AccountStatus.Live,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                });

            var request = new MakePaymentRequest()
            {
                CreditorAccountNumber = "CreditorAccountNumber",
                DebtorAccountNumber = "DebtorAccountNumber",
                Amount = 10.0M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
        }

        [Fact]
        public void GivenValidChapsPaymentRequest_AndAccountExistsAndActive_ThenPaymentSuccessful()
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns(new Account()
                {
                    AccountNumber = "123456",
                    Balance = 1000M,
                    Status = AccountStatus.Live,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
                });

            var request = new MakePaymentRequest()
            {
                CreditorAccountNumber = "CreditorAccountNumber",
                DebtorAccountNumber = "DebtorAccountNumber",
                Amount = 10.0M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Chaps
            };

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            _accountDataStore.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void GivenValidChapsPaymentRequest_AndAccountExistsAndInActive_ThenPaymentUnSuccessful()
        {
            _accountDataStore
                .Setup(a => a.GetAccount(It.IsAny<string>()))
                .Returns(new Account()
                {
                    AccountNumber = "123456",
                    Balance = 1000M,
                    Status = AccountStatus.Disabled,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
                });

            var request = new MakePaymentRequest()
            {
                CreditorAccountNumber = "CreditorAccountNumber",
                DebtorAccountNumber = "DebtorAccountNumber",
                Amount = 10.0M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Chaps
            };

            var result = _sutPaymentService.MakePayment(request);

            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
        }

    }
}
