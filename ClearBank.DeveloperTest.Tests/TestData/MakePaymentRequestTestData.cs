using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.TestData
{
    public class MakePaymentRequestTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {
                    new MakePaymentRequest()
                        {
                            CreditorAccountNumber = "CreditorAccountNumber",
                            DebtorAccountNumber = "DebtorAccountNumber",
                            Amount = 10.0M,
                            PaymentDate = DateTime.Now,
                            PaymentScheme = PaymentScheme.Bacs
                        }
                },
            new object[] {
                    new MakePaymentRequest()
                        {
                            CreditorAccountNumber = "CreditorAccountNumber",
                            DebtorAccountNumber = "DebtorAccountNumber",
                            Amount = 10.0M,
                            PaymentDate = DateTime.Now,
                            PaymentScheme = PaymentScheme.FasterPayments
                        }
                },
            new object[] {
                    new MakePaymentRequest()
                        {
                            CreditorAccountNumber = "CreditorAccountNumber",
                            DebtorAccountNumber = "DebtorAccountNumber",
                            Amount = 10.0M,
                            PaymentDate = DateTime.Now,
                            PaymentScheme = PaymentScheme.Chaps
                        }
                },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
