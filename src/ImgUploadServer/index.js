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

// 3. API Upload
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

app.listen(3000, () => {
    console.log('Upload Service chạy tại http://localhost:3000');
});