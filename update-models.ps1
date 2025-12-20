# Script cập nhật Entity từ Database
# Chạy script này sau khi bạn đã sửa Database (chạy knex migrate xong)
# npx knex migrate:latest
# .\update-models.ps1
echo "Dang cap nhat code tu Database..."

dotnet ef dbcontext scaffold "Host=localhost;Database=myshop_db;Username=myshop_user;Password=MyShop@2025" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir ../MyShop.Domain/Entities --context-dir Data --namespace MyShop.Domain.Entities --context-namespace MyShop.Infrastructure.Data --project src/Backend/MyShop.Infrastructure/MyShop.Infrastructure.csproj --force --no-onconfiguring

echo "Xong! Nho kiem tra lai code nhe."
