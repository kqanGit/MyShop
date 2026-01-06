/**
 * Seed User_Config
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('User_Config').del();
  await knex('User_Config').insert([
    { setting_id: 1, user_id: 1, per_page: 50, last_module: "Dashboard" },
    { setting_id: 2, user_id: 2, per_page: 20, last_module: "Dashboard" },
    { setting_id: 3, user_id: 3, per_page: 10, last_module: "Dashboard" }
  ]);
};
