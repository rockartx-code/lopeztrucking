using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class CompanyPlacesAdminViewModel : ViewModelBase
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IPlaceRepository _placeRepository;
    private readonly ICompanyPlaceRepository _companyPlaceRepository;
    private LookupItem? _selectedCompany;
    private LookupItem? _selectedPlaceToAdd;
    private CompanyPlaceLinkViewModel? _selectedLink;
    private int _sortOrderToAdd;

    public CompanyPlacesAdminViewModel(
        ICompanyRepository companyRepository,
        IPlaceRepository placeRepository,
        ICompanyPlaceRepository companyPlaceRepository)
    {
        _companyRepository = companyRepository;
        _placeRepository = placeRepository;
        _companyPlaceRepository = companyPlaceRepository;
        Companies = new ObservableCollection<LookupItem>();
        Places = new ObservableCollection<LookupItem>();
        CompanyPlaces = new ObservableCollection<CompanyPlaceLinkViewModel>();
        AddLinkCommand = new RelayCommand(AddLink, CanAddLink);
        RemoveLinkCommand = new RelayCommand(RemoveLink, () => SelectedLink is not null);
        SaveCommand = new RelayCommand(SaveLinks, () => SelectedCompany is not null);
        RefreshCommand = new RelayCommand(() => _ = LoadAsync());
    }

    public event EventHandler? DataSaved;

    public ObservableCollection<LookupItem> Companies { get; }

    public ObservableCollection<LookupItem> Places { get; }

    public ObservableCollection<CompanyPlaceLinkViewModel> CompanyPlaces { get; }

    public LookupItem? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            if (SetField(ref _selectedCompany, value))
            {
                RaiseCommandStates();
                _ = LoadLinksAsync();
            }
        }
    }

    public LookupItem? SelectedPlaceToAdd
    {
        get => _selectedPlaceToAdd;
        set
        {
            if (SetField(ref _selectedPlaceToAdd, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public CompanyPlaceLinkViewModel? SelectedLink
    {
        get => _selectedLink;
        set
        {
            if (SetField(ref _selectedLink, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public int SortOrderToAdd
    {
        get => _sortOrderToAdd;
        set => SetField(ref _sortOrderToAdd, value);
    }

    public ICommand AddLinkCommand { get; }

    public ICommand RemoveLinkCommand { get; }

    public ICommand SaveCommand { get; }

    public ICommand RefreshCommand { get; }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _companyRepository
            .GetAllAsync(cancellationToken)
            .ConfigureAwait(true);
        var places = await _placeRepository
            .GetAllAsync(cancellationToken)
            .ConfigureAwait(true);

        Companies.Clear();
        foreach (var company in companies)
        {
            Companies.Add(new LookupItem(company.Id, company.Name));
        }

        Places.Clear();
        foreach (var place in places.OrderBy(place => place.Name))
        {
            Places.Add(new LookupItem(place.Id, place.Name));
        }

        SelectedCompany ??= Companies.FirstOrDefault();
    }

    private async Task LoadLinksAsync(CancellationToken cancellationToken = default)
    {
        CompanyPlaces.Clear();
        SelectedLink = null;

        if (SelectedCompany is null)
        {
            return;
        }

        var links = await _companyPlaceRepository
            .GetByCompanyIdAsync(SelectedCompany.Id, cancellationToken)
            .ConfigureAwait(true);

        var placeLookup = Places.ToDictionary(place => place.Id, place => place.Name);
        foreach (var link in links.OrderBy(link => link.SortOrder))
        {
            if (!placeLookup.TryGetValue(link.PlaceId, out var placeName))
            {
                placeName = link.PlaceId.ToString();
            }

            CompanyPlaces.Add(new CompanyPlaceLinkViewModel(link.PlaceId, placeName, link.SortOrder));
        }
    }

    private void AddLink()
    {
        if (SelectedCompany is null || SelectedPlaceToAdd is null)
        {
            return;
        }

        var existing = CompanyPlaces.FirstOrDefault(link => link.PlaceId == SelectedPlaceToAdd.Id);
        if (existing is not null)
        {
            existing.SortOrder = SortOrderToAdd;
            SelectedLink = existing;
        }
        else
        {
            var link = new CompanyPlaceLinkViewModel(
                SelectedPlaceToAdd.Id,
                SelectedPlaceToAdd.Name,
                SortOrderToAdd);
            CompanyPlaces.Add(link);
            SelectedLink = link;
        }

        SortOrderToAdd = 0;
        SelectedPlaceToAdd = null;
    }

    private void RemoveLink()
    {
        if (SelectedLink is null)
        {
            return;
        }

        var link = SelectedLink;
        SelectedLink = null;
        CompanyPlaces.Remove(link);
    }

    private async void SaveLinks()
    {
        if (SelectedCompany is null)
        {
            return;
        }

        var payload = CompanyPlaces
            .Select(link => new CompanyPlaceLinkDto(link.PlaceId, link.SortOrder))
            .ToList();

        await _companyPlaceRepository
            .ReplaceLinksAsync(SelectedCompany.Id, payload)
            .ConfigureAwait(true);
        await _companyPlaceRepository.SaveChangesAsync().ConfigureAwait(true);
        DataSaved?.Invoke(this, EventArgs.Empty);
    }

    private bool CanAddLink()
    {
        return SelectedCompany is not null && SelectedPlaceToAdd is not null;
    }

    private void RaiseCommandStates()
    {
        (AddLinkCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (RemoveLinkCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }
}
