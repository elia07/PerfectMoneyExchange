using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.InternetBankModules
{
    public class CardTransferHistoryModel
    {
        public DateTime Date { set; get; }
        public string Description { set; get; }
        public long AmountIn { set; get; }
        public long AmountOut { set; get; }
        public string DocumentNumber { set; get; }
        public string CodePeygiri { set; get; }
        public string CodeErja { set; get; }
        public string CardNumber { set; get; }
    }
}
