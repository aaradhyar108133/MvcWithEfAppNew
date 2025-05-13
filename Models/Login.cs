namespace CardPayment.Models
{
    public class Login
    {
        public int AID { get; set; }
        public string? ACCTYPE { get; set; }
        public string? ACCOUNT { get; set; }
        public string? ADD2 { get; set; }
         public string? ADD1 { get; set; }
        public string? ADD3 { get; set; }
         public string? TELEPHONE { get; set; }
          public string? REF { get; set; }
         public string? CCno { get; set; }
    }
    public class LCardTempDET
    {
        public int? DLCARDID { get; set; }
        public double? BALAMT { get; set; }
        public double? TOTALAMT { get; set; }
        public double? ADDAMT { get; set; }
        public double? SALEAMT { get; set; }
        public int? BILLID { get; set; }
        public decimal? LCARDDESC { get; set; }
        public DateTime? CDATE { get; set; }
    }
    public class LOYALTYCARD
    {
        public int? LCARDID { get;set; }
        public decimal? LCARDDESC { get;set; }
        public bool? ACTIVATE { get;set; }

    }
    public class MainModel
    {
        public List<Login> masterData { get; set; }

        public List<LCardTempDET> lCardTempdetData { get; set; }
        public List<LOYALTYCARD> loyaltyCard { get; set; }

    }
    
}
