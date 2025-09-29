using System.Net.Http.Json;
using System.Text.Json;

namespace bgk.Services;

public record PaymentRequest(decimal Amount, string Description, string RedirectUrl);
public record PaymentResponse(string Id, string Status, string CheckoutUrl);

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(decimal amount, string description, string redirectUrl);
    Task<PaymentResponse> GetPaymentStatusAsync(string paymentId);
}

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CreatePaymentAsync(decimal amount, string description, string redirectUrl)
    {
        try
        {
            var paymentRequest = new PaymentRequest(amount, description, redirectUrl);

            // In a real implementation, this would call your backend API
            // For now, return a mock URL
            await Task.Delay(500); // Simulate API call

            return "https://www.mollie.com/checkout/test-payment-url";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating payment for amount: {amount}. Error: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentResponse> GetPaymentStatusAsync(string paymentId)
    {
        try
        {
            // In a real implementation, this would call your backend API
            // For now, return a mock response
            await Task.Delay(500); // Simulate API call

            return new PaymentResponse(paymentId, "paid", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving payment status for ID: {paymentId}. Error: {ex.Message}");
            throw;
        }
    }
}