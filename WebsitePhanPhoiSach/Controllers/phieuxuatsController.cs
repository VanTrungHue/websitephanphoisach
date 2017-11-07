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
    public class phieuxuatsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();
        
        // GET: phieuxuats
        public ActionResult Index()
        {
            
            var phieuxuat = db.phieuxuats.Include(p => p.daily);
            return View(phieuxuat.ToList());
        }

        // GET: phieuxuats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuats.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuxuat);
        }

        // GET: phieuxuats/Create
        public ActionResult Create()
        {
            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl");
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            return View();
        }

        // POST: phieuxuats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "phieuxuat")] phieuxuat phieuxuat,
                                   [Bind(Prefix = "ct")] ctpx[] ctpx)
        {
            if (ModelState.IsValid)
            {
                decimal tongtien = 0;
                int idpx = 1;
                if (db.phieuxuats.Any())
                    idpx = db.phieuxuats.Max(o => o.idpx) + 1;
                int idct = 1;
                foreach (ctpx ct in ctpx)
                {
                    ct.idpx = idpx;
                    ct.idctpx = idct;
                    idct++;
                    
                    //cap nhat ton kho hien tai
                    tonkho tk = new tonkho();
                    tk.thoidiem = DateTime.Now;
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    //kiem tra xem cuon sach du so luong de xuat ko
                    if (tkht != null && tkht.soluongton > ct.soluong)
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = tkht.soluongton - ct.soluong;
                        db.tonkhoes.Add(tk);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không đủ số lượng hoặc chưa nhập sách về");
                        ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl");
                        ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                        return View();
                    }

                    //cap nhat so sach da gui cho dai ly
                    hangtoncuadaily htdl = db.hangtoncuadailies.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    if (htdl != null)
                    {
                        htdl.soluongchuaban = htdl.soluongchuaban + ct.soluong;
                        db.Entry(htdl).State = EntityState.Modified;
                    }
                    else
                    {
                        htdl = new hangtoncuadaily();
                        htdl.iddl = phieuxuat.iddl;
                        htdl.idsach = ct.idsach;
                        htdl.soluongchuaban = ct.soluong;
                        db.hangtoncuadailies.Add(htdl);
                    }
                    // tạo mới công nợ
                    tongtien += (decimal)(ct.soluong * db.saches.Find(ct.idsach).giaxuat);
                }
                congnotheothoigian cn = new congnotheothoigian();
                cn.thoidiem = (DateTime)phieuxuat.ngayxuat;
                congnotheothoigian cnht = db.congnotheothoigians.OrderByDescending(o => o.id).FirstOrDefault(o => o.iddl == phieuxuat.iddl);
                if (cnht.congno>0 && tongtien > cnht.congno)
                {
                    ModelState.AddModelError("", "Tổng tiền lớn hơn công nợ");
                    ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl");
                    ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
                    return View();
                }
                if (cnht.congno != null)
                {
                    cn.iddl = phieuxuat.iddl;
                    System.Diagnostics.Debug.WriteLine("Tổng tiền là : " +tongtien);
                    cn.congno = cnht.congno + tongtien;
                    System.Diagnostics.Debug.WriteLine("Công nợ là : " + cn.congno);
                    db.congnotheothoigians.Add(cn);
                }
                else
                {
                    cn.iddl = phieuxuat.iddl;
                    cn.congno = tongtien;
                    db.congnotheothoigians.Add(cn);
                }
                phieuxuat.ctpxes = ctpx;
                db.phieuxuats.Add(phieuxuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl");
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            return View();
        }

        // GET: phieuxuats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuats.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
           
            return re(phieuxuat);
        }

        // POST: phieuxuats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "phieuxuat")] phieuxuat phieuxuat,
                                 [Bind(Prefix = "ct")] ctpx[] ctpx)
        {
            if (ModelState.IsValid)
            {
                int idpx = phieuxuat.idpx;
                int idct = 1;

                //xóa chi tiết cũ trong database table sach
                //tinh tong tien cu
                int tongtiencu = 0;
                var ctpxcu = db.ctpxes.Where(ct => ct.idpx == phieuxuat.idpx).ToList();
                foreach (var ct in ctpxcu)
                {
                    sach s = db.saches.Find(ct.idsach);
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    int soluonghientai = (int)(tkht.soluongton + ct.soluong);
                    tkht.soluongton = soluonghientai;
                    tongtiencu += (int)(ct.soluong * s.giaxuat);
                }
                //thêm chi tiết sửa vào database table hangtoncuadaily
                foreach (ctpx ct in ctpx)
                {
                    ct.idpx = idpx;
                    ct.idctpx = idct;
                    idct++;
                    hangtoncuadaily ht = db.hangtoncuadailies.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    ht.soluongchuaban = (int)(ht.soluongchuaban + ct.soluong);
                }
                //xoa chi tiet cu trong database table hangtoncuadaily
                foreach (ctpx ct in ctpxcu)
                {
                    hangtoncuadaily ht = db.hangtoncuadailies.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    int hangtondaily = (int)(ht.soluongchuaban - ct.soluong);
                    if (hangtondaily < 0)
                    {
                        ModelState.AddModelError("", "so sach nay da duoc ban" + hangtondaily);
                        phieuxuat.ctpxes = ctpx;
                        return re(phieuxuat);
                    }
                    db.ctpxes.Remove(ct);
                }
                //tinh tong tien moi
                int tongtien = 0;
                //thêm chi tiết sửa vào database table sach
                foreach (ctpx ct in ctpx)
                {
                    sach s = db.saches.Find(ct.idsach);
                    tonkho tkht = db.tonkhoes.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    if (tkht.soluongton != null) tkht.soluongton = tkht.soluongton - ct.soluong;
                    if (tkht.soluongton < 0)
                    {
                        ModelState.AddModelError("", "so luong hien tai khong du de xuat " + tkht.soluongton + " vui long kiem tra lai");
                        phieuxuat.ctpxes = ctpx;
                        return re(phieuxuat);
                    }
                    tongtien += (int)(ct.soluong * s.giaxuat);
                }

                //cap nhat cong no
                daily dl = db.dailies.Find(phieuxuat.iddl);
                congnotheothoigian cnht = db.congnotheothoigians.OrderByDescending(o => o.id).FirstOrDefault(o => o.iddl == phieuxuat.iddl);
                if (cnht.congno > 0 && tongtien > cnht.congno)
                {
                    ModelState.AddModelError("", "tong tien phieu xuat lon hon cong no hien tai cua dai ly");
                    phieuxuat.ctpxes = ctpx;
                    return re(phieuxuat);
                }

                if (cnht.congno != null) cnht.congno = cnht.congno + tongtien - tongtiencu;
                if (cnht.congno < 0)
                {
                    ModelState.AddModelError("", "cong no dai ly la so am => co sach da ban trong phieu chua sua");
                    phieuxuat.ctpxes = ctpx;
                    return re(phieuxuat);
                }

                foreach (ctpx ct in ctpx)
                {
                    db.ctpxes.Add(ct);
                }


                db.Entry(phieuxuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); ;
            }
            phieuxuat.ctpxes = ctpx;
            return re(phieuxuat);
        }

        // GET: phieuxuats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuats.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuxuat);
        }

        // POST: phieuxuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phieuxuat phieuxuat = db.phieuxuats.Find(id);
            db.phieuxuats.Remove(phieuxuat);
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

        private ActionResult re(phieuxuat px)
        {
            ViewBag.iddl = new SelectList(db.dailies, "iddl", "tendl", px.iddl);
            ViewBag.idsach = new SelectList(db.saches, "idsach", "tensach");
            phieuxuatviewmodel pxvm1 = new phieuxuatviewmodel();
            pxvm1.phieuxuat = px;
            return View(pxvm1);
        }
    }
}
