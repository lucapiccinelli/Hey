namespace Hey.Core.Models
{
    public class HeyRememberResultDto
    {
        public HeyRememberStatus Status { get; }
        public HeyRememberDto HeyRemember { get; }

        public HeyRememberResultDto(HeyRememberDto heyRemember, HeyRememberStatus status)
        {
            Status = status;
            HeyRemember = heyRemember;
        }
    }
}