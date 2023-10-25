using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asis_Batia.Helpers
{
    public static class LocationService
    {
        public static async Task<string> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return "None";
        }

        private static CancellationTokenSource _cancelTokenSource;
        public static bool _isCheckingLocation;

        public static string Message { get; set; }

        public static async Task<bool> CheckMock()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
            Location location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null && location.IsFromMockProvider)
            {
                return true;
            }
            else
                return false;
        }

        public static async Task<Location> GetCurrentLocation()
        {
            try
            {
                if (await CheckMock())
                {
                    Message = "Detectamos el uso de aplicaciones de simulación de ubicación. Por favor, desactívalas para continuar utilizando la aplicación correctamente.";
                    return null;
                }
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return location;
            }
            catch (FeatureNotEnabledException ex)
            {
                Message = "Los servicios de localización no están activos, por favor active el GPS";
                return null;
            }
            catch (PermissionException ex)
            {
                // Handle permission exception               
                Message = "No se concedió permiso a la aplicación para usar su ubicación, permita el acceso a su ubicación en las configuraiónes del dispositivo";
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Message = "No se puede obtener la ubicación";
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public static void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

        public static double CalcularDistancia(Location origin, Location destination)
        {
            return Location.CalculateDistance(origin, destination, DistanceUnits.Kilometers);
        }
    }
}
