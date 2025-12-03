using System.Collections.Generic;

namespace EyeCareHub.API.Helper
{
    public class Pagination<T>
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public IReadOnlyList<T> data { get; set; }

        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.count = count;
            this.data = data;
        }
        
    }
}
