namespace EXE_BE.Services.Models
{
    public class PayOsPaymentRequestBody
    {
        public long OrderCode { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public string BuyerName { get; set; }
        public string BuyerCompanyName { get; set; }
        public string BuyerTaxCode { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public List<PayOsPaymentItem> Items { get; set; }
        public string CancelUrl { get; set; }
        public string ReturnUrl { get; set; }
        public PayOsPaymentInvoice Invoice { get; set; }
        public long ExpiredAt { get; set; }
        public string Signature { get; set; }
    }

    public class PayOsPaymentItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public string Unit { get; set; }
        public int TaxPercentage { get; set; }
    }

    public class PayOsPaymentInvoice
    {
        public bool BuyerNotGetInvoice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
