namespace Backend.SharedKernel.Common.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode);
