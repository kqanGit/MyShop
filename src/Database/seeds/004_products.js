/**
 * Seed Products
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Product').del();
  await knex('Product').insert([
    { product_id: 0, product_name: "Sản phẩm mẫu (không bán)", category_id: 1, unit: "Cái", cost: 0, price: 0, stock: 0, is_removed: 1 },
    
    // =========================================================================
    // CATEGORY 1: THỰC PHẨM & ĐỒ UỐNG
    // =========================================================================
    { product_id: 1, product_name: "Snack Oishi Tôm Cay", category_id: 1, unit: "Gói", cost: 4000, price: 6000, stock: 100, is_removed: false },
    { product_id: 2, product_name: "Bánh ChocoPie Orion (Hộp 6P)", category_id: 1, unit: "Hộp", cost: 28000, price: 35000, stock: 40, is_removed: false },
    { product_id: 3, product_name: "Kẹo Dẻo Chip Chip Hải Hà", category_id: 1, unit: "Gói", cost: 15000, price: 20000, stock: 60, is_removed: false },
    { product_id: 4, product_name: "Rong Biển Sấy Khô", category_id: 1, unit: "Lốc", cost: 22000, price: 30000, stock: 25, is_removed: false },
    { product_id: 5, product_name: "Nước Ngọt Coca Cola 1.5L", category_id: 1, unit: "Chai", cost: 16000, price: 20000, stock: 50, is_removed: false },
    { product_id: 6, product_name: "Bia Heineken Silver (Lon)", category_id: 1, unit: "Lon", cost: 17500, price: 21000, stock: 240, is_removed: false },
    { product_id: 7, product_name: "Trà Ô Long TEA+ Plus", category_id: 1, unit: "Chai", cost: 7000, price: 10000, stock: 35, is_removed: false },
    { product_id: 8, product_name: "Mì Hảo Hảo Tôm Chua Cay", category_id: 1, unit: "Gói", cost: 3800, price: 4500, stock: 500, is_removed: false },
    { product_id: 9, product_name: "Nước Mắm Nam Ngư 750ml", category_id: 1, unit: "Chai", cost: 38000, price: 48000, stock: 30, is_removed: false },
    { product_id: 10, product_name: "Hạt Nêm Knorr Thịt Thăn 400g", category_id: 1, unit: "Gói", cost: 32000, price: 40000, stock: 20, is_removed: false },
    { product_id: 17, product_name: "Sữa Tươi Tiệt Trùng Vinamilk 1L", category_id: 1, unit: "Hộp", cost: 28000, price: 34000, stock: 4, is_removed: false },
    { product_id: 18, product_name: "Lốc 4 Hộp Sữa Chua Nha Đam", category_id: 1, unit: "Lốc", cost: 24000, price: 30000, stock: 20, is_removed: false },
    { product_id: 19, product_name: "Phô Mai Con Bò Cười (Hộp 8 miếng)", category_id: 1, unit: "Hộp", cost: 30000, price: 38000, stock: 15, is_removed: false },
    { product_id: 23, product_name: "Dầu Ăn Tường An 1L", category_id: 1, unit: "Chai", cost: 42000, price: 55000, stock: 40, is_removed: false },
    { product_id: 24, product_name: "Gạo ST25 Ông Cua (5kg)", category_id: 1, unit: "Túi", cost: 150000, price: 190000, stock: 10, is_removed: false },
    { product_id: 25, product_name: "Tương Ớt Chin-su 250g", category_id: 1, unit: "Chai", cost: 12000, price: 15000, stock: 60, is_removed: false },
    { product_id: 26, product_name: "Xúc Xích Vissan (Gói 5 cây)", category_id: 1, unit: "Gói", cost: 18000, price: 25000, stock: 25, is_removed: false },
    { product_id: 27, product_name: "Mì Omachi Xốt Bò Hầm", category_id: 1, unit: "Gói", cost: 7000, price: 9000, stock: 100, is_removed: false },
    { product_id: 28, product_name: "Cà Phê G7 3in1 (Hộp 21 gói)", category_id: 1, unit: "Hộp", cost: 48000, price: 58000, stock: 30, is_removed: false },
    { product_id: 29, product_name: "Bia 333 (Thùng 24 lon)", category_id: 1, unit: "Thùng", cost: 240000, price: 270000, stock: 10, is_removed: false },
    { product_id: 30, product_name: "Sữa Đặc Ông Thọ Lon", category_id: 1, unit: "Lon", cost: 22000, price: 28000, stock: 45, is_removed: false },
    { product_id: 31, product_name: "Nước Suối Aquafina 500ml", category_id: 1, unit: "Chai", cost: 4000, price: 6000, stock: 120, is_removed: false },

    // =========================================================================
    // CATEGORY 2: HÓA PHẨM & CHĂM SÓC CÁ NHÂN
    // =========================================================================
    { product_id: 11, product_name: "Nước Rửa Chén Sunlight Chanh 750g", category_id: 2, unit: "Chai", cost: 22000, price: 28000, stock: 45, is_removed: false },
    { product_id: 12, product_name: "Nước Lau Sàn Gift Bạc Hà 1L", category_id: 2, unit: "Can", cost: 25000, price: 32000, stock: 15, is_removed: false },
    { product_id: 13, product_name: "Bột Giặt OMO Matic Cửa Trên 2kg", category_id: 2, unit: "Túi", cost: 85000, price: 105000, stock: 10, is_removed: false },
    { product_id: 14, product_name: "Dầu Gội Head & Shoulders Bạc Hà", category_id: 2, unit: "Chai", cost: 110000, price: 135000, stock: 12, is_removed: false },
    { product_id: 15, product_name: "Kem Đánh Răng Colgate Ngừa Sâu Răng", category_id: 2, unit: "Hộp", cost: 28000, price: 36000, stock: 40, is_removed: false },
    { product_id: 16, product_name: "Sữa Tắm Nam X-Men Wood", category_id: 2, unit: "Chai", cost: 130000, price: 160000, stock: 8, is_removed: false },
    { product_id: 32, product_name: "Nước Xả Vải Downy Huyền Bí 1.5L", category_id: 2, unit: "Túi", cost: 125000, price: 160000, stock: 15, is_removed: false },
    { product_id: 33, product_name: "Nước Lau Kính Gift 580ml", category_id: 2, unit: "Chai", cost: 18000, price: 25000, stock: 20, is_removed: false },
    { product_id: 34, product_name: "Nước Tẩy Toilet Vim Xanh", category_id: 2, unit: "Chai", cost: 35000, price: 45000, stock: 25, is_removed: false },
    { product_id: 35, product_name: "Sữa Rửa Mặt Pond's Hồng", category_id: 2, unit: "Tuýp", cost: 65000, price: 85000, stock: 10, is_removed: false },
    { product_id: 36, product_name: "Bông Tẩy Trang Jomi (Túi)", category_id: 2, unit: "Túi", cost: 30000, price: 40000, stock: 30, is_removed: false },
    { product_id: 37, product_name: "Dung Dịch Vệ Sinh Dạ Hương", category_id: 2, unit: "Chai", cost: 25000, price: 35000, stock: 20, is_removed: false },
    { product_id: 38, product_name: "Bàn Chải Đánh Răng P/S", category_id: 2, unit: "Cây", cost: 15000, price: 25000, stock: 50, is_removed: false },
    { product_id: 39, product_name: "Lăn Khử Mùi Nivea Men", category_id: 2, unit: "Chai", cost: 45000, price: 60000, stock: 12, is_removed: false },
    { product_id: 40, product_name: "Dầu Xả Sunsilk Óng Mượt", category_id: 2, unit: "Chai", cost: 80000, price: 100000, stock: 8, is_removed: false },
    { product_id: 41, product_name: "Sáp Vuốt Tóc X-Men", category_id: 2, unit: "Hộp", cost: 55000, price: 75000, stock: 15, is_removed: false },
    { product_id: 42, product_name: "Dao Cạo Râu Gillette", category_id: 2, unit: "Cây", cost: 15000, price: 25000, stock: 40, is_removed: false },
    { product_id: 43, product_name: "Khăn Mặt Cotton Cao Cấp", category_id: 2, unit: "Cái", cost: 20000, price: 35000, stock: 30, is_removed: false },
    { product_id: 44, product_name: "Nước Rửa Tay Lifebuoy", category_id: 2, unit: "Chai", cost: 60000, price: 75000, stock: 18, is_removed: false },
    { product_id: 45, product_name: "Băng Vệ Sinh Diana Siêu Thấm", category_id: 2, unit: "Gói", cost: 18000, price: 24000, stock: 50, is_removed: false },
    { product_id: 46, product_name: "Xà Bông Cục Lifebuoy", category_id: 2, unit: "Cục", cost: 12000, price: 16000, stock: 60, is_removed: false },
    { product_id: 47, product_name: "Sữa Tắm Enchanteur Vàng", category_id: 2, unit: "Chai", cost: 140000, price: 180000, stock: 5, is_removed: false },

    // =========================================================================
    // CATEGORY 3: ĐỒ DÙNG GIA ĐÌNH
    // =========================================================================
    { product_id: 20, product_name: "Giấy Vệ Sinh E'mos Classic (10 cuộn)", category_id: 3, unit: "Lốc", cost: 60000, price: 75000, stock: 30, is_removed: false },
    { product_id: 21, product_name: "Khăn Ướt Baby Care 80 tờ", category_id: 3, unit: "Gói", cost: 15000, price: 22000, stock: 50, is_removed: false },
    { product_id: 22, product_name: "Vỉ 4 Pin Tiểu AA Panasonic", category_id: 3, unit: "Vỉ", cost: 12000, price: 18000, stock: 2, is_removed: false },
    { product_id: 48, product_name: "Màng Bọc Thực Phẩm Ringo", category_id: 3, unit: "Hộp", cost: 20000, price: 30000, stock: 25, is_removed: false },
    { product_id: 49, product_name: "Giấy Bạc Nướng Thực Phẩm", category_id: 3, unit: "Cuộn", cost: 22000, price: 32000, stock: 20, is_removed: false },
    { product_id: 50, product_name: "Túi Đựng Rác 3 Màu (1kg)", category_id: 3, unit: "Kg", cost: 25000, price: 35000, stock: 40, is_removed: false },
    { product_id: 51, product_name: "Bóng Đèn LED Điện Quang 20W", category_id: 3, unit: "Cái", cost: 50000, price: 75000, stock: 15, is_removed: false },
    { product_id: 52, product_name: "Ổ Cắm Điện Quang 3 Lỗ 5m", category_id: 3, unit: "Cái", cost: 80000, price: 110000, stock: 10, is_removed: false },
    { product_id: 53, product_name: "Pin Tiểu AAA Con Ó (Vỉ 4 viên)", category_id: 3, unit: "Vỉ", cost: 8000, price: 12000, stock: 60, is_removed: false },
    { product_id: 54, product_name: "Chổi Quét Nhà Cỏ Bông", category_id: 3, unit: "Cây", cost: 30000, price: 45000, stock: 20, is_removed: false },
    { product_id: 55, product_name: "Bộ Cây Lau Nhà 360 Độ", category_id: 3, unit: "Bộ", cost: 250000, price: 350000, stock: 5, is_removed: false },
    { product_id: 56, product_name: "Móc Áo Nhôm (Lốc 10 cái)", category_id: 3, unit: "Lốc", cost: 25000, price: 35000, stock: 30, is_removed: false },
    { product_id: 57, product_name: "Kẹp Quần Áo Nhựa (Vỉ 20 cái)", category_id: 3, unit: "Vỉ", cost: 15000, price: 22000, stock: 40, is_removed: false },
    { product_id: 58, product_name: "Găng Tay Cao Su Rửa Chén", category_id: 3, unit: "Đôi", cost: 18000, price: 25000, stock: 25, is_removed: false },
    { product_id: 59, product_name: "Bùi Nhùi Rửa Chén Inox", category_id: 3, unit: "Cái", cost: 5000, price: 8000, stock: 50, is_removed: false },
    { product_id: 60, product_name: "Hộp Đựng Thực Phẩm Nhựa 1L", category_id: 3, unit: "Cái", cost: 35000, price: 50000, stock: 20, is_removed: false },
    { product_id: 61, product_name: "Bình Nước Học Sinh 500ml", category_id: 3, unit: "Cái", cost: 40000, price: 60000, stock: 15, is_removed: false },
    { product_id: 62, product_name: "Ca Nhựa Tắm", category_id: 3, unit: "Cái", cost: 10000, price: 15000, stock: 30, is_removed: false },
    { product_id: 63, product_name: "Thau Nhựa Rửa Rau", category_id: 3, unit: "Cái", cost: 25000, price: 40000, stock: 20, is_removed: false },
    { product_id: 64, product_name: "Ghế Nhựa Thấp", category_id: 3, unit: "Cái", cost: 30000, price: 45000, stock: 25, is_removed: false },
    { product_id: 65, product_name: "Dép Xốp Đi Trong Nhà", category_id: 3, unit: "Đôi", cost: 35000, price: 50000, stock: 20, is_removed: false },
    { product_id: 66, product_name: "Ủng Đi Mưa Nilon", category_id: 3, unit: "Đôi", cost: 15000, price: 25000, stock: 50, is_removed: false }
  ]);
};
