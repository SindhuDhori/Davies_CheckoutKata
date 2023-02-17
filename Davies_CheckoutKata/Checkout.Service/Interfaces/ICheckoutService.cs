namespace Checkout.Service.Services
{
    public interface ICheckoutService
    {
        void ScanProducts(char SKU);
        IList<Product> GetScannedProducts();
        double GetTotalPrice();
    }
}
