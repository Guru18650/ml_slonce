using Newtonsoft.Json;
using Innovative.SolarCalculator;

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
            TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            SolarTimes solarTimes = new SolarTimes(DateTime.Now.Date, double.Parse(latE.Text), double.Parse(lonE.Text));
            DateTime sunrise = solarTimes.Sunrise.ToUniversalTime();
            DateTime sunset = solarTimes.Sunset.ToUniversalTime();

            try
            {

                TimeSpan DayLength = sunset - sunrise;
                TimeSpan Day = new TimeSpan(24, 0, 0);
                TimeSpan NightLength = Day - DayLength;
                TimeSpan longDay = new TimeSpan(16, 46, 0);
                TimeSpan shortDay = new TimeSpan(7, 20, 0);
                string t = $"Wschód słońca: {sunrise.TimeOfDay.ToString("hh':'mm':'ss''")}\nZachód słońca: {sunset.TimeOfDay.ToString("hh':'mm':'ss''")}\nDługość dnia: {DayLength.ToString("h'h 'm'm 's's'")}\nDługość nocy: {NightLength.ToString("h'h 'm'm 's's'")}\nRóżnica od najdłuższego dnia: {(longDay-DayLength).ToString("h'h 'm'm 's's'")}\nRóżnica od najkrótszego dnia: {(DayLength-shortDay).ToString("h'h 'm'm 's's'")}\n\nPodawane w czasie UTC (Polska jest +1 ze względu na zmiane czasu)";
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