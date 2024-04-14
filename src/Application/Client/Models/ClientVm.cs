using ClientDirectory.Domain.Entities;

namespace ClientDirectory.Application.Client.Models;

public class ClientVm
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public string PhysicalAddress { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Client, ClientVm>();
        }
    }
}
