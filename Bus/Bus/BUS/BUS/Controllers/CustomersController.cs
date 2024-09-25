using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BUS.Models;

namespace BUS.Controllers
{
    public class CustomersController : Controller
    {
        private BusEasyEntities db = new BusEasyEntities();
        // GET: Customer/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (email.Equals("admin") && password.Equals("123456"))
            {
                // Login successful
                // Redirect to dashboard or any other page
                Session["admin"] = "logged_in";
                return RedirectToAction("Index", "Buses");
            }
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

            var customer = db.Customers.SingleOrDefault(c => c.EMAIL == email && c.PASSWORD == password);
            if (customer == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Set session variables
            Session["name"] = customer.C_NAME;
            Session["Id"] = customer.C_ID;

            // Redirect to the "Details" action of "Customer" controller
            return RedirectToAction("Create", "Tickets");
        }
        public ActionResult Logout()
        {
            Session["name"] = null;
            Session["Id"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Customers
        public ActionResult Index()
        {
            if (Session["admin"] == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
            if (Session["Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int customerId = (int)Session["Id"];
            Customer customer = db.Customers.Find(customerId);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "C_ID,C_NAME,EMAIL,MOBILE_NUMBER,ADDRESS,PASSWORD")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                Session["name"] = customer.C_NAME;
                Session["Id"] = customer.C_ID;
                db.Customers.Add(customer);
                db.SaveChanges();

            }

            Session["name"] = customer.C_NAME;
            Session["Id"] = customer.C_ID;
            return RedirectToAction("Details", "Customers");
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "C_ID,C_NAME,EMAIL,MOBILE_NUMBER,ADDRESS,PASSWORD")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                Session["name"] = customer.C_NAME;
                Session["Id"] = customer.C_ID;
                return RedirectToAction("Details", "Customers");
            }
            Session["name"] = customer.C_NAME;
            Session["Id"] = customer.C_ID;
            return RedirectToAction("Details", "Customers");
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
