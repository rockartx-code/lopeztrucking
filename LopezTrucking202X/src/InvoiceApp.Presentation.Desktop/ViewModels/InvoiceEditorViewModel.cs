using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using InvoiceApp.Application.Interfaces;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class InvoiceEditorViewModel : INotifyPropertyChanged
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IPlaceRepository _placeRepository;
    private string _invoiceNo = string.Empty;
    private DateTime _invoiceDate = DateTime.Today;
    private string _checkNo = string.Empty;
    private SubhaulerInfo? _selectedSubhauler;
    private string _dispatch = string.Empty;
    private string _fb = string.Empty;
    private DateTime _detailDate = DateTime.Today;
    private int _empties;
    private decimal _amountBase;
    private decimal _advance;
    private DetailGroupViewModel? _selectedDetailGroup;
    private bool _isEditing;
    private decimal _subtotal;
    private decimal _total;

    public InvoiceEditorViewModel(ICompanyRepository companyRepository, IPlaceRepository placeRepository)
    {
        _companyRepository = companyRepository;
        _placeRepository = placeRepository;
        Subhaulers = new ObservableCollection<SubhaulerInfo>
        {
            new() { Name = "Lopez Trucking", ContactName = "R. Lopez", Phone = "(555) 123-4567" },
            new() { Name = "Hernandez Hauling", ContactName = "A. Hernandez", Phone = "(555) 987-6543" }
        };

        Companies = new ObservableCollection<SelectableItem>();
        FromPlaces = new ObservableCollection<SelectableItem>();
        ToPlaces = new ObservableCollection<SelectableItem>();

        DetailGroups = new ObservableCollection<DetailGroupViewModel>();
        DetailGroups.CollectionChanged += OnDetailGroupsChanged;

        SubscribeToSelectionChanges(Companies, UpdateEntryCompanyDisplay, () => _ = UpdatePlacesForCompaniesAsync());
        SubscribeToSelectionChanges(FromPlaces, UpdateEntryFromDisplay);
        SubscribeToSelectionChanges(ToPlaces, UpdateEntryToDisplay);

        AddDetailGroupCommand = new RelayCommand(AddOrUpdateDetailGroup, CanAddOrUpdateDetailGroup);
        EditDetailGroupCommand = new RelayCommand(BeginEditDetailGroup, () => SelectedDetailGroup is not null);
        DeleteDetailGroupCommand = new RelayCommand(DeleteDetailGroup, () => SelectedDetailGroup is not null);
        CancelEditCommand = new RelayCommand(CancelEdit, () => IsEditing);

        UpdateEntryCompanyDisplay();
        UpdateEntryFromDisplay();
        UpdateEntryToDisplay();
        UpdateTotals();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<SubhaulerInfo> Subhaulers { get; }

    public SubhaulerInfo? SelectedSubhauler
    {
        get => _selectedSubhauler;
        set => SetField(ref _selectedSubhauler, value);
    }

    public string InvoiceNo
    {
        get => _invoiceNo;
        set => SetField(ref _invoiceNo, value);
    }

    public DateTime InvoiceDate
    {
        get => _invoiceDate;
        set => SetField(ref _invoiceDate, value);
    }

    public string CheckNo
    {
        get => _checkNo;
        set => SetField(ref _checkNo, value);
    }

    public ObservableCollection<SelectableItem> Companies { get; }

    public ObservableCollection<SelectableItem> FromPlaces { get; }

    public ObservableCollection<SelectableItem> ToPlaces { get; }

    public string EntryCompaniesDisplay { get; private set; } = string.Empty;

    public string EntryFromDisplay { get; private set; } = string.Empty;

    public string EntryToDisplay { get; private set; } = string.Empty;

    public string Dispatch
    {
        get => _dispatch;
        set => SetField(ref _dispatch, value);
    }

    public string Fb
    {
        get => _fb;
        set => SetField(ref _fb, value);
    }

    public DateTime DetailDate
    {
        get => _detailDate;
        set => SetField(ref _detailDate, value);
    }

    public int Empties
    {
        get => _empties;
        set => SetField(ref _empties, value);
    }

    public decimal AmountBase
    {
        get => _amountBase;
        set => SetField(ref _amountBase, value);
    }

    public ObservableCollection<DetailGroupViewModel> DetailGroups { get; }

    public DetailGroupViewModel? SelectedDetailGroup
    {
        get => _selectedDetailGroup;
        set
        {
            if (SetField(ref _selectedDetailGroup, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public bool IsEditing
    {
        get => _isEditing;
        private set
        {
            if (SetField(ref _isEditing, value))
            {
                OnPropertyChanged(nameof(AddButtonText));
                RaiseCommandStates();
            }
        }
    }

    public string AddButtonText => IsEditing ? "Save" : "Add";

    public decimal Subtotal
    {
        get => _subtotal;
        private set => SetField(ref _subtotal, value);
    }

    public decimal Advance
    {
        get => _advance;
        set
        {
            if (SetField(ref _advance, value))
            {
                UpdateTotals();
            }
        }
    }

    public decimal Total
    {
        get => _total;
        private set => SetField(ref _total, value);
    }

    public ICommand AddDetailGroupCommand { get; }

    public ICommand EditDetailGroupCommand { get; }

    public ICommand DeleteDetailGroupCommand { get; }

    public ICommand CancelEditCommand { get; }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _companyRepository
            .GetAllAsync(cancellationToken)
            .ConfigureAwait(true);

        ReplaceSelectableItems(Companies, companies.Select(company => (company.Id, company.Name)));
        await UpdatePlacesForCompaniesAsync(cancellationToken).ConfigureAwait(true);
    }

    public void LoadFromBrowser(InvoiceBrowserItem invoice)
    {
        if (invoice is null)
        {
            return;
        }

        InvoiceNo = invoice.InvoiceNumber;
        InvoiceDate = invoice.InvoiceDate;
        SelectedSubhauler = Subhaulers.FirstOrDefault(subhauler =>
            string.Equals(subhauler.Name, invoice.SubhaulerName, StringComparison.OrdinalIgnoreCase));
    }

    private void AddOrUpdateDetailGroup()
    {
        if (IsEditing && SelectedDetailGroup is not null)
        {
            SelectedDetailGroup.Dispatch = Dispatch;
            SelectedDetailGroup.Fb = Fb;
            SelectedDetailGroup.DetailDate = DetailDate;
            SelectedDetailGroup.Empties = Empties;
            SelectedDetailGroup.AmountBase = AmountBase;
            SelectedDetailGroup.Companies = EntryCompaniesDisplay;
            SelectedDetailGroup.FromPlaces = EntryFromDisplay;
            SelectedDetailGroup.ToPlaces = EntryToDisplay;
            SelectedDetailGroup.UpdateComputedAmounts();
        }
        else
        {
            var detail = new DetailGroupViewModel
            {
                Dispatch = Dispatch,
                Fb = Fb,
                DetailDate = DetailDate,
                Empties = Empties,
                AmountBase = AmountBase,
                Companies = EntryCompaniesDisplay,
                FromPlaces = EntryFromDisplay,
                ToPlaces = EntryToDisplay
            };

            detail.UpdateComputedAmounts();
            DetailGroups.Add(detail);
        }

        ClearEntry();
        IsEditing = false;
        UpdateTotals();
    }

    private bool CanAddOrUpdateDetailGroup()
    {
        return !string.IsNullOrWhiteSpace(Dispatch)
               || !string.IsNullOrWhiteSpace(Fb)
               || AmountBase > 0
               || Empties > 0
               || !string.IsNullOrWhiteSpace(EntryCompaniesDisplay)
               || !string.IsNullOrWhiteSpace(EntryFromDisplay)
               || !string.IsNullOrWhiteSpace(EntryToDisplay);
    }

    private void BeginEditDetailGroup()
    {
        if (SelectedDetailGroup is null)
        {
            return;
        }

        Dispatch = SelectedDetailGroup.Dispatch;
        Fb = SelectedDetailGroup.Fb;
        DetailDate = SelectedDetailGroup.DetailDate;
        Empties = SelectedDetailGroup.Empties;
        AmountBase = SelectedDetailGroup.AmountBase;

        SetSelectionsFromDisplay(Companies, SelectedDetailGroup.Companies);
        SetSelectionsFromDisplay(FromPlaces, SelectedDetailGroup.FromPlaces);
        SetSelectionsFromDisplay(ToPlaces, SelectedDetailGroup.ToPlaces);

        IsEditing = true;
    }

    private void DeleteDetailGroup()
    {
        if (SelectedDetailGroup is null)
        {
            return;
        }

        DetailGroups.Remove(SelectedDetailGroup);
        SelectedDetailGroup = null;
        UpdateTotals();
    }

    private void CancelEdit()
    {
        ClearEntry();
        IsEditing = false;
    }

    private void ClearEntry()
    {
        Dispatch = string.Empty;
        Fb = string.Empty;
        DetailDate = DateTime.Today;
        Empties = 0;
        AmountBase = 0m;
        ClearSelections(Companies);
        ClearSelections(FromPlaces);
        ClearSelections(ToPlaces);
    }

    private void OnDetailGroupsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
        {
            foreach (DetailGroupViewModel item in e.NewItems)
            {
                item.PropertyChanged += OnDetailGroupPropertyChanged;
            }
        }

        if (e.OldItems is not null)
        {
            foreach (DetailGroupViewModel item in e.OldItems)
            {
                item.PropertyChanged -= OnDetailGroupPropertyChanged;
            }
        }

        UpdateTotals();
    }

    private void OnDetailGroupPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(DetailGroupViewModel.Total) or nameof(DetailGroupViewModel.AmountBase))
        {
            UpdateTotals();
        }
    }

    private void UpdateTotals()
    {
        Subtotal = DetailGroups.Sum(group => group.Total);
        Total = Subtotal - Advance;
    }

    private void SubscribeToSelectionChanges(
        ObservableCollection<SelectableItem> items,
        Action updateDisplay,
        Action? selectionChanged = null)
    {
        void AttachHandler(SelectableItem item)
        {
            item.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(SelectableItem.IsSelected))
                {
                    updateDisplay();
                    selectionChanged?.Invoke();
                }
            };
        }

        foreach (var item in items)
        {
            AttachHandler(item);
        }

        items.CollectionChanged += (_, args) =>
        {
            if (args.NewItems is not null)
            {
                foreach (SelectableItem item in args.NewItems)
                {
                    AttachHandler(item);
                }
            }
        };
    }

    private void UpdateEntryCompanyDisplay()
    {
        EntryCompaniesDisplay = string.Join(", ", Companies.Where(item => item.IsSelected).Select(item => item.Name));
        OnPropertyChanged(nameof(EntryCompaniesDisplay));
        RaiseCommandStates();
    }

    private void UpdateEntryFromDisplay()
    {
        EntryFromDisplay = string.Join(", ", FromPlaces.Where(item => item.IsSelected).Select(item => item.Name));
        OnPropertyChanged(nameof(EntryFromDisplay));
        RaiseCommandStates();
    }

    private void UpdateEntryToDisplay()
    {
        EntryToDisplay = string.Join(", ", ToPlaces.Where(item => item.IsSelected).Select(item => item.Name));
        OnPropertyChanged(nameof(EntryToDisplay));
        RaiseCommandStates();
    }

    private async Task UpdatePlacesForCompaniesAsync(CancellationToken cancellationToken = default)
    {
        var selectedCompanyIds = Companies
            .Where(item => item.IsSelected)
            .Select(item => item.Id)
            .ToArray();

        var places = await _placeRepository
            .GetByCompanyIdsAsync(selectedCompanyIds, cancellationToken)
            .ConfigureAwait(true);

        ReplaceSelectableItems(FromPlaces, places.Select(place => (place.Id, place.Name)), autoSelectSingle: true);
        ReplaceSelectableItems(ToPlaces, places.Select(place => (place.Id, place.Name)), autoSelectSingle: true);
        UpdateEntryFromDisplay();
        UpdateEntryToDisplay();
    }

    private static void ReplaceSelectableItems(
        ObservableCollection<SelectableItem> items,
        IEnumerable<(Guid Id, string Name)> values,
        bool autoSelectSingle = false)
    {
        items.Clear();
        foreach (var (id, name) in values)
        {
            items.Add(new SelectableItem(id, name));
        }

        if (autoSelectSingle && items.Count == 1)
        {
            items[0].IsSelected = true;
        }
    }

    private static void SetSelectionsFromDisplay(ObservableCollection<SelectableItem> items, string display)
    {
        var selected = new HashSet<string>(display.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        foreach (var item in items)
        {
            item.IsSelected = selected.Contains(item.Name);
        }
    }

    private static void ClearSelections(ObservableCollection<SelectableItem> items)
    {
        foreach (var item in items)
        {
            item.IsSelected = false;
        }
    }

    private void RaiseCommandStates()
    {
        (AddDetailGroupCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (EditDetailGroupCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (DeleteDetailGroupCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (CancelEditCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class SubhaulerInfo
{
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Phone { get; set; }

    public string Display => string.IsNullOrWhiteSpace(ContactName)
        ? Name
        : $"{Name} ({ContactName})";
}

public sealed class SelectableItem : INotifyPropertyChanged
{
    private bool _isSelected;

    public SelectableItem(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Guid Id { get; }

    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value)
            {
                return;
            }

            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }
}

public sealed class DetailGroupViewModel : INotifyPropertyChanged
{
    private string _dispatch = string.Empty;
    private string _fb = string.Empty;
    private DateTime _detailDate = DateTime.Today;
    private int _empties;
    private decimal _amountBase;
    private decimal _emptyAmount;
    private decimal _total;
    private string _companies = string.Empty;
    private string _fromPlaces = string.Empty;
    private string _toPlaces = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Dispatch
    {
        get => _dispatch;
        set => SetField(ref _dispatch, value);
    }

    public string Fb
    {
        get => _fb;
        set => SetField(ref _fb, value);
    }

    public DateTime DetailDate
    {
        get => _detailDate;
        set => SetField(ref _detailDate, value);
    }

    public int Empties
    {
        get => _empties;
        set => SetField(ref _empties, value);
    }

    public decimal AmountBase
    {
        get => _amountBase;
        set => SetField(ref _amountBase, value);
    }

    public decimal EmptyAmount
    {
        get => _emptyAmount;
        private set => SetField(ref _emptyAmount, value);
    }

    public decimal Total
    {
        get => _total;
        private set => SetField(ref _total, value);
    }

    public string Companies
    {
        get => _companies;
        set => SetField(ref _companies, value);
    }

    public string FromPlaces
    {
        get => _fromPlaces;
        set => SetField(ref _fromPlaces, value);
    }

    public string ToPlaces
    {
        get => _toPlaces;
        set => SetField(ref _toPlaces, value);
    }

    public void UpdateComputedAmounts()
    {
        EmptyAmount = Empties * 0m;
        Total = AmountBase + EmptyAmount;
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute ?? (() => true);
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute();

    public void Execute(object? parameter) => _execute();

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
