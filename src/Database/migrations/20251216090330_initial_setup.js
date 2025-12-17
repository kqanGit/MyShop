/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function (knex) {
    await knex.raw(`
        CREATE TABLE "Role" (
        "role_id" int PRIMARY KEY,
        "role_name" varchar
        );

        CREATE TABLE "User" (
        "user_id" int PRIMARY KEY,
        "user_name" varchar,
        "password" varchar,
        "full_name" varchar,
        "role_id" int,
        "status" bit
        );

        CREATE TABLE "User_Config" (
        "setting_id" int PRIMARY KEY,
        "user_id" int,
        "per_page" int,
        "last_module" varchar
        );

        CREATE TABLE "Customer" (
        "customer_id" int PRIMARY KEY,
        "full_name" varchar,
        "phone" char(10) UNIQUE,
        "address" varchar,
        "point" int,
        "tier_id" int,
        "create_date" date
        );

        CREATE TABLE "Membership" (
        "tier_id" int PRIMARY KEY,
        "tier_name" varchar,
        "threshold" int,
        "discount" float
        );

        CREATE TABLE "Category" (
        "category_id" int PRIMARY KEY,
        "category_name" varchar
        );

        CREATE TABLE "Product" (
        "product_id" int PRIMARY KEY,
        "product_name" varchar,
        "category_id" int,
        "unit" varchar,
        "cost" decimal,
        "price" decimal,
        "stock" int,
        "image" varchar,
        "is_removed" bit
        );

        CREATE TABLE "Voucher" (
        "voucher_id" int PRIMARY KEY,
        "voucher_code" varchar,
        "description" varchar,
        "type" int,
        "discount" decimal,
        "min_threshold" decimal,
        "start_date" date,
        "end_date" date,
        "is_removed" boolean
        );

        CREATE TABLE "Order" (
        "order_id" int PRIMARY KEY,
        "order_code" varchar,
        "customer_id" int,
        "user_id" int,
        "order_date" date,
        "total_price" decimal,
        "voucher_id" int,
        "discount_amount" decimal,
        "final_price" decimal,
        "status" int
        );

        CREATE TABLE "OrderDetail" (
        "order_id" int,
        "product_id" int,
        "quantity" int,
        "current_price" decimal,
        "current_cost" decimal,
        "total_line" decimal,
        PRIMARY KEY ("order_id", "product_id")
        );

        -- Foreign Key Constraints

        ALTER TABLE "User" ADD FOREIGN KEY ("role_id") REFERENCES "Role"("role_id");
        ALTER TABLE "User_Config" ADD FOREIGN KEY ("user_id") REFERENCES "User"("user_id");
        ALTER TABLE "Customer" ADD FOREIGN KEY ("tier_id") REFERENCES "Membership"("tier_id");
        ALTER TABLE "Product" ADD FOREIGN KEY ("category_id") REFERENCES "Category"("category_id");
        ALTER TABLE "Order" ADD FOREIGN KEY ("customer_id") REFERENCES "Customer"("customer_id");
        ALTER TABLE "Order" ADD FOREIGN KEY ("user_id") REFERENCES "User"("user_id");
        ALTER TABLE "Order" ADD FOREIGN KEY ("voucher_id") REFERENCES "Voucher"("voucher_id");
        ALTER TABLE "OrderDetail" ADD FOREIGN KEY ("order_id") REFERENCES "Order"("order_id");
        ALTER TABLE "OrderDetail" ADD FOREIGN KEY ("product_id") REFERENCES "Product"("product_id");
    `);


};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function (knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS "User_Config";
        DROP TABLE IF EXISTS "OrderDetail";
        DROP TABLE IF EXISTS "Order";
        DROP TABLE IF EXISTS "Product";
        DROP TABLE IF EXISTS "Customer";
        DROP TABLE IF EXISTS "User";
        DROP TABLE IF EXISTS "Voucher";
        DROP TABLE IF EXISTS "Category";
        DROP TABLE IF EXISTS "Membership";
        DROP TABLE IF EXISTS "Role";
    `);
};
