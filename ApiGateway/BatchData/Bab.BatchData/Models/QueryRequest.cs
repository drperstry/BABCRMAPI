namespace Bab.BatchData.Models
{
    public class QueryRequest
    {
        //[Required]
        public string QueryCondition { get; set; }

        public string[] Parameters { get; set; }
    }
}
