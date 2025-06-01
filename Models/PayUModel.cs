using System.Text.Json.Serialization;

namespace CardPayment.Models
{
    public class PayUModelRequest
    {
        public decimal Card { get; set; }
        public int Amount { get; set; }

        // PayU related fields
        public string Key { get; set; }
        public string TxnId { get; set; }
        public string AmountStr { get; set; }
        public string ProductInfo { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Surl { get; set; }
        public string Furl { get; set; }
        public string Hash { get; set; }
    }

    public class PayUModelResponse
    {
        public string message { get; set; }
        public string PayUUrl { get; set; }
        public bool status { get; set; }
    }
}
