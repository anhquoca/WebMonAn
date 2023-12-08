namespace ApiHoaDon.Helper
{
    public class PageResult<T>
    {
        public Pagination pagination { get; set; }
        public IEnumerable<T> listData { get; set; }
        public PageResult()
        {

        }
        public PageResult(Pagination pagination, IEnumerable<T> listData)
        {
            this.pagination = pagination;
            this.listData = listData;
        }

        public static IEnumerable<T> ToPageResult(Pagination pagination, IEnumerable<T> listData)
        {
            pagination.PageNumber=pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
            
            if (pagination.PageSize > 0)
            {
                 listData= listData.Skip(pagination.PageSize*(pagination.PageNumber-1)).Take(pagination.PageSize);
            }
            
            return listData;
           
            

        }
    }
}
