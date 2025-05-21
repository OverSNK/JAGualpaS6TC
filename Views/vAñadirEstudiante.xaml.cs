using System.Net;

namespace JAGualpaS6TC.Views;

public partial class vAñadirEstudiante : ContentPage
{
	public vAñadirEstudiante()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();



            cliente.UploadValues("http://localhost:8080/api/ventas/guardar", "POST", parametros);
            Navigation.PushAsync(new vCrud());
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }
}