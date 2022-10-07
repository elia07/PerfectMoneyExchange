using Newtonsoft.Json;
using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Provision
{
    public class CardToCardManager : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            using (BankCardRepository bcr = new BankCardRepository(null,true))
            {
                using (CartTransferHistoryRepository cthr = new CartTransferHistoryRepository())
                {
                    foreach (var item in SectionInfo.InputCardsInternetBanks)
                    {
                        BankCard bcInstance = bcr.GetByID(item.Key);


                        /* DateTime fromDate = cthr.GetLastFetchDateByBankCardID(bcInstance.xID);
                         if(fromDate==null || fromDate==new DateTime())
                         {
                             fromDate = DateTime.Now.AddMinutes(-30);
                         }
                         else
                         {
                             fromDate = fromDate.AddMinutes(-30);
                         }*/
                        var fromDate = DateTime.Now.AddMinutes(-180);
                        var records = item.Value.GetCartTransferHistory(fromDate, DateTime.Now.Date.AddDays(1).AddMinutes(-1));
                        foreach (var cItem in records)
                        {
                            if (!cthr.IsDocumentNumberExist(cItem.DocumentNumber))
                            {
                                CartTransferHistory cthInsatnce = new CartTransferHistory();
                                cthInsatnce.xAmountIn = cItem.AmountIn;
                                cthInsatnce.xAmountOut = cItem.AmountOut;
                                cthInsatnce.xBankCardID = bcInstance.xID;
                                cthInsatnce.xCardNumber = cItem.CardNumber;
                                cthInsatnce.xCodeErja = cItem.CodeErja;
                                cthInsatnce.xCodePeigiri = cItem.CodePeygiri;
                                cthInsatnce.xDate = DateTime.Now;
                                cthInsatnce.xDescription = cItem.Description;
                                cthInsatnce.xDocumentDate = cItem.Date;
                                cthInsatnce.xDocumentNumber = cItem.DocumentNumber;

                                cthr.Insert(cthInsatnce);
                            }
                        }



                        var currentBalance = item.Value.GetBalance();
                        if (currentBalance.Item1 >= bcInstance.xMinToTransfer)
                        {
                            long transferAmount = bcInstance.xBalance - Convert.ToInt64(bcInstance.xMinDeposit);
                            var repoCard = bcr.GetAppropriteRepoCard();
                            if(repoCard!=null)
                            {
                                var res=item.Value.NormalTransfer(repoCard.xAccountNumber, transferAmount, "Transfer to Repo", "Transfer to Repo");
                                new SystemLogRepository().Log(SystemLogType.InternetBank, "Transfer to Repo", JsonConvert.SerializeObject(res));
                                repoCard.xLastUsageDate = DateTime.Now;
                                bcr.Update(repoCard);

                            }

                            



                        }


                        var balance = currentBalance;
                        bcInstance.xBalance = balance.Item1;
                        bcInstance.xBannedAmount = balance.Item2;
                        bcInstance.xDepositStatus = balance.Item3;
                        bcr.Update(bcInstance);

                        
                        if (bcInstance.xBankName == "تجارت")
                        {
                            item.Value.Exit();

                        }


                    }
                }
            }
                
        }
       

        void IJob.Execute(IJobExecutionContext context)
        {
            Worker++;
            if (Worker > 1)
            {
                Worker--;
                return;
            }
            try
            {
                Manage();
            }
            catch { }
            Worker--;
        }
    }
}