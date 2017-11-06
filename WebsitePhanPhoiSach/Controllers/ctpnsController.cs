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
    public class ctpnsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: ctpns
        public ActionResult Index()
        {
            var ctpn = db.ctpns.Include(c => c.phieunhap).Include(c => c.sach);
            return View(ctpn.ToList());
        }

        // GET: ctpns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctpn ctpn = db.ctpns.Find(id);
            if (ctpn == null)
            {
                return HttpNotFound();
            }
            return View(ctpn);
        }

        // GET: ctpns/Create
        public ActionResult Create()
        {
            ViewBag.idpn = new SelectList(db.phieunhaps, "idpn", "nguoigiaosach");
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            return View();
        }

        // POST: ctpns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idpn,idctpn,idsach,soluong")] ctpn ctpn)
        {
            if (ModelState.IsValid)
            {
                db.ctpns.Add(ctpn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idpn = new SelectList(db.phieunhaps, "idpn", "nguoigiaosach", ctpn.idpn);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach", ctpn.idsach);
            return View(ctpn);
        }

        // GET: ctpns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctpn ctpn = db.ctpns.Find(id);
            if (ctpn == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpn = new SelectList(db.phieunhaps, "idpn", "nguoigiaosach", ctpn.idpn);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach", ctpn.idsach);
            return View(ctpn);
        }

        // POST: ctpns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idpn,idctpn,idsach,soluong")] ctpn ctpn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctpn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idpn = new SelectList(db.phieunhaps, "idpn", "nguoigiaosach", ctpn.idpn);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach", ctpn.idsach);
            return View(ctpn);
        }

        // GET: ctpns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctpn ctpn = db.ctpns.Find(id);
            if (ctpn == null)
            {
                return HttpNotFound();
            }
            return View(ctpn);
        }

        // POST: ctpns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ctpn ctpn = db.ctpns.Find(id);
            db.ctpns.Remove(ctpn);
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
