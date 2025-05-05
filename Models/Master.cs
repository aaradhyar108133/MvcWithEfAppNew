namespace MvcWithEfApp.Models
{
    public class Master
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
        public double? TOTALAMT { get; set; }
        public double? ADDAMT { get; set; }
        public double? SALEAMT { get; set; }
        public int? BILLID { get; set; }
        public decimal? LCARDDESC { get; set; }
        public DateTime? CDATE { get; set; }
    }

    public class MainModel
    {
        public List<Master> masterData { get; set; }

        public List<LCardTempDET> lCardTempdetData { get; set; }

    }
}
