using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Color = Windows.UI.Color;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {

        public string InStockText { get; } = "In stock";
        public string StockHealthText { get; } = "78%";
        public double StockHealthValue { get; } = 78;

        public string TotalSalesTodayText { get; } = "$12.5k";
        public string ProfitTodayText { get; } = "300";
        public string OrdersTodayText { get; } = "26";
        public string NewCustomersTodayText { get; } = "8";

        public string TotalSalesDeltaText { get; } = "+8% from yesterday";
        public string ProfitDeltaText { get; } = "+5% from yesterday";
        public string OrdersDeltaText { get; } = "+12% from yesterday";
        public string CustomersDeltaText { get; } = "+0.5% from yesterday";

        // ===== Separate tile =====
        public string TotalProductsText { get; } = "128";

        // ===== Other sections =====
        public string CurrentMonthText { get; } = $"Month {DateTime.Now.Month}/{DateTime.Now.Year}";

        public ObservableCollection<RevenueBar> MonthlyRevenue { get; } = new();
        public ObservableCollection<RecentOrder> Recent3Orders { get; } = new();
        public ObservableCollection<TopSeller> Top5BestSellers { get; } = new();
        public ObservableCollection<LowStockProduct> LowStockTop5 { get; } = new();

        public DashboardPage()
        {
            InitializeComponent();

            // Monthly revenue bars (placeholder, weekly)
            MonthlyRevenue.Add(new RevenueBar("W1", 90));
            MonthlyRevenue.Add(new RevenueBar("W2", 140));
            MonthlyRevenue.Add(new RevenueBar("W3", 110));
            MonthlyRevenue.Add(new RevenueBar("W4", 175));
            MonthlyRevenue.Add(new RevenueBar("W5", 130));

            // Latest 3 orders
            Recent3Orders.Add(new RecentOrder("ORD-1025", "Nguyen Van A", "$245.90", "Pending", "10:42 AM"));
            Recent3Orders.Add(new RecentOrder("ORD-1024", "Tran Thi B", "$89.40", "Completed", "09:58 AM"));
            Recent3Orders.Add(new RecentOrder("ORD-1023", "Le Van C", "$120.10", "Pending", "09:21 AM"));

            // Top 5 best sellers
            Top5BestSellers.Add(new TopSeller("Coca-Cola 12-Pack", 320, 180));
            Top5BestSellers.Add(new TopSeller("Organic Whole Milk 1G", 260, 140));
            Top5BestSellers.Add(new TopSeller("Fresh Bananas (lb)", 210, 110));
            Top5BestSellers.Add(new TopSeller("Premium Ground Beef", 170, 85));
            Top5BestSellers.Add(new TopSeller("Fresh Strawberries", 130, 60));

            // Top 5 low stock (<5)
            LowStockTop5.Add(LowStockProduct.Make("Avocados", 4));
            LowStockTop5.Add(LowStockProduct.Make("Baby Spinach", 3));
            LowStockTop5.Add(LowStockProduct.Make("Almond Milk", 2));
            LowStockTop5.Add(LowStockProduct.Make("Apple Pie", 2));
            LowStockTop5.Add(LowStockProduct.Make("Organic Eggs", 1));
        }
    }

    // ===== Models =====
    public sealed class RevenueBar
    {
        public string Label { get; }
        public double Height { get; }
        public RevenueBar(string label, double height) => (Label, Height) = (label, height);
    }

    public sealed class RecentOrder
    {
        public string OrderId { get; }
        public string Customer { get; }
        public string TotalText { get; }
        public string Status { get; }
        public string TimeText { get; }

        public RecentOrder(string orderId, string customer, string totalText, string status, string timeText)
            => (OrderId, Customer, TotalText, Status, TimeText) = (orderId, customer, totalText, status, timeText);
    }

    public sealed class TopSeller
    {
        public string Name { get; }
        public double BarWidth { get; }
        public int Sold { get; }
        public string SoldText => $"{Sold} sold";

        public TopSeller(string name, double barWidth, int sold)
            => (Name, BarWidth, Sold) = (name, barWidth, sold);
    }

    public sealed class LowStockProduct
    {
        public string Name { get; }
        public int Remaining { get; }
        public string RemainingText => Remaining.ToString();

        public Brush BadgeBg { get; }
        public Brush BadgeFg { get; }

        private LowStockProduct(string name, int remaining, Brush bg, Brush fg)
            => (Name, Remaining, BadgeBg, BadgeFg) = (name, remaining, bg, fg);

        public static LowStockProduct Make(string name, int remaining)
        {
            if (remaining <= 1)
            {
                return new LowStockProduct(
                    name, remaining,
                    new SolidColorBrush(Color.FromArgb(30, 239, 68, 68)),
                    new SolidColorBrush(Color.FromArgb(255, 239, 68, 68))
                );
            }

            if (remaining <= 3)
            {
                return new LowStockProduct(
                    name, remaining,
                    new SolidColorBrush(Color.FromArgb(30, 249, 115, 22)),
                    new SolidColorBrush(Color.FromArgb(255, 249, 115, 22))
                );
            }

            return new LowStockProduct(
                name, remaining,
                new SolidColorBrush(Color.FromArgb(30, 59, 130, 246)),
                new SolidColorBrush(Color.FromArgb(255, 59, 130, 246))
            );
        }
    }
}