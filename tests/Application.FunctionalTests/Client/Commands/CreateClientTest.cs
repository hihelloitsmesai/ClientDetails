using ClientDirectory.Application.Client.Commands.CreateClient;
using ClientDirectory.Application.Common.Exceptions;
using ClientDirectory.Application.TodoLists.Commands.CreateTodoList;
using ClientDirectory.Domain.Entities;
//using ClientDirectory.Application.TodoLists.Commands.CreateTodoList;

namespace ClientDirectory.Application.FunctionalTests.Client.Commands;

using static Testing;

public class CreateClientTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateClientCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueMobileNumber()
    {
        await SendAsync(new CreateClientCommand
        {
            MobileNumber = "9700027778"
        });

        var command = new CreateClientCommand
        {
            MobileNumber = "9700027778"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueIdNumber()
    {
        await SendAsync(new CreateClientCommand
        {
            IdNumber = "9700027778"
        });

        var command = new CreateClientCommand
        {
            IdNumber = "9700027778"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateClient()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateClientCommand
        {
            FirstName = "Sai",
            LastName = "Jonnadula",
            MobileNumber = "9700027778",
            IdNumber = "123456789",
            PhysicalAddress = "Address"
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Domain.Entities.Client>(id);

        list.Should().NotBeNull();
        list!.FirstName.Should().Be(command.FirstName);
        list.Id.Should().BeGreaterThan(0);
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
