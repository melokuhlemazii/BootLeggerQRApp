using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BootLeggerQRApp.Data;
using BootLeggerQRApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BootLeggerQRApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public PaymentController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Payment (show summary and payment options)
        public async Task<IActionResult> Index(int? orderId)
        {
            Order order;

            if (orderId == null)
            {
                // if no id given, fetch latest order
                order = await _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .OrderByDescending(o => o.OrderTime)
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    TempData["Error"] = "No orders found. Please place an order first.";
                    return RedirectToAction("Index", "QRCode");
                }
            }
            else
            {
                order = await _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    return NotFound();
            }

            ViewBag.Subtotal = order.OrderItems.Sum(i => i.MenuItem.Price * i.Quantity);
            ViewBag.GrandTotal = ViewBag.Subtotal; // extend later for tax/fees

            return View(order);
        }

        // POST: Confirm payment choice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int orderId, string paymentMethod)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();

            // Update status based on payment method
            order.Status = OrderStatus.Preparing;

            await _context.SaveChangesAsync();

            ViewBag.Subtotal = order.OrderItems.Sum(i => i.MenuItem.Price * i.Quantity);
            ViewBag.GrandTotal = ViewBag.Subtotal;
            ViewBag.PaymentMethod = paymentMethod;

            return View("Confirm", order);
        }
    }
}



