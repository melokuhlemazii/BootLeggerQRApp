using BootLeggerQRApp.Data;
using BootLeggerQRApp.Helpers;
using BootLeggerQRApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootLeggerQRApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDBContext _context;

        public OrdersController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Orders.Include(o => o.Table);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Orders/MyOrder (customer-facing current order)
        public async Task<IActionResult> MyOrder()
        {
            var tableId = HttpContext.Session.GetInt32(Sessionkeys.CurrentTableId);
            if (tableId == null)
            {
                TempData["Error"] = "You must join a table first by scanning the QR.";
                return RedirectToAction("Index", "QRCode");
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.TableId == tableId && o.Status == OrderStatus.Preparing);

            if (order == null)
            {
                // return an empty order model so the view shows the "cart is empty" message
                order = new Order
                {
                    OrderItems = new List<OrderItem>()
                };
            }

            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Table)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,TableId,CustomerName,OrderTime,Status,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", order.TableId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", order.TableId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,TableId,CustomerName,OrderTime,Status,TotalPrice")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", order.TableId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Table)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToOrder(int menuItemId, int quantity)
        {
            // find current table (from session)
            var tableId = HttpContext.Session.GetInt32(Sessionkeys.CurrentTableId);
            if (tableId == null)
            {
                TempData["Error"] = "You must join a table first by scanning the QR.";
                return RedirectToAction("Index", "QRCode");
            }

            // find or create active order for this table
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.TableId == tableId && o.Status == OrderStatus.Preparing);

            if (order == null)
            {
                order = new Order
                {
                    TableId = tableId.Value,
                    OrderTime = DateTime.Now,
                    Status = OrderStatus.Preparing,
                    CustomerName = "Guest"
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            // add item
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                MenuItemId = menuItemId,
                Quantity = quantity
            };
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            TempData["Banner"] = "Item added to your order!";
            return RedirectToAction("Menu", "QRCode");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int orderItemId)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyOrder");
        }




    }
}