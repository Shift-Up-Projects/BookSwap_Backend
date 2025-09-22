namespace BookSwap.Core.Enums
{
    public enum ExchangeOfferStatus
    {
        Pending =0,   // العرض قيد الانتظار
        Accepted,  // العرض تم قبوله
        Rejected,  // العرض تم رفضه
        Cancelled  // العرض تم إلغاؤه
    }
}
