using Newtonsoft.Json;

namespace ml_slonce
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();


        }

        public async Task<(string lat, string lon)> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return (location.Latitude.ToString(), location.Longitude.ToString());
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("FeatureNotSupportedException", fnsEx.ToString(), "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("FeatureNotEnabledException", fneEx.ToString(), "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("PermissionException", pEx.ToString(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ex", ex.ToString(), "OK");
            }

            return ("0","0");
        }

        public async void Button_Clicked(object sender, EventArgs e)
        {
                HttpClient client = new HttpClient();
                string apikey = $"https://api.weatherapi.com/v1/astronomy.json?key=e7a040f9ef3d40a89f2214845241101&q={latE.Text},{lonE.Text}";
            try
            {

                var response = await client.GetStringAsync(apikey);
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(response)!;
                DateTime sunrise = DateTime.Parse(DateTime.Now.ToString("d") + " " + dynamicObject.astronomy.astro.sunrise);
                DateTime sunset = DateTime.Parse(DateTime.Now.ToString("d") + " " + dynamicObject.astronomy.astro.sunset);
                TimeSpan DayLength = sunset - sunrise;
                TimeSpan Day = new TimeSpan(24, 0, 0);
                TimeSpan NightLength = Day - DayLength;
                TimeSpan longDay = new TimeSpan(16, 46, 0);
                TimeSpan shortDay = new TimeSpan(7, 20, 0);
                string t = $"Wschód słońca: {sunrise.TimeOfDay.ToString()}\nZachód słońca: {sunset.TimeOfDay.ToString()}\nDługość dnia: {DayLength.ToString()}\nDługość nocy: {NightLength.ToString()}\nRóżnica od najdłuższego dnia: {(longDay-DayLength).ToString()}\nRóżnica od najkrótszego dnia: {(DayLength-shortDay).ToString()}\n\nPodawane w czasie lokalnym";
                lbl.Text = t;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", "Podaj prawidłowe dane", "Ok");
                return;
            }

              

        
        }

        private async void gD_Clicked(object sender, EventArgs e)
        {
            (string lat, string lon) = await GetCachedLocation();
            latE.Text = lat;
            lonE.Text = lon;
        }
    }
}