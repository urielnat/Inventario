using AppBarCode.servicios;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(AppBarCode.Droid.Service.QrCodeScanningService))]

namespace AppBarCode.Droid.Service
{
	public class QrCodeScanningService : IQrCodeScanningService
	{
		public async Task<string> ScanAsync()
		{
			var optionsDefault = new MobileBarcodeScanningOptions();
			var optionsCustom = new MobileBarcodeScanningOptions()
			{
				//UseFrontCameraIfAvailable = true,
				//Check diferents formats in http://barcode.tec-it.com/en
				// PossibleFormats = new List<ZXing.BarcodeFormat> {  ZXing.BarcodeFormat.CODE_128 }
			};
			var scanner = new MobileBarcodeScanner()
			{
				TopText = "Acerca la cámara al elemento",
				BottomText = "Toca la pantalla para enfocar"
			};

			var scanResults = await scanner.Scan(optionsCustom);

			//Fix by Ale 2017-07-06
			return (scanResults != null) ? scanResults.Text : string.Empty;
		}
	}
}
