﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Hosting;

using MilkTeaECommerce.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;

using MilkTeaECommerce.Utility;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostEnvironment _hostEnvironment;
        public HomeController(ApplicationDbContext db, IHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;

        }
        public IActionResult Index()
        {

            //var listshop = _db.Shops.ToList();

            //ViewData["listshop"] = _db.Shops.Select(c => new SelectListItem()
            //{
            //    Text = c.Name,
            //    Value = c.ApplicationUserId
            //}).ToList();
            var lsShop = _db.Shops.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.ApplicationUserId
            }).ToList();
            var parameter = new DynamicParameters();
            parameter.Add("@shopid",lsShop[0].Value.ToString());
            parameter.Add("@year", Convert.ToInt32(DateTime.Now.Year));
            // lấy về dưới dạng json
            var obj = SP_Call.ExecuteJson("statistic_number", parameter);//ok
            if(obj != null)
            {
                TempData["earning"] = obj.Select(x => x["earning"].ToString()).ToList()[0].ToString();
                TempData["sumproduct"] = obj.Select(x => x["count_product"].ToString()).ToList()[0].ToString();
                TempData["sumcustomer"] = obj.Select(x => x["count_cus"].ToString()).ToList()[0].ToString();

            }
            return View(lsShop);
        }
        //public IActionResult GetDataTable(string Id)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        //{
        //    var parameter = new DynamicParameters();
        //    parameter.Add("@shopid", Id);
        //    var obj = SP_Call.ExecuteJson("USP_stastisticTable", parameter);//ok
        //    var list = obj.ToList();
        //    //var lstCustomerid = obj.Select(x => x["cusid"].ToString()).ToList();
        //    //var lstCustomerMoney = obj.Select(x => x["SumMoney"].ToString()).ToList();
        //    return Json(new { data = list });

        //}
        public IActionResult GetDatanumber(string Id,int year)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {
            string earning;
            string sumproduct;
            string sumcustomer;
            var parameter = new DynamicParameters();
            parameter.Add("@shopid", Id);
            parameter.Add("@year", year);
            var obj1 = SP_Call.ExecuteJson("statistic_number", parameter);//ok
            if(obj1 == null)
            {
                earning = "0";
                sumproduct = "0";
                sumcustomer = "0";
                return Json(new { earning, sumproduct, sumcustomer });
            }    
            earning = obj1.Select(x => x["earning"].ToString()).ToList()[0].ToString();
            sumproduct = obj1.Select(x => x["count_product"].ToString()).ToList()[0].ToString();
            sumcustomer = obj1.Select(x => x["count_cus"].ToString()).ToList()[0].ToString();
            return Json(new { earning,sumproduct,sumcustomer });
            
        }
        public IActionResult GetData(string Id,int year)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {
            //var totalShop = _db.Shops.Include(x => x.Products).Where(x => x.ApplicationUserId == Id);

            //var orderCount = _db.OrderDetails
            //    .Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id)
            //    .Select(x => new { 
            //        product = x.Product.Name,
            //        price = x.Price
            //    })
            //    .GroupBy(x=>x.product)
            //    .Select(x=> new {
            //        Key = x.Key,
            //        Value = x.Sum(s => s.price)    
            //    }).ToList();

            //var a = new List<string>();
            //var b = new List<float>();
            //foreach (var item in orderCount)
            //{
            //    SumPrice += (float)item.Value;
            //    a.Add(item.Key);
            //    b.Add((float)item.Value);
            //}

            float SumPrice = 0;
            string[] labels = new string[12];
            labels = new string[]{ "1","2", "3","4","5", "6","7","8","9","10","11", "12" };
            var values = new float[12];
            values = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var parameter = new DynamicParameters();
            parameter.Add("@shopid", Id);
            parameter.Add("@year", year);
            // lấy về dưới dạng json
            var obj = SP_Call.ExecuteJson("USP_Statistic", parameter);//ok
            List<string> label = new List<string>();
            List<float> value = new List<float>();
            try
            {
                label = obj.Select(x => x["stastistic_month"].ToString()).ToList();
                value = obj.Select(x => float.Parse(x["sum_price"].ToString())).ToList();
            }
            catch
            {
            }
            //if()
            //for(int i = 1; i<= ; )
            if(label != null)
            {
                int i = 0;
                foreach(var item in label)
                {
                    values[Convert.ToInt32(item)-1] = value[i];
                    SumPrice += value[i];
                    i++;
                }    
            }
            
            return Json(new { labels,values});
            //var totalProduct = _db.OrderDetails.Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id && x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).ToString();
            //return NotFound();
        }
        [HttpPost]
        public IActionResult ImportExcel(IFormFile files)
        {
            if (files == null)
            {
                return Json(new { success = false, message = "Ko đọc được file" });
            }
               
            List<Product> products = new List<Product>();
            string fileName = "";
            string folderName = "wwwroot\\Media\\";
            string webRootPath = _hostEnvironment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string fullPath = "";
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (files != null )
            {
                fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                fullPath = Path.Combine(newPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    files.CopyTo(stream);
                }
            }
            try
            {
                var dataset = ExcelProvider.Read(fullPath);
                var ExcelTable = dataset.Tables[0];
                foreach (DataRow row in ExcelTable.Rows)
                {

                    var a = row.ItemArray[0].ToString();

                    if (a != "Id")
                    {
                        var p = new Product
                        {
                            Id = row.ItemArray[0].ToString(),
                            Name = row.ItemArray[1].ToString(),
                            Description = row.ItemArray[2].ToString(),
                            ImageUrl = row.ItemArray[3].ToString(),
                            Price = float.Parse(row.ItemArray[4].ToString()),
                            IsConfirm = bool.Parse(row.ItemArray[5].ToString()),
                            Quantity = int.Parse(row.ItemArray[6].ToString()),
                            CategoryId = row.ItemArray[7].ToString(),
                            ShopId = row.ItemArray[8].ToString()

                        };
                        products.Add(p);
                    }
                }
                _db.Products.AddRange(products);
                _db.SaveChanges();
                return Json(new { success = true, message = "Thêm danh sách sản phẩm bằng excel thành công" });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.InnerException.Message });
            }
            
        }
    }
}
