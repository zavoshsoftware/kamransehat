using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using System.IO;

namespace KamranSehat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TeamMembersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: TeamMembers
        public ActionResult Index()
        {
            return View(db.TeamMembers.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: TeamMembers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeamMember teamMember, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Team/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    teamMember.ImageUrl = newFilenameUrl;
                }
                #endregion
                teamMember.IsDeleted=false;
				teamMember.CreationDate= DateTime.Now; 
                teamMember.Id = Guid.NewGuid();
                db.TeamMembers.Add(teamMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeamMember teamMember, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Team/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    teamMember.ImageUrl = newFilenameUrl;
                }
                #endregion
                teamMember.IsDeleted=false;
                db.Entry(teamMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TeamMember teamMember = db.TeamMembers.Find(id);
			teamMember.IsDeleted=true;
			teamMember.DeletionDate=DateTime.Now;
 
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
