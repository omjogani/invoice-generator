using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

namespace InvoiceGenerator.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDBContext _context;

        public ProductController(ProductDBContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.User = HomeController.currUser;
           
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.products.Where(c=>c.userId==HomeController.currUser.Id).ToListAsync());
        }

        // GET: Product/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Products());
            else
                return View(_context.products.Find(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("productId,userId,productName,Qty,price,total")] Products product)
        {
            if (ModelState.IsValid)
            {
                if (product.productId == 0)
                {
                    product.userId = HomeController.currUser.Id;
                    product.total = product.Qty * product.price;    
                    _context.Add(product);
                }
                else
                {
                    product.total = product.Qty * product.price;
                    _context.Update(product);
                }
                    
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.products.FindAsync(id);
            _context.products.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> pdfview()
        {
            return View(await _context.products.Where(c => c.userId == HomeController.currUser.Id).ToListAsync());
        }
       
        public FileResult GeneratePdf(string html)
        {

            html = html.Replace("strtTag", "<").Replace("EndTag", ">");
            HtmlToPdf objhtml = new HtmlToPdf();
            objhtml.Options.PdfPageSize = PdfPageSize.A4;
            objhtml.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            objhtml.Options.MarginLeft = 200;
            objhtml.Options.MarginRight = 10;
            objhtml.Options.MarginTop = 20;
            objhtml.Options.MarginBottom = 20;
            
            PdfDocument objdoc = objhtml.ConvertHtmlString(html);
            
            byte[] pdf = objdoc.Save();
            objdoc.Close();
            return File(pdf, "application/pdf", "Mypdf.pdf");
        }
    }
}
