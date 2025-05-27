using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace JAGualpaS6TC.Views;

public partial class vAgregarVenta : ContentPage
{
    public vAgregarVenta()
    {
        InitializeComponent();
    }

    private async void btnAddVenta_Clicked(object sender, EventArgs e)
    {
        try
        {
            var ventaData = new
            {

                idCliente = string.IsNullOrEmpty(txtIdcliente.Text) ? 0 : int.Parse(txtIdcliente.Text),
                idProducto = string.IsNullOrEmpty(txtIdproducto.Text) ? 0 : int.Parse(txtIdproducto.Text),
                fecha = dpFecha.Date.ToString("yyyy-MM-dd"), 

                descripcion = txtDescripcion.Text,
                cantidad = string.IsNullOrEmpty(txtCantidad.Text) ? 0 : int.Parse(txtCantidad.Text),
                precio = string.IsNullOrEmpty(txtPrecio.Text) ? 0 : int.Parse(txtPrecio.Text) 
            };

            string jsonContent = JsonSerializer.Serialize(ventaData);

            Console.WriteLine($"JSON enviado desde MAUI: {jsonContent}");

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://localhost:8080/api/ventas/guardar", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Venta añadida correctamente", "OK");
                    await Navigation.PushAsync(new vCrud());
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error del servidor", $"El servidor devolvió un error: {response.StatusCode} - {errorResponse}", "OK");
                }
            }
        }
        catch (FormatException fex)
        {
            await DisplayAlert("Error de formato", "Por favor, asegúrese de que ID Cliente, ID Producto, Cantidad y Precio sean números válidos.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", ex.Message, "OK");
        }
    }
}