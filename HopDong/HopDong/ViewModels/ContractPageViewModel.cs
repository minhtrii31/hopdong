using HopDong.Models;
using Microsoft.VisualBasic.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

public class ContractPageViewModel : BindableBase, INavigationAware
{
    private ObservableCollection<Contract> _contractCollection;
    public ObservableCollection<Contract> ContractCollection
    {
        get { return _contractCollection; }
        set { SetProperty(ref _contractCollection, value); }
    }

    public DelegateCommand<Contract> DetailCommand { get; private set; }
    public DelegateCommand<Contract> EditCommand { get; private set; }
    public DelegateCommand<Contract> DeleteCommand { get; private set; }

    private readonly IRegionManager _regionManager;

    public ContractPageViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        using (var context = new HopDongDbContext())
        {
            ContractCollection = new ObservableCollection<Contract>(context.Contracts.ToList());
        }
        DetailCommand = new DelegateCommand<Contract>(Detail);
        EditCommand = new DelegateCommand<Contract>(Edit);
        DeleteCommand = new DelegateCommand<Contract>(Delete);
    }

    private void Detail(Contract contract)
    {
        var parameters = new NavigationParameters();
        parameters.Add("SelectedContract", contract);
        _regionManager.RequestNavigate("ContentRegion", "ContractDetail", parameters);
    }

    private void Edit(Contract contract)
    {
        var parameters = new NavigationParameters();
        parameters.Add("SelectedContract", contract);
        _regionManager.RequestNavigate("ContentRegion", "ContractUpdate", parameters);
    }

    private void Delete(Contract contract)
    {
        var parameters = new NavigationParameters();
        parameters.Add("SelectedContract", contract);
        _regionManager.RequestNavigate("ContentRegion", "ContractDelete", parameters);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        using (var context = new HopDongDbContext())
        {
            ContractCollection = new ObservableCollection<Contract>(context.Contracts.ToList());
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        
    }
}
