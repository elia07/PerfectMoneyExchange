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
    
    public partial class SystemLog
    {
        public long xID { get; set; }
        public byte xType { get; set; }
        public string xDetail { get; set; }
        public System.DateTime xDate { get; set; }
        public string xShortDetail { get; set; }
        public Nullable<long> xAdminID { get; set; }
    
        public virtual Admin Admin { get; set; }
    }
}