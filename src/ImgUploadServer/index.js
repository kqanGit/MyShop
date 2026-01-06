require('dotenv').config();
const express = require('express');
const multer = require('multer');
const { createClient } = require('@supabase/supabase-js');
const cors = require('cors');

const app = express();
app.use(cors()); 

const supabaseUrl = process.env.SUPABASE_URL;
const supabaseKey = process.env.SUPABASE_KEY;
const supabase = createClient(supabaseUrl, supabaseKey);


const storage = multer.memoryStorage();
const upload = multer({ storage: storage });

const BUCKET_NAME = 'MyShop - Product images';

// 3. API Upload đơn
app.post('/api/upload', upload.single('imageFile'), async (req, res) => {
    try {
        const file = req.file;
        if (!file) return res.status(400).json({ error: 'Chưa gửi file lên!' });

        
        const fileName = `${Date.now()}_${file.originalname.replace(/\s/g, '-')}`;


        const { data, error } = await supabase.storage
            .from(BUCKET_NAME)
            .upload(fileName, file.buffer, {
                contentType: file.mimetype
            });

        if (error) throw error;

        
        const { data: publicUrlData } = supabase.storage
            .from(BUCKET_NAME)
            .getPublicUrl(fileName);


        res.json({ 
            url: publicUrlData.publicUrl 
        });

    } catch (err) {
        console.error(err);
        res.status(500).json({ error: err.message });
    }
});

// 4. API Upload nhiều ảnh
app.post('/api/upload-multiple', upload.array('imageFiles', 10), async (req, res) => {
    try {
        const files = req.files;
        if (!files || files.length === 0) {
            return res.status(400).json({ error: 'Chưa gửi file nào!' });
        }

        const uploadPromises = files.map(async (file) => {
            const fileName = `${Date.now()}_${Math.random().toString(36).substring(7)}_${file.originalname.replace(/\s/g, '-')}`;
            
            const { data, error } = await supabase.storage
                .from(BUCKET_NAME)
                .upload(fileName, file.buffer, {
                    contentType: file.mimetype
                });

            if (error) throw error;

            const { data: publicUrlData } = supabase.storage
                .from(BUCKET_NAME)
                .getPublicUrl(fileName);

            return publicUrlData.publicUrl;
        });

        const urls = await Promise.all(uploadPromises);
        
        res.json({ 
            count: urls.length,
            urls: urls 
        });

    } catch (err) {
        console.error(err);
        res.status(500).json({ error: err.message });
    }
});

app.listen(3000, () => {
    console.log('Upload Service chạy tại http://localhost:3000');
});