# 🧪 Hướng dẫn Test API bằng Postman

## 1. Cấu hình Môi trường (Environment)
*   **Base URL**: `http://localhost:5126`
*   **Protocol**: HTTP

## 2. Authentication (Xác thực)

### 2.1. Đăng ký (Register)
*   **Method**: `POST`
*   **URL**: `{{BaseURL}}/api/auth/register`
*   **Body** (JSON):
    ```json
    {
      "username": "test_staff",
      "password": "password123",
      "email": "test@myshop.com",  // (Trường này có thể bỏ qua nếu code đã bỏ)
      "fullName": "Test Staff",
      "phoneNumber": "0987654321"
    }
    ```
*   **Expected**: `200 OK` + Token.

### 2.2. Đăng nhập (Login)
*   **Method**: `POST`
*   **URL**: `{{BaseURL}}/api/auth/login`
*   **Body** (JSON):
    ```json
    {
      "username": "test_staff", 
      "password": "password123"
    }
    ```
*   *(Hoặc thử user admin sẵn có: username `admin`, password `adminpassword`)*
*   **Expected**: `200 OK`.
*   **Lưu ý**: Copy chuỗi `token` trong response để dùng cho các bước sau.

### 2.3. Đăng xuất (Logout)
*   **Method**: `POST`
*   **URL**: `{{BaseURL}}/api/auth/logout`
*   **Body** (JSON):
    ```json
    {
      "refresh_token": "dummy_string"
    }
    ```
*   **Expected**: `200 OK`.

---

## 3. User & Config (Cần Token)

**🔑 Authorization**: Tab **Auth** -> Type **Bearer Token** -> Paste Token vào.

### 3.1. Lấy thông tin cá nhân (Get Profile)
*   **Method**: `GET`
*   **URL**: `{{BaseURL}}/api/users/profile`
*   **Expected**: `200 OK` + JSON Info user.

### 3.2. Tạo nhân viên mới (Create Staff)
*   **Method**: `POST`
*   **URL**: `{{BaseURL}}/api/users`
*   **Body** (JSON):
    ```json
    {
      "username": "staff_02",
      "password": "123",
      "fullName": "Nhân viên 2",
      "roleId": 3,
      "phoneNumber": "0912345678"
    }
    ```

### 3.3. Lấy cấu hình (Get Config)
*   **Method**: `GET`
*   **URL**: `{{BaseURL}}/api/user-configs`

### 3.4. Lưu cấu hình (Update Config)
*   **Method**: `PUT`
*   **URL**: `{{BaseURL}}/api/user-configs`
*   **Body** (JSON):
    ```json
    {
      "perPage": 50,
      "lastModule": "Orders"
    }
    ```

---

## 4. Customer (Khách hàng)

**🔑 Authorization**: Tab **Auth** -> Type **Bearer Token** -> Paste Token vào.

### 4.1. Tìm kiếm khách hàng
*   **Method**: `GET`
*   **URL**: `{{BaseURL}}/api/customers?phone=09&name=H`
*   **Expected**: Danh sách khách hàng thỏa mãn điều kiện.

### 4.2. Thêm khách hàng mới
*   **Method**: `POST`
*   **URL**: `{{BaseURL}}/api/customers`
*   **Body** (JSON):
    ```json
    {
      "fullName": "Khách Mới Tanh",
      "phone": "0999888777",
      "address": "123 Đường Láng"
    }
    ```
*   **Expected**: `200 OK` (Point = 0, Tier = Silver).

### 4.3. Xem chi tiết
*   **Method**: `GET`
*   **URL**: `{{BaseURL}}/api/customers/{id}` (Thay `{id}` bằng ID khách vừa tạo, VD: `1`)

### 4.4. Cập nhật khách hàng
*   **Method**: `PUT`
*   **URL**: `{{BaseURL}}/api/customers/{id}`
*   **Body** (JSON):
    ```json
    {
      "fullName": "Khách Đã Sửa",
      "phone": "0999888777",
      "address": "456 Xã Đàn"
    }
    ```
