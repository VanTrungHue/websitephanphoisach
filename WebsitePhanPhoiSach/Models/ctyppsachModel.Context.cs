﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ctyppsachEntities : DbContext
    {
        public ctyppsachEntities()
            : base("name=ctyppsachEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<congnotheothoigian> congnotheothoigians { get; set; }
        public virtual DbSet<ctdmsdb> ctdmsdbs { get; set; }
        public virtual DbSet<ctpn> ctpns { get; set; }
        public virtual DbSet<ctpx> ctpxes { get; set; }
        public virtual DbSet<daily> dailies { get; set; }
        public virtual DbSet<danhmucsachdaban> danhmucsachdabans { get; set; }
        public virtual DbSet<hangtoncuadaily> hangtoncuadailies { get; set; }
        public virtual DbSet<linhvuc> linhvucs { get; set; }
        public virtual DbSet<nxb> nxbs { get; set; }
        public virtual DbSet<phieunhap> phieunhaps { get; set; }
        public virtual DbSet<phieutratien> phieutratiens { get; set; }
        public virtual DbSet<phieuxuat> phieuxuats { get; set; }
        public virtual DbSet<sach> saches { get; set; }
        public virtual DbSet<sotienphaitrachonxb> sotienphaitrachonxbs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tonkho> tonkhoes { get; set; }
    }
}
