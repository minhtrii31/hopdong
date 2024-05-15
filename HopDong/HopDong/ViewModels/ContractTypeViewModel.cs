using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HopDong.ViewModels
{
	public class ContractTypeViewModel : BindableBase
	{
        private string _selectedContractType;
        public string SelectedContractType
        {
            get { return _selectedContractType; }
            set { SetProperty(ref _selectedContractType, value); }
        }

        private List<string> _contractTypes;
        public List<string> ContractTypes
        {
            get { return _contractTypes; }
            set { SetProperty(ref _contractTypes, value); }
        }

        private readonly IRegionManager _regionManager;

        public DelegateCommand CreateContractTypeCommand { get; private set; }

        public ContractTypeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            ContractTypes = new List<string>
            {
                "Hợp đồng",
                "Thương thảo",
                "Báo giá",
                "Nghiệm thu"
            };

            SelectedContractType = ContractTypes.FirstOrDefault();

            CreateContractTypeCommand = new DelegateCommand(ExecuteCreateContractTypeCommand);
        }

        private void ExecuteCreateContractTypeCommand()
        {
            if (SelectedContractType == "Thương thảo")
            {
                _regionManager.RequestNavigate("ContentRegion", "ThuongThao");
            }
            else if (SelectedContractType == "Hợp đồng")
            {
                _regionManager.RequestNavigate("ContentRegion", "TaoHopDong");
            }
            else
            {
                MessageBox.Show("Chỉ khi chọn '' mới có thể thêm mới hợp đồng.");
            }
        }
    }
}
