using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace bgk.Services;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(decimal amount, string description, string redirectUrl);
    Task<PaymentResponse> GetPaymentStatusAsync(string paymentId);
}

public class PaymentService : IPaymentService
{
    private readonly IPaymentClient _paymentClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentClient paymentClient, IConfiguration configuration, ILogger<PaymentService> logger)
    {
        _paymentClient = paymentClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> CreatePaymentAsync(decimal amount, string description, string redirectUrl)
    {
        try
        {
            var paymentRequest = new PaymentRequest
            {
                Amount = new Amount(Currency.EUR, amount),
                Description = description,
                RedirectUrl = redirectUrl,
                Method = PaymentMethod.Ideal // Default to iDEAL for Danish customers
            };

            var paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);

            _logger.LogInformation("Payment created with ID: {PaymentId}", paymentResponse.Id);

            return paymentResponse.Links.Checkout.Href;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for amount: {Amount}", amount);
            throw;
        }
    }

    public async Task<PaymentResponse> GetPaymentStatusAsync(string paymentId)
    {
        try
        {
            var payment = await _paymentClient.GetPaymentAsync(paymentId);
            _logger.LogInformation("Retrieved payment status: {Status} for ID: {PaymentId}", payment.Status, paymentId);
            return payment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment status for ID: {PaymentId}", paymentId);
            throw;
        }
    }
}