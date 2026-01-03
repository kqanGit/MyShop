/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries
  await knex('Customer').del()
  await knex('Customer').insert([
    { customer_id: 1, full_name: "Khách Vãng Lai", phone: "0000000000", address: "Tại quầy", point: 0, tier_id: 1, create_date: '2025-10-01', is_removed: false },
    { customer_id: 2, full_name: "Nguyễn Thị Thu", phone: "0912345678", address: "Q. Cầu Giấy, Hà Nội", point: 2500000, tier_id: 2, create_date: '2025-09-15', is_removed: false },
    { customer_id: 3, full_name: "Trần Văn Bình", phone: "0988777666", address: "Q. Bình Thạnh, TP.HCM", point: 12000000, tier_id: 3, create_date: '2025-08-20', is_removed: false },
    { customer_id: 4, full_name: "Lê Thị Hồng", phone: "0905111222", address: "Q. Hải Châu, Đà Nẵng", point: 500, tier_id: 1, create_date: '2025-10-05', is_removed: false },
    { customer_id: 5, full_name: "Phạm Văn Minh", phone: "0933444555", address: "TP. Thủ Đức, TP.HCM", point: 8000000, tier_id: 2, create_date: '2025-09-30', is_removed: false },
    { customer_id: 6, full_name: "Hoàng Thị Lan", phone: "0977888999", address: "TP. Biên Hòa, Đồng Nai", point: 100, tier_id: 1, create_date: '2025-10-10', is_removed: false },
    { customer_id: 7, full_name: "Vũ Đức Thắng", phone: "0911223344", address: "Q. Hoàn Kiếm, Hà Nội", point: 0, tier_id: 1, create_date: '2025-10-12', is_removed: false },
    { customer_id: 8, full_name: "Đặng Thị Thanh", phone: "0966555444", address: "Q. 7, TP.HCM", point: 3000000, tier_id: 1, create_date: '2025-09-25', is_removed: false },
    { customer_id: 9, full_name: "Bùi Văn An", phone: "0888999000", address: "TP. Cần Thơ", point: 15000000, tier_id: 3, create_date: '2025-08-10', is_removed: false },
    { customer_id: 10, full_name: "Ngô Thị Tuyết", phone: "0944555666", address: "TP. Hải Phòng", point: 50, tier_id: 1, create_date: '2025-10-15', is_removed: false }
  ]);
};
