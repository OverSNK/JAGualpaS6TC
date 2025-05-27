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
            // NOTA IMPORTANTE:
            // 1. NO ENVIAR idVenta si el backend lo genera autom�ticamente.
            // 2. Convertir idCliente e idProducto a int si la entidad Java los espera como int.
            // 3. Convertir precio a int si la entidad Java lo espera como int.
            // 4. El formato de fecha debe ser el que tu backend (java.util.Date) espera.

            var ventaData = new
            {
                // idVenta: ELIMINADO - NO SE DEBE ENVIAR SI ES GENERADO AUTOM�TICAMENTE POR LA BD

                // Aseg�rate de que los campos no est�n vac�os antes de convertir a int
                idCliente = string.IsNullOrEmpty(txtIdcliente.Text) ? 0 : int.Parse(txtIdcliente.Text),
                idProducto = string.IsNullOrEmpty(txtIdproducto.Text) ? 0 : int.Parse(txtIdproducto.Text),

                // Formato de fecha: Por ahora, mant�n yyyy-MM-dd. Si sigue fallando por la fecha, investiga m�s.
                fecha = dpFecha.Date.ToString("yyyy-MM-dd"), // Este formato es com�nmente aceptado.
                                                             // Si tu backend requiere un formato m�s completo (con hora),
                                                             // prueba "yyyy-MM-ddTHH:mm:ss.fffZ" o similar.

                descripcion = txtDescripcion.Text,
                cantidad = string.IsNullOrEmpty(txtCantidad.Text) ? 0 : int.Parse(txtCantidad.Text),
                precio = string.IsNullOrEmpty(txtPrecio.Text) ? 0 : int.Parse(txtPrecio.Text) // Cambiado a int.Parse
            };

            string jsonContent = JsonSerializer.Serialize(ventaData);

            Console.WriteLine($"JSON enviado desde MAUI: {jsonContent}");

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://localhost:8080/api/ventas/guardar", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("�xito", "Venta a�adida correctamente", "OK");
                    await Navigation.PushAsync(new vCrud());
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error del servidor", $"El servidor devolvi� un error: {response.StatusCode} - {errorResponse}", "OK");
                }
            }
        }
        catch (FormatException fex)
        {
            await DisplayAlert("Error de formato", "Por favor, aseg�rese de que ID Cliente, ID Producto, Cantidad y Precio sean n�meros v�lidos.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", ex.Message, "OK");
        }
    }
}