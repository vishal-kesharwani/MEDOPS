namespace MedOps.Domain.ValueObjects;

public record ContactInfo
{
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;

    public ContactInfo() { }

    public ContactInfo(string email, string phone)
    {
        Email = email;
        Phone = phone;
    }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Phone);
}