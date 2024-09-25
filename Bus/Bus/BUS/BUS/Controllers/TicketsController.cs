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
    public class TicketsController : Controller
    {
        private BusEasyEntities db = new BusEasyEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            return View(db.Tickets.ToList());
        }
        public ActionResult BookingHistory()
        {
            if(Session["Id"]==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = Convert.ToInt32(Session["Id"]);
            return View(db.Tickets.Where(c => c.C_ID ==id).ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
       
        // GET: Tickets/Create
        public ActionResult Create()
        {
           // if (Session["Ticket"] != null)
            //{

           // }

            // Retrieve all buses and pass them to the view
            ViewBag.BusList = new SelectList(db.Buses.ToList(), "BUS_ID", "BUS_NUMBER"); // Use appropriate display field
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TICKET_ID,C_ID,BUS_ID,SEAT_NUMBER,JOURNEY_DATE,PRICE")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                Session["date"]=ticket.JOURNEY_DATE;
                // Set customer ID from session
                ticket.C_ID = Convert.ToInt32(Session["Id"]);
                ticket.PRICE = 50;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                Session["Ticket_id"] = ticket.TICKET_ID;
                var id =  db.Tickets.Where(c => c.C_ID == ticket.C_ID && c.BUS_ID == ticket.BUS_ID && c.SEAT_NUMBER == ticket.SEAT_NUMBER).Select(c => c.TICKET_ID);

                return RedirectToAction("Create","Payments");
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
        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TICKET_ID,C_ID,BUS_ID,SEAT_NUMBER,JOURNEY_DATE,PRICE")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            if (Session["p"].Equals("1"))
            {
                return RedirectToAction("BookingHistory");
            }

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


        public ActionResult ViewTicketPDF(int id)
        {
            // Set the ticket ID in ViewBag
            ViewBag.TicketId = id;

            // Return the view
            return View();
        }


        // Action to allow the user to download the PDF
        public ActionResult DownloadTicketPDF()
        {
            // Path to the static PDF file
            string filePath = Server.MapPath("~/Content/TicketDocuments/YourTicket.pdf");
            // Name of the file that will be shown to the user during download
            string fileName = "YourTicket.pdf";

            // Return the file to the user with the specified file name
            return File(filePath, "application/pdf", fileName);
        }





    }
}
