//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Saraf365.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class CartTransferHistory
    {
        public long xID { get; set; }
        public Nullable<long> xTransactionID { get; set; }
        public long xBankCardID { get; set; }
        public System.DateTime xDate { get; set; }
        public long xAmountIn { get; set; }
        public long xAmountOut { get; set; }
        public string xCodePeigiri { get; set; }
        public string xCodeErja { get; set; }
        public string xDescription { get; set; }
        public string xDocumentNumber { get; set; }
        public System.DateTime xDocumentDate { get; set; }
        public string xCardNumber { get; set; }
    
        public virtual BankCard BankCard { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
