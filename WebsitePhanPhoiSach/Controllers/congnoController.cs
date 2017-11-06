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
    public class congnoController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();
        // GET: congno
        public ActionResult Index(string thoidiem)
        {
            DateTime searchDate;
            if (DateTime.TryParse(thoidiem, out searchDate))
            {

                List<congnotheothoigian> cn = new List<congnotheothoigian>();
                DateTime tomorrowdate = searchDate.AddDays(1);
                var congno = db.congnotheothoigians.Include(c => c.daily)
                    .OrderByDescending(o => o.id)
                    .FirstOrDefault(h => h.thoidiem <= tomorrowdate);

                cn.Add(congno);

                return View(cn);
                // do not use .Equals() which can not be converted to SQL
            }


            var congnno = db.congnotheothoigians.Include(t => t.daily);
            return View(congnno.ToList());
        }
    }
}