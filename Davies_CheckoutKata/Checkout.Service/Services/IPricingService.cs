namespace Checkout.Service.Services
{
    public interface IPricingService
    {
        double GetDiscountedPrice(IList<Product> scannedProducts);
        double GetNonDiscountedPrice(IList<Product> scannedProducts);
    }
}
