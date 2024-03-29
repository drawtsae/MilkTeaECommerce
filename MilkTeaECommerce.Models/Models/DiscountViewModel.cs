﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class DiscountViewModel
    {
        public string Id { get; set; }
        [DisplayName("Tên")]
        [Required(ErrorMessage = "Phải nhập tên cho Khuyến mãi")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Phải nhập mô tả cho Khuyến mãi")]
        public string Description { get; set; }

        [DisplayName("Ngày bắt đầu")]
        public DateTime? DateStart { get; set; }

        [DisplayName("Ngày kết thúc")]
        public DateTime? DateExpired { get; set; }

        public int? TimeUsed { get; set; }
        [DisplayName("Số lượng khuyến mãi")]
        [Required(ErrorMessage = "Phải nhập số lượng khuyến mãi")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Dữ liệu không hợp lệ")]
        public int? TimesUseLimit { get; set; }
       

        [DisplayName("Phần trăm giảm")]
        [Required(ErrorMessage = "Phải nhập số phần trăm khuyến mãi")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Dữ liệu không hợp lệ")]
        public int? PercentDiscount { get; set; }
        [DisplayName("Giảm tối đa (VND)")]
        [Required(ErrorMessage = "Phải nhập giá giảm tối đa")]
        [Range(minimum: 0, maximum: 1000000000, ErrorMessage = "Dữ liệu không hợp lệ")]
        public float? MaxDiscount { get; set; }
        [Required(ErrorMessage = "Phải nhập Code cho Khuyến mãi")]
        public string Code { get; set; }

        public  List<string> CategoryDiscounts { get; set; }
        public  List<string> DeliveryDiscounts { get; set; }
        public  List<string> ProductDiscounts { get; set; }
        public DiscountViewModel()
        {
            CategoryDiscounts = new List<string>();
            DeliveryDiscounts = new List<string>();
            ProductDiscounts = new List<string>();
        }


    }
}
