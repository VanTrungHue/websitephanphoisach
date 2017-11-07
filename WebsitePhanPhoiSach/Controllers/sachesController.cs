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
    public class sachesController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: saches
        public ActionResult Index()
        {
            var sach = db.saches.Include(s => s.linhvuc).Include(s => s.nxb);
            return View(sach.ToList());
        }

        // GET: saches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: saches/Create
        public ActionResult Create()
        {
            ViewBag.idlv = new SelectList(db.linhvucs, "idlv", "tenlv");
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb");
            return View();
        }

        // POST: saches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idsach,tensach,tacgia,idnxb,idlv,giaxuat,gianhap")] sach sach)
        {
            if (ModelState.IsValid)
            {
                foreach (sach s in db.saches)
                {
                    if(sach.tensach == s.tensach)
                    {
                        ModelState.AddModelError("", "Trùng tên sách hoặc sách đã tồn tại");
                        ViewBag.idlv = new SelectList(db.linhvucs, "idlv", "tenlv", sach.idlv);
                        ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", sach.idnxb);
                        return View();
                    }

                }
                tonkho tk = new tonkho();
                tk.idsach = sach.idsach;
                tk.thoidiem = DateTime.Now;
                tk.soluongton = 0;
                db.tonkhoes.Add(tk);
                db.saches.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idlv = new SelectList(db.linhvucs, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // GET: saches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.idlv = new SelectList(db.linhvucs, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // POST: saches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idsach,tensach,tacgia,idnxb,idlv,giaxuat,gianhap")] sach sach)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idlv = new SelectList(db.linhvucs, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxbs, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // GET: saches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: saches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sach sach = db.saches.Find(id);
            foreach (tonkho tk in db.tonkhoes)
            {
                if (tk.idsach == sach.idsach)
                {
                    db.tonkhoes.Remove(tk);
                }
            }
            foreach(ctpn ct in db.ctpns)
            {
                if(ct.idsach == sach.idsach)
                {
                    db.ctpns.Remove(ct);
                    phieunhap pn = db.phieunhaps.Find(ct.idpn);
                    db.phieunhaps.Remove(pn);
                }
             }
            db.saches.Remove(sach);
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
