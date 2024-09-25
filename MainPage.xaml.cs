using System.Runtime.CompilerServices;

namespace Module04Activity01
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetLocationClicked(Object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";

                    //Get Geocoding - Get address frmo latitude and longitude

                    var placemarks = await Geocoding.Default.GetPlacemarksAsync
                        (location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        AddressLabel.Text = $"Address:" +
                            $"{placemark.Thoroughfare}," +
                            $"{placemark.Locality}," +
                            $"{placemark.AdminArea}," +
                            $"{placemark.PostalCode}," +
                            $"{placemark.CountryName}";
                    }
                    else
                    {
                        AddressLabel.Text = "Unable to determine address";
                    }
                    //end of Geocoding
                }
                else
                {
                    LocationLabel.Text = "Unable to get location";
                }
            }
            catch (Exception ex)
            {
                LocationLabel.Text = $"Error: {ex.Message}";
            }
        }

        private async void OnCapturePhotoClicked(Object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    //Capture a photo using MediaPicker
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        await LoadPhotoAsync(photo);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error has occured: {ex.Message}", "OK");
            }
        }

        //Load photo and display it in the Image control
        private async Task LoadPhotoAsync(FileResult photo)
        {
            if (photo == null)
                return;

            Stream stream = await photo.OpenReadAsync();

            CaptureImage.Source = ImageSource.FromStream(() => stream);
        }

    }
}
