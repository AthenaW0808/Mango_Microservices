using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data;

public class AppDbContext : DbContext
{
    /*
     * DbContext：这是AppDbContext类的基类。
     * DbContext是Entity Framework Core提供的一个基础类，它允许您与数据库进行交互。
     * (DbContextOptions<AppDbContext> options)：这是构造函数的参数列表。它接受一个DbContextOptions<AppDbContext>的实例
     * ，这是一种表示AppDbContext类配置选项的类型。
     * : base(options)：这部分是构造函数的基类调用。它调用基类（DbContext）
     * 的构造函数并将options参数传递给它。这是必要的，
     * 以确保在执行与AppDbContext特定的任何附加逻辑之前，基类被正确初始化。
     */

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = 1,
            CouponCode = "100FF",
            DiscountAmount = 10,
            MinAmount = 20
        });

        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = 2,
            CouponCode = "200FF",
            DiscountAmount = 20,
            MinAmount = 40
        });
    }
}