using JAGualpaS6TC.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace JAGualpaS6TC.Views;

public partial class vCrud : ContentPage
{
    private const string URL_GET_VENTAS = "http://localhost:8080/api/ventas/ver";
    private const string URL_DELETE_VENTA = "http://localhost:8080/api/ventas/eliminar"; 
    private HttpClient cliente = new HttpClient();
    private ObservableCollection<Venta> _ventaTem;

    public vCrud()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        mostrarVenta(); 
    }

    public async void mostrarVenta()
    {
        activityIndicator.IsRunning = true; 
        activityIndicator.IsVisible = true; 

        try
        {
            var content = await cliente.GetStringAsync(URL_GET_VENTAS);
            List<Venta> lista = JsonConvert.DeserializeObject<List<Venta>>(content);
            _ventaTem = new ObservableCollection<Venta>(lista);
            lvVentas.ItemsSource = _ventaTem;
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Error de Conexión", $"No se pudo conectar al servidor: {ex.Message}", "OK");
        }
        catch (JsonSerializationException ex)
        {
            await DisplayAlert("Error de Datos", $"No se pudieron procesar los datos recibidos: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error inesperado: {ex.Message}", "OK");
        }
        finally
        {
            activityIndicator.IsRunning = false; 
            activityIndicator.IsVisible = false; 
        }
    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.vAgregarVenta());
    }

    private async void btnEditar_Clicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
        var ventaSeleccionada = (Venta)button.CommandParameter;

        if (ventaSeleccionada != null)
        {

            await Navigation.PushAsync(new vEditarVenta(ventaSeleccionada));
        }
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var ventaSeleccionada = (Venta)button.CommandParameter;

        if (ventaSeleccionada != null)
        {
            bool confirm = await DisplayAlert("Confirmar Eliminación",
                                              $"¿Está seguro de que desea eliminar la venta con ID: {ventaSeleccionada.Idventa}?",
                                              "Sí", "No");
            if (confirm)
            {
                await EliminarVenta(ventaSeleccionada.Idventa);
            }
        }
    }

    private async Task EliminarVenta(int idVenta)
    {
        try
        {
            // Construye la URL de eliminación con el ID
            string deleteUrl = $"{URL_DELETE_VENTA}/{idVenta}";

            HttpResponseMessage response = await cliente.DeleteAsync(deleteUrl);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Venta eliminada correctamente", "OK");
                // Refrescar la lista después de eliminar
                mostrarVenta();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error al Eliminar", $"No se pudo eliminar la venta. Código: {response.StatusCode} - {errorResponse}", "OK");
            }
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Error de Conexión", $"No se pudo conectar al servidor para eliminar la venta: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", $"Ocurrió un error al intentar eliminar la venta: {ex.Message}", "OK");
        }
    }
}