# Project proposal: MyShop
## Môn học: Lập trình Window
---

## 0. Thông tin thành viên nhóm
| STT | Họ và tên       | MSSV     |
|:---:|:----------------|:---------|
|  1  | Võ Thiện Nhân   | 23120066 |
|  2  | Dương Trọng Hòa | 23120127 |
|  3  | Thái Thiên Phú  | 23120327 |
|  4  | Bùi Minh Quân   | 23120337 |
|  5  | Lục Hoàng Tuấn  | 23120393 |


---

## 1. Chức năng ứng dụng
### 1.1. Danh sách chức năng chính
Dự án được chia thành các module chính sau:

* **Module 1: Xác thực & Phân quyền**
    * Đăng nhập
    * Đăng xuất
    * Đăng ký
    * Hiển thị thông tin phiên bản của chương trình
    * Cho phép cấu hình server từ màn hình Config
    * Xử lý phiên đăng nhập
* **Module 2: Dashboard tổng quan**
    * Số lượng khách hàng mới 
    * Tổng số sản phẩm 
    * Top 5 sản phẩm sắp hết hàng (số lượng < 5)
    * Top 5 sản phẩm bán chạy
    * Tổng số đơn hàng trong ngày
    * Tổng doanh thu, lợi nhuận trong ngày
    * Chi tiết 3 đơn hàng gần nhất
    * Biểu đồ doanh thu trong tháng hiện tại
* **Module 3: Quản lý Sản phẩm**
    * CRUD Sản phẩm (Thêm, sửa, xóa, xem)
    * Hỗ trợ phân trang 
    * CRUD danh mục sản phẩm
    * Tìm kiếm, lọc sản phẩm theo loại, theo khoảng giá
    * Cho phép import dữ liệu từ Excel, Access
* **Module 4: Quản lý Đơn hàng**
    * CRUD đơn hàng
    * Tìm kiếm, lọc đơn hàng theo ngày, theo khách hàng
    * Hỗ trợ phân trang danh sách đơn hàng
    * In đơn hàng
    * Áp dụng khuyến mãi, voucher
* **Quản lý khách hàng**
    * Lưu thông tin khách hàng mỗi lần mua hàng
    * Mỗi lần mua hàng sẽ tích lũy điểm để lên hạng -> Nhận voucher tương ứng
    * Chủ shop có thể tìm kiếm, xem chi tiết thông tin khách hàng
    * Xuất ra file excel
* **Module 5: Báo cáo & Thống kê**
    * Thống kê doanh thu, lợi nhuận (theo ngày, tuần, tháng, năm) bằng biểu đồ cột / bánh
    * Thống kê sản phẩm, số lượng bán (theo ngày, tuần, tháng, năm) bằng biểu đồ đường
    * Xuất ra file excel
* **Module 6: Cấu hình chương trình**
    * Hiệu chỉnh số lượng sản phẩm mỗi trang khi phân trang
    * Lưu lại chức năng chính lần cuối mở 

* Sau đó là đóng gói thành file exe để tự cài chương trình vào hệ thống

* Một số chức năng thêm:
    * Hỗ trợ sắp xếp khi xem danh sách theo nhiều tiêu chí, tùy biến chiều tăng / giảm
    * Hỗ trợ responsive 
    * Sắp xếp danh sách theo tiêu chí 

### 1.2. Ánh xạ Chuẩn Đầu Ra (CĐR) Môn học

| CĐR Môn học (Từ Đề cương) | Chức năng liên quan của dự án | Mô tả cách đáp ứng |
|:---|:---|:---|
| **G1.1, G2.1, G2.2** | Toàn bộ giao diện Client (WinUI) | Sử dụng các điều khiển cơ bản (Button, TextBox), các container (Grid, StackPanel), và các tài nguyên (hình ảnh, hộp thoại) để xây dựng giao diện người dùng đồ họa. |
| **G2.3** | Module 2: "Biểu đồ doanh thu" | Chức năng này đáp ứng trực tiếp yêu cầu tạo và hiển thị các đối tượng đồ họa (biểu đồ), được mô tả trong **Tuần 7** của đề cương. |
| **G2.4** | Toàn bộ giao diện Client (WinUI) | Xử lý các sự kiện chuột (click) và bàn phím (nhập liệu) thông qua cơ chế Data Binding và `ICommand` của MVVM (đáp ứng CĐR G2.4). |
| **G2.5, G2.6** | Module 6: "Cấu hình chương trình" <br/> Module 1: "Cấu hình server" | Chức năng "Lưu lại chức năng cuối", "Hiệu chỉnh số lượng", "Cấu hình server" sẽ cần làm việc với **Registry** hoặc **hệ thống tập tin** để lưu trữ các cài đặt này local trên máy người dùng. |
| **G3.1, G3.2** | Toàn bộ quy trình làm việc nhóm | Áp dụng quy trình Git, jira để cộng tác phát triển dự án, giúp các thành viên làm việc độc lập và tích hợp sản phẩm nhóm  |
| **G4.1, G4.2** | Báo cáo đồ án & Tính năng nâng cao | Hoàn thành tài liệu báo cáo (file Markdown này)  và các tính năng nâng cao (mục 7) để trình bày các chủ đề đã tìm hiểu. |
| **G5** | Modules 2, 3, 4, 5 | Toàn bộ các chức năng nghiệp vụ (Dashboard, Sản phẩm, Đơn hàng, Báo cáo) đều yêu cầu thao tác với CSDL (PostgreSQL) thông qua Backend. |
| **G6.1** | Toàn bộ dự án | Đây là kiến trúc cốt lõi: Ứng dụng WinUI (Client) **gọi Restful API** (do Backend cung cấp) để trao đổi dữ liệu, đáp ứng CĐR G6.1. |

---

## 2. Giao diện (Prototype)
Nhóm đã thiết kế các luồng chức năng chính của ứng dụng WinUI bằng Figma.

* **Link Figma Prototype:** https://www.figma.com/design/lMUi46rd3rmFDp86CJBi6p/WP_Project?node-id=0-1&t=211qV7fUj4GhVfCg-1
---

## 3. Làm việc nhóm
### 3.1. Phân công công việc
| Thành viên | Nhiệm vụ |
|:---|:---|
| **Bùi Minh Quân** | Team lead, quản lý tiến độ, Backend | 
| **Thái Thiên Phú** | Frontend |
| **Lục Hoàng Tuấn** | Frontend | 
| **Võ Thiện Nhân** | Backend | 
| **Dương trọng hòa** | Backend | 

### 3.2. Công cụ theo dõi tiến độ
* **Quản lý Task:** Nhóm sử dụng **jira** (hoặc GitHub Projects/Notion) để quản lý công việc theo dạng bảng Kanban (To Do, In Progress, Done, Testing).
* **Trao đổi:** Sử dụng **Zalo** để trao đổi nhanh và **Jitsi** để họp nhóm hàng tuần.
* **Lưu trữ code:** **GitHub**.

### 3.3. Chiến lược làm việc Git (Git Workflow)
Nhóm áp dụng mô hình **Feature Branch Workflow** đơn giản:

1.  **`main`:** Là nhánh chính, luôn ở trạng thái ổn định (production-ready). Chỉ `merge` code từ `develop` vào khi đã test kỹ.
2.  **`develop`:** Là nhánh phát triển chính (nhánh tích hợp). Mọi người sẽ tạo Pull Request (PR) vào nhánh này.
3.  **Nhánh `feature/...`:** (ví dụ: `feature/login`, `feature/crud-product`)
    * Khi bắt đầu một chức năng mới, thành viên sẽ tạo một nhánh `feature/...` từ `develop`.
    * Sau khi làm xong, thành viên đó sẽ tạo **Pull Request (PR)** vào `develop`.
    * **Yêu cầu:** Cần ít nhất 1 thành viên khác **review code** và **approve** PR trước khi merge.
4.  **Commit Convention:** Nhóm thống nhất sử dụng **Conventional Commits** để viết nội dung commit rõ ràng.
    * `feat:` (Thêm tính năng mới)
    * `fix:` (Sửa lỗi)
    * `docs:` (Viết tài liệu)
    * `style:` (Chỉnh sửa format, không ảnh hưởng code)
    * `refactor:` (Tái cấu trúc code)

---

## 4. Kiến trúc phần mềm
Hệ thống được chia làm 2 dự án riêng biệt: **Backend (Server)** và **Client (WinUI)**, giao tiếp với nhau qua RESTful API.

### 4.1. Backend (ASP.NET Core Web API)
Áp dụng **Clean Architecture** kết hợp **3-Layer**:

* **Domain:** Chứa các Entity (POCO) và logic nghiệp vụ cốt lõi nhất.
* **Application (BLL):** Chứa các `Interface`, DTOs và `Services` (logic nghiệp vụ chính). Đây là nơi thực thi các use case.
* **Infrastructure (DAL):** Chứa code truy cập CSDL (dùng EF Core/Dapper), kết nối dịch vụ bên ngoài.
* **Presentation (API Layer):** Chứa các `Controllers` để cung cấp API cho client.

Các công nghệ chính được sử dụng trong phần này:
* **Ngôn ngữ:** C# (.NET 7, .NET 8 hoặc .NET 10)
* **Framework:** ASP.NET Core Web API
* **CSDL:** PostgreSQL
* **ORM:** Entity Framework Core hoặc Dapper
* **Authentication:** JWT (JSON Web Token)
* **Dependency Injection:** Tích hợp sẵn trong .NET
* **Kiến trúc:** Clean Architecture + 3-Layer

#### Chi tiết cài đặt
| Thành phần | Có sẵn trong .NET? | Cài thêm? | Trang cài đặt / Package | Vai trò |
|:---|:---|:---|:---|:---|
| C# (.NET 7/8) | Có sẵn khi cài .NET SDK | Không | dotnet.microsoft.com/download | Ngôn ngữ lập trình |
| ASP.NET Core Web API | Có sẵn | Không | (tạo bằng `dotnet new webapi`) | Tạo API |
| PostgreSQL | Không | Có | postgresql.org/download | CSDL chính |
| Entity Framework Core | Không | Có (qua NuGet) | Microsoft.EntityFrameworkCore | ORM truy xuất CSDL |
| Dapper | Không | Có (qua NuGet) | Dapper | ORM nhẹ |
| JWT Authentication | Không | Có (qua NuGet) | Microsoft.AspNetCore.Authentication.JwtBearer | Xác thực bảo mật |
| Dependency Injection | Có sẵn | Không | Tích hợp trong .NET | Quản lý phụ thuộc |


#### Chi tiết các tầng (Layer)
| Tầng | Vai trò | Chứa | Cài đặt cần có |
|:---|:---|:---|:---|
| **Domain** | Core Layer. Chứa Logic thuần túy. | **Entities**, **Enums**. | Không cần cài gì thêm. |
| **Application** | Business Logic Layer. Chứa các dịch vụ (Service), xử lý nghiệp vụ, mapping. | **Interfaces** (khai báo contract), **Services** (ProductService, OrderService), **DTOs**, **Mappings** (AutoMapper). | AutoMapper, FluentValidation. |
| **Infrastructure** | Data Access Layer. Kết nối và thao tác dữ liệu. | **Data** (AppDbContext.cs), **Repositories** (Code thực thi CRUD), **Configurations** (Cấu hình EF Core), **Migrations**. | PostgreSQL client, EF Core PostgreSQL provider. |
| **Presentation** | Web API Layer. Giao tiếp với người dùng (client). | **Controllers** (ProductsController.cs, OrdersController.cs), **Program.cs** (cấu hình hệ thống: DI, CSDL, CORS, Auth, Swagger). | ASP.NET Core SDK, Swagger, JWT (NuGet). |
| **Test** | Dành cho kiểm thử Unit Test và Integration Test. | | xUnit hoặc NUnit, Moq (mock dữ liệu). |


#### Chức năng Backend
| Chức năng | Mô tả | Thành phần chính |
|:---|:---|:---|
| Đăng nhập - Phân quyền | "Đăng nhập qua JWT, phân quyền Admin / Sale / Moderator" | "AuthController, AuthService" |
| Quản lý sản phẩm | "CRUD sản phẩm, giảm giá, điểm thưởng" | "ProductController, ProductService, ProductRepository" |
| Quản lý đơn hàng | "Tạo đơn hàng, tính tổng, in PDF, KPI" | "OrderController, OrderService" |
| Quản lý khách hàng | "Theo dõi tích điểm, lịch sử mua, hạng" | "CustomerController, CustomerService" |
| Báo cáo - Dashboard | "Thống kê doanh thu, top sale" | "ReportController, ReportService" |

### 4.2. Client (WinUI App)
Áp dụng kiến trúc **MVVM (Model - View - ViewModel)**:

Phần Client là một ứng dụng Desktop Windows được xây dựng bằng **WinUI 3**. Ứng dụng này áp dụng triệt để kiến trúc **MVVM (Model-View-ViewModel)** để tách biệt logic giao diện (UI) khỏi logic nghiệp vụ (business logic).

Client **không** kết nối trực tiếp với cơ sở dữ liệu (PostgreSQL). Thay vào đó, mọi thao tác dữ liệu (xem sản phẩm, tạo đơn hàng, xem báo cáo) đều được thực hiện bằng cách gọi và tiêu thụ **RESTful API** do phần Backend cung cấp.

#### Công nghệ sử dụng (Tech Stack)
* **Framework:** WinUI 3 (Windows App SDK)
* **Ngôn ngữ:** C# (.NET 8/9/10)
* **Kiến trúc:** MVVM (Model-View-ViewModel)
* **Thư viện MVVM:** MVVM Toolkit (CommunityToolkit.Mvvm)
    * `ObservableObject` (thay cho `INotifyPropertyChanged`)
    * `RelayCommand` (thay cho `ICommand`)
* **Giao tiếp API:** `HttpClient` (tích hợp sẵn trong .NET)
* **JSON:** `System.Text.Json` (để tuần tự hóa và giải tuần tự hóa dữ liệu)

#### Kiến trúc MVVM (Model-View-ViewModel)
Kiến trúc MVVM là nền tảng của dự án Client, giúp code dễ bảo trì, dễ test và tách biệt rõ ràng:

##### View (V)
    * Chứa các control của WinUI (Button, TextBox, DataGrid, ListView).
    * Sử dụng cơ chế **Data Binding** (ví dụ: `{x:Bind ViewModel.TenSanPham}`) để liên kết các control với thuộc tính (property) trong ViewModel.

#####  ViewModel (VM)
    * **Chứa các thuộc tính (Properties):** Là các dữ liệu mà View sẽ hiển thị (ví dụ: `public ObservableCollection<Product> Products { get; set; }`).
    * **Chứa các Lệnh (Commands):** Là các hành động mà View có thể thực thi (ví dụ: `public ICommand LoadProductsCommand { get; }`).
    * **Nơi gọi API:** Đây chính là nơi `HttpClient` được sử dụng để gọi đến các API của Backend.
    * **Thông báo thay đổi:** Kế thừa từ `ObservableObject` (MVVM Toolkit) để tự động thông báo cho View khi dữ liệu thay đổi.

#####  Model (M)
    * Trong dự án này, Model chính là các **DTOs (Data Transfer Objects)** mà Backend định nghĩa (ví dụ: `ProductDto`, `OrderDto`).
    * Khi ViewModel gọi API và nhận về một chuỗi JSON, nó sẽ dùng `System.Text.Json` để "giải mã" (deserialize) chuỗi JSON đó thành các đối tượng Model này.

**Luồng giao tiếp Client - Backend (Ví dụ: Tải DS Sản phẩm)**

1.  **View (Tương tác):** Người dùng nhấn vào nút "Tải sản phẩm". Nút này được bind với một `Command` trong ViewModel:
    * `Button Content="Tải" Command="{x:Bind ViewModel.LoadProductsCommand}"`
2.  **ViewModel (Xử lý):** `LoadProductsCommand` được thực thi. Hàm này (thường là `async`) sẽ:
    * Hiển thị một vòng xoay "Đang tải...".
    * Sử dụng một `HttpClient` (thường được inject qua Dependency Injection) để gọi API Backend.
    * `var jsonResponse = await _httpClient.GetAsync("https://your-api-url/api/products");`
3.  **Backend (Phản hồi):** Backend nhận yêu cầu, truy vấn CSDL và trả về một chuỗi JSON chứa danh sách sản phẩm.
4.  **ViewModel (Cập nhật):**
    * ViewModel nhận chuỗi JSON.
    * Giải tuần tự hóa (deserialize) JSON thành một danh sách các đối tượng `ProductDto` (Model).
    * Cập nhật danh sách đó vào thuộc tính `public ObservableCollection<ProductDto> Products`.
    * Ẩn vòng xoay "Đang tải...".
5.  **View (Hiển thị):**
    * Một `ListView` hoặc `DataGrid` trong View đã được bind với `Products`.
    * `ListView ItemsSource="{x:Bind ViewModel.Products}"`
    * Do `Products` là `ObservableCollection`, View sẽ **tự động cập nhật** và hiển thị toàn bộ danh sách sản phẩm cho người dùng mà không cần thêm bất kỳ code C# nào ở View.

### 4.3. Sơ đồ kết hợp
Luồng dữ liệu tổng thể khi người dùng thực hiện một chức năng (ví dụ: tải danh sách sản phẩm):

[Sơ đồ kiến trúc tổng thể của dự án]

`View (WinUI)` → `ViewModel (Command)` → `HttpClient (Gọi API)` → `API Controller (Backend)` → `Service (Backend)` → `Repository (Backend)` → `PostgreSQL DB`

---

## 5. Design Pattern
Dự án áp dụng các mẫu thiết kế (Design Pattern) để giải quyết các bài toán cụ thể, giúp code linh hoạt, dễ bảo trì và dễ mở rộng. Dưới đây là các pattern được sửá dụng, dựa trên tài liệu thiết kế backend:

*(Nhóm có thể chọn 4 trong 5 pattern này để báo cáo, tương ứng với 4 thành viên)*

#### 1. Facade Pattern 
* **Áp dụng vào:** **Giao diện dashboard (Module 2)**.
* **Lý do:** Đúng như mô tả "Đơn giản hóa giao diện, giảm sự phụ thuộc vào các hệ thống con". Thay vì Client (ViewModel) phải gọi 7-8 API nhỏ lẻ để lấy dữ liệu cho Dashboard (top sản phẩm, doanh thu, đơn hàng mới...), Backend cung cấp một API duy nhất (`GET /api/dashboard`). Lớp `DashboardFacadeService` ở Backend sẽ tự mình gọi các service con (ProductService, OrderService, ReportService) để thu thập dữ liệu và trả về một DTO duy nhất.

#### 2. Strategy Pattern 
* **Áp dụng vào:** **Tính hoa hồng / Sắp xếp**  hoặc Chức năng **Áp dụng khuyến mãi (Module 4)**.
* **Lý do:** "Cho phép thay đổi thuật toán linh hoạt khi chạy". Cho phép hệ thống dễ dàng thêm các "chiến lược" khuyến mãi mới (ví dụ: Giảm 10%, Mua 1 tặng 1, Giảm 50k cho đơn trên 500k...) mà không cần sửa đổi code logic chính của `OrderService`.

#### 3. Observer Pattern 
* **Áp dụng vào:** **Tích điểm cho khách hàng, cập nhật dashboard (Module 2, 4)**.
* **Lý do:** "Cho phép các đối tượng tự động cập nhật khi có hành động xảy ra (thêm đơn hàng)". Khi một đơn hàng mới được tạo (Subject), nó sẽ phát một thông báo. Các service khác (Observers) như `CustomerService` (để cộng điểm) và `DashboardService` (để cập nhật doanh thu) sẽ tự động nhận thông báo và hành động, giúp giảm sự phụ thuộc trực tiếp giữa các service.

#### 4. Proxy Pattern 
* **Áp dụng vào:** **Phân quyền người dùng**.
* **Lý do:** "Cho phép hệ thống nhận diện quyền user mà không cần thay đổi lớp Service". Cung cấp một lớp "Proxy" (ví dụ `ProductServiceProxy`) đứng trước lớp `ProductService` thật. Lớp Proxy này sẽ kiểm tra quyền hạn của người dùng (từ JWT Token) trước khi quyết định có "ủy quyền" cho lớp Service thật thực thi hay không.

#### 5. Factory Pattern 
* **Áp dụng vào:** **Tạo đối tượng linh hoạt**  (ví dụ: tạo các loại báo cáo trong Module 5).
* **Lý do:** "Dễ mở rộng repository/service mới". Ví dụ, khi người dùng yêu cầu xuất báo cáo, `ReportService` sẽ gọi một Factory để quyết định tạo đối tượng `PdfReportGenerator` hay `ExcelReportGenerator` dựa trên yêu cầu, mà không cần `ReportService` biết chi tiết cách tạo của từng loại.
---

## 6. Đảm bảo chất lượng (QA)
### 6.1. Coding Convention
Nhóm thống nhất tuân thủ các quy tắc sau:
* **C#:** Theo chuẩn `.NET C# Coding Conventions` của Microsoft.
    * Tên lớp, phương thức, thuộc tính: `PascalCase`.
    * Tên biến local, tham số: `camelCase`.
    * Biến private: `_camelCase`.
    * Sử dụng `var` khi kiểu dữ liệu đã rõ ràng.
* **XAML:** [Quy tắc XAML của nhóm, ví dụ: tên control, cách đặt margin...]
* **Công cụ:** Sử dụng `.editorconfig` và các bộ phân tích (Analyzer) có sẵn của .NET để đồng bộ hóa format code.

### 6.2. Unit Test
* **Framework:** Sử dụng **xUnit** (hoặc NUnit) để viết Unit Test.
* **Thư viện Mock:** Sử dụng **Moq** (hoặc NSubstitute) để giả lập các dependency (ví dụ: mock `IProductRepository` khi test `ProductService`).
* **Phạm vi:** Tập trung test các logic nghiệp vụ quan trọng trong **Application Layer (Services)** của Backend. Đảm bảo các hàm tính toán (tính tổng tiền, tính khuyến mãi) chạy đúng.

### 6.3. Manual Test
* **Quy trình:** sẽ dựa trên tài liệu yêu cầu và thiết kế Figma để viết các **Test Case** chi tiết cho từng luồng chức năng (ví dụ: Test case Đăng nhập, Test case Tạo đơn hàng).
* **Công cụ:** Quản lý các Test Case bằng Excel hoặc TestRail (nếu có).
* **Thực thi:** Test thủ công trên ứng dụng WinUI đã được build sau mỗi lần `develop` có cập nhật lớn.

### 6.4. Test tự động giao diện (Automated UI Test)
* **Mục tiêu:** Tự động hóa các kịch bản test quan trọng, lặp đi lặp lại để phát hiện lỗi sớm.
* **Công cụ:** Sử dụng **WinAppDriver** (của Microsoft) để điều khiển và tương tác tự động với các thành phần UI của ứng dụng WinUI.
* **Phạm vi:** Viết script test cho 2 luồng quan trọng nhất:
    1.  Luồng Đăng nhập (Login flow).
    2.  Luồng Tạo đơn hàng (Happy path).

---

## 7. Tính năng nâng cao

* Phân quyền admin và moderator / sale để truy cập dữ liệu hạn chế khác nhau. (Ví dụ sale chỉ thấy được giá bán còn admin thấy được cả giá nhập hoặc sale A chỉ thấy được các đơn hàng do mình bán trong ngày mà không thấy được các đơn hàng của sale B)
    * Trả thêm hoa hồng bán hàng cho sale dựa trên doanh số (KPI) 

* Chương trình có khả năng mở rộng động theo kiến trúc plugin

* UI Animation

* Đồng bộ hóa ngoại tuyến:
    Ứng dụng WinUI sẽ có khả năng hoạt động khi mất kết nối mạng (offline). Khi ở chế độ offline, nhân viên bán hàng (sale) vẫn có thể tạo đơn hàng, và các đơn hàng này sẽ được lưu tạm vào một cơ sở dữ liệu local (ví dụ: SQLite) ngay trên máy tính của Client. Khi có kết nối mạng trở lại, ứng dụng sẽ tự động đồng bộ hóa các đơn hàng đang chờ này lên Backend (PostgreSQL).

* Tìm kiếm nâng cao: tìm không dấu, tìm gần đúng...

---

## 8. Kế hoạch nháp ban đầu (Timeline)
| Giai đoạn | Thời gian | Mục tiêu |
|:---|:---|:---|
| **GĐ 1: Khởi động** | Tuần 1 | Thống nhất yêu cầu, công nghệ. Thiết kế CSDL (v1). Thiết kế Figma (v1). | 
| **GĐ 2: Dựng khung** | Tuần 2-3 | Cài đặt dự án Backend (Clean Arch), dự án Client (MVVM). Hoàn thành API Xác thực. |
| **GĐ 3: Phát triển Lõi** | Tuần 4-5  | Hoàn thành API CRUD Sản phẩm, CRUD Đơn hàng. WinUI liên kết được API. |
| **GĐ 4: Hoàn thiện** | Tuần 4-5 | Hoàn thành các chức năng còn lại (Báo cáo). Viết Unit Test. |
| **GĐ 5: Tính năng nâng cao** | Tuần 4-5 | Phát triển 4 tính năng nâng cao đã đăng ký. |
| **GĐ 6: Kiểm thử & Sửa lỗi**| Tuần 6 | Manual Test, Automated Test, Sửa các lỗi phát sinh (bug). |
| **GĐ 7: Hoàn tất** | Tuần 6 | Viết tài liệu, chuẩn bị báo cáo, demo. |

