/**
 * Seed Roles
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Role').del();
  await knex('Role').insert([
    { role_id: 1, role_name: 'Administrator'},
    { role_id: 2, role_name: 'Manager'},
    { role_id: 3, role_name: 'Saler'}
  ]);
};
