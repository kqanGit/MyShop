/**
 * Seed Orders (minimal)
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function (knex) {
  await knex('Order').del();
  await knex('Order').insert([
    {
      order_id: 1,
      order_code: "DH001",
      customer_id: 3, // Khách Vàng
      user_id: 3,     // Staff
      order_date: "2025-10-20T09:30:00",
      total_price: 1089000,
      voucher_id: 3,  // Giảm 50k cho đơn > 1tr
      discount_amount: 50000,
      final_price: 1039000,
      status: 2
    },
    {
      order_id: 2,
      order_code: "DH002",
      customer_id: 2, // Khách Bạc
      user_id: 4,     // Staff
      order_date: "2025-10-21T14:15:00",
      total_price: 226000,
      voucher_id: 2,  // Giảm 5%
      discount_amount: 11300,
      final_price: 214700,
      status: 2
    },
    {
      order_id: 3,
      order_code: "DH003",
      customer_id: 1, // Khách vãng lai
      user_id: 3,     // Staff
      order_date: "2025-10-21T16:00:00",
      total_price: 60000,
      voucher_id: null,
      discount_amount: 0,
      final_price: 60000,
      status: 2
      
    }
  ]);
};
