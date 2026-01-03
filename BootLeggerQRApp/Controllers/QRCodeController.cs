using System.Linq;
using System.Threading.Tasks;
using BootLeggerQRApp.Data;
using BootLeggerQRApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace BootLeggerQRApp.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QRCodeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Landing/instructions + dev helpers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // handy for dev: show some tables to "join"
            ViewBag.Tables = await _context.Tables
                .OrderBy(t => t.TableNumber)
                .Take(12)
                .ToListAsync();

            int? tableId = HttpContext.Session.GetInt32(Sessionkeys.CurrentTableId);
            ViewBag.CurrentTableId = tableId;

            return View();
        }

        // Admin/staff utility: list all tables with QR codes
        [HttpGet]
        public async Task<IActionResult> Codes()
        {
            var tables = await _context.Tables
                .OrderBy(t => t.TableNumber)
                .ToListAsync();
            return View(tables);
        }

        // Generate the QR PNG for a specific table
        [HttpGet]
        public IActionResult Code(int tableId)
        {
            var url = Url.Action(
                action: "Join",
                controller: "QRCode",
                values: new { tableId },
                protocol: HttpContext.Request.Scheme
            );

            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(data);
            var bytes = png.GetGraphic(20);

            return File(bytes, "image/png");
        }

        // /QRCode/Join?tableId=5   (simulate scanning a QR at the table)
        [HttpGet]
        public async Task<IActionResult> Join(int tableId)
        {
            var exists = await _context.Tables.AnyAsync(t => t.TableId == tableId);
            if (!exists)
            {
                TempData["Error"] = $"Table {tableId} not found.";
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetInt32(Sessionkeys.CurrentTableId, tableId);
            TempData["Banner"] = $"You're now at Table {tableId}.";
            return RedirectToAction(nameof(Menu));
        }

        // forget the current table
        [HttpGet]
        public IActionResult Leave()
        {
            HttpContext.Session.Remove(Sessionkeys.CurrentTableId);
            TempData["Banner"] = "You have left the table.";
            return RedirectToAction(nameof(Index));
        }

        // the customer-facing menu
        [HttpGet]
        public async Task<IActionResult> Menu(string category = null, string q = null)
        {
            var tableId = HttpContext.Session.GetInt32(Sessionkeys.CurrentTableId);
            ViewBag.CurrentTableId = tableId;

            var query = _context.MenuItems.AsQueryable();

            // only available items visible to customers
            query = query.Where(m => m.IsAvailable);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(m => m.Category == category);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(m => m.Name.Contains(q) || m.Description.Contains(q));

            var items = await query
                .OrderBy(m => m.Category).ThenBy(m => m.Name)
                .ToListAsync();

            // categories for filter pills
            ViewBag.Categories = await _context.MenuItems
                .Where(m => m.IsAvailable)
                .Select(m => m.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.Query = q;
            ViewBag.SelectedCategory = category;

            return View(items);
        }
    }
}

