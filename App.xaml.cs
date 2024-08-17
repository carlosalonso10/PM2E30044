using Plugin.Maui.Audio;
using PM2E30044carlos.Views;
using PM2E30044carlos.Controllers;
using PM2E30044carlos.Models;

namespace PM2E30044carlos
{
    public partial class App : Application
    {
        public static FirebaseController db = new FirebaseController();
        public static Notas nota;

        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new PaginaPrincipal(AudioManager.Current));

        }
    }
}
