using DotnetInterview.Models;
using Npgsql;

namespace DotnetInterview.Data;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Supabase")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Supabase");
    }

    public async Task<long> InsertAsync(User user, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO public.users
              (first_name, last_name, email, phone, profile_base64, birth_day, occupation, gender)
            VALUES
              (@first_name, @last_name, @email, @phone, @profile_base64, @birth_day, @occupation, @gender)
            RETURNING id;";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("first_name", user.FirstName);
        cmd.Parameters.AddWithValue("last_name", user.LastName);
        cmd.Parameters.AddWithValue("email", user.Email);
        cmd.Parameters.AddWithValue("phone", user.Phone);
        cmd.Parameters.AddWithValue("profile_base64", user.ProfileBase64);
        cmd.Parameters.AddWithValue("birth_day", user.BirthDay);
        cmd.Parameters.AddWithValue("occupation", user.Occupation);
        cmd.Parameters.AddWithValue("gender", user.Gender);

        var result = await cmd.ExecuteScalarAsync(ct);
        return Convert.ToInt64(result);
    }
}
