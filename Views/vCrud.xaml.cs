using JAGualpaS6TC.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace JAGualpaS6TC.Views;

public partial class vCrud : ContentPage
{
	private const string URL = "http://localhost:8080/api/ventas/ver";
	private HttpClient cliente = new HttpClient();
	private ObservableCollection<Venta> _ventaTem;
    public DDvCrud()
	{
		InitializeComponent();
		mostrarVenta();
    }
	public async void mostrarVenta()
	{
		var content = await cliente.GetStringAsync(URL);
		List<Venta> lista = JsonConvert.DeserializeObject<List<Venta>>(content);
        _ventaTem = new ObservableCollection<Venta>(lista);
		lvVentas.ItemsSource = _ventaTem;
    }
}