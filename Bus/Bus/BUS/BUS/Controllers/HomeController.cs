using BUS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BUS.Controllers
{
    public class HomeController : Controller
    {
        private BusEasyEntities db = new BusEasyEntities();

        // GET: Tickets/Create
        public ActionResult Index()
        {

         // Retrieve all buses and pass them to the view
            ViewBag.BusList = new SelectList(db.Buses.ToList(), "BUS_ID", "BUS_NUMBER"); // Use appropriate display field
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TICKET_ID,C_ID,BUS_ID,SEAT_NUMBER,JOURNEY_DATE,PRICE")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                if (Session["Id"] == null)
                {
                    Session["Ticket"] = ticket;
                }

                return RedirectToAction("Login","Customers");
            }

            // Retrieve all buses again if model state is invalid
            ViewBag.BusList = new SelectList(db.Buses.ToList(), "BUS_ID", "BUS_NUMBER"); // Use appropriate display field
            return View(ticket);
        }

        // GET: Tickets/GetBusCapacity
        public JsonResult GetBusCapacity(int busId)
        {
            var bus = db.Buses.SingleOrDefault(b => b.BUS_ID == busId);
            if (bus == null)
            {
                return Json(new { capacity = 0 }, JsonRequestBehavior.AllowGet);
            }

            var route = db.Routes.SingleOrDefault(r => r.ROUTE_ID == bus.ROUTE_ID);
            var takenSeats = db.Tickets.Where(t => t.BUS_ID == busId).Select(t => t.SEAT_NUMBER).ToList();

            return Json(new
            {
                capacity = bus.CAPACITY,
                startLocation = route?.START_LOCATION,
                endLocation = route?.END_LOCATION,
                takenSeats = takenSeats
            }, JsonRequestBehavior.AllowGet);
        }

         public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }

        // POST: Student/Login
        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {

            if (username.Equals("admin") && password.Equals("123456"))
            {
                // Login successful
                // Redirect to dashboard or any other page
                Session["admin"] = "logged_in";
                return RedirectToAction("Index", "Buses");
            }
            else
            {
                ViewBag.Log = "Login Failed.Please Enter Correct Username and Password";
                // Login failed
                // You can add a ViewBag message here to show an error message on the login page
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }

        }
        public ActionResult AdminLogout()
        {
            Session["admin"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}