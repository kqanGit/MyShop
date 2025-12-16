/**
 * Clear child tables before parent tables to avoid FK errors
 * Run: `npx knex seed:run --knexfile knexfile.js --env development`
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  // Use a transaction and delete in child-first order
  await knex.transaction(async trx => {
    await trx('User_Config').del();
    await trx('OrderDetail').del();
    await trx('Order').del();
    await trx('Product').del();
    await trx('Customer').del();
    await trx('User').del();
    await trx('Voucher').del();
    await trx('Category').del();
    await trx('Membership').del();
    await trx('Role').del();
  });
};
