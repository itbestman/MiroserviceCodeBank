using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var affected = await connection
                .ExecuteAsync("insert into coupon (productname,description,amount) values " +
                "(@ProductName, @Description,@Amount)", new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                });

            if (affected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
            (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync("delete from coupon where productname = @ProductName", new
            {
                ProductName = productName
            });
            if (affected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from coupon where productName = @ProductName", new { ProductName = productName });

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc",

                };
            }
            else
            {
                return coupon;
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
            (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync("Update coupon set productname=@ProductName, " +
                "description=@Description, amount = @Amount where id = @Id", new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                    Id = coupon.Id
                });
            if (affected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
    }
}
