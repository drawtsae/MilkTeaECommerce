﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DiscountsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {

            return View();
        }
        public IActionResult Product(int? page)
        {
            ViewBag.Name = "Product";
            int num = 9;
            var listDiscount = _db.Discounts.Where(x => x.ProductDiscount.Count > 0);

            int discountCount = listDiscount.Count();
            // điều kiện để phân trang
            if (page != null && page > 0)
            {
                int NumPageCanHave = discountCount % num == 0 ? discountCount / num : (discountCount / num) + 1;

                ViewBag.page = page;
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= NumPageCanHave && discountCount % num != 0)
                {

                    // lấy số lẻ
                    listDiscount = listDiscount.Skip(num * (NumPageCanHave - 1)).Take(discountCount % num);

                }
                //trường hợp 1
                else
                {

                    listDiscount = listDiscount.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }
            }
            else
            {
                ViewBag.page = 1;
                listDiscount = listDiscount.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = listDiscount
                .Select(x => new DiscountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DateStart = x.DateStart
                }).ToList();
            return View(obj);
        }
        public IActionResult Delivery(int? page)
        {
            ViewBag.Name = "Delivery";
            int num = 9;
            var listDiscount = _db.Discounts.Include(x => x.DeliveryDiscount)
                .OrderByDescending(x => x.DateStart).Where(x => x.DeliveryDiscount.Count > 0);

            int discountCount = listDiscount.Count();
            // điều kiện để phân trang
            if (page != null && page > 0)
            {
                int NumPageCanHave = discountCount % num == 0 ? discountCount / num : (discountCount / num) + 1;

                ViewBag.page = page;
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= NumPageCanHave && discountCount % num != 0)
                {

                    // lấy số lẻ
                    listDiscount = listDiscount.Skip(num * (NumPageCanHave - 1)).Take(discountCount % num);

                }
                //trường hợp 1
                else
                {

                    listDiscount = listDiscount.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }
            }
            else
            {
                ViewBag.page = 1;
                listDiscount = listDiscount.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = listDiscount
                .Select(x => new DiscountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DateStart = x.DateStart
                }).ToList();
            return View("Product", obj);
        }
        public IActionResult Category(int? page)
        {
            ViewBag.Name = "Category";
            int num = 9;
            var listDiscount = _db.Discounts.Where(x => x.CategoryDiscount.Count > 0);

            int discountCount = listDiscount.Count();
            // điều kiện để phân trang
            if (page != null && page > 0)
            {
                int NumPageCanHave = discountCount % num == 0 ? discountCount / num : (discountCount / num) + 1;

                ViewBag.page = page;
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= NumPageCanHave && discountCount % num != 0)
                {

                    // lấy số lẻ
                    listDiscount = listDiscount.Skip(num * (NumPageCanHave - 1)).Take(discountCount % num);

                }
                //trường hợp 1
                else
                {

                    listDiscount = listDiscount.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }
            }
            else
            {
                ViewBag.page = 1;
                listDiscount = listDiscount.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = listDiscount
                .Select(x => new DiscountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DateStart = x.DateStart
                }).ToList();
            return View("Product", obj);
        }
        public IActionResult productdiscount()
        {
            var listDiscount = _db.Discounts.OrderByDescending(x => x.DateStart)
                .Where(x => x.ProductDiscount.Count > 0 && (x.DateExpired==null || x.DateExpired>DateTime.Now))
                .Take(4).ToList();

            var obj = listDiscount
                .Select(x => new 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code=x.Code,
                    DateStart=x.DateStart.GetValueOrDefault().ToLocalTime().ToShortDateString()
                }).ToList();
            return Json(obj);
        }
        public IActionResult deliverydiscount()
        {
          
            var listDiscount = _db.Discounts.Include(x=>x.DeliveryDiscount)
                .OrderByDescending(x=>x.DateStart)
                .Where(x => x.DeliveryDiscount.Count > 0 && (x.DateExpired ==null || x.DateExpired>DateTime.Now))
                .Take(4).ToList();

           
            var obj = listDiscount
                .Select(x => new 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DateStart = x.DateStart.GetValueOrDefault().ToLocalTime().ToShortDateString()
                }).ToList();
            return Json(obj);
        }
        public IActionResult catediscount(int? page)
        {
        
            var listDiscount = _db.Discounts.OrderByDescending(x => x.DateStart)
                .Where(x => x.CategoryDiscount.Count > 0 && (x.DateExpired==null ||x.DateExpired>DateTime.Now))
                .Take(4).ToList();

            var obj = listDiscount
                .Select(x => new 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DateStart = x.DateStart.GetValueOrDefault().ToLocalTime().ToShortDateString()
                }).ToList();
            return Json( obj );
        }

        public IActionResult getdetails(string id)
        {
            var obj = _db.Discounts.Where(x => x.Id == id).Select(x => new
            {
                Id=x.Id,
                Name=x.Name,
                DateStart=x.DateStart== (DateTime?)null ?"": x.DateStart.GetValueOrDefault().ToLocalTime().ToShortDateString(),
                DateExpired = x.DateExpired == (DateTime?)null ? "" : x.DateExpired.GetValueOrDefault().ToLocalTime().ToShortDateString(),
                Desc =x.Description,
                Code=x.Code
            }).SingleOrDefault();
            return Json(obj);
        }
    }
}
