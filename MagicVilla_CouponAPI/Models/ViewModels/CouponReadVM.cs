﻿namespace MagicVilla_CouponAPI.Models.ViewModels
{
    public class CouponReadVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
