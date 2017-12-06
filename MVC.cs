using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MVC.Controllers
{
     [Authorize]
    public class MVCController : Controller
    {
        private SarvodayaEntities db = new SarvodayaEntities();

        
        public ActionResult Index()
        {
            var BranchLog = new List<int>();
            BranchLog = db.sp_UserBranch(SessionLog.LevelId, SessionLog.BranchId).ToList().Select(s => s.BranchId_Pk).ToList();
            // int[] Branch = branchList.ToArray();          
            var rE_Branch = db.RE_Branch.Include(r => r.m_HO).Include(r => r.m_Region).Include(r => r.m_Region1).Include(r => r.m_Zone);
            var branchList= (from data in rE_Branch join b in db.RE_Branch on data.BranchId_Pk equals b.BranchId_Pk where BranchLog.Contains(b.BranchId_Pk) select data).Distinct().ToList();
            return View(branchList.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RE_Branch rE_Branch = db.RE_Branch.Find(id);
            if (rE_Branch == null)
            {
                return HttpNotFound();
            }
            return View(rE_Branch);
        }

      
        public ActionResult Create()
        {
            ViewBag.hoId_Fk = new SelectList(db.m_HO, "HoId_Pk", "HO");
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region");
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region");
            ViewBag.ZoneId_Fk = new SelectList(db.m_Zone, "ZoneId_Id", "ZoneName");
            ViewBag.StateId_Fk = new SelectList(db.m_State.OrderBy(s => s.State), "stateId_Pk", "State");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchId_Pk,BranchLevel,hoId_Fk,ZoneId_Fk,StateId_Fk,RegionId_Fk,Branch,BranchAddress,PhoneNo,MobileNo,EmailId,CreateDate,CreatedBy")] RE_Branch rE_Branch)
        {
            rE_Branch.CreateDate = DateTime.Now;
            rE_Branch.CreatedBy = SessionLog.EmpId;
            rE_Branch.BranchLevel = 4;
            if (ModelState.IsValid)
            {
                db.RE_Branch.Add(rE_Branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hoId_Fk = new SelectList(db.m_HO, "HoId_Pk", "HO", rE_Branch.hoId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.ZoneId_Fk = new SelectList(db.m_Zone, "ZoneId_Id", "ZoneName", rE_Branch.ZoneId_Fk);
            ViewBag.StateId_Fk = new SelectList(db.m_State.OrderBy(s => s.State), "stateId_Pk", "State");
            return View(rE_Branch);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RE_Branch rE_Branch = db.RE_Branch.Find(id);
            if (rE_Branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.hoId_Fk = new SelectList(db.m_HO, "HoId_Pk", "HO", rE_Branch.hoId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.ZoneId_Fk = new SelectList(db.m_Zone, "ZoneId_Id", "ZoneName", rE_Branch.ZoneId_Fk);
            ViewBag.StateId_Fk = new SelectList(db.m_State.OrderBy(s => s.State), "stateId_Pk", "State");
            return View(rE_Branch);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchId_Pk,BranchLevel,hoId_Fk,ZoneId_Fk,StateId_Fk,RegionId_Fk,Branch,BranchAddress,PhoneNo,MobileNo,EmailId,CreateDate,CreatedBy")] RE_Branch rE_Branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rE_Branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hoId_Fk = new SelectList(db.m_HO, "HoId_Pk", "HO", rE_Branch.hoId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.RegionId_Fk = new SelectList(db.m_Region, "RegionId_Pk", "Region", rE_Branch.RegionId_Fk);
            ViewBag.ZoneId_Fk = new SelectList(db.m_Zone, "ZoneId_Id", "ZoneName", rE_Branch.ZoneId_Fk);
            ViewBag.StateId_Fk = new SelectList(db.m_State.OrderBy(s => s.State), "stateId_Pk", "State");
            return View(rE_Branch);
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
