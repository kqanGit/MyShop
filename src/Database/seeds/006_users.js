/**
 * Seed Users
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('User').del();
  await knex('User').insert([
    { user_id: 1, user_name: "admin", password: "adminpassword", full_name: "Nguyễn Thành Đạt", role_id: 1, status: 1 },
    { user_id: 2, user_name: "quanly01", password: "managerpassword", full_name: "Trần Thu Hà", role_id: 2, status: 1 },
    { user_id: 3, user_name: "staff01", password: "staffpassword1", full_name: "Lê Văn Hùng", role_id: 3, status: 1 },
    { user_id: 4, user_name: "staff02", password: "staffpassword2", full_name: "Phạm Thị Mai", role_id: 3, status: 1 },
    { user_id: 5, user_name: "staff03", password: "staffpassword3", full_name: "Hoàng Văn Nam", role_id: 3, status: 1 },
    { user_id: 6, user_name: "staff04", password: "staffpassword4", full_name: "Vũ Thị Tuyết", role_id: 3, status: 0 }, // Đã nghỉ
    { user_id: 7, user_name: "staff05", password: "staffpassword5", full_name: "Đặng Tiến Dũng", role_id: 3, status: 1 },
    { user_id: 8, user_name: "staff06", password: "staffpassword6", full_name: "Bùi Thị Lan", role_id: 3, status: 1 },
    { user_id: 9, user_name: "staff07", password: "staffpassword7", full_name: "Ngô Văn Quyền", role_id: 3, status: 1 },
    { user_id: 10, user_name: "staff08", password: "staffpassword8", full_name: "Đỗ Thị Bích", role_id: 3, status: 1 }
  ]);
};
