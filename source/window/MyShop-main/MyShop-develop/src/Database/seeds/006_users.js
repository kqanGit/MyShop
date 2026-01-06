/**
 * Seed Users
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function (knex) {
  await knex('User').del();
  await knex('User').insert([
    { user_id: 1, user_name: "admin", password: "$2a$11$ULDpwzA0qloqh3d5JFxKi.vgarmU6aNYuHGkySiqluPVgWIX27blq", full_name: "Nguyễn Thành Đạt", role_id: 1, status: 1 },
    { user_id: 2, user_name: "quanly01", password: "$2a$11$3avCv5NNBPQB.N/rN4NTiuORdcFZW69keNE6r4UbzWg1whl9g8zzS", full_name: "Trần Thu Hà", role_id: 2, status: 1 },
    { user_id: 3, user_name: "staff01", password: "$2a$11$8M9xz4hGfdnVCYcSCigtiebJ0hPis4yuAygzUV8faBps5fKHDPST2", full_name: "Lê Văn Hùng", role_id: 3, status: 1 },
    { user_id: 4, user_name: "staff02", password: "$2a$11$SdBaXKfs25c3lI4JNZ.q9ut4mp.ccPEau6uo0PIhIUQH.H3smhULm", full_name: "Phạm Thị Mai", role_id: 3, status: 1 },
    { user_id: 5, user_name: "staff03", password: "$2a$11$Nhlw61MprUWKeSF88XFPaOhToK10RYiHulXHVgYoJKXLh619o8Pjq", full_name: "Hoàng Văn Nam", role_id: 3, status: 1 },
    { user_id: 6, user_name: "staff04", password: "$2a$11$sQVf.c1q0wbm9IcgAlG0E.M3c59OSAZp3NDQ5zAyoPga1v6AbD7GG", full_name: "Vũ Thị Tuyết", role_id: 3, status: 0 },
    { user_id: 7, user_name: "staff05", password: "$2a$11$U85GsD.gdrtXQh1EcEcTpOvLCXaiaVYhS4/t.7dyoP73eNt0L6nGi", full_name: "Đặng Tiến Dũng", role_id: 3, status: 1 },
    { user_id: 8, user_name: "staff06", password: "$2a$11$CnpsAkkGU.pAWxUyDt7BG.zeIpB38sTPxrzZBaEiCeI6j4944I3t6", full_name: "Bùi Thị Lan", role_id: 3, status: 1 },
    { user_id: 9, user_name: "staff07", password: "$2a$11$nKLkH8zsYYtGPmdkWAJZOutp./IGXGVvtRd8zlFw3PqKyn1Ygzanq", full_name: "Ngô Văn Quyền", role_id: 3, status: 1 },
    { user_id: 10, user_name: "staff08", password: "$2a$11$nqCWw6XDSVNwAZu4otmxhuH1kna1I3Q2EAPrD9HepEkWmiXCF7V62", full_name: "Đỗ Thị Bích", role_id: 3, status: 1 },
    // Custom Users
    { user_id: 11, user_name: "01@gmail.com", password: "$2b$11$yMzWTEycwoHwtIx3tr36vORHEQBiETZyYZvjTBAn1KOzduDRPW6vy", full_name: "Admin User", role_id: 1, status: 1 },
    { user_id: 12, user_name: "02@gmail.com", password: "$2b$11$yMzWTEycwoHwtIx3tr36vORHEQBiETZyYZvjTBAn1KOzduDRPW6vy", full_name: "Manager User", role_id: 2, status: 1 },
    { user_id: 13, user_name: "03@gmail.com", password: "$2b$11$yMzWTEycwoHwtIx3tr36vORHEQBiETZyYZvjTBAn1KOzduDRPW6vy", full_name: "Staff User", role_id: 3, status: 1 }
  ]);
};
