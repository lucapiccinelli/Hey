namespace Hey.Soardi.Sms
{
    public interface INotEnoughCredit
    {
        void Handle(NotEnoughCreditDto notEnoughCreditDto);
    }
}