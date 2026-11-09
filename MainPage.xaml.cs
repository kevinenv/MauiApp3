using System.Numerics;
using System.Threading.Tasks;

namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            TraerArchivo();
            MostrarVector();
        }
        struct TDMecanica
        {
            public string Placa;
            public string Marca;
            public DateTime Fecha;
            public string Tipo;
        }
        TDMecanica[] VMecanica = new TDMecanica[20];
        int Vis = -1;

        private async void Procesar(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Placa.Text))
            {
                await DisplayAlert("Campo Obligatorio", "Debe ingresar una placa", "OK");
                Placa.Focus();
                return;
            }
            GuardarVector();
            MostrarVector();
            GuardarArchivo();
        }
        private void GuardarVector() 
        {
            Vis++;
            VMecanica[Vis].Placa = Placa.Text;
            VMecanica[Vis].Marca = Marca.Text;
            VMecanica[Vis].Fecha = Fecha.Date;
            VMecanica[Vis].Tipo = Tipo.SelectedItem != null ? Tipo.SelectedItem.ToString() : string.Empty;
        }
        private void MostrarVector() { 
        for(int V=0;V<=Vis;V++)
            {
                Dgv.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                string[] Datos =
                {
                    VMecanica[V].Placa,
                    VMecanica[V].Marca,
                    VMecanica[V].Fecha.ToShortDateString(),
                    VMecanica[V].Tipo,
                };
                for (int V2 = 0; V2 < Datos.Length; V2++) 
                {
                    var Lbl = new Label
                    {
                        Text = Datos[V2],
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    Dgv.Add(Lbl, V2, V + 1);
                }
            }
        }
        private void GuardarArchivo() 
        {
            if (Vis < 0)
            {
                DisplayAlert("SIN DATOS", "No hay trabajos en la mecanica", "OK");
                return;
            }
            string Ruta = Path.Combine(FileSystem.AppDataDirectory, "Trabajos.txt");
            var Datos = new System.Text.StringBuilder();
            for (int V = 0; V <= Vis; V++)
                Datos.AppendLine($"{VMecanica[V].Placa}*{VMecanica[V].Marca}*" +
                    $"{VMecanica[V].Fecha:dd-MM-yyyy}*{VMecanica[V].Tipo}*");

            File.WriteAllText(Ruta, Datos.ToString());
           // DisplayAlert("GUADADO EN", $"{Ruta}","OK");
        }
        private async void TraerArchivo()
        {
            string Ruta = Path.Combine(FileSystem.AppDataDirectory, "Trabajos.txt");
            if (!File.Exists(Ruta)) 
            {
                try
                {
                    using Stream ArchivoIncluido = await FileSystem.OpenAppPackageFileAsync("Trabajos.txt");
                    using FileStream ArchivoLocal = File.Create(Ruta);
                    await ArchivoIncluido.CopyToAsync(ArchivoLocal);
                }
                catch (FileNotFoundException)
                {
                    await DisplayAlert("NO HAY ARCHIVO", "No encontre el archivo Trabajos.txt", "OK");
                    return;
                }
            }
            string[] Datos = File.ReadAllLines(Ruta);
            if (Datos.Length < 1) 
            {
                return;
            }
            Vis = -1;
            for (int V = 0; V < Datos.Length; V++) 
            {
                string[] Separados = Datos[V].Split('*');
                Vis++;
                VMecanica[Vis].Placa = Separados[0];
                VMecanica[Vis].Marca = Separados[1];
                VMecanica[Vis].Fecha = Convert.ToDateTime(Separados[2]);
                VMecanica[Vis].Tipo = Separados[3];
            }
        }
    }
}















