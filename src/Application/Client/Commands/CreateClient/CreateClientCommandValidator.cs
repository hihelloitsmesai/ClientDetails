using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ClientDirectory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClientDirectory.Application.Client.Commands.CreateClient;
public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("'{PropertyName}' Required.")
            .WithErrorCode("Required");

        RuleFor(v => v.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("'{PropertyName}' Required.")
            .WithErrorCode("Required");

        RuleFor(v => v.IdNumber)
            .NotEmpty()
            .MaximumLength(20)
            .Must(IsValidSouthAfricanIdNumber)
            .WithMessage("'{PropertyName}' is not in correct format.")
            .WithErrorCode("Required");

        RuleFor(v => v.IdNumber)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(IdNumberDuplicateCheck)
            .WithMessage("'{PropertyName}' is already used.")
            .WithErrorCode("Duplicate");

        RuleFor(v => v.MobileNumber)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(MobileNumberDuplicateCheck)
            .WithMessage("'{PropertyName}' is already used.")
            .WithErrorCode("Required");
    }

    public bool IsValidSouthAfricanIdNumber(string idNumber)
    {
        // Check if the ID number matches the format
        Regex regex = new Regex(@"^\d{13}$");
        if (!regex.IsMatch(idNumber))
            return false;

        // Check the birthdate
        int year = int.Parse(idNumber.Substring(0, 2));
        int month = int.Parse(idNumber.Substring(2, 2));
        int day = int.Parse(idNumber.Substring(4, 2));

        // Adjusting the year
        year = year >= 0 && year <= 21 ? 2000 + year : 1900 + year;

        try
        {
            DateTime birthDate = new DateTime(year, month, day);
            // Ensure the birthdate is in a reasonable range
            if (birthDate > DateTime.Now || birthDate < DateTime.Parse("1800-01-01"))
                return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false; // Invalid date components
        }

        // Validate the checksum
        int sum = 0;
        int[] weights = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(idNumber[i].ToString());
            sum += digit * weights[i];
        }
        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit == int.Parse(idNumber[0].ToString());
    }

    public async Task<bool> MobileNumberDuplicateCheck(string mobileNumber, CancellationToken cancellationToken)
    {
        var result = await _context.Clients.AnyAsync(l => l.MobileNumber == mobileNumber, cancellationToken);
        return !result; 
    }

    public async Task<bool> IdNumberDuplicateCheck(string idNumber, CancellationToken cancellationToken)
    {
        var result = await _context.Clients.AnyAsync(l => l.IdNumber == idNumber, cancellationToken);
        return !result;
    }


}
