namespace School.Api.Contracts
{
    public class StudentDtos
    {
        public record StudentDto(int Id, string? FirstName, string? LastName, DateTime DateOfBirth);

        public record StudentCreateDto(string? FirstName, string? LastName, DateTime DateOfBirth);

        public record StudentUpdateDto(string? FirstName, string? LastName, DateTime DateOfBirth);
    }
}
