using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using SecurityDemo.Data;
using SecurityDemo.Models;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SecurityDemo.Repositories
{
    public class SqlDbRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public SqlDbRepository(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public List<string> GetCities(out string message)
        {
            message = string.Empty;
            List<string> cities = new List<string>();
            try
            {
                var result = _context.Cities;
                foreach (var c in result)
                {
                    cities.Add($"{c.cityId}, {c.cityName}");
                }
            }
            catch (Exception e)
            {
                message = $"Error retrieving cities";
            }
            if (cities.Count() == 0)
            {
                message = $"No cities";
            }
            return cities;
        }
        public string GetCityName(string cityId)
        {
            string cityName = string.Empty;

            try
            {
                cityName = _context.Cities.Where((c) => c.cityId == Int32.Parse(cityId)).Select((c) => c.cityName).FirstOrDefault();
            }
            catch (Exception e)
            {
                string message = $"Error getting city information";
            }
            return cityName;
        }

        public List<string> GetBuildingsInCity(string cityId)
        {
            List<string> list = new List<string>();

            try
            {
                var query = from building in _context.Buildings
                            join room in _context.Rooms on building.buildingId equals room.buildingId
                            where building.cityId == Int32.Parse(cityId)
                            select new
                            {
                                building.buildingId,
                                building.name,
                                RoomName = room.name,
                                room.capacity
                            };

                var result = query.ToList();
                foreach (var c in result)
                {
                    list.Add($"{c.buildingId},{c.name},{c.RoomName},{c.capacity}");
                }

            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name";
            }
            return list;
        }
        public List<string> GetRegisteredUsers()
        {
            List<string> list = new List<string>();
            string cityName = string.Empty;

            try
            {
                list = _context.Users
           .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
           .Join(_context.Roles, ur => ur.userRole.RoleId, role => role.Id, (ur, role) => new { ur.user.Id, ur.user.UserName, RoleId = role.Id })
           .Select(result => $"{result.Id},{result.UserName},{result.RoleId}")
           .ToList();
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name";
            }
            return list;
        }
        public List<string> GetBuildingsInCity2(string cityId)
        {
            string message = string.Empty;
            List<string> list = new List<string>();

            try
            {
                int id = int.Parse(cityId);
                list = _context.Buildings
                  .Where(b => b.cityId == id)
                  .Join(_context.Rooms, building => building.buildingId, room => room.buildingId, (building, room) => new { building, room })
                  .Select(result => $"{result.building.buildingId},{result.building.name},{result.room.name},{result.room.capacity}")
                  .ToList();
            }
            catch (Exception e)
            {
                message = $"Error retrieving cities";
            }
            if (list.Count() == 0)
            {
                message = $"No cities";
            }
            return list;
        }

        public List<ProductVM> GetProducts()
        {
            List<ProductVM> products = new List<ProductVM>();

            try
            {
                products = _context.Products.Select(p => new
                ProductVM
                {
                    ProdID = p.ProdID,
                    ProdName = p.ProdName,
                    Price = p.Price
                }).ToList();
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name";
            }
            return products;
        }

        public ProductVM GetProduct(string productID)
        {
            ProductVM productVM = new ProductVM();

            try
            {
                productVM = _context.Products
                   .Where(p => p.ProdID == productID)
                   .Select(p => new ProductVM
                   {
                       ProdID = p.ProdID,
                       ProdName = p.ProdName,
                       Price = p.Price
                   })
                   .FirstOrDefault();

            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name";
            }

            return productVM;
        }
    }
}
