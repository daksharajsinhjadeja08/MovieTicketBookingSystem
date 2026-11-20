namespace MOVIETICKETBOOKINGSYSTEM.Interfaces;

public interface IPaymentService
{
    void ProcessPayment(decimal amount);
}