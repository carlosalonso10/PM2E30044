namespace PM2E30044carlos.Views;
using Plugin.Maui.Audio;
using PM2E30044carlos.Models;

public partial class Detalle : ContentPage
{

    Notas nota;
    IAudioPlayer player;
    public Detalle(Notas nota)
	{
		InitializeComponent();
        this.nota = nota;
        lblDescripcion.Text = nota.Descripcion;
    }
    //
    private async void OnBtnPictureClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PhotoView(nota.Photo_record));
    }

    //
    private void OnBtnPlayClicked(object sender, EventArgs e)
    {
        //btnPlay.BackgroundColor = Colors.Cyan;
        Stream stream = new MemoryStream(nota.Audio_record);
        player = AudioManager.Current.CreatePlayer(stream);
        player.Play();
    }

    //
    private async void OnBtnEditClicked(object sender, EventArgs e)
    {
        App.nota = this.nota;
        //App.Current.MainPage = new CapturaDatos(AudioManager.Current);
        //await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new PaginaPrincipal(AudioManager.Current));
    }

    //
    private async void OnBtnDeleteClicked(object sender, EventArgs e)
    {
        App.db.Delete(nota.Id_nota);
        await Navigation.PopAsync();
    }

}