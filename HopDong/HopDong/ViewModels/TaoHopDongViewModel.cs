using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HopDong.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HopDong.ViewModels
{
	public class TaoHopDongViewModel : BindableBase
	{
        private string _contractID;
        public string ContractID
        {
            get { return _contractID; }
            set { SetProperty(ref _contractID, value); }
        }

        private string _contractName;
        public string ContractName
        {
            get { return _contractName; }
            set { SetProperty(ref _contractName, value); }
        }

        private DateTime _contractDate;
        public DateTime ContractDate
        {
            get { return _contractDate; }
            set { SetProperty(ref _contractDate, value); }
        }

        private string _contractLocation;
        public string ContractLocation
        {
            get { return _contractLocation; }
            set { SetProperty(ref _contractLocation, value); }
        }

        private string _selectedPartyA;
        public string SelectedPartyA
        {
            get { return _selectedPartyA; }
            set { SetProperty(ref _selectedPartyA, value); }
        }

        private string _selectedPartyB;
        public string SelectedPartyB
        {
            get { return _selectedPartyB; }
            set { SetProperty(ref _selectedPartyB, value); }
        }

        private string _contractValue;
        public string ContractValue
        {
            get { return _contractValue; }
            set { SetProperty(ref _contractValue, value); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private ObservableCollection<string> _terms;
        public ObservableCollection<string> Terms
        {
            get { return _terms; }
            set { SetProperty(ref _terms, value); }
        }

        private ObservableCollection<string> _partyAOptions;
        public ObservableCollection<string> PartyAOptions
        {
            get { return _partyAOptions; }
            set { SetProperty(ref _partyAOptions, value); }
        }

        private ObservableCollection<string> _partyBOptions;
        public ObservableCollection<string> PartyBOptions
        {
            get { return _partyBOptions; }
            set { SetProperty(ref _partyBOptions, value); }
        }

        public DelegateCommand AddTermCommand { get; private set; }

        private int _nextTermNumber;

        public DelegateCommand GenerateDocumentCommand { get; private set; }

        public TaoHopDongViewModel()
        {
            Terms = new ObservableCollection<string>();
            PartyAOptions = new ObservableCollection<string>();
            PartyBOptions = new ObservableCollection<string>();
            AddTermCommand = new DelegateCommand(ExecuteAddTermCommand);
            _nextTermNumber = 1;
            ContractID = Guid.NewGuid().ToString();
            ContractDate = DateTime.Now;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            LoadPartyOptions();

            GenerateDocumentCommand = new DelegateCommand(ExecuteGenerateDocumentCommand);
        }

        private void ExecuteAddTermCommand()
        {
            Terms.Add($"Điều {_nextTermNumber++}:");
        }

        private void LoadPartyOptions()
        {
            using (var dbContext = new HopDongDbContext())
            {
                var partyAList = dbContext.Parties.Select(p => p.PartyName).ToList();
                var partyBList = dbContext.Parties.Select(p => p.PartyName).ToList();

                PartyAOptions.AddRange(partyAList);
                PartyBOptions.AddRange(partyBList);
            }
        }

        private void ExecuteGenerateDocumentCommand()
        {
            Contract newContract = new()
            {
                ContractId = ContractID,
                ContractDate = ContractDate,
                ContractName = ContractName,
                ContractType = "LHD001",
                ContractLocation = ContractLocation,
                ContractStart = StartDate,
                ContractEnd = EndDate
            };

            using (var context = new HopDongDbContext())
            {
                context.Contracts.Add(newContract);
                context.SaveChanges();

                string newContractID = newContract.ContractId;

                var selectedPartyAID = context.Parties.Where(p => p.PartyName == SelectedPartyA).Select(p => p.PartyId).FirstOrDefault();
                var selectedPartyBID = context.Parties.Where(p => p.PartyName == SelectedPartyB).Select(p => p.PartyId).FirstOrDefault();

                ContractsDetail contractDetail = new ContractsDetail
                {
                    ContractDetailId = Guid.NewGuid().ToString(),
                    ContractId = newContractID,
                    ContractDetailContent = ContractName,
                    ContractDetailValue = int.Parse(ContractValue),
                    ContractDetailStatus = "Đã ký",
                    ContractDetailResign = 0,
                    PartyA = selectedPartyAID,
                    PartyB = selectedPartyBID,
                };

                context.ContractsDetails.Add(contractDetail);
                context.SaveChanges();
            }

            GenerateWordDocument();
        }

        private void GenerateWordDocument()
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


        /*private void GenerateWordDocument()
        {
            string templateFilePath = "./Assets/Templates/hopdong.docx";
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputFilePath = Path.Combine(documentsFolderPath, "output_hopdong.docx");

            File.Copy(templateFilePath, outputFilePath, true);

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(outputFilePath, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                if (mainPart != null)
                {
                    FillInDocument(mainPart.Document.Body);
                }
            }
        }*/

        private string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }


        private void FillInDocument(Body body)
        {
            FillInTextContent(body, "<<tenHD/>>", ContractName);
            FillInTextContent(body, "<<ngay/>>", ContractDate.Day.ToString());
            FillInTextContent(body, "<<thang/>>", ContractDate.Month.ToString());
            FillInTextContent(body, "<<nam/>>", ContractDate.Year.ToString());
            FillInTextContent(body, "<<diadiem/>>", ContractLocation);
            FillInTextContent(body, "<<ngayDB/>>", DateTimeToString(StartDate));
            FillInTextContent(body, "<<ngayKT/>>", DateTimeToString(EndDate));
            FillInTextContent(body, "<<giatri/>>", ContractValue.ToString());

            using (var dbContext = new HopDongDbContext())
            {
                var selectedPartyAInfo = dbContext.Parties.FirstOrDefault(p => p.PartyName == SelectedPartyA);
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

                var selectedPartyBInfo = dbContext.Parties.FirstOrDefault(p => p.PartyName == SelectedPartyB);
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
    }
}
