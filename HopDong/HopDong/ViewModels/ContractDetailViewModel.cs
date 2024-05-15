using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HopDong.Models;
using HopDong.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HopDong.ViewModels
{
    public class ContractDetailViewModel : BindableBase, INavigationAware
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

        public DelegateCommand ExportFile { get; private set; }

        public ContractDetailViewModel(Contract contract)
        {
            ExportFile = new DelegateCommand(ExportToDocx);
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
                    if(partyAInDB != null)
                    {
                        SelectedPartyA = partyAInDB;
                    }

                    var partyBinDB = context.Parties.FirstOrDefault(pB => pB.PartyId == contractDetailInDB.PartyB);
                    if(partyBinDB != null)
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

        private void ExportToDocx()
        {
            using (var context = new HopDongDbContext())
            {
                var contractFile = context.ContractTypes
                                         .Where(ct => ct.ContractTypeId == "LHD001")
                                         .Select(ct => ct.ContractFile)
                                         .FirstOrDefault();

                if (contractFile != null && contractFile.Length > 0)
                {
                    string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string outputFilePath = Path.Combine(documentsFolderPath, "output_hopdong.docx");

                    File.WriteAllBytes(outputFilePath, contractFile);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(outputFilePath, true))
                    {
                        MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                        if (mainPart != null)
                        {
                            FillInDocument(mainPart.Document.Body);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Không tìm thấy file docx trong cơ sở dữ liệu.");
                }
            }
        }

        private void FillInDocument(Body body)
        {
            FillInTextContent(body, "<<tenHD/>>", SelectedContract.ContractName);
            FillInTextContent(body, "<<ngay/>>", SelectedContract.ContractDate.Value.Day.ToString());
            FillInTextContent(body, "<<thang/>>", SelectedContract.ContractDate.Value.Month.ToString());
            FillInTextContent(body, "<<nam/>>", SelectedContract.ContractDate.Value.Year.ToString());
            FillInTextContent(body, "<<diadiem/>>", SelectedContract.ContractLocation);
            FillInTextContent(body, "<<ngayDB/>>", DateTimeToString(SelectedContract.ContractStart));
            FillInTextContent(body, "<<ngayKT/>>", DateTimeToString(SelectedContract.ContractEnd));
            FillInTextContent(body, "<<giatri/>>", SelectedContractDetail.ContractDetailValue.ToString());

            using (var dbContext = new HopDongDbContext())
            {
                var selectedPartyAInfo = dbContext.Parties.FirstOrDefault(p => p.PartyId == SelectedContractDetail.PartyA);
                if (selectedPartyAInfo != null)
                {
                    FillInTextContent(body, "<<benA/>>", selectedPartyAInfo.PartyName);
                    FillInTextContent(body, "<<diachiA/>>", selectedPartyAInfo.PartyAddress);
                    FillInTextContent(body, "<<dienthoaiA/>>", selectedPartyAInfo.PartyContact.ToString());
                    FillInTextContent(body, "<<taikhoanA/>>", selectedPartyAInfo.PartyAccount.ToString());
                    FillInTextContent(body, "<<mstA/>>", selectedPartyAInfo.PartyTax.ToString());
                    FillInTextContent(body, "<<daidienA/>>", selectedPartyAInfo.PartyRepresentative);
                    FillInTextContent(body, "<<chucvuA/>>", selectedPartyAInfo.PartyPosition);
                }

                var selectedPartyBInfo = dbContext.Parties.FirstOrDefault(p => p.PartyId == SelectedContractDetail.PartyB);
                if (selectedPartyBInfo != null)
                {
                    FillInTextContent(body, "<<benB/>>", selectedPartyBInfo.PartyName);
                    FillInTextContent(body, "<<diachiB/>>", selectedPartyBInfo.PartyAddress);
                    FillInTextContent(body, "<<dienthoaiB/>>", selectedPartyBInfo.PartyContact.ToString());
                    FillInTextContent(body, "<<taikhoanB/>>", selectedPartyBInfo.PartyAccount.ToString());
                    FillInTextContent(body, "<<mstB/>>", selectedPartyBInfo.PartyTax.ToString());
                    FillInTextContent(body, "<<daidienB/>", selectedPartyBInfo.PartyRepresentative);
                    FillInTextContent(body, "<<chucvuB/>>", selectedPartyBInfo.PartyPosition);
                }
            }
        }

        private void FillInTextContent(Body body, string placeholder, string value)
        {
            foreach (var textElement in body.Descendants<Text>())
            {
                if (textElement.Text.Contains(placeholder))
                {
                    textElement.Text = textElement.Text.Replace(placeholder, value);
                }
            }
        }

        private string DateTimeToString(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return "";
            }
            return dateTime.Value.ToString("dd/MM/yyyy");
        }

    }
}
