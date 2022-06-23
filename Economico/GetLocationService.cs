using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Economico
{
	public class GetLocationService
	{
		readonly bool stopping = false; // PARA ROMPER EL CICLO DE OBTENCIO DE UBICACION
		public GetLocationService()
		{
		}

		public async Task Run(CancellationToken token)
		{
			await Task.Run(async () => {
				while (!stopping)
				{
					// CADA VEZ QUE SE LE PASE UN TOKEN CE CANCELACION, ESTE LANZARA UNA EXCEPCION
					// ES DE ESA FORMA QUE SE DETENDRA LA ITERACION
					token.ThrowIfCancellationRequested();
					try
					{
						await Task.Delay(2000);

						var request = new GeolocationRequest(GeolocationAccuracy.High);
						var location = await Geolocation.GetLocationAsync(request);
						if (location != null)
						{
							var message = new LocationMessage
							{
								Latitude = location.Latitude,
								Longitude = location.Longitude
							};

							Device.BeginInvokeOnMainThread(() =>
							{
								// MessagingCenter = PERMITE ENVIAR LA INFORMACION EN SEGUNDO PLANO Y ASI UTILIZARLA
								MessagingCenter.Send(message, "Location");
							});
						}
					}
					catch (Exception)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							var errormessage = new LocationErrorMessage();
							MessagingCenter.Send(errormessage, "LocationError");
						});
					}
				}
				return;
			}, token);
		}
	}
}
