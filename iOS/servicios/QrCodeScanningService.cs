﻿using AppBarCode.servicios;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(AppBarCode.iOS.Service.QrCodeScanningService))]
namespace AppBarCode.iOS.Service
{
	public class QrCodeScanningService : IQrCodeScanningService
	{
		public async Task<string> ScanAsync()
		{
			var scanner = new MobileBarcodeScanner();
			var scanResults = await scanner.Scan();
			//Fix by Ale
			return (scanResults != null) ? scanResults.Text : string.Empty;

		}
	}
}
