namespace UniTrackBackend.Api.DTO.ResultDtos;

public class SubjectResultDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<string> TeacherNames { get; set; }
}