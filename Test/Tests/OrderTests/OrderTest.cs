// using System;
// using CinemaluxAPI.Auth;
// using CinemaluxAPI.Common.Enumerations;
// using CinemaluxAPI.DAL.CinemaluxCatalogue;
// using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;
// using CinemaluxAPI.Services;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using NUnit.Framework;
//
// namespace Test.OrderTests
// {
//     public class OrderTests
//     {
//         private CinemaluxDbContext _testDbContext;
//         private IConfiguration _configuration;
//         private OrdersService _ordersService;
//         private Employee _employee;
//         private Identity _employeeSession;
//         
//         [SetUp]
//         public void Setup()
//         {
//             _configuration = Helper.InitConfiguration();
//             _testDbContext = new CinemaluxDbContext(_configuration);
//             _ordersService = new OrdersService(_testDbContext);
//             _employee = new Employee
//             {
//                 Key = "123",
//                 Role = 4,
//                 Name = "Order",
//                 Surname = "Tester",
//                 Username = "OrderTester",
//                 Password = "notrelevant",
//                 BornAt = DateTime.Now,
//                 ContactPhone = "notrelevant",
//                 Address = "notrelevant",
//                 Salary = 200f,
//                 Email = "notrelevant@gmail.com"
//             };
//             
//             _testDbContext.Employees.Add(_employee);
//             _testDbContext.SaveChanges();
//
//             _employeeSession = new Identity
//             {
//                 Id = _employee.Id,
//                 Name = _employee.Name,
//                 Surname = _employee.Surname,
//                 Role = (Role) _employee.Role
//             };
//         }
//         
//         [SetUpAttribute]
//         public void PrepareDatabase()
//         {
//             _testDbContext.Database.ExecuteSqlRaw("DELETE FROM [order];");
//         }
//
//         [Test]
//         public void TestOrderGetter()
//         {
//             Order newOrder = _ordersService.CreateOrder(new CreateOrderDTO
//             {
//                 ReservationId = null,
//                 PaymentTypeCode = null
//             }, _employeeSession);
//             
//             OrderResponseDTO orderResponse = _ordersService.GetOrder((int) newOrder.Id);
//                 
//             Assert.AreEqual(orderResponse.order.CreatedBy, _employee.Username);
//             Assert.AreEqual(orderResponse.order.TotalPrice, 0);
//         }
//         
//         [Test]
//         public void TestCreateWithAllParameters()
//         {
//
//             Order newOrder = _ordersService.CreateOrder(new CreateOrderDTO
//             {
//                 ReservationId = null,
//                 PaymentTypeCode = "CASH",
//                 CreatedBy = _employee.Username
//             }, _employeeSession);
//             
//             OrderResponseDTO orderResponse = _ordersService.GetOrder((int) newOrder.Id);
//                 
//             Assert.AreEqual(orderResponse.order.PaymentTypeCode, "CASH");
//             Assert.AreEqual(orderResponse.order.CreatedBy, _employee.Username);
//             Assert.AreEqual(orderResponse.order.TotalPrice, 0);
//         }
//         
//         //TODO Test order item cascading
//     }
// }