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
    
    public partial class congnotheothoigian
    {
        public int id { get; set; }
        public Nullable<int> iddl { get; set; }
        public Nullable<decimal> congno { get; set; }
        public System.DateTime thoidiem { get; set; }
    
        public virtual daily daily { get; set; }
    }
}
