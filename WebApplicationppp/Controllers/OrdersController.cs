using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationppp.Models;
using System.Linq.Expressions;

namespace WebApplicationppp.Controllers
{
    public class OrdersController : ApiController
    {
        private DiplomnaEntities db = new DiplomnaEntities();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(ReservationsView))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Expression<Func<ReservationsView, bool>> where;

            switch (id)
            {
                case 1: where = x => x.Date <= DateTime.Today && x.Accepted;
                    break;
                case 2: where = x => true;
                    break;
                case 3: where = x => x.Rejected;
                    break;
                default: where = x => true;
                    break;
            }

            var service = await db.ReservationsViews.Where(where).ToListAsync();

            return Ok(service);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(OrderInsert order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order newO = new Order()
            {
                FirstName = order.FirstName,
                LastName = order.LastName,
                StartTime = order.StartTime,
                EndTime = order.EndTime,
                Phone = order.Phone,
                SalonID = order.SalonID,
                Sex = order.Sex,
                UserName = order.UserName,
                OrderDetails = order.Services,
                CreateOn = DateTime.Now,
                Date = order.Date
            };

            db.Orders.Add(newO);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = newO.OrderID }, newO);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}