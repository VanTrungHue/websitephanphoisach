﻿using System;
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
    public class nxbsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: nxbs
        public ActionResult Index()
        {
            return View(db.nxbs.ToList());
        }

        // GET: nxbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nxb nxb = db.nxbs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // GET: nxbs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: nxbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idnxb,tennxb,diachi,sodt,sotk")] nxb nxb)
        {
            
           
            if (ModelState.IsValid)
            {
                foreach (nxb n in db.nxbs)
                {
                    if (n.tennxb == nxb.tennxb)
                    {
                        ModelState.AddModelError("", "Nhà xuất bản đã tồn tại");
                        
                        return View();
                    }

                }
                
                sotienphaitrachonxb a = new sotienphaitrachonxb();
                a.idnxb = nxb.idnxb;
                a.sotienphaitra = 0;
                a.thoidiem = DateTime.Now;
                db.sotienphaitrachonxbs.Add(a);
                db.nxbs.Add(nxb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nxb);
        }

        // GET: nxbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nxb nxb = db.nxbs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // POST: nxbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idnxb,tennxb,diachi,sodt,sotk")] nxb nxb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nxb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

        // GET: nxbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nxb nxb = db.nxbs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // POST: nxbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            nxb nxb = db.nxbs.Find(id);
            foreach (sotienphaitrachonxb st in db.sotienphaitrachonxbs)
            {
                if (st.idnxb == nxb.idnxb)
                {
                    db.sotienphaitrachonxbs.Remove(st);
                }
            }
            db.nxbs.Remove(nxb);
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
