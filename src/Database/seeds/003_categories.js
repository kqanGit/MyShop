/**
 * Seed Categories
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Category').del();
  await knex('Category').insert([
    { category_id: 1, category_name: "Thực phẩm & Đồ uống" },       // Gộp: Bánh kẹo, Nước, Gia vị, Sữa
    { category_id: 2, category_name: "Hóa phẩm & Chăm sóc cá nhân" }, // Gộp: Tẩy rửa, Chăm sóc cá nhân
    { category_id: 3, category_name: "Đồ dùng gia đình" }             // Gộp: Giấy, Đồ dùng, Tiện ích
  ]);
};
