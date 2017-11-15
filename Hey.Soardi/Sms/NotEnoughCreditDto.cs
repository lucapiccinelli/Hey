namespace Hey.Soardi.Sms
{
    public class NotEnoughCreditDto
    {
        public double Credit { get; }

        public NotEnoughCreditDto(double credit)
        {
            Credit = credit;
        }
    }
}