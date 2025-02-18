using System.Text.Json.Serialization;

namespace DevKid.src.Application.Dto
{
    public class PayOSWebhookModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } // Mã lỗi
        [JsonPropertyName("desc")]
        public string Desc { get; set; } // Thông tin lỗi
        [JsonPropertyName("success")]
        public bool Success { get; set; } // Trạng thái thành công
        [JsonPropertyName("data")]
        public PayOSWebhookData Data { get; set; } // Dữ liệu chi tiết
        [JsonPropertyName("signature")]
        public string Signature { get; set; } // Chữ ký để kiểm tra
    }

    public class PayOSWebhookData
    {
        [JsonPropertyName("orderCode")]
        public int OrderCode { get; set; } // Mã đơn hàng
        [JsonPropertyName("amount")]
        public int Amount { get; set; } // Số tiền
        [JsonPropertyName("description")]
        public string Description { get; set; } // Mô tả giao dịch
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } // Số tài khoản giao dịch
        [JsonPropertyName("reference")]
        public string Reference { get; set; } // Mã tham chiếu
        [JsonPropertyName("transactionDateTime")]
        public string transactionDateTime { get; set; } // Thời gian giao dịch
        [JsonPropertyName("currency")]
        public string Currency { get; set; } // Loại tiền tệ
        [JsonPropertyName("paymentLinkId")]
        public string PaymentLinkId { get; set; } // ID liên kết thanh toán
        [JsonPropertyName("code")]
        public string Code { get; set; } // Mã trạng thái giao dịch
        [JsonPropertyName("desc")]
        public string Desc { get; set; } // Mô tả trạng thái giao dịch
        [JsonPropertyName("counterAccountBankId")]
        public string CounterAccountBankId { get; set; } // ID ngân hàng đối ứng
        [JsonPropertyName("counterAccountBankName")]
        public string CounterAccountBankName { get; set; } // Tên ngân hàng đối ứng
        [JsonPropertyName("counterAccountName")]
        public string CounterAccountName { get; set; } // Tên tài khoản đối ứng
        [JsonPropertyName("counterAccountNumber")]
        public string CounterAccountNumber { get; set; } // Số tài khoản đối ứng
        [JsonPropertyName("virtualAccountName")]
        public string VirtualAccountName { get; set; } // Tên tài khoản ảo
        [JsonPropertyName("virtualAccountNumber")]
        public string VirtualAccountNumber { get; set; } // Số tài khoản ảo
    }

}
