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
    
    public partial class TelegramSupport
    {
        public long xID { get; set; }
        public long xChatID { get; set; }
        public string xMessage { get; set; }
        public long xMessageID { get; set; }
        public string xUsername { get; set; }
        public System.DateTime xDate { get; set; }
        public string xResponse { get; set; }
        public Nullable<long> xResponseAdmin { get; set; }
        public Nullable<long> xSystemFileID { get; set; }
        public Nullable<System.DateTime> xResponseDate { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual SystemFile SystemFile { get; set; }
    }
}
