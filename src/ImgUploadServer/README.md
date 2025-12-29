# ImgUploadServer - Hướng dẫn chạy & kết quả

## Cách chạy
```bash
npm install
node index.js
```
Khi chạy thành công, console sẽ in:
```
Upload Service chạy tại http://localhost:3000
```

## Kết quả nhận được (khi upload ảnh)
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
  "url": "https://<project>.supabase.co/storage/v1/object/public/MyShop%20-%20Product%20images/1735548123456_anh.jpg"
}
```
