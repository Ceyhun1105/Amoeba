namespace Amoeba.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> values, int allpagecount, int elementcount , int activepage)
        {
            this.AddRange(values);
            TotalPageCount = (int)Math.Ceiling(allpagecount /(double)elementcount);
            ActivePage= activepage; 
            ElementCount = elementcount;
        }

        public int TotalPageCount { get; set; }
        public int ActivePage { get; set; }
        public int ElementCount { get; set; }

        public bool HasPrevious { get => ActivePage > 1; }
        public bool HasNext { get => ActivePage < TotalPageCount; }



        public static PaginatedList<T> Create(IQueryable<T> query, int activepage, int elementcount)
        {
            return new PaginatedList<T>(query.Skip((activepage - 1) * elementcount).Take(elementcount).ToList(), query.Count(), elementcount, activepage);

        }
    }
}
