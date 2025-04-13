using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tpmodul14_103022300082
{
    public class Rootobject
    {
        [JsonPropertyName("satuan_suhu")]
        public string Satuan_Suhu { get; set; } = "celcius";
        [JsonPropertyName("batas_hari_demam")]
        public int Batas_Hari_Demam { get; set; } = 14;
        [JsonPropertyName("pesan_ditolak")]
        public string Pesan_Ditolak { get; set; } = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
        [JsonPropertyName("pesan_diterima")]
        public string Pesan_Diterima { get; set; } = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
    }

    public class CovidConfig_103022300082
    {
        private readonly Rootobject objekJSON;

        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "covid_config.json");

        public CovidConfig_103022300082()
        {
            objekJSON = new Rootobject();
        }

        public void UbahSatuanSuhuKeFahrenheit()
        {

            objekJSON.Satuan_Suhu = objekJSON.Satuan_Suhu == "celcius" ? "fahrenheit" : "celcius";
            SaveConfig();
        }

        public void LoadConfig()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File tidak ditemukan, membuat file baru");
                SaveConfig();
            }

            try
            {
                var configJson = File.ReadAllText(FilePath);
                var configFromFile = JsonSerializer.Deserialize<CovidConfig_103022300082>(configJson) ?? throw new Exception("Gagal memuat konfigurasi dari file JSON.");
                objekJSON.Satuan_Suhu = configFromFile.Dapat_Satuan_suhu();
                objekJSON.Batas_Hari_Demam = configFromFile.Dapat_Batas_hari_demam();
                objekJSON.Pesan_Ditolak = configFromFile.Dapat_Tolak_Pesanan();
                objekJSON.Pesan_Diterima = configFromFile.Dapat_Terima_Pesanan();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
            }
        }

        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
        };

        public void SaveConfig()
        {
            try
            {
                // var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(FilePath, jsonString);
                Console.WriteLine("Berhasil menyimpan konfigurasi baru");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal menyimpan konfigurasi baru: {ex.Message}");
            }
        }

        // 3 metode getter ini dipanggil dalam main, jadi harus dibuat public
        public string Dapat_Pesan(bool isRejected)
        {return isRejected ? objekJSON.Pesan_Ditolak : objekJSON.Pesan_Diterima;}

        public string Dapat_Satuan_suhu(){return objekJSON.Satuan_Suhu;}
        public int Dapat_Batas_hari_demam(){return objekJSON.Batas_Hari_Demam;}

        // 2 metode getter ini tidak dipanggil dalam main, jadi bisa dibuat private
        private string Dapat_Terima_Pesanan(){return objekJSON.Pesan_Diterima;}
        private string Dapat_Tolak_Pesanan(){return objekJSON.Pesan_Ditolak;}

        // Antisipasi apabila nilai default dari variabel Satuan_Suhu adalah fahrenheit
        public void UbahSatuanSuhuKeCelcius()
        {
            objekJSON.Satuan_Suhu = objekJSON.Satuan_Suhu == "fahrenheit" ? "celcius" : "fahrenheit";
            SaveConfig();
        }
    }
}