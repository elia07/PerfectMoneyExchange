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
    
    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            this.AdminNotification = new HashSet<AdminNotification>();
            this.AdminRole = new HashSet<AdminRole>();
            this.SystemLog = new HashSet<SystemLog>();
            this.AdminNotification1 = new HashSet<AdminNotification>();
            this.TelegramSupport = new HashSet<TelegramSupport>();
        }
    
        public long xID { get; set; }
        public string xEmail { get; set; }
        public string xPassword { get; set; }
        public string xName { get; set; }
        public byte xType { get; set; }
        public bool xIsActive { get; set; }
        public string xSalt { get; set; }
        public Nullable<long> xTelegramID { get; set; }
        public string xTelegramUsername { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminNotification> AdminNotification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminRole> AdminRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemLog> SystemLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminNotification> AdminNotification1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TelegramSupport> TelegramSupport { get; set; }
    }
}
