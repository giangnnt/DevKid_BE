using DevKid.src.Domain.Entities;
using Net.payOS;
using Net.payOS.Types;
using System;

namespace DevKid.src.Application.ExternalService
{
    public interface IPayOSService
    {
        public Task<string> GeneratePaymentUrl(Course course);
        public class PayOSService : IPayOSService
        {
            public async Task<string> GeneratePaymentUrl(Course course)
            {
                var clientId = Environment.GetEnvironmentVariable("CLIENT_ID") ?? throw new Exception("client id is null");
                var apiKet = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("api key is null");
                var checkSumKey = Environment.GetEnvironmentVariable("CHECKSUM_KEY") ?? throw new Exception("checksum key is null");

                var payOS = new PayOS(clientId, apiKet, checkSumKey);
                var paymentLinkRequest = new PaymentData(
                    orderCode: (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000,
                    amount: course.Price,
                    description: "Thanh toan don hang",
                    items: [new ItemData(course.Name, 1, course.Price)],
                    cancelUrl: "https://devkid.com.vn/cancel",
                    returnUrl: "https://devkid.com.vn/success"
                    );
                var response = await payOS.createPaymentLink(paymentLinkRequest);
                return response.checkoutUrl;
            }
        }
    }
}
