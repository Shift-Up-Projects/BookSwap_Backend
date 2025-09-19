namespace BookSwap.Data.Result
{
    namespace BookSwap.Core.Results
    {
        // تعريف أنواع الفشل المحتملة
        public enum ResultFailureType
        {
            None = 0,
            NotFound = 1,
            BadRequest = 2, // لأخطاء قواعد العمل أو المدخلات
            Unauthorized = 3,
            Forbidden = 4,
            Conflict = 5,
            InternalError = 6 // للأخطاء غير المتوقعة
        }
    }
}
