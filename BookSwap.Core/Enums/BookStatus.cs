using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Core.Enums
{
    public enum BookStatus
    {
        Available = 0,       // الكتاب متاح للتبادل
        Personal = 1,        // الكتاب شخصي (غير معروض للتبادل حاليًا، لكنه يمكن أن يصبح كذلك)
        PendingExchange = 2, // الكتاب في عرض تبادل قيد الانتظار
        Exchanged = 3,       // الكتاب تم تبادله
        Removed = 4        // الكتاب تم إزالته
    }
}
