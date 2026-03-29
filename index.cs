using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

class Program
{
    static string bancoSqlite = "Data Source=IndiceBusca.db";

    static void Main()
    {
        MontarBancoDados();

        var caminhoBanco = @""; //tem que colocar o diretorio do banco chamado IndiceBusca.db aqui

        var arquivosBusca = new string[]
        {
            Path.Combine(caminhoBanco, "documentos"),
            Path.Combine(caminhoBanco, "backup"),
            Path.Combine(caminhoBanco, "fotos")
        };

        Console.WriteLine("=== INICIANDO INDEXAÇÃO NO SQLITE (CONCORRENTE) ===");

        Parallel.ForEach(arquivosBusca, arquivo =>
        {
            if (Directory.Exists(arquivo))
            {
                Console.WriteLine($"Thread [{Task.CurrentId}] escaneando: {arquivo}");
                buscarAmazenar(arquivo);
            }
        });

        Console.WriteLine("\n=== PROCESSO CONCLUÍDO ===");
        ExibirResumo();
    }

    static void MontarBancoDados()
    {
        var connection = new SqliteConnection(bancoSqlite);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Busca (
                termo TEXT PRIMARY KEY,
                diretorios TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }

    static void buscarAmazenar(string arquivo)
    {
        try
        {
            var listaArquivos = Directory.EnumerateFiles(arquivo, "*.*", SearchOption.AllDirectories);

            foreach (var enderecoArquivo in listaArquivos)
            {
                var termo = Path.GetFileName(enderecoArquivo);
                var diretorio = Path.GetDirectoryName(enderecoArquivo);

                atualizarTermo(termo, diretorio);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar {arquivo}: {ex.Message}");
        }
    }

    static void atualizarTermo(string termo, string novoDiretorio)
    {
        lock (bancoSqlite)
        {
            var connection = new SqliteConnection(bancoSqlite);
            connection.Open();

            var buscaTermo = connection.CreateCommand();
            buscaTermo.CommandText = "SELECT diretorios FROM Busca WHERE termo = $termo";
            buscaTermo.Parameters.AddWithValue("$termo", termo);

            var leitor = buscaTermo.ExecuteReader();

            if (leitor.Read())
            {
                var diretorioAtual = leitor.GetString(0);
                if (!diretorioAtual.Contains(novoDiretorio))
                {
                    var atualizarDiretorio = diretorioAtual + "; " + novoDiretorio;
                    var atualizarLinha = connection.CreateCommand();
                    atualizarLinha.CommandText = "UPDATE Busca SET diretorios = $diretorio WHERE termo = $termo";
                    atualizarLinha.Parameters.AddWithValue("$diretorio", atualizarDiretorio);
                    atualizarLinha.Parameters.AddWithValue("$termo", termo);
                    atualizarLinha.ExecuteNonQuery();
                }
            }
            else
            {
                var inserirLinha = connection.CreateCommand();
                inserirLinha.CommandText = "INSERT INTO Busca (termo, diretorios) VALUES ($termo, $diretorio)";
                inserirLinha.Parameters.AddWithValue("$termo", termo);
                inserirLinha.Parameters.AddWithValue("$diretorio", novoDiretorio);
                inserirLinha.ExecuteNonQuery();
            }
        }
    }

    static void ExibirResumo()
    {
        var connection = new SqliteConnection(bancoSqlite);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Busca";
        var total = command.ExecuteScalar();
        Console.WriteLine($"Total de termos únicos no SQLite: {total}");
    }
}