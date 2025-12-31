using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using InvoiceApp.Application.Interfaces;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class PlacesAdminViewModel : ViewModelBase
{
    private readonly IPlaceRepository _placeRepository;
    private PlaceItemViewModel? _selectedPlace;

    public PlacesAdminViewModel(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
        Places = new ObservableCollection<PlaceItemViewModel>();
        NewPlaceCommand = new RelayCommand(AddNewPlace);
        SavePlaceCommand = new RelayCommand(SavePlace, () => SelectedPlace is not null);
        DeletePlaceCommand = new RelayCommand(DeletePlace, () => SelectedPlace is not null);
        RefreshCommand = new RelayCommand(() => _ = LoadAsync());
    }

    public event EventHandler? DataSaved;

    public ObservableCollection<PlaceItemViewModel> Places { get; }

    public PlaceItemViewModel? SelectedPlace
    {
        get => _selectedPlace;
        set
        {
            if (SetField(ref _selectedPlace, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public ICommand NewPlaceCommand { get; }

    public ICommand SavePlaceCommand { get; }

    public ICommand DeletePlaceCommand { get; }

    public ICommand RefreshCommand { get; }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var places = await _placeRepository
            .GetAllAsync(cancellationToken)
            .ConfigureAwait(true);

        Places.Clear();
        foreach (var place in places.Select(PlaceItemViewModel.FromPlace))
        {
            Places.Add(place);
        }

        SelectedPlace ??= Places.FirstOrDefault();
    }

    private void AddNewPlace()
    {
        var place = new PlaceItemViewModel(Guid.NewGuid(), isNew: true);
        Places.Add(place);
        SelectedPlace = place;
    }

    private async void SavePlace()
    {
        if (SelectedPlace is null)
        {
            return;
        }

        if (SelectedPlace.IsNew)
        {
            await _placeRepository.AddAsync(SelectedPlace.ToPlace()).ConfigureAwait(true);
            SelectedPlace.IsNew = false;
        }
        else
        {
            await _placeRepository.UpdateAsync(SelectedPlace.ToPlace()).ConfigureAwait(true);
        }

        await _placeRepository.SaveChangesAsync().ConfigureAwait(true);
        DataSaved?.Invoke(this, EventArgs.Empty);
    }

    private async void DeletePlace()
    {
        if (SelectedPlace is null)
        {
            return;
        }

        var toRemove = SelectedPlace;
        SelectedPlace = null;

        Places.Remove(toRemove);
        if (!toRemove.IsNew)
        {
            await _placeRepository.DeleteAsync(toRemove.Id).ConfigureAwait(true);
            await _placeRepository.SaveChangesAsync().ConfigureAwait(true);
        }

        SelectedPlace = Places.FirstOrDefault();
        DataSaved?.Invoke(this, EventArgs.Empty);
    }

    private void RaiseCommandStates()
    {
        (SavePlaceCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (DeletePlaceCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }
}
