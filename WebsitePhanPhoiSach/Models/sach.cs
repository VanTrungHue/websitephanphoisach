//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsitePhanPhoiSach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sach()
        {
            this.ctdmsdbs = new HashSet<ctdmsdb>();
            this.ctpns = new HashSet<ctpn>();
            this.ctpxes = new HashSet<ctpx>();
            this.hangtoncuadailies = new HashSet<hangtoncuadaily>();
            this.tonkhoes = new HashSet<tonkho>();
        }
    
        public int idsach { get; set; }
        public string tensach { get; set; }
        public string tacgia { get; set; }
        public Nullable<int> idnxb { get; set; }
        public Nullable<int> idlv { get; set; }
        public Nullable<decimal> giaxuat { get; set; }
        public Nullable<decimal> gianhap { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctdmsdb> ctdmsdbs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctpn> ctpns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctpx> ctpxes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hangtoncuadaily> hangtoncuadailies { get; set; }
        public virtual linhvuc linhvuc { get; set; }
        public virtual nxb nxb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tonkho> tonkhoes { get; set; }
    }
}
