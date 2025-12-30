using System.ComponentModel;
using System.Runtime.CompilerServices;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.UseCases;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class AddDetailGroupViewModel : INotifyPropertyChanged
{
    private readonly ISettingRepository _settingRepository;
    private decimal _amountBase;
    private int _emptiesCount;
    private decimal _emptyUnitPrice;

    public AddDetailGroupViewModel(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public decimal AmountBase
    {
        get => _amountBase;
        set
        {
            if (_amountBase == value)
            {
                return;
            }

            _amountBase = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(LineTotal));
        }
    }

    public int EmptiesCount
    {
        get => _emptiesCount;
        set
        {
            if (_emptiesCount == value)
            {
                return;
            }

            _emptiesCount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(LineTotal));
        }
    }

    public decimal EmptyUnitPrice
    {
        get => _emptyUnitPrice;
        private set
        {
            if (_emptyUnitPrice == value)
            {
                return;
            }

            _emptyUnitPrice = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(LineTotal));
        }
    }

    public decimal LineTotal => AmountBase + (EmptiesCount * EmptyUnitPrice);

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var emptyUnitPrice = await _settingRepository
            .GetDecimalByKeyAsync(SettingKeys.EmptyUnitPrice, cancellationToken)
            .ConfigureAwait(false);

        EmptyUnitPrice = emptyUnitPrice ?? 0m;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
