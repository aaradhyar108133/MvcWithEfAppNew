using System.Text.Json.Serialization;

namespace CardPayment.Models
{
    public class PayUModelRequest
    {
        public string Key { get; set; }
        public string TxnId { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName ("cardId")]
        public string CardId { get; set; }

        public string ProductInfo { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Surl { get; set; }  // Success URL
        public string Furl { get; set; }  // Failure URL
        public string Hash { get; set; }
        public string ServiceProvider { get; set; } = "payu_paisa";
    }

    public class PayUModelResponse
    {
        public string message { get; set; }
        public bool status { get; set; }
    }
}
