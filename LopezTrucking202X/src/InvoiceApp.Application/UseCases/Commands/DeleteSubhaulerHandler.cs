using InvoiceApp.Application.Interfaces;

namespace InvoiceApp.Application.UseCases.Commands;

public sealed class DeleteSubhaulerHandler
{
    private readonly ISubhaulerRepository _subhaulerRepository;

    public DeleteSubhaulerHandler(ISubhaulerRepository subhaulerRepository)
    {
        _subhaulerRepository = subhaulerRepository;
    }

    public async Task<bool> HandleAsync(DeleteSubhauler command, CancellationToken cancellationToken = default)
    {
        var subhauler = await _subhaulerRepository
            .GetByIdAsync(command.Id, cancellationToken)
            .ConfigureAwait(false);

        if (subhauler is null)
        {
            return false;
        }

        _subhaulerRepository.Remove(subhauler);
        await _subhaulerRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }
}
