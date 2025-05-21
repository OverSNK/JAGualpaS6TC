using System.Net;

namespace JAGualpaS6TC.Views;

public partial class vAgregarVenta : ContentPage
{
	public vAgregarVenta()
	{
		InitializeComponent();
	}

    private void btnAddVenta_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("idVenta", txtIdventa.Text);
            parametros.Add("idCliente", txtIdcliente.Text);
            parametros.Add("idProducto", txtIdproducto.Text);
            parametros.Add("fecha", txtFecha.Text); // Asegúrate de que sea en formato válido: "yyyy-MM-dd"
            parametros.Add("descripcion", txtDescripcion.Text);
            parametros.Add("cantidad", txtCantidad.Text);
            parametros.Add("precio", txtPrecio.Text);
            cliente.UploadValues("http://localhost:8080/api/ventas/guardar", "POST", parametros);
            Navigation.PushAsync(new vCrud());
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }
}