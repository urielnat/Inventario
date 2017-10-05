using System.Threading.Tasks;

namespace AppBarCode.servicios
{
	public interface IQrCodeScanningService
	{
		Task<string> ScanAsync();
	}
}
