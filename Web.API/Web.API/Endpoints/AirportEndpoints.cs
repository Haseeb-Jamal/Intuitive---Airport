using Dapper;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using Web.API.Models;
using Web.API.Services;


namespace Web.API.Endpoints
{
    public static class AirportEndpoints
    {
        public static void MapAirportEndpoints(this IEndpointRouteBuilder builder) {
            builder.MapGet("airports", async (SqlConnectionFactory sqlConnectionFactory) => {

                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT ID, IATACode from AirportDB";

                var airports = await connection.QueryAsync<Airport>(sql);

                return Results.Ok(airports);
            });

            builder.MapGet("airports/{id}", async(int id, SqlConnectionFactory sqlConnectionFactory ) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = """
                                SELECT Id, IATACode,GeographyLevel1ID,Type
                                From AirportDB
                                Where Id = @AirportId
                                """;

                var airport = await connection.QuerySingleOrDefaultAsync<Airport>(
                    sql,
                    new {AirportId = id});

                return airport is not null ? Results.Ok(airport) : Results.NotFound();
            });

            builder.MapGet("countries", async (SqlConnectionFactory sqlConnectionFactory) => {

                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT GeographyLevel1ID, Name from CountryDB";

                var countries = await connection.QueryAsync<Country>(sql);

                return Results.Ok(countries);
            });

            builder.MapPost("countries", async (Country country, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = """
                    INSERT INTO CountryDB (Name)
                    VALUES(@Name)
                """;
                await connection.ExecuteAsync(sql, country);
                return Results.Ok(country);
            });
                        
            builder.MapDelete("countries/{id}", async (int id,SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = "DELETE FROM CountryDB WHERE Id = @GeographyLevel1IDId";

                await connection.ExecuteAsync(sql, new { GeographyLevel1ID = id });
                return Results.NoContent();
            });

            builder.MapGet("routes", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT RouteID,DepartureAirportID,ArrivalAirportID from RouteDB";

                var routes = await connection.QueryAsync<AirRoute>(sql);

                return Results.Ok(routes);
            });


            builder.MapPost("routes", async (AirRoute airroute, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = """
                    INSERT INTO RouteDB (DepartureAirportID,ArrivalAirportID )
                    VALUES(@DepartureAirportID,@ArrivalAirportID)
                """;
                await connection.ExecuteAsync(sql, airroute);
                return Results.Ok(airroute);
            });
        }
    }
}
