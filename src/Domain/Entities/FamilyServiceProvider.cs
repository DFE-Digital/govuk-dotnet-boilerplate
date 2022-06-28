namespace CleanArchitecture.Domain.Entities;
public class FamilyServiceProvider
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Cost { get; set; } = default!;
    public bool IsThereWaitList { get; set; } = default!;
}
