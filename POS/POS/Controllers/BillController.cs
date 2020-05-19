using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace POS.Controllers
{
    public class BillController : Controller
    {
        POSContext context = new POSContext();

        // GET: Bill
        public ActionResult Index()
        {
            return View(context.Bill.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bill bill)
        {

            bill.CreatedBy = User.Identity.Name;
            context.Bill.Add(bill);
            context.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = context.Bill.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }

            return View(bill);
        }

        [HttpPost]
        public ActionResult Edit(Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.UpdatedBy = User.Identity.Name;
                bill.UpdatedDate = DateTime.Now;
                context.Entry(bill).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bill);
        }

        public ActionResult AddItem(int id)
        {
            var products = context.Product.ToList();
            ViewBag.Products = products;
            BillItems items = new Models.BillItems();
            items.BillNo = id;
            return View(items);
        }

        [HttpPost]
        public ActionResult AddItem(BillItems billItems, int id)
        {
            billItems.CreatedBy = User.Identity.Name;
            context.BillItems.Add(billItems);
            context.SaveChanges();

            Bill bill = context.Bill.Find(id);
            bill.BillTotal = context.BillItems.Where(x => x.BillNo == id).Sum(x => x.Total) + (context.BillItems.Where(x => x.BillNo == id).Sum(x => x.Total)) * 10 / 100;
            context.Entry(bill).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("BillItems", new { id = id });
        }

        public ActionResult BillItems(int id)
        {
            var bill = context.Bill.Find(id);

            ViewBag.bill = bill;

            var items = from bi in context.BillItems
                        join pro in context.Product on bi.ProId equals pro.UniqueId
                        where bi.BillNo == id
                        select new ProductItem
                        {
                            ProductName = pro.Name,
                            Quantity = bi.Quantity,
                            Total = bi.Total,
                            price = pro.Price,
                            UniqueId = bi.UniqueId
                        };


            return View(items.ToList());
        }

        public ActionResult EditBillItem(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillItems billitem = context.BillItems.Find(id);
            if (billitem == null)
            {
                return HttpNotFound();
            }

            var products = context.Product.ToList();
            ViewBag.Products = products;

            return View(billitem);
        }

        [HttpPost]
        public ActionResult EditBillItem(BillItems billitem)
        {
            if (ModelState.IsValid)
            {
                billitem.UpdatedBy = User.Identity.Name;
                billitem.UpdatedDate = DateTime.Now;
                context.Entry(billitem).State = EntityState.Modified;
                context.SaveChanges();


                Bill bill = context.Bill.Find(billitem.BillNo);
                bill.BillTotal = context.BillItems.Where(x => x.BillNo == billitem.BillNo).Sum(x => x.Total) + (context.BillItems.Where(x => x.BillNo == billitem.BillNo).Sum(x => x.Total)) * 10 / 100;
                context.Entry(bill).State = EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("BillItems", new { id = billitem.BillNo });
            }
            return View(billitem);
        }

        public ActionResult RemoveItem(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillItems billitem = context.BillItems.Find(id);
            if (billitem == null)
            {
                return HttpNotFound();
            }

            context.BillItems.Remove(billitem);
            context.SaveChanges();

            Bill bill = context.Bill.Find(billitem.BillNo);
            bill.BillTotal = context.BillItems.Where(x => x.BillNo == billitem.BillNo).Sum(x => x.Total) + (context.BillItems.Where(x => x.BillNo == billitem.BillNo).Sum(x => x.Total)) * 10 / 100;
            context.Entry(bill).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("BillItems", new { id = billitem.BillNo });
        }

        public ActionResult GenerateBill(int id)
        {
            var bill = context.Bill.Find(id);

            ViewBag.bill = bill;

            var items = from bi in context.BillItems
                        join pro in context.Product on bi.ProId equals pro.UniqueId
                        where bi.BillNo == id
                        select new ProductItem
                        {
                            ProductName = pro.Name,
                            Quantity = bi.Quantity,
                            Total = bi.Total,
                            price = pro.Price,
                            UniqueId = bi.UniqueId
                        };


            return View(items.ToList());
        }

        public static string GetMailBody(Bill bill, List<ProductItem> items)
        {
            string messageBody = string.Empty;
            try
            {
                var SgstCgstPrice = (items.Sum(x => x.Total)) * 10 / 100;
                var TotalPrice = items.Sum(x => x.Total) + SgstCgstPrice;

                 messageBody = "<html>" +
                "<body>" +
                      "<table style=\"font-family:Trebuchet MS,Arial,Helvetica, sans-serif;border-collapse:collapse;width:100 %\">" +
                      "<tr>" +
                      "<td style=\"border: 1px solid #ddd;padding: 8px;\"><b> Table No:</b>" + bill.TableNo + "</td>" +
                      "<td style=\"border: 1px solid #ddd;padding: 8px;\">" +
                      "<b>Bill No:</b>" + bill.Id.ToString() +
                      "</td>" +
                      "<td style=\"border: 1px solid #ddd;padding: 8px;\"><b> date :</b>" + bill.CreatedDate.ToString() + "</td>" +
                      "</tr>" +
                      "</table><hr />" +
                      "<table style=\"font-family:Trebuchet MS,Arial, Helvetica, sans-serif;border-collapse:collapse;width:100 %\"><thead><tr><th style=\"border: 1px solid #ddd;padding: 8px;\">Product</th>" +
                      "<th style=\"border: 1px solid #ddd;padding: 8px;\">Quantity</th><th style=\"border: 1px solid #ddd;padding: 8px;\">price</th><th style=\"border: 1px solid #ddd;padding: 8px;\">Total</th></thead><tbody>";

                foreach (var item in items)
                {
                    messageBody = messageBody + "<tr><td style=\"border: 1px solid #ddd;padding: 8px;\">" + item.ProductName + "</td><td style=\"border: 1px solid #ddd;padding: 8px;\">" + item.Quantity + "</td><td style=\"border: 1px solid #ddd;padding: 8px;\">" + item.price + "</td><td style=\"border: 1px solid #ddd;padding: 8px;\">" + item.Total + "</td></tr>";
                }

                messageBody = messageBody + "</tbody></table><hr /> <table style=\"font-family:Trebuchet MS,Arial, Helvetica, sans-serif;border-collapse:collapse;width:100 %\"><tr><td style=\"border: 1px solid #ddd;padding: 8px;\"><b>SGST(5%) + CGST(5%)&nbsp;:&nbsp;&nbsp;&nbsp;</b>" + SgstCgstPrice +
                    " Rs.</td><td style=\"border: 1px solid #ddd;padding: 8px;\"><b>Total Cost&nbsp;:&nbsp;&nbsp;&nbsp;</b>" + TotalPrice +"Rs.</td></tr></table>"+
                    "</body>" +
                      "</html>";
            }
            catch (Exception ex)
            {
                throw;
            }

            return messageBody;
        }

        public ActionResult SendMail(int Bllid,string mailAdress)
        {
            try
            {
                var bill = context.Bill.Find(Bllid);

                var items = from bi in context.BillItems
                            join pro in context.Product on bi.ProId equals pro.UniqueId
                            where bi.BillNo == Bllid
                            select new ProductItem
                            {
                                ProductName = pro.Name,
                                Quantity = bi.Quantity,
                                Total = bi.Total,
                                price = pro.Price,
                                UniqueId = bi.UniqueId
                            };

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(ConfigurationManager.AppSettings["FromMailAddress"].ToString());
                message.To.Add(new MailAddress(mailAdress));
                message.Subject = "Your Bill";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = GetMailBody(bill, items.ToList());
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromMailAddress"].ToString(),
                    ConfigurationManager.AppSettings["password"].ToString());
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}