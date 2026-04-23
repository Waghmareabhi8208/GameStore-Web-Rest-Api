namespace GameStore.Api.Dtos;

// A DTO (Data Transfer Object) is a contract between the Client and the Server,
//since it represents a shared agreement about how data will be transferred and used.

public record GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
