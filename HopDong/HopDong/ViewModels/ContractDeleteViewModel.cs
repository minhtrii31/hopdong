using DocumentFormat.OpenXml.InkML;
using HopDong.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace HopDong.ViewModels
{
    public class ContractDeleteViewModel : BindableBase, INavigationAware
    {
        private Contract _selectedContract;
        public Contract SelectedContract
        {
            get { return _selectedContract; }
            set { SetProperty(ref _selectedContract, value); }
        }

        private ContractsDetail _selectedContractDetail;
        public ContractsDetail SelectedContractDetail
        {
            get { return _selectedContractDetail; }
            set { SetProperty(ref _selectedContractDetail, value); }
        }

        private Party _selectedPartyA;
        public Party SelectedPartyA
        {
            get { return _selectedPartyA; }
            set { SetProperty(ref _selectedPartyA, value); }
        }

        private Party _selectedPartyB;
        public Party SelectedPartyB
        {
            get { return _selectedPartyB; }
            set { SetProperty(ref _selectedPartyB, value); }
        }

        public DelegateCommand<Contract> CancelCommand { get; private set; }
        public DelegateCommand<Contract> AcceptCommand { get; private set; }
        private readonly IRegionManager _regionManager;
        public ContractDeleteViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand<Contract>(Cancel);
            AcceptCommand = new DelegateCommand<Contract>(Accept);
        }

        private void Accept(Contract contract)
        {
            contract = SelectedContract;
            using (var context = new HopDongDbContext())
            {
                var contractDetailInDB = context.ContractsDetails.FirstOrDefault(cd => cd.ContractId == contract.ContractId);
                if (contractDetailInDB != null)
                {
                    context.ContractsDetails.Remove(contractDetailInDB);
                }

                var contractInDB = context.Contracts.FirstOrDefault(c => c.ContractId == contract.ContractId);
                if (contractInDB != null)
                {
                    context.Contracts.Remove(contractInDB);
                }

                context.SaveChanges();
            }
            _regionManager.RequestNavigate("ContentRegion", "ContractPage");
        }

        private void Cancel(Contract contract)
        {
            _regionManager.RequestNavigate("ContentRegion", "ContractPage");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("SelectedContract", out Contract selectedContract))
            {
                SelectedContract = selectedContract;

                using (var context = new HopDongDbContext())
                {
                    var contractDetailInDB = context.ContractsDetails.FirstOrDefault(cd => cd.ContractId == selectedContract.ContractId);
                    if (contractDetailInDB != null)
                    {
                        SelectedContractDetail = contractDetailInDB;
                    }

                    var partyAInDB = context.Parties.FirstOrDefault(pA => pA.PartyId == contractDetailInDB.PartyA);
                    if (partyAInDB != null)
                    {
                        SelectedPartyA = partyAInDB;
                    }

                    var partyBinDB = context.Parties.FirstOrDefault(pB => pB.PartyId == contractDetailInDB.PartyB);
                    if (partyBinDB != null)
                    {
                        SelectedPartyB = partyBinDB;
                    }
                }
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
}
