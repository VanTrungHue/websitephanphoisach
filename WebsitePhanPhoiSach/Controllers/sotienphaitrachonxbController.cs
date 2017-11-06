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
    public class sotienphaitrachonxbController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();
        // GET: sotienphaitrachonxb
        public ActionResult Index(string thoidiem)
        {
             DateTime searchDate;
            if (DateTime.TryParse(thoidiem, out searchDate))
            {
                
                List<sotienphaitrachonxb> list = new List<sotienphaitrachonxb>();
                DateTime tomorrowdate = searchDate.AddDays(1);
                var sotienphaitrachonxb = db.sotienphaitrachonxbs.Include(b =>b.nxb)
                    .OrderByDescending(o => o.id)
                    .FirstOrDefault(h => h.thoidiem <= tomorrowdate);
                
                list.Add(sotienphaitrachonxb);
               
                return View(list);
                // do not use .Equals() which can not be converted to SQL
            }
        

            var sotienphatrachonxbs = db.sotienphaitrachonxbs.Include(t => t.nxb);
            return View(sotienphatrachonxbs.ToList());
        }
    }
}