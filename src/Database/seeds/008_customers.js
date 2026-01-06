/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries
  await knex('Customer').del()
  await knex('Customer').insert([
    { customer_id: 1, full_name: "Trần Quỳnh Như", phone: "0912132311", address: "Trà Vinh", point: 0, tier_id: 1, create_date: '2025-10-01', is_removed: false },
    { customer_id: 2, full_name: "Nguyễn Thị Thu", phone: "0912345678", address: "Q. Cầu Giấy, Hà Nội", point: 2500000, tier_id: 2, create_date: '2025-09-15', is_removed: false },
    { customer_id: 3, full_name: "Trần Văn Bình", phone: "0988777666", address: "Q. Bình Thạnh, TP.HCM", point: 12000000, tier_id: 3, create_date: '2025-08-20', is_removed: false },
    { customer_id: 4, full_name: "Lê Thị Hồng", phone: "0905111222", address: "Q. Hải Châu, Đà Nẵng", point: 500, tier_id: 1, create_date: '2025-10-05', is_removed: false },
    { customer_id: 5, full_name: "Phạm Văn Minh", phone: "0933444555", address: "TP. Thủ Đức, TP.HCM", point: 8000000, tier_id: 2, create_date: '2025-09-30', is_removed: false },
    { customer_id: 6, full_name: "Hoàng Thị Lan", phone: "0977888999", address: "TP. Biên Hòa, Đồng Nai", point: 100, tier_id: 1, create_date: '2025-10-10', is_removed: false },
    { customer_id: 7, full_name: "Vũ Đức Thắng", phone: "0911223344", address: "Q. Hoàn Kiếm, Hà Nội", point: 0, tier_id: 1, create_date: '2025-10-12', is_removed: false },
    { customer_id: 8, full_name: "Đặng Thị Thanh", phone: "0966555444", address: "Q. 7, TP.HCM", point: 3000000, tier_id: 1, create_date: '2025-09-25', is_removed: false },
    { customer_id: 9, full_name: "Bùi Văn An", phone: "0888999000", address: "TP. Cần Thơ", point: 15000000, tier_id: 3, create_date: '2025-08-10', is_removed: false },
    { customer_id: 10, full_name: "Ngô Thị Tuyết", phone: "0944555666", address: "TP. Hải Phòng", point: 50, tier_id: 1, create_date: '2025-10-15', is_removed: false },
    { customer_id: 11, full_name: "Phạm Thành Nam", phone: "0912111222", address: "Số 12 Chùa Bộc, Đống Đa, Hà Nội", point: 4500000, tier_id: 2, create_date: '2025-10-18', is_removed: false },
    { customer_id: 12, full_name: "Nguyễn Mai Phương", phone: "0983445566", address: "Khu đô thị Sala, Quận 2, TP.HCM", point: 20000000, tier_id: 4, create_date: '2025-10-20', is_removed: false },
    { customer_id: 13, full_name: "Trần Thế Vinh", phone: "0909778899", address: "156 Trần Hưng Đạo, Quận 1, TP.HCM", point: 1500, tier_id: 1, create_date: '2025-10-21', is_removed: false },
    { customer_id: 14, full_name: "Lê Ngọc Hân", phone: "0971000222", address: "45 Lê Lợi, TP. Huế, Thừa Thiên Huế", point: 9500000, tier_id: 3, create_date: '2025-10-22', is_removed: false },
    { customer_id: 15, full_name: "Đỗ Minh Quân", phone: "0932555777", address: "Ngõ 102 Khuất Duy Tiến, Thanh Xuân, Hà Nội", point: 300, tier_id: 1, create_date: '2025-10-25', is_removed: false },
    { customer_id: 16, full_name: "Võ Thị Sáu", phone: "0966444111", address: "22 Hùng Vương, TP. Vũng Tàu, Bà Rịa - Vũng Tàu", point: 11000000, tier_id: 3, create_date: '2025-10-26', is_removed: false },
    { customer_id: 17, full_name: "Bùi Xuân Huấn", phone: "0988222333", address: "Phố Mới, Quế Võ, Bắc Ninh", point: 55000000, tier_id: 4, create_date: '2025-10-28', is_removed: false },
    { customer_id: 18, full_name: "Lý Gia Thành", phone: "0911999888", address: "Chợ Lớn, Quận 5, TP.HCM", point: 7200000, tier_id: 2, create_date: '2025-10-30', is_removed: false },
    { customer_id: 19, full_name: "Đặng Thu Thảo", phone: "0945123456", address: "89 Nguyễn Văn Cừ, Quận Ninh Kiều, Cần Thơ", point: 200000, tier_id: 1, create_date: '2025-11-01', is_removed: false },
    { customer_id: 20, full_name: "Trương Mỹ Linh", phone: "0902666888", address: "Vinhomes Central Park, Bình Thạnh, TP.HCM", point: 18000000, tier_id: 4, create_date: '2025-11-02', is_removed: false },
    { customer_id: 21, full_name: "Phan Văn Khải", phone: "0977333555", address: "Đường 30/4, Quận Hải Châu, Đà Nẵng", point: 3200000, tier_id: 2, create_date: '2025-11-05', is_removed: false },
    { customer_id: 22, full_name: "Hồ Anh Tuấn", phone: "0915888777", address: "Làng Sen, Nam Đàn, Nghệ An", point: 50, tier_id: 1, create_date: '2025-11-07', is_removed: false },
    { customer_id: 23, full_name: "Ngô Bảo Châu", phone: "0982444999", address: "Viện Toán Học, Cầu Giấy, Hà Nội", point: 12500000, tier_id: 3, create_date: '2025-11-08', is_removed: false },
    { customer_id: 24, full_name: "Đinh Tiến Dũng", phone: "0934111000", address: "12 Lạch Tray, Ngô Quyền, Hải Phòng", point: 4100000, tier_id: 2, create_date: '2025-11-10', is_removed: false },
    { customer_id: 25, full_name: "Tạ Đình Phong", phone: "0903888222", address: "Khu dân cư Trung Sơn, Bình Chánh, TP.HCM", point: 0, tier_id: 1, create_date: '2025-11-12', is_removed: false },
    { customer_id: 26, full_name: "Quách Thị An", phone: "0969121212", address: "Thị trấn Sa Pa, Lào Cai", point: 8800000, tier_id: 2, create_date: '2025-11-15', is_removed: false },
    { customer_id: 27, full_name: "Dương Văn Đức", phone: "0888555666", address: "55 Kim Mã, Ba Đình, Hà Nội", point: 35000000, tier_id: 4, create_date: '2025-11-16', is_removed: false },
    { customer_id: 28, full_name: "Lương Bích Hữu", phone: "0944000999", address: "Sư Vạn Hạnh, Quận 10, TP.HCM", point: 15000000, tier_id: 3, create_date: '2025-11-18', is_removed: false },
    { customer_id: 29, full_name: "Đoàn Văn Hậu", phone: "0972333444", address: "Hưng Hà, Thái Bình", point: 6000000, tier_id: 2, create_date: '2025-11-20', is_removed: false },
    { customer_id: 30, full_name: "Cao Thái Sơn", phone: "0912777333", address: "Phan Xích Long, Phú Nhuận, TP.HCM", point: 100, tier_id: 1, create_date: '2025-11-22', is_removed: false }
  ]);
};
