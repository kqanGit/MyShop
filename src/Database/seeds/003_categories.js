/**
 * Seed Categories
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Category').del();
  await knex('Category').insert([
    { category_id: 1, category_name: "Bánh Kẹo & Đồ Ăn Vặt" },
    { category_id: 2, category_name: "Nước Giải Khát & Bia Rượu" },
    { category_id: 3, category_name: "Gia Vị & Thực Phẩm Khô" },
    { category_id: 4, category_name: "Hóa Phẩm & Tẩy Rửa" },
    { category_id: 5, category_name: "Chăm Sóc Cá Nhân" },
    { category_id: 6, category_name: "Sữa & Chế Phẩm Từ Sữa" },
    { category_id: 7, category_name: "Giấy & Đồ Dùng Gia Đình" }
  ]);
};
