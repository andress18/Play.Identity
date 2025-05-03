using System;

namespace Play.Identity.Service.Exceptions;

[Serializable]
internal class InsifficientFundsException : Exception
{

    public Guid UserId { get; }
    public decimal GilToDebit { get; }

    public InsifficientFundsException(Guid userId, decimal gilToDebit)
    : base($"Not enough gil to debit {gilToDebit} from user {userId}.")
    {
        UserId = userId;
        GilToDebit = gilToDebit;
    }

}