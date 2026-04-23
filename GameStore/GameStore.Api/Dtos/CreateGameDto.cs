using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto(
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
