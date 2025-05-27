using JAGualpaS6TC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace JAGualpaS6TC.Views;

public partial class vEditarVenta : ContentPage
{
    private const string URL_UPDATE_VENTA_BASE = "http://localhost:8080/api/ventas/actualizar"; 
    private readonly Venta _ventaOriginal;
    private static readonly HttpClient _httpClient = new HttpClient();

    public vEditarVenta(Venta venta)
    {
        InitializeComponent();
        _ventaOriginal = venta;
        CargarDatosVenta();
    }

    private void CargarDatosVenta()
    {
        if (_ventaOriginal != null)
        {
            txtIdventa.Text = _ventaOriginal.Idventa.ToString();
            txtIdcliente.Text = _ventaOriginal.Idcliente.ToString();
            txtIdproducto.Text = _ventaOriginal.Idproducto.ToString();
            dpFecha.Date = _ventaOriginal.Fecha;
            txtDescripcion.Text = _ventaOriginal.Descripcion;
            txtCantidad.Text = _ventaOriginal.Cantidad.ToString();
            txtPrecio.Text = _ventaOriginal.Precio.ToString();
        }
    }

    private async void btnActualizarVenta_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtIdcliente.Text, out int idCliente) ||
                !int.TryParse(txtIdproducto.Text, out int idProducto) ||
                !int.TryParse(txtCantidad.Text, out int cantidad) ||
                !double.TryParse(txtPrecio.Text, out double precio)) 
            {
                await DisplayAlert("Error de Entrada", "Por favor, ingrese números válidos para ID Cliente, ID Producto, Cantidad y Precio.", "OK");
                return;
            }

            var ventaActualizada = new
            {

                idCliente = idCliente,
                idProducto = idProducto,
                fecha = dpFecha.Date.ToString("yyyy-MM-dd"),
                descripcion = txtDescripcion.Text,
                cantidad = cantidad,
                precio = precio
            };

            string jsonContent = JsonSerializer.Serialize(ventaActualizada);
            Console.WriteLine($"JSON para actualizar enviado desde MAUI: {jsonContent}");

            string fullUpdateUrl = $"{URL_UPDATE_VENTA_BASE}/{_ventaOriginal.Idventa}";

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(fullUpdateUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Venta actualizada correctamente", "OK");
                await Navigation.PopAsync(); 
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error del servidor", $"El servidor devolvió un error al actualizar: {response.StatusCode} - {errorResponse}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", $"Ocurrió un error al intentar actualizar la venta: {ex.Message}", "OK");
        }
    }
}