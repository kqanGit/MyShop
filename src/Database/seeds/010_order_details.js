/**
 * Seed OrderDetail
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('OrderDetail').del();
  await knex('OrderDetail').insert([
    { 
      order_id: 1, 
      product_id: 6, // Bia Heineken
      quantity: 24,  // 1 thùng (24 lon)
      current_cost: 17500, 
      current_price: 21000, 
      total_line: 504000 
    },
    { 
      order_id: 1, 
      product_id: 13, // Bột giặt OMO 2kg
      quantity: 1, 
      current_cost: 85000, 
      current_price: 105000, 
      total_line: 105000 
    },
    { 
      order_id: 1, 
      product_id: 16, // Sữa tắm X-Men
      quantity: 1, 
      current_cost: 130000, 
      current_price: 160000, 
      total_line: 160000 
    },
    { 
      order_id: 1, 
      product_id: 20, // Giấy vệ sinh E'mos
      quantity: 2,    // 2 lốc
      current_cost: 60000, 
      current_price: 75000, 
      total_line: 150000 
    },
    { 
      order_id: 1, 
      product_id: 17, // Sữa Vinamilk 1L
      quantity: 5, 
      current_cost: 28000, 
      current_price: 34000, 
      total_line: 170000 
    },

    // --- Chi tiết Đơn 2 (Tổng 226.000đ) - Đồ ăn & Gia vị ---
    { 
      order_id: 2, 
      product_id: 18, // Sữa chua Nha Đam
      quantity: 2,    // 2 lốc
      current_cost: 24000, 
      current_price: 30000, 
      total_line: 60000 
    },
    { 
      order_id: 2, 
      product_id: 9, // Nước mắm Nam Ngư
      quantity: 2,   // 2 chai
      current_cost: 38000, 
      current_price: 48000, 
      total_line: 96000 
    },
    { 
      order_id: 2, 
      product_id: 5, // Coca Cola 1.5L
      quantity: 2, 
      current_cost: 16000, 
      current_price: 20000, 
      total_line: 40000 
    },
    { 
      order_id: 2, 
      product_id: 1, // Snack Oishi
      quantity: 5, 
      current_cost: 4000, 
      current_price: 6000, 
      total_line: 30000 
    },

    // --- Chi tiết Đơn 3 (Tổng 60.000đ) - Đồ lặt vặt ---
    { 
      order_id: 3, 
      product_id: 22, // Pin AA Panasonic
      quantity: 1, 
      current_cost: 12000, 
      current_price: 18000, 
      total_line: 18000 
    },
    { 
      order_id: 3, 
      product_id: 21, // Khăn ướt Baby Care
      quantity: 1, 
      current_cost: 15000, 
      current_price: 22000, 
      total_line: 22000 
    },
    { 
      order_id: 3, 
      product_id: 7, // Trà Ô Long
      quantity: 2, 
      current_cost: 7000, 
      current_price: 10000, 
      total_line: 20000 
    }
  ]);
};
