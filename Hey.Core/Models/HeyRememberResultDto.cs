namespace Hey.Core.Models
{
    public class HeyRememberResultDto
    {
        public HeyRememberStatus Status { get; }
        public string JobId { get; }
        public HeyRememberDto HeyRemember { get; }

        public HeyRememberResultDto(string jobId, HeyRememberDto heyRemember, HeyRememberStatus status)
        {
            JobId = jobId;
            Status = status;
            HeyRemember = heyRemember;
        }
    }
}