/**
 * Seed Products
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Product').del();
  await knex('Product').insert([
    { product_id: 1, product_name: "Snack Oishi Tôm Cay", category_id: 1, unit: "Gói", cost: 4000, price: 6000, stock: 100, is_removed: 0 },
    { product_id: 2, product_name: "Bánh ChocoPie Orion (Hộp 6P)", category_id: 1, unit: "Hộp", cost: 28000, price: 35000, stock: 40, is_removed: 0 },
    { product_id: 3, product_name: "Kẹo Dẻo Chip Chip Hải Hà", category_id: 1, unit: "Gói", cost: 15000, price: 20000, stock: 60, is_removed: 0 },
    { product_id: 4, product_name: "Rong Biển Sấy Khô", category_id: 1, unit: "Lốc", cost: 22000, price: 30000, stock: 25, is_removed: 0 },

    // --- Category 2: Nước Giải Khát & Bia Rượu (3 items) ---
    { product_id: 5, product_name: "Nước Ngọt Coca Cola 1.5L", category_id: 2, unit: "Chai", cost: 16000, price: 20000, stock: 50, is_removed: 0 },
    { product_id: 6, product_name: "Bia Heineken Silver (Lon)", category_id: 2, unit: "Lon", cost: 17500, price: 21000, stock: 240, is_removed: 0 },
    { product_id: 7, product_name: "Trà Ô Long TEA+ Plus", category_id: 2, unit: "Chai", cost: 7000, price: 10000, stock: 35, is_removed: 0 },

    // --- Category 3: Gia Vị & Thực Phẩm Khô (3 items) ---
    { product_id: 8, product_name: "Mì Hảo Hảo Tôm Chua Cay", category_id: 3, unit: "Gói", cost: 3800, price: 4500, stock: 500, is_removed: 0 },
    { product_id: 9, product_name: "Nước Mắm Nam Ngư 750ml", category_id: 3, unit: "Chai", cost: 38000, price: 48000, stock: 30, is_removed: 0 },
    { product_id: 10, product_name: "Hạt Nêm Knorr Thịt Thăn 400g", category_id: 3, unit: "Gói", cost: 32000, price: 40000, stock: 20, is_removed: 0 },

    // --- Category 4: Hóa Phẩm & Tẩy Rửa (3 items) ---
    { product_id: 11, product_name: "Nước Rửa Chén Sunlight Chanh 750g", category_id: 4, unit: "Chai", cost: 22000, price: 28000, stock: 45, is_removed: 0 },
    { product_id: 12, product_name: "Nước Lau Sàn Gift Bạc Hà 1L", category_id: 4, unit: "Can", cost: 25000, price: 32000, stock: 15, is_removed: 0 },
    { product_id: 13, product_name: "Bột Giặt OMO Matic Cửa Trên 2kg", category_id: 4, unit: "Túi", cost: 85000, price: 105000, stock: 10, is_removed: 0 },

    // --- Category 5: Chăm Sóc Cá Nhân (3 items) ---
    { product_id: 14, product_name: "Dầu Gội Head & Shoulders Bạc Hà", category_id: 5, unit: "Chai", cost: 110000, price: 135000, stock: 12, is_removed: 0 },
    { product_id: 15, product_name: "Kem Đánh Răng Colgate Ngừa Sâu Răng", category_id: 5, unit: "Hộp", cost: 28000, price: 36000, stock: 40, is_removed: 0 },
    { product_id: 16, product_name: "Sữa Tắm Nam X-Men Wood", category_id: 5, unit: "Chai", cost: 130000, price: 160000, stock: 8, is_removed: 0 },

    // --- Category 6: Sữa & Chế Phẩm Từ Sữa (3 items) ---
    { product_id: 17, product_name: "Sữa Tươi Tiệt Trùng Vinamilk 1L", category_id: 6, unit: "Hộp", cost: 28000, price: 34000, stock: 4, is_removed: 0 }, // Sắp hết
    { product_id: 18, product_name: "Lốc 4 Hộp Sữa Chua Nha Đam", category_id: 6, unit: "Lốc", cost: 24000, price: 30000, stock: 20, is_removed: 0 },
    { product_id: 19, product_name: "Phô Mai Con Bò Cười (Hộp 8 miếng)", category_id: 6, unit: "Hộp", cost: 30000, price: 38000, stock: 15, is_removed: 0 },

    // --- Category 7: Giấy & Đồ Dùng Gia Đình (3 items) ---
    { product_id: 20, product_name: "Giấy Vệ Sinh E'mos Classic (10 cuộn)", category_id: 7, unit: "Lốc", cost: 60000, price: 75000, stock: 30, is_removed: 0 },
    { product_id: 21, product_name: "Khăn Ướt Baby Care 80 tờ", category_id: 7, unit: "Gói", cost: 15000, price: 22000, stock: 50, is_removed: 0 },
    { product_id: 22, product_name: "Vỉ 4 Pin Tiểu AA Panasonic", category_id: 7, unit: "Vỉ", cost: 12000, price: 18000, stock: 2, is_removed: 0 } // Sắp hết
  ]);
};
