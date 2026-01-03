using System;

namespace MyShop_Frontend.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Point { get; set; }
        public string TierName { get; set; }
        // CreateDate is not in the JSON requirement but might be useful or harmless to keep if API returns it. 
        // Plan said: "Ensure CustomerId, FullName, Phone, Address, Point exist". 
        // API struct in prompt: { "customerId", "fullName", "phone", "address", "point", "tierName" }
        // It doesn't explicitly mention CreateDate, but I'll keep it if it's not hurting, or remove it if I want to be strict.
        // User prompt JSON: { "items": [{ "customerId", "fullName", "phone", "address", "point", "tierName" }], ... }
        // I will remove CreateDate to be strictly aligned with the prompt unless I see it's needed elsewhere.
        // Wait, the previous file had CreateDate. I'll remove it to match the prompt exactly or just leave it. 
        // The prompt says "Cập nhật interface/model của Customer để khớp với cấu trúc JSON trả về từ API".
        // Use exact matching.
    }
}
