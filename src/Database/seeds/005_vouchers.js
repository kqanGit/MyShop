/**
 * Seed Vouchers
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Voucher').del();
  await knex('Voucher').insert([
    { voucher_id: 1, voucher_code: "CHAOBANMOI", description: "Giảm 10k cho đơn đầu tiên", type: 2, discount: 10000, min_threshold: 50000, start_date: '2024-01-01', end_date: '2024-12-31', is_removed: 1 },
    { voucher_id: 2, voucher_code: "CUOITUAN5", description: "Giảm 5% dịp cuối tuần", type: 1, discount: 5, min_threshold: 200000, start_date: '2024-01-01', end_date: '2024-12-31', is_removed: 1 },
    { voucher_id: 3, voucher_code: "VIPDAY", description: "Giảm 50k cho hóa đơn trên 1 triệu", type: 2, discount: 50000, min_threshold: 1000000, start_date: '2024-01-01', end_date: '2024-12-31', is_removed: 1 }
  ]);
};
