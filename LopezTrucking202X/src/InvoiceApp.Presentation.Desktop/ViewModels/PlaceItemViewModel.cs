using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class PlaceItemViewModel : ViewModelBase
{
    private string _name = string.Empty;
    private string _addressLine1 = string.Empty;
    private string? _addressLine2;
    private string _city = string.Empty;
    private string _state = string.Empty;
    private string _postalCode = string.Empty;
    private bool _isCompany;
    private bool _isOrigin;
    private bool _isDestiny;

    public PlaceItemViewModel(Guid id, bool isNew)
    {
        Id = id;
        IsNew = isNew;
    }

    public Guid Id { get; }

    public bool IsNew { get; set; }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string AddressLine1
    {
        get => _addressLine1;
        set => SetField(ref _addressLine1, value);
    }

    public string? AddressLine2
    {
        get => _addressLine2;
        set => SetField(ref _addressLine2, value);
    }

    public string City
    {
        get => _city;
        set => SetField(ref _city, value);
    }

    public string State
    {
        get => _state;
        set => SetField(ref _state, value);
    }

    public string PostalCode
    {
        get => _postalCode;
        set => SetField(ref _postalCode, value);
    }

    public bool IsCompany
    {
        get => _isCompany;
        set => SetField(ref _isCompany, value);
    }

    public bool IsOrigin
    {
        get => _isOrigin;
        set => SetField(ref _isOrigin, value);
    }

    public bool IsDestiny
    {
        get => _isDestiny;
        set => SetField(ref _isDestiny, value);
    }

    public static PlaceItemViewModel FromPlace(Place place)
    {
        return new PlaceItemViewModel(place.Id, isNew: false)
        {
            Name = place.Name,
            AddressLine1 = place.AddressLine1,
            AddressLine2 = place.AddressLine2,
            City = place.City,
            State = place.State,
            PostalCode = place.PostalCode,
            IsCompany = place.IsCompany,
            IsOrigin = place.IsFrom,
            IsDestiny = place.IsTo
        };
    }

    public Place ToPlace()
    {
        return new Place
        {
            Id = Id,
            Name = Name,
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            City = City,
            State = State,
            PostalCode = PostalCode,
            IsCompany = IsCompany,
            IsFrom = IsOrigin,
            IsTo = IsDestiny
        };
    }
}
