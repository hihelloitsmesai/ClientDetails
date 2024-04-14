namespace ClientDirectory.Domain.Entities;
public class Client : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public string PhysicalAddress { get; set; } = string.Empty;

}
