/**
 * Seed Membership tiers
 */

/** @param { import("knex").Knex } knex */
exports.seed = async function(knex) {
  await knex('Membership').del();
  await knex('Membership').insert([
    { tier_id: 1, tier_name: 'Bronze', threshold: 0, discount: 0},
    { tier_id: 2, tier_name: 'Silver', threshold: 5000000, discount: 0.05},
    { tier_id: 3, tier_name: 'Gold', threshold: 10000000, discount: 0.07},
    { tier_id: 4, tier_name: 'Platinum', threshold: 15000000, discount: 0.1}
  ]);
};
