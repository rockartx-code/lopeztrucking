namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class CompanyPlaceLinkViewModel : ViewModelBase
{
    private int _sortOrder;

    public CompanyPlaceLinkViewModel(Guid placeId, string placeName, int sortOrder)
    {
        PlaceId = placeId;
        PlaceName = placeName;
        _sortOrder = sortOrder;
    }

    public Guid PlaceId { get; }

    public string PlaceName { get; }

    public int SortOrder
    {
        get => _sortOrder;
        set => SetField(ref _sortOrder, value);
    }
}
