using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

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
    public class PaymentRequest
    {
        public bool Status { get; set; }
        public string JsonRes { get; set; }
        public string JsonReq { get; set; }
    }
    public class PayUModelResponse
    {
        public string message { get; set; }
        public string PayUUrl { get; set; }
        public bool status { get; set; }
    }

    public class PaymentLogs
    {
        [Key]
        public int UserID { get; set; } // EF will treat this as identity

        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string PaymentStatus { get; set; }
        public string PayUJson { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
