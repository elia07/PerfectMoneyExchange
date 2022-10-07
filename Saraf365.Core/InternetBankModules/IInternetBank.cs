
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public interface IInternetBank
    {
        string GetInfo();

        bool Login();
        Tuple<bool, string> CardTransfer(string toCardNumber, long amount);

        Tuple<bool, string> NormalTransfer(string toDepositNumber, long amount, string sourceComment = "", string destinationComment = "");
        List<CardTransferHistoryModel> GetCartTransferHistory(DateTime fromDate, DateTime toDate);
        Tuple<long, long, string> GetBalance();

        string GetCardHolderName(string cardnumber);

        bool Refresh();

        void Exit();

    }
}
