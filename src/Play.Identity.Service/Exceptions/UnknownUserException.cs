using System;

namespace Play.Identity.Service.Exceptions;

[Serializable]
internal class UnknownUserException(Guid userId) : Exception($"Unknown user '{userId}'")
{
    public Guid UserId { get; } = userId;
}