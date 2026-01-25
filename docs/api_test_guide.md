# 🧪 API Analysis & Test Guide

> [!IMPORTANT]
> **Lưu ý về Format JSON**:
> Backend .NET Core mặc định dùng **camelCase** (chữ cái đầu viết thường).
> Trong yêu cầu của bạn có nhắc đến `user_name`, `role_id` (snake_case) -> Cái này **KHÔNG** chạy được với code hiện tại.
> Bạn **BẮT BUỘC** phải dùng format camelCase như bên dưới (ví dụ `username`, `roleId`).

---

## 1. AUTH & USER

### Login (`POST /api/auth/login`)
*   **Logic**: Kiểm tra username/password, nếu đúng trả về Token.
*   **Test Body** (Use this exact JSON):
    ```json
    {
      "username": "admin",
      "password": "adminpassword"
    }
    ```
*   **Expected Response (Mock Data)**:
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsIn...",
      "username": "admin",
      "email": "email-removed-in-db-schema",
      "role": "1",
      "expiresAt": "2025-12-26T10:00:00Z"
    }
    ```

### Logout (`POST /api/auth/logout`)
*   **Logic**: Server nhận request, hiện tại chỉ trả về OK (do chưa lưu Refresh Token db).
*   **Test Body**:
    ```json
    {
      "refreshToken": "sample_refresh_token_string"
    }
    ```
    *(Lưu ý: dùng `refreshToken` viết liền, không phải `refresh_token`)*
*   **Expected Response**:
    ```json
    {
      "message": "Logged out successfully"
    }
    ```

### Create Staff (`POST /api/users`)
*   **Logic**: Tạo user mới với role và phone number.
*   **Test Body**:
    ```json
    {
      "username": "staff01",
      "password": "123",
      "fullName": "Nguyen Van A",
      "roleId": 2,
      "phoneNumber": "0987654321"
    }
    ```
*   **Expected Response**:
    ```json
    {
      "userId": 5,
      "username": "staff01",
      "fullName": "Nguyen Van A",
      "roleId": 2,
      "isActive": true
    }
    ```

### Get Profile (`GET /api/users/profile`)
*   **Headers**: `Authorization: Bearer <your_token>`
*   **Expected Response**:
    ```json
    {
      "userId": 1,
      "username": "admin",
      "fullName": "Administrator",
      "roleId": 1,
      "isActive": true
    }
    ```

---

## 2. CONFIGURATION

### Get Config (`GET /api/user-configs`)
*   **Headers**: `Authorization: Bearer <your_token>`
*   **Expected Response** (Nếu chưa có config sẽ trả mặc định):
    ```json
    {
      "perPage": 10,
      "lastModule": "Dashboard"
    }
    ```

### Update Config (`PUT /api/user-configs`)
*   **Headers**: `Authorization: Bearer <your_token>`
*   **Test Body**:
    ```json
    {
      "perPage": 20,
      "lastModule": "Orders"
    }
    ```
*   **Expected Response**: `200 OK` (hoặc JSON config mới tùy code controller trả về).

---

## 3. CUSTOMER

### Search Customer (`GET /api/customers?phone=09&name=H`)
*   **Query Params**:
    *   `phone`: 09
    *   `name`: H
*   **Expected Response** (Array):
    ```json
    [
      {
        "customerId": 3,
        "fullName": "Tran Van H",
        "phone": "0911223344",
        "address": "Ha Noi",
        "point": 150,
        "tierName": "Gold"
      }
    ]
    ```

### Create Customer (`POST /api/customers`)
*   **Test Body**:
    ```json
    {
      "fullName": "Khach Moi",
      "phone": "0999888777",
      "address": "Sai Gon"
    }
    ```
*   **Expected Response**:
    ```json
    {
      "customerId": 10,
      "fullName": "Khach Moi",
      "phone": "0999888777",
      "address": "Sai Gon",
      "point": 0,
      "tierName": "Silver"
    }
    ```

### Update Customer (`PUT /api/customers/{id}`)
*   **Test Body**:
    ```json
    {
      "fullName": "Khach Da Sua",
      "phone": "0999888777",
      "address": "Da Nang"
    }
    ```
*   **Expected Response**: Returns updated object similar to Create.

### Get Customer Detail (`GET /api/customers/{id}`)
*   **Expected Response** (Full Detail):
    ```json
    {
      "customerId": 10,
      "fullName": "Khach Da Sua",
      "phone": "0999888777",
      "address": "Da Nang",
      "point": 0,
      "tierName": "Silver",
      "createDate": "2025-12-26T10:30:00Z",
      "membership": {
        "tierId": 1,
        "tierName": "Silver",
        "discount": 0
      }
    }
    ```
