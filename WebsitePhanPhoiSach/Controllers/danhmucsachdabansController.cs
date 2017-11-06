using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsitePhanPhoiSach.Models;

namespace WebsitePhanPhoiSach.Controllers
{
    public class danhmucsachdabansController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        
        // GET: danhmucsachdabans
        public ActionResult Index()
        {
            var danhmucsachdaban = db.danhmucsachdabans.Include(d => d.daily);
            return View(danhmucsachdaban.ToList());
        }
        [HttpPost]
        public ActionResult Index(string iddmsdb)
        {
            int id;
            if (int.TryParse(iddmsdb, out id))
            {
                edittratien(id);
            }
            var danhmucsachdaban = db.danhmucsachdabans.Include(d => d.daily);
            return View(danhmucsachdaban.ToList());
        }

        // GET: danhmucsachdabans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdabans.Find(id);
            if (danhmucsachdaban == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsachdaban);
        }

        // GET: danhmucsachdabans/Create
        public ActionResult Create()
        {
            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl");
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            return View();
        }

        // POST: danhmucsachdabans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "danhmucsachdaban")] danhmucsachdaban danhmucsachdaban,
                                    [Bind(Prefix = "ct")] ctdmsdb[] ctdmsdb)
        {
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();
            if (ModelState.IsValid)
            {
                danhmucsachdaban.tinhtrang = "Waiting";
                danhmucsachdaban.sotiendathanhtoan = 0;
                decimal tongtien = 0;
                decimal tongtienbanduoc = 0;
                int iddm = 1;
                if(db.danhmucsachdabans.Any())
                iddm = db.danhmucsachdabans.Max(a => a.iddmsdb) + 1;
                int idct = 1;
                foreach(ctdmsdb ct in ctdmsdb)
                {
                    ct.iddmsdb = iddm;
                    ct.idctdmsdb = idct;
                    idct++;
                    hangtoncuadaily htdl = db.hangtoncuadailies
                    .FirstOrDefault(b => b.iddl == danhmucsachdaban.iddl && b.idsach == ct.idsach);
                        if(htdl != null && htdl.soluongchuaban >= ct.soluong)
                    {
                        htdl.soluongchuaban = htdl.soluongchuaban - ct.soluong;
                        db.Entry(htdl).State = EntityState.Modified;
                    }
                        else
                    {
                        ModelState.AddModelError("", "Số lượng sách đã bán lớn hơn số sách xuất cho đại lý");
                        ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
                        ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    tongtien += (decimal)(ct.soluong * db.saches.Find(ct.idsach).giaxuat);
                    //cập nhật số tiền phải tra cho nhà xuất bản 
                    sach s = db.saches.Find(ct.idsach);
                    System.Diagnostics.Debug.WriteLine("Id sách " + ct.idsach);
                    nxb n = db.nxbs.Find(s.idnxb);
                    System.Diagnostics.Debug.WriteLine("Id nhà xuất bản : " + n.idnxb);
                    System.Diagnostics.Debug.WriteLine("Nhà xuất bản : "+n.tennxb);
                    sotienphaitrachonxb st = new sotienphaitrachonxb();
                    st.thoidiem = (DateTime)danhmucsachdaban.thoigian;
                    sotienphaitrachonxb stht = db.sotienphaitrachonxbs.OrderByDescending(o => o.id).FirstOrDefault(o => o.idnxb == n.idnxb);
                    System.Diagnostics.Debug.WriteLine("Id nhà xuất bản sau khi lấy : " + stht.idnxb);
                    System.Diagnostics.Debug.WriteLine("Số tiền phải trả cho nhà xuất bản : " + stht.sotienphaitra);

                    tongtienbanduoc = (decimal)(ct.soluong * s.gianhap);
                    System.Diagnostics.Debug.WriteLine(tongtienbanduoc);
                    if (stht != null)
                        {
                        st.idnxb = n.idnxb;
                        st.sotienphaitra = (ct.soluong * s.gianhap);
                        System.Diagnostics.Debug.WriteLine("Đây là số tiền phải trả : " +
                            st.sotienphaitra);
                        db.sotienphaitrachonxbs.Add(st);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không lấy được số tiền phải trả cho nhà xuất bản");
                        ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
                        ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                }
                /*congnotheothoigian cn = new congnotheothoigian();
                cn.thoidiem = (DateTime)danhmucsachdaban.thoigian;
                congnotheothoigian cnht = db.congnotheothoigians.OrderByDescending(o => o.id).FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl);
                if(cnht.congno!=null)
                {
                    cn.iddl = danhmucsachdaban.iddl;
                    cn.congno = cnht.congno - tongtien;
                    db.congnotheothoigians.Add(cn);
                }
                else
                {
                    cn.iddl = danhmucsachdaban.iddl;
                    cn.congno = tongtien;
                    db.congnotheothoigians.Add(cn);
                }
               */
                danhmucsachdaban.sotienconno = tongtien;
                danhmucsachdaban.ctdmsdbs = ctdmsdb;
                db.danhmucsachdabans.Add(danhmucsachdaban);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }

        // GET: danhmucsachdabans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdabans.Find(id);
            if (danhmucsachdaban == null)
            {
                return HttpNotFound();
            }
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();

            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }

        // POST: danhmucsachdabans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "danhmucsachdaban")] danhmucsachdaban danhmucsachdaban,
                                 [Bind(Prefix = "ct")] ctdmsdb[] ctdmsdb)
        {
            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();

            if (ModelState.IsValid)
            {
                decimal tongtien = 0;
                decimal tongtienmoi = 0;
                int iddmsdb = danhmucsachdaban.iddmsdb;
                int idct = 1;
                //xoa chi tiet cu trong database table hangtoncuadaily
                var ctcudmsdb = db.ctdmsdbs.Where(o => o.iddmsdb == danhmucsachdaban.iddmsdb);
                foreach (ctdmsdb ct in ctcudmsdb)
                {
                    hangtoncuadaily ht = db.hangtoncuadailies.FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl && o.idsach == ct.idsach);
                    int hangtondaily = (int)(ht.soluongchuaban + ct.soluong);
                }
                //thêm chi tiết sửa vào database table hangtoncuadaily
                foreach (ctdmsdb ct in ctdmsdb)
                {
                    ct.iddmsdb = iddmsdb;
                    ct.idctdmsdb = idct;
                    idct++;
                    hangtoncuadaily ht = db.hangtoncuadailies.FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl && o.idsach == ct.idsach);
                    ht.soluongchuaban = (int)(ht.soluongchuaban - ct.soluong);
                    if (ht.soluongchuaban < 0)
                    {
                        danhmucsachdaban.ctdmsdbs = ctdmsdb;
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    sach s = db.saches.Find(ct.idsach);
                    nxb n = db.nxbs.Find(s.idnxb);
                    sotienphaitrachonxb st = new sotienphaitrachonxb();
                    st.thoidiem = (DateTime)danhmucsachdaban.thoigian;
                    sotienphaitrachonxb stht = db.sotienphaitrachonxbs.OrderByDescending(o => o.id).FirstOrDefault(o => o.idnxb == n.idnxb);
                    if (stht != null)
                    {
                        st.idnxb = n.idnxb;
                        st.sotienphaitra = (ct.soluong * s.gianhap);
                        db.sotienphaitrachonxbs.Add(st);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không lấy được số tiền phải trả cho nhà xuất bản");
                        ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", danhmucsachdaban.iddl);
                        ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    tongtien += (decimal)(ct.soluong * s.giaxuat);
                }
                foreach (ctdmsdb ct in ctcudmsdb)
                {
                    nxb n = db.nxbs.Find(ct.sach.idnxb);
                   
                    sotienphaitrachonxb stht = db.sotienphaitrachonxbs.OrderByDescending(o => o.id).FirstOrDefault(o => o.idnxb == n.idnxb);
                        
                        tongtienmoi = (decimal)(stht.sotienphaitra - (ct.soluong * ct.sach.gianhap));
                        System.Diagnostics.Debug.WriteLine("Số tiền phải trả cũ là : " + stht.sotienphaitra);
                        System.Diagnostics.Debug.WriteLine("số tiền phải trả mới là : "+tongtienmoi);
                        
                    
                   if (tongtienmoi < 0)
                    {
                        danhmucsachdaban.ctdmsdbs = ctdmsdb;
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    db.ctdmsdbs.Remove(ct);
                }
                foreach (ctdmsdb ct in ctdmsdb)
                {
                    db.ctdmsdbs.Add(ct);
                }
                danhmucsachdaban.sotienconno = tongtien;
                danhmucsachdaban.sotiendathanhtoan = 0;
                danhmucsachdaban.tinhtrang = "Waiting";
                db.Entry(danhmucsachdaban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            danhmucsachdaban.ctdmsdbs = ctdmsdb;
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }
        private void edittratien(int iddmsdb)
        {
            decimal tongtien = 0;
            danhmucsachdaban dm = db.danhmucsachdabans.Find(iddmsdb);
            var ctdmsdb = dm.ctdmsdbs.Where(o=>o.iddmsdb == dm.iddmsdb);
            dm.sotiendathanhtoan = db.ctdmsdbs.Where(ct => ct.iddmsdb == iddmsdb).Select(ct => ct.soluong * ct.sach.giaxuat).DefaultIfEmpty(0).Sum();
            dm.sotienconno = 0;
            dm.tinhtrang = "Completed";
            foreach(ctdmsdb ct in ctdmsdb)
            {
                tongtien += (decimal)(ct.soluong * ct.sach.giaxuat);
            }
            congnotheothoigian cn = new congnotheothoigian();
            cn.thoidiem = (DateTime)dm.thoigian;
            congnotheothoigian cnht = db.congnotheothoigians.OrderByDescending(o => o.id).FirstOrDefault(o => o.iddl == dm.iddl);
            if (cnht.congno != null)
            {
                cn.iddl = dm.iddl;
                cn.congno = cnht.congno - tongtien;
                db.congnotheothoigians.Add(cn);
            }
            else
            {
                cn.iddl = dm.iddl;
                cn.congno = tongtien;
                db.congnotheothoigians.Add(cn);
            }
            db.SaveChanges();
        }
        // GET: danhmucsachdabans/Delete/5
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdabans.Find(id);
            if (danhmucsachdaban == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsachdaban);
        }

        // POST: danhmucsachdabans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdabans.Find(id);
            db.danhmucsachdabans.Remove(danhmucsachdaban);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
