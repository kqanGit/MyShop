# ImgUploadServer - Hướng dẫn chạy & kết quả

## Cấu hình
Tạo file `.env` với nội dung:
```env
SUPABASE_URL=https://kakdxifllhkjnxcqukwg.supabase.co
SUPABASE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imtha2R4aWZsbGhram54Y3F1a3dnIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2NzAyNzg2MSwiZXhwIjoyMDgyNjAzODYxfQ.rMvr3wv3Yhdrctq2vNDr7NfycGkafEJ4qYBIa-Jm1yI
```

## Cách chạy
```bash
npm install
node index.js
```
Khi chạy thành công, console sẽ in:
```
Upload Service chạy tại http://localhost:3000
```

## Test với Postman
1. Mở Postman
2. Import file `ImgUploadServer.postman_collection.json`
3. Chọn request "Upload Multiple Images"
4. Trong tab **Body**, chọn từng dòng `imageFiles` và click **Select Files** để chọn ảnh
5. Click **Send** để test

## API Endpoints

### 1. Upload ảnh đơn
- Gửi yêu cầu `POST http://localhost:3000/api/upload` với `multipart/form-data`, field: `imageFile`.

Ví dụ cURL:
```bash
curl -X POST "http://localhost:3000/api/upload" \
  -H "Accept: application/json" \
  -F "imageFile=@/duong/dan/toi/anh.jpg"
```
Phản hồi JSON (mẫu):
```json
{
  "url": "https://kakdxifllhkjnxcqukwg.supabase.co/storage/v1/object/public/MyShop%20-%20Product%20images/1735548123456_anh.jpg"
}
```

### 2. Upload nhiều ảnh
- Gửi yêu cầu `POST http://localhost:3000/api/upload-multiple` với `multipart/form-data`, field: `imageFiles` (có thể gửi tối đa 10 ảnh).

Ví dụ cURL:
```bash
curl -X POST "http://localhost:3000/api/upload-multiple" \
  -H "Accept: application/json" \
  -F "imageFiles=@/duong/dan/toi/anh1.jpg" \
  -F "imageFiles=@/duong/dan/toi/anh2.jpg" \
  -F "imageFiles=@/duong/dan/toi/anh3.jpg"
```
Phản hồi JSON (mẫu):
```json
{
  "count": 3,
  "urls": [
    "https://kakdxifllhkjnxcqukwg.supabase.co/storage/v1/object/public/MyShop%20-%20Product%20images/1735548123456_abc123_anh1.jpg",
    "https://kakdxifllhkjnxcqukwg.supabase.co/storage/v1/object/public/MyShop%20-%20Product%20images/1735548123457_def456_anh2.jpg",
    "https://kakdxifllhkjnxcqukwg.supabase.co/storage/v1/object/public/MyShop%20-%20Product%20images/1735548123458_ghi789_anh3.jpg"
  ]
}
```
