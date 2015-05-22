using System;
using NUnit.Framework;
using Silanis.ESL.SDK;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDK.Examples
{
	[TestFixture()]
    public class DownloadReportExampleTest
    {
		[Test()]
		public void VerifyResult()
		{
            DownloadReportExample example = new DownloadReportExample(Props.GetInstance());
			example.Run();

            // Assert correct download of completion report for a sender
            CompletionReport completionReportForSender = example.sdkCompletionReportForSenderDraft;
            Assert.AreEqual(completionReportForSender.Senders.Count, 1, "There should be only 1 sender.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages.Count, 1, "Number of package completion reports should be greater than 1.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages[0].Documents.Count, 1, "Number of document completion reports should be greater than 1.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages[0].Signers.Count, 1, "Number of signer completion reports should be greater than 1.");

            AssertCreatedPackageIncludedInCompletionReport(completionReportForSender, example.senderUID, example.PackageId, "DRAFT");

            Assert.IsNotNull(example.csvCompletionReportForSenderDraft);
            Assert.IsNotEmpty(example.csvCompletionReportForSenderDraft);

            CSVReader reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvCompletionReportForSenderDraft))));
            IList<string[]> rows = reader.readAll();

            if(completionReportForSender.Senders[0].Packages.Count > 0) {
                Assert.AreEqual(completionReportForSender.Senders[0].Packages.Count + 1, rows.Count);
            }

            AssertCreatedPackageIncludedInCSV(rows, example.PackageId, "DRAFT");

            completionReportForSender = example.sdkCompletionReportForSenderSent;
            Assert.AreEqual(completionReportForSender.Senders.Count, 1, "There should be only 1 sender.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages.Count, 1, "Number of package completion reports should be greater than 1.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages[0].Documents.Count, 1, "Number of document completion reports should be greater than 1.");
            Assert.GreaterOrEqual(completionReportForSender.Senders[0].Packages[0].Signers.Count, 1, "Number of signer completion reports should be greater than 1.");

            AssertCreatedPackageIncludedInCompletionReport(completionReportForSender, example.senderUID, example.package2Id, "SENT");

            Assert.IsNotNull(example.csvCompletionReportForSenderSent);
            Assert.IsNotEmpty(example.csvCompletionReportForSenderSent);

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvCompletionReportForSenderSent))));
            rows = reader.readAll();

            if(completionReportForSender.Senders[0].Packages.Count > 0) {
                Assert.AreEqual(completionReportForSender.Senders[0].Packages.Count + 1, rows.Count);
            }

            AssertCreatedPackageIncludedInCSV(rows, example.package2Id, "SENT");

            // Assert correct download of completion report for all senders
            CompletionReport completionReport = example.sdkCompletionReportDraft;
            Assert.GreaterOrEqual(completionReport.Senders.Count, 1, "Number of sender should be greater than 1.");
            Assert.GreaterOrEqual(completionReport.Senders[0].Packages.Count, 0, "Number of package completion reports should be greater than 0.");

            AssertCreatedPackageIncludedInCompletionReport(completionReport, example.senderUID, example.PackageId, "DRAFT");

            Assert.IsNotNull(example.csvCompletionReportDraft);
            Assert.IsNotEmpty(example.csvCompletionReportDraft);

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvCompletionReportDraft))));
            rows = reader.readAll();

            if(completionReport.Senders[0].Packages.Count > 0) {
                Assert.AreEqual(GetCompletionReportCount(completionReport) + 1, rows.Count);
            }

            AssertCreatedPackageIncludedInCSV(rows, example.PackageId, "DRAFT");

            completionReport = example.sdkCompletionReportSent;
            Assert.GreaterOrEqual(completionReport.Senders.Count, 1, "Number of sender should be greater than 1.");
            Assert.GreaterOrEqual(completionReport.Senders[0].Packages.Count, 0, "Number of package completion reports should be greater than 0.");

            AssertCreatedPackageIncludedInCompletionReport(completionReport, example.senderUID, example.package2Id, "SENT");

            Assert.IsNotNull(example.csvCompletionReportSent);
            Assert.IsNotEmpty(example.csvCompletionReportSent);

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvCompletionReportSent))));
            rows = reader.readAll();

            if(completionReport.Senders[0].Packages.Count > 0) {
                Assert.AreEqual(GetCompletionReportCount(completionReport) + 1, rows.Count);
            }

            AssertCreatedPackageIncludedInCSV(rows, example.package2Id, "SENT");

            // Assert correct download of usage report
            UsageReport usageReport = example.sdkUsageReport;
            Assert.Greater(usageReport.SenderUsageReports.Count, 0, "There should be only 1 sender.");
            Assert.Greater(usageReport.SenderUsageReports[0].CountByUsageReportCategory.Count, 0, "Number of dictionary entries should be greater than 0.");
            Assert.IsTrue(usageReport.SenderUsageReports[0].CountByUsageReportCategory.ContainsKey(UsageReportCategory.DRAFT), "There should be at a draft key in packages map.");
            Assert.Greater(usageReport.SenderUsageReports[0].CountByUsageReportCategory[UsageReportCategory.DRAFT], 0, "Number of drafts should be greater than 0.");

            Assert.IsNotNull(example.csvUsageReport, "Usage report in csv cannot be null.");
            Assert.IsNotEmpty(example.csvUsageReport, "Usage report in csv cannot be empty.");

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvUsageReport))));
            rows = reader.readAll();

            if(usageReport.SenderUsageReports.Count > 0) {
                Assert.AreEqual(usageReport.SenderUsageReports.Count + 1, rows.Count);
            }

			// Assert correct download of delegation report
            DelegationReport delegationReportForAccountWithoutDate = example.sdkDelegationReportForAccountWithoutDate;
            Assert.GreaterOrEqual(delegationReportForAccountWithoutDate.DelegationEvents.Count, 0, "Number of DelegationEventReports should be greater than 0.");

            Assert.IsNotNull(example.csvDelegationReportForAccountWithoutDate, "Delegation report in csv cannot be null.");
            Assert.IsNotEmpty(example.csvDelegationReportForAccountWithoutDate, "Delegation report in csv cannot be empty.");

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvDelegationReportForAccountWithoutDate))));
            rows = reader.readAll();

            if(delegationReportForAccountWithoutDate.DelegationEvents.Count > 0) {
                rows = GetRowsBySender(rows, example.senderUID);
                Assert.AreEqual(delegationReportForAccountWithoutDate.DelegationEvents[example.senderUID].Count, rows.Count);
            }

            DelegationReport delegationReportForAccount = example.sdkDelegationReportForAccount;
            Assert.GreaterOrEqual(delegationReportForAccount.DelegationEvents.Count, 0, "Number of DelegationEventReports should be greater than 0.");

            Assert.IsNotNull(example.csvDelegationReportForAccount, "Delegation report in csv cannot be null.");
            Assert.IsNotEmpty(example.csvDelegationReportForAccount, "Delegation report in csv cannot be empty.");

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvDelegationReportForAccount))));
            rows = reader.readAll();

            if(delegationReportForAccount.DelegationEvents.Count > 0) {
                rows = GetRowsBySender(rows, example.senderUID);
                Assert.AreEqual(delegationReportForAccount.DelegationEvents[example.senderUID].Count, rows.Count);
            }

            DelegationReport delegationReportForSender = example.sdkDelegationReportForSender;
            Assert.GreaterOrEqual(delegationReportForSender.DelegationEvents.Count, 0, "Number of DelegationEventReports should be greater than 0.");

            Assert.IsNotNull(example.csvDelegationReportForSender, "Delegation report in csv cannot be null.");
            Assert.IsNotEmpty(example.csvDelegationReportForSender, "Delegation report in csv cannot be empty.");

            reader = new CSVReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(example.csvDelegationReportForSender))));
            rows = reader.readAll();

            if(delegationReportForSender.DelegationEvents.Count > 0) {
                rows = GetRowsBySender(rows, example.senderUID);
                Assert.AreEqual(delegationReportForSender.DelegationEvents[example.senderUID].Count, rows.Count);
            }
		}

        private int GetCompletionReportCount(CompletionReport completionReport) {
            int count = 0;
            foreach(SenderCompletionReport senderCompletionReport in completionReport.Senders) {
                count += senderCompletionReport.Packages.Count;
            }
            return count;
        }

        private void AssertCreatedPackageIncludedInCompletionReport(CompletionReport completionReport, string sender, PackageId packageId, string packageStatus) {
            PackageCompletionReport createdPackageCompletionReport = GetCreatedPackageCompletionReport(completionReport, sender, packageId);

            Assert.IsNotNull(createdPackageCompletionReport);
            Assert.IsNotNull(createdPackageCompletionReport.DocumentPackageStatus);
            Assert.AreEqual(packageStatus, createdPackageCompletionReport.DocumentPackageStatus.GetName());
        }

        private void AssertCreatedPackageIncludedInCSV(IList<string[]> rows, PackageId packageId, string packageStatus) {
            string[] createdPackageRow = GetCreatedPackageCSVRow(rows, packageId);
            Assert.IsNotNull(createdPackageRow);
            Assert.IsTrue(HasItems(createdPackageRow, packageId.Id, packageStatus));
        }

        private bool HasItems(string[] row, string packageId, string packageStatus) {
            bool hasPackageId = false;
            bool hasPackageStatus = false;

            foreach(string data in row) {
                if(data.Equals(packageId)) {
                    hasPackageId = true;
                }
                if(data.Equals(packageStatus)) {
                    hasPackageStatus = true;
                }
            }
            return (hasPackageId && hasPackageStatus);
        }

        private PackageCompletionReport GetCreatedPackageCompletionReport(CompletionReport completionReport, string sender, PackageId packageId) {
            SenderCompletionReport senderCompletionReport = GetSenderCompletionReport(completionReport, sender);

            IList<PackageCompletionReport> packageCompletionReports = senderCompletionReport.Packages;
            foreach(PackageCompletionReport packageCompletionReport in packageCompletionReports) {
                if(packageCompletionReport.Id.Equals(packageId.Id)) {
                    return packageCompletionReport;
                }
            }
            return null;
        }

        private SenderCompletionReport GetSenderCompletionReport(CompletionReport completionReport, string sender) {
            foreach(SenderCompletionReport senderCompletionReport in completionReport.Senders) {
                if(senderCompletionReport.Sender.Id.Equals(sender)) {
                    return senderCompletionReport;
                }
            }
            return null;
        }

        private string[] GetCreatedPackageCSVRow(IList<string[]> rows, PackageId packageId) {
            foreach(string[] row in rows) {
                foreach(string word in row) {
                    if(word.Contains(packageId.Id)) {
                        return row;
                    }
                }
            }
            return null;
        }

        private IList<string[]> GetRowsBySender(IList<string[]> rows, string sender) {
            IList<string[]> result = new List<string[]>();
            foreach(string[] row in rows) {
                foreach(string word in row) {
                    if(word.Contains(sender)) {
                        result.Add(row);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
