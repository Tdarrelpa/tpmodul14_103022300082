// See https://aka.ms/new-console-template for more information
namespace tpmodul14_103022300082
{
    public class Program
    {
        public static void Main()
        {
            var config = new CovidConfig_103022300082();
            config.LoadConfig();

            Console.WriteLine("Selamat datang di aplikasi pemeriksaan Covid-19");
            Console.WriteLine($"Satuan suhu saat ini: {config.Dapat_Satuan_suhu()}");
            Console.Write("Apakah anda ingin mengubah satuan suhu (Y/N) : ");
            string? input = Console.ReadLine();
            
            // Validasi input untuk memastikan hanya 'y' atau 'n' yang diterima
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input tidak valid, silakan masukkan 'Y' atau 'N'.");
                return;
            }

            if (input.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                // Untuk mengantisipasi apabila nilai default dari variabel Satuan_Suhu adalah fahrenheit
                if (config.Dapat_Satuan_suhu() == "celcius")
                {
                    config.UbahSatuanSuhuKeFahrenheit();
                }
                else
                {
                    config.UbahSatuanSuhuKeCelcius();
                }
                Console.WriteLine($"Satuan suhu diubah menjadi: {config.Dapat_Satuan_suhu()}");
            }

            Console.WriteLine("\nPertanyaan");
            Console.Write($"Berapa suhu badan anda saat ini? (dalam nilai {config.Dapat_Satuan_suhu()}): ");
            double suhu = Convert.ToDouble(Console.ReadLine());

            bool isValidTemperature = config.Dapat_Satuan_suhu() == "celcius"
               ? suhu >= 36.5 && suhu <= 37.5
               : suhu >= 97.7 && suhu <= 99.5;

            // Untuk jarak suhu yang lebih jauh dari batas normal
            if (!isValidTemperature)
            {
                Console.WriteLine($"\n{config.Dapat_Pesan(true)}");
                return;
            }

            Console.Write("Berapa hari yang lalu anda terakhir memiliki gejala demam? : ");
            int hari = Convert.ToInt32(Console.ReadLine());
            /* Alternatif untuk
            if (hari <= config.Dapat_Batas_hari_demam())
            {
                Console.WriteLine($"\n{config.Dapat_Pesan(true)}");
            }
            else
            {
                Console.WriteLine($"\n{config.Dapat_Pesan(false)}");
            }
            */
            Console.WriteLine(hari <= config.Dapat_Batas_hari_demam() ? $"\n{config.Dapat_Pesan(true)}"
                : $"\n{config.Dapat_Pesan(false)}");
        }
    }
}