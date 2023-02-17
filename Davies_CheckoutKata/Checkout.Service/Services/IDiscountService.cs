namespace Checkout.Service.Services
{
    public interface IDiscountService
    {
        IList<DiscountOnQty> GetDiscountPrices();
        double GetDiscountedPrice(IList<Product> scannedProducts);
    }
}
