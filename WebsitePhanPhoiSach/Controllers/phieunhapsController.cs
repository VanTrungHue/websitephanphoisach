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
    public class phieunhapsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: phieunhaps
        public ActionResult Index()
        {
            var phieunhap = db.phieunhaps.Include(p => p.nxb);
            return View(phieunhap.ToList());
        }

        // GET: phieunhaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhaps.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            return View(phieunhap);
        }

        // GET: phieunhaps/Create
        public ActionResult Create()
        {
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb");
            ViewBag.idsach = new SelectList(db.saches,"idsach","tensach");
            return View();
        }

        // POST: phieunhaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "phieunhap")] phieunhap phieunhap,
                                  [Bind(Prefix = "ct")] ctpn[] ctpn)
        {
            if (ModelState.IsValid)
            {
                int idpn = 1;
                if (db.phieunhaps.Any())
                    idpn = db.phieunhaps.Max(o => o.idpn) + 1;
                int idct = 1;
                foreach (ctpn ct in ctpn)
                {
                    ct.idpn = idpn;
                    ct.idctpn = idct;
                    idct++;
                    tonkho tk = new tonkho();
                    tk.thoidiem = DateTime.Now;

                    //lấy tồn kho mới nhất của sách này
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);

                    if (tkht != null)
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = tkht.soluongton + ct.soluong;
                    }
                    else
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = ct.soluong;
                    }
                    db.tonkhoes.Add(tk);
                }
                phieunhap.ctpns = ctpn;
                db.phieunhaps.Add(phieunhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", phieunhap.idnxb);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            return View();
        }

        // GET: phieunhaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhaps.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", phieunhap.idnxb);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            phieunhapviewmodel pnvm = new phieunhapviewmodel();
            pnvm.phieunhap = phieunhap;
            return View(pnvm);
        }

        // POST: phieunhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix ="phieunhap")] phieunhap phieunhap,
                                 [Bind(Prefix ="ct")] ctpn[] ctpn)
        {
            if (ModelState.IsValid)
            {
                int idpn = phieunhap.idpn;
                int idct = 1;

                //thêm chi tiết sửa vào database
                foreach (ctpn ct in ctpn)
                {
                    ct.idpn = idpn;
                    ct.idctpn = idct;
                    idct++;
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    if (tkht.soluongton != null) tkht.soluongton = tkht.soluongton + ct.soluong;
                    else tkht.soluongton = ct.soluong;
                }

                //xóa chi tiết cũ trong database
                var ctpncu = db.ctpns.Where(ct => ct.idpn == phieunhap.idpn).ToList();
                foreach (var ct in ctpncu)
                {
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    int soluonghientai = (int)(tkht.soluongton - ct.soluong);
                    if (soluonghientai < 0)
                    {
                        ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                        ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", phieunhap.idnxb);
                        phieunhapviewmodel pnvm = new phieunhapviewmodel();
                        phieunhap.ctpns = ctpn;
                        pnvm.phieunhap = phieunhap;
                        return View(pnvm);
                    }
                    tkht.soluongton = soluonghientai;
                    db.ctpns.Remove(ct);
                }
                foreach (ctpn ct in ctpn)
                {
                    db.ctpns.Add(ct);
                }
                db.Entry(phieunhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", phieunhap.idnxb);
            phieunhapviewmodel pnvm1 = new phieunhapviewmodel();
            phieunhap.ctpns = ctpn;
            pnvm1.phieunhap = phieunhap;
            return View(pnvm1);
        }

        // GET: phieunhaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhaps.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            return View(phieunhap);
        }

        // POST: phieunhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phieunhap phieunhap = db.phieunhaps.Find(id);
            db.phieunhaps.Remove(phieunhap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
