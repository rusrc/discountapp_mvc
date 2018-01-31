using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.MVC.Controllers
{
    public class AddressesController : Controller
    {
        private DiscountappDbContext db = new DiscountappDbContext();

        // GET: Addresses
        public async Task<ActionResult> Index()
        {
            var addresses = db.Addresses.Include(a => a.BusinessDomain).Include(a => a.City);
            return View(await addresses.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            ViewBag.BusinessDomainId = new SelectList(db.BusinessDomains, "Id", "Name");
            ViewBag.CityId = new SelectList(db.Cities, "Id", "NameMultiLangJson");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BusinessDomainId,CityId,MapJsonCoord,Description,Information,WorkTime,WorkTimeSaturday,WorkTimeSunday")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessDomainId = new SelectList(db.BusinessDomains, "Id", "Name", address.BusinessDomainId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "NameMultiLangJson", address.CityId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessDomainId = new SelectList(db.BusinessDomains, "Id", "Name", address.BusinessDomainId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "NameMultiLangJson", address.CityId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BusinessDomainId,CityId,MapJsonCoord,Description,Information,WorkTime,WorkTimeSaturday,WorkTimeSunday")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessDomainId = new SelectList(db.BusinessDomains, "Id", "Name", address.BusinessDomainId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "NameMultiLangJson", address.CityId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Address address = await db.Addresses.FindAsync(id);
            db.Addresses.Remove(address);
            await db.SaveChangesAsync();
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
