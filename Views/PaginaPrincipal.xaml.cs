namespace PM2E30044carlos.Views;
using Plugin.Maui.Audio;
using PM2E30044carlos.Models;

public partial class PaginaPrincipal : ContentPage
{
    private byte[] fotoArray;
    private byte[] audioArray;
    private readonly IAudioRecorder audioRecorder;
    private PermissionStatus permiso;

    public PaginaPrincipal(IAudioManager audioManager)
	{
		InitializeComponent();
        this.audioRecorder = audioManager.CreateRecorder();
        //
        if (App.nota != null)
        {
            fotoArray = App.nota.Photo_record;
            audioArray = App.nota.Audio_record;
            txtDescripcion.Text = App.nota.Descripcion;

            System.IO.Stream stream = new MemoryStream(fotoArray);
            imgFoto.Source = ImageSource.FromStream(() => stream);

            SetAudioButtonStyle("done_ico.png", Colors.Cyan);
        }
    }
    //
    protected override async void OnAppearing()
    {
        base.OnAppearing();


        permiso = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (permiso == PermissionStatus.Granted)
        {
            return;
        }
        else
        {
            permiso = await Permissions.RequestAsync<Permissions.Microphone>();
        }
    }
    //
    public async void OnBtnFotoClicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo != null)
            {
                try
                {
                    using Stream stream = await photo.OpenReadAsync();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        fotoArray = memoryStream.ToArray();
                    }
                    ImageSource imgSource = StreamImageSource.FromStream(() => new MemoryStream(fotoArray));
                    imgFoto.Source = imgSource;


                }
                catch (Exception ex)
                {
                    await DisplayAlert("Atención", ex.Message, "Aceptar");
                }
            }
        }
    }
    //
    private async void OnBtnStartRecordingClicked(object sender, EventArgs e)
    {
        if (permiso == PermissionStatus.Granted)
        {
            if (!audioRecorder.IsRecording)
            {
                await audioRecorder.StartAsync();
                SetAudioButtonStyle("stop_ico.png", Colors.Red);

            }
            else
            {
                var audio = await audioRecorder.StopAsync();
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream st = audio.GetAudioStream();
                    await st.CopyToAsync(ms);
                    audioArray = ms.ToArray();
                }

                SetAudioButtonStyle("done_ico.png", Colors.Cyan);
            }
        }
        else
        {
            await DisplayAlert("Grabar", "No se concedieron permisos para grabar", "aceptar");
        }
    }
    //
    private async void OnBtnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            Notas nota = new Notas();


            if (App.nota != null)
            {
                nota.Id_nota = App.nota.Id_nota;
                nota.Fecha = DateTime.Now;
            }
            else
            {
                nota.Id_nota = await App.db.NewId();
            }

            nota.Descripcion = txtDescripcion.Text;
            nota.Photo_record = fotoArray;
            nota.Audio_record = audioArray;




            if (!nota.GetDatosInvalidos().Any())
            {
                if (App.nota != null)
                {
                    App.db.Update(nota.Id_nota, nota);
                }
                else
                {
                    await App.db.Insert(nota);
                }
                App.nota = null;
                LimpiarCampos();
                await DisplayAlert("Guardar", "Datos guardados", "aceptar");

            }
            else
            {
                string msj = string.Join("\n", nota.GetDatosInvalidos());
                await DisplayAlert("Datos Invalidos:", msj, "aceptar");
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Guardar", ex.Message, "aceptar");
        }
    }
    //
      private async void OnBtnListaClicked(object sender, EventArgs e) {
        await Navigation.PushAsync(new Listado());
    }
    //
    private void SetAudioButtonStyle(string ico, Color color)
    {
        btnBtnStartRecording.BackgroundColor = color;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = ico;
    }
    //
    public void OnBtnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarCampos();
    }
    //
    private void LimpiarCampos()
    {
        audioArray = new byte[0];
        fotoArray = new byte[0];
        imgFoto.Source = null;
        txtDescripcion.Text = string.Empty;
        SetAudioButtonStyle("microphone_ico.png", Colors.YellowGreen);
    }
}