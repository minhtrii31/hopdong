using HopDong.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HopDong.ViewModels
{
    public class ContractUpdateViewModel : BindableBase, INavigationAware
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

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        private readonly IRegionManager _regionManager;

        public ContractUpdateViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            SaveCommand = new DelegateCommand(SaveContract);
            CancelCommand = new DelegateCommand(CancelEdit);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
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


        private void SaveContract()
        {
            using (var context = new HopDongDbContext())
            {
                if (context.Contracts.Any(c => c.ContractId == SelectedContract.ContractId))
                {
                    var contractToUpdate = context.Contracts
                        .FirstOrDefault(c => c.ContractId == SelectedContract.ContractId);

                    if (contractToUpdate != null)
                    {
                        contractToUpdate.ContractName = SelectedContract.ContractName;
                        contractToUpdate.ContractDate = SelectedContract.ContractDate;
                        contractToUpdate.ContractLocation = SelectedContract.ContractLocation;
                        contractToUpdate.ContractDate = SelectedContract.ContractDate;
                        contractToUpdate.ContractEnd = SelectedContract.ContractEnd;
                    }

                    var detailToUpdate = context.ContractsDetails
                        .FirstOrDefault(cd => cd.ContractId == SelectedContract.ContractId);

                    if (detailToUpdate != null)
                    {
                        detailToUpdate.ContractDetailContent = SelectedContractDetail.ContractDetailContent;
                        detailToUpdate.ContractDetailValue = SelectedContractDetail.ContractDetailValue;
                        detailToUpdate.ContractDetailResign = SelectedContractDetail.ContractDetailResign;
                        detailToUpdate.ContractDetailStatus = SelectedContractDetail.ContractDetailStatus;
                        detailToUpdate.PartyA = SelectedContractDetail.PartyA;
                        detailToUpdate.PartyB = SelectedContractDetail.PartyB;
                    }

                    context.SaveChanges();
                }
            }
            _regionManager.RequestNavigate("ContentRegion", "ContractPage");
        }

        private void CancelEdit()
        {
            _regionManager.RequestNavigate("ContentRegion", "ContractPage");
        }

    }
}
