/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Image').del()
  
  const images = [];
  const baseUrl = 'https://anaqzzawzkikttqlzdsm.supabase.co/storage/v1/object/sign/Images/';
  
  // Generate images for product_id 1 to 66
  // Pattern: XXYY1, XXYY2, XXYY3 where XX is product_id (padded), YY is image number
  for (let productId = 1; productId <= 66; productId++) {
    const paddedId = String(productId).padStart(3, '0');
    
    // First image (primary)
    images.push({
      product_id: productId,
      image_url: `${baseUrl}${paddedId}1.jpg`,
      is_primary: true
    });
    
    // Second image (secondary)
    images.push({
      product_id: productId,
      image_url: `${baseUrl}${paddedId}2.jpg`,
      is_primary: false
    });
    
    // Third image (secondary)
    images.push({
      product_id: productId,
      image_url: `${baseUrl}${paddedId}3.jpg`,
      is_primary: false
    });
  }
  
  await knex('Image').insert(images);
};
