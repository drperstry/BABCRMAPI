namespace BabCrm.Service.Models
{
    public class CustomerMessageDetails: CustomerMessageModel 
    {
        public string ReadByUserId { get; set; }
        
        public string ReadBy { get; set; }

        public DateTime ReadDate { get; set; }

        public string RepliedBy { get; set; }
        
        public string RepliedByUserId { get; set; }

        public DateTime ReplyDate { get; set; }

        public string ConvertedToCaseBy { get; set; }
      
        public string ConvertedToCaseByUserId { get; set; }

        public DateTime ConvertedToCaseDate { get; set; }
    }


}
