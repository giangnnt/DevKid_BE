using DevKid.src.Application.Dto;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DevKid.src.Application.Service
{
    public interface IPayOSService
    {
        public Task<GeneratePaymentUrlResponse> GeneratePaymentUrl(Course course, Guid StudentId);
        public bool ValidateSignature(PayOSWebhookModel webhookData);
        public class PayOSService : IPayOSService
        {
            private readonly IOrderRepo _orderRepo;
            private readonly IConfiguration _configuration;
            private readonly IUserRepo _userRepo;
            public PayOSService(IOrderRepo orderRepo, IConfiguration configuration, IUserRepo userRepo)
            {
                _orderRepo = orderRepo;
                _configuration = configuration;
                _userRepo = userRepo;
            }
            public async Task<GeneratePaymentUrlResponse> GeneratePaymentUrl(Course course, Guid StudentId)
            {
                var clientId = _configuration["PayOS:CLIENT_ID"] ?? throw new Exception("client id is null");
                var apiKet = _configuration["PayOS:API_KEY"] ?? throw new Exception("api key is null");
                var checkSumKey = _configuration["PayOS:CHECKSUM_KEY"] ?? throw new Exception("checksum key is null");
                var payOS = new PayOS(clientId, apiKet, checkSumKey);
                var paymentLinkRequest = new PaymentData(
                    orderCode: (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000,
                    amount: course.Price,
                    description: "Thanh toan don hang",
                    items: [new ItemData(course.Name, 1, course.Price)],
                    cancelUrl: "http://localhost:5173/checkout",
                    returnUrl: "http://localhost:5173/payment-success"
                    );
                var response = await payOS.createPaymentLink(paymentLinkRequest);
                // create pending order
                var user = await _userRepo.GetUser(StudentId);
                if (user == null)
                {
                    throw new Exception("Student not found");
                }
                var order = new Order
                {
                    Id = response.orderCode,
                    StudentId = user.Id,
                    CourseId = course.Id,
                    Price = course.Price,
                    Status = Order.StatusEnum.Pending
                };
                var result = await _orderRepo.AddOrder(order);
                if (!result)
                {
                    throw new Exception("Order not created");
                }
                return new GeneratePaymentUrlResponse
                {
                    PaymentLink = response.checkoutUrl,
                    QrCode = response.qrCode
                };
            }

            public bool ValidateSignature(PayOSWebhookModel webhookData)
            {
                var checksumKey = Environment.GetEnvironmentVariable("CHECKSUM_KEY") ?? throw new Exception("checksum key is null");
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(webhookData.Data)) ?? throw new Exception("dataDict is null");

                var sortedData = dataDict.OrderBy(kv => kv.Key)
                                         .Where(kv => kv.Key != null)
                                         .Select(kv => $"{kv.Key}={kv.Value}");

                string dataString = string.Join("&", sortedData);

                string computedSignature = ComputeHmacSha256(dataString, checksumKey);
                return computedSignature == webhookData.Signature;

            }
            private string ComputeHmacSha256(string data, string key)
            {
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }

        }
    }
}
