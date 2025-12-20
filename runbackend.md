# 📘 HƯỚNG DẪN CHẠY DỰ ÁN MYSHOP

Tài liệu này hướng dẫn chi tiết cách chạy dự án **MyShop** sau khi đã đồng bộ (clone / pull) mã nguồn.

---

## 0️⃣ Yêu cầu tiên quyết

Trước khi bắt đầu, hãy đảm bảo máy của bạn đã cài đặt và sẵn sàng các công cụ sau:

* **Docker Desktop**

  * Phải được cài đặt và **đang chạy**
  * Kiểm tra biểu tượng 🐳 Docker ở thanh taskbar

* **Node.js**

  * Dùng để chạy migration database

* **.NET SDK**

  * Dùng để chạy Backend

---

## 1️⃣ Cài đặt Database (sử dụng Docker)

### 1.1. Chạy Database bằng Docker

1. Mở **Terminal** tại thư mục gốc của project (`MyShop-main`)

2. Di chuyển vào thư mục Backend:

   ```bash
   cd src/Backend
   ```

3. Nếu **chưa có file `docker-compose.yml`**, hãy tạo mới file này với nội dung sau:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15-alpine
    container_name: myshop_postgres
    environment:
      POSTGRES_USER: myshop_user
      POSTGRES_PASSWORD: MyShop@2025
      POSTGRES_DB: myshop_db
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: unless-stopped

  pgadmin:
    image: dpage/pgadmin4
    container_name: myshop_pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@myshop.com
      PGADMIN_DEFAULT_PASSWORD: admin123
    ports:
      - "5050:80"
    depends_on:
      - postgres
    restart: unless-stopped

volumes:
  postgres_data:
```

4. Chạy Docker Compose:

   ```bash
   docker-compose up -d
   ```

> ⚠️ **Lưu ý**:
> Nếu gặp lỗi dạng `open //./pipe/...` ➜ hãy đảm bảo **Docker Desktop đang được bật**.

---

### 1.2. Cấu hình kết nối Database

1. Di chuyển vào thư mục Database:

   ```bash
   cd ../Database
   ```

2. **QUAN TRỌNG**: Tạo file `.env` trong thư mục:

   ```text
   src/Database/.env
   ```

3. Copy nội dung sau vào file `.env` (phải khớp với cấu hình Docker):

```env
POSTGRES_HOST=127.0.0.1
POSTGRES_PORT=5432
POSTGRES_USER=myshop_user
POSTGRES_PASSWORD=MyShop@2025
POSTGRES_DATABASE=myshop_db
```

---

### 1.3. Chạy Migration (tạo bảng Database)

1. Cài đặt các package cần thiết:

   ```bash
   npm install
   ```

2. Chạy migration:

   ```bash
   npx knex migrate:latest
   ```

### 1.4. Nạp Dữ liệu mẫu (Gồm Admin, User, Product...)

1. Chạy lệnh seed:

   ```bash
   npx knex seed:run
   ```

---

## 2️⃣ Chạy Backend

1. Từ thư mục `src/Database`, quay lại thư mục gốc của project:

   ```bash
   cd ../..
   ```

2. Chạy Backend:

   ```bash
   dotnet run --project src/Backend/MyShop.Presentation
   ```

---

## 3️⃣ Kiểm tra hệ thống

Sau khi chạy thành công:

* 🔹 **Backend API**:

  ```
  http://localhost:5xxx
  ```

* 🔹 **Swagger (xem & test API)**:
  👉 `http://localhost:5xxx/swagger`

* 🔹 **pgAdmin (quản lý Database)**:
  👉 `http://localhost:5050`

  * Email: `admin@myshop.com`
  * Password: `admin123`

---

✅ **Hoàn tất!** Dự án MyShop đã sẵn sàng để sử dụng.

Nếu gặp lỗi, hãy kiểm tra Docker Desktop, file `.env` và các bước migration.
