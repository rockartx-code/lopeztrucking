using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.UseCases.Commands;

public sealed class CreateOrUpdateSubhaulerHandler
{
    private readonly ISubhaulerRepository _subhaulerRepository;

    public CreateOrUpdateSubhaulerHandler(ISubhaulerRepository subhaulerRepository)
    {
        _subhaulerRepository = subhaulerRepository;
    }

    public async Task<Subhauler> HandleAsync(
        CreateOrUpdateSubhauler command,
        CancellationToken cancellationToken = default)
    {
        var subhauler = await _subhaulerRepository
            .GetByIdAsync(command.Id, cancellationToken)
            .ConfigureAwait(false);

        if (subhauler is null)
        {
            subhauler = new Subhauler { Id = command.Id };
            await _subhaulerRepository
                .AddAsync(subhauler, cancellationToken)
                .ConfigureAwait(false);
        }

        subhauler.Name = command.Name;
        subhauler.ContactName = command.ContactName;
        subhauler.Phone = command.Phone;

        await _subhaulerRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return subhauler;
    }
}
