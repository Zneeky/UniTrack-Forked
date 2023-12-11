using System.Text;
using Microsoft.Extensions.Logging;
using UniTrackBackend.Api.ViewModels.ResultViewModels;
using UniTrackBackend.Data;
using UniTrackBackend.Data.Models;

namespace UniTrackBackend.Services.AnalysisService;

public class AnalysisService : IAnalysisService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<AnalysisService> _logger;
    public AnalysisService(UnitOfWork unitOfWork, ILogger<AnalysisService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    private List<SubjectAverage>? CalculateStudentPerformanceBySubject(Student student)
    {
        try
        {
            var performanceList = new List<SubjectAverage>();

            var marksGroupedBySubject = student.Marks.GroupBy(m => m.Subject.Name);

            foreach (var subjectGroup in marksGroupedBySubject)
            {
                var subjectName = subjectGroup.Key;
                var averageMark = subjectGroup.Average(m => m.Value);
                var performanceDescription = GetPerformanceDescription(averageMark);

                performanceList.Add(new SubjectAverage
                {
                    SubjectName = subjectName,
                    Average = performanceDescription
                });
            }

            return performanceList;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {CalculateStudentPerformanceBySubject}: {EMessage}", nameof(CalculateStudentPerformanceBySubject), e.Message);
            return null;
        }
    }


    private string? GetPerformanceDescription(decimal averageMark)
    {
        try
        {
            return averageMark switch
            {
                >= 6m => "Excellent - Outstanding performance and mastery of the subject.",
                >= 5m => "Good - Demonstrates strong understanding and ability.",
                >= 4m => "Average - Meets basic requirements but has room for growth.",
                >= 3m => "Below Average - Improvement needed to meet the standards.",
                _ => "Struggling - Needs significant improvement and support."
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {GetPerformanceDescription}: {EMessage}", nameof(GetPerformanceDescription), e.Message);
            return null;
        }

    }

    private string? GeneratePerformanceSummary(StudentAnalysisResultViewModel viewModel)
    {
        try
        {
            var summary = new StringBuilder();

            // Overall Performance
            summary.AppendLine($"Overall Performance: {AssessOverallPerformance(viewModel.OverallAverage)}");

            // Strengths and Weaknesses in Each Subject
            foreach (var detailedSubject in viewModel.DetailedSubjectPerformance)
            {
                var subjectName = detailedSubject.SubjectName;
                var detail = detailedSubject.Details;
                var classAverageForSubject = viewModel.ClassAverageComparison
                    .FirstOrDefault(ca => ca.ClassName == subjectName)?.Average ?? 0m; // Default to 0 if not found

                var subjectPerformance = AssessSubjectPerformance(detail, classAverageForSubject);
                summary.AppendLine($"{subjectName}: {subjectPerformance}");
            }
 
            // Attendance
            var attendanceImpact = AssessAttendanceImpact(viewModel.Attendance);
            summary.AppendLine($"Attendance Impact: {attendanceImpact}");

            return summary.ToString();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {GeneratePerformanceSummaryName}: {EMessage}", nameof(GeneratePerformanceSummary), e.Message);
            return null;
        }
    }

    private string? AssessOverallPerformance(decimal overallAverage)
    {
        try
        {
            return overallAverage switch
            {
                >= 5.5m => "Excellent - Consistently high achiever in all subjects.",
                >= 4.5m => "Good - Above average performance with room for further excellence.",
                >= 3.5m => "Average - Meets basic standards but requires more effort to excel.",
                _ => "Needs Improvement - Below average performance, considerable effort required."
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {AssessOverallPerformance}: {EMessage}", nameof(AssessOverallPerformance), e.Message);
            return null;
        }

    }

    private string? AssessSubjectPerformance(SubjectDetail detail, decimal classAvg)
    {
        try
        {
            var performance = new StringBuilder();
            var avgMark =Math.Round((detail.HighestMark + detail.LowestMark) / 2.0m, 2) ;

            if (avgMark >= classAvg + 1m)
                performance.Append("Outperforming most peers, showing excellent understanding.");
            else if (avgMark >= classAvg)
                performance.Append("Performing on par with peers, solid understanding demonstrated.");
            else
                performance.Append("Falling behind peers, needs to focus more on this subject.");

            performance.Append($" Score range observed: {detail.LowestMark}-{detail.HighestMark}.");

            return performance.ToString();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {AssessSubjectPerformance}: {EMessage}", nameof(AssessSubjectPerformance), e.Message);
            return null;
        }

    }

    private string? AssessAttendanceImpact(AttendanceRecord attendance)
    {
        try
        {
            var totalAbsences = Math.Round(attendance.TotalAbsence, 1);
            var unexcusedAbsences =Math.Round(attendance.UnExcusedAbsence, 1);

            if (unexcusedAbsences / totalAbsences > 0.5m)
                return "High number of unexcused absences, likely impacting academic performance negatively.";
        
            return totalAbsences > 10 ? "Frequent absences observed, potentially hindering learning progress." : "Good attendance record, positively contributing to learning engagement.";
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {AssessAttendanceImpact}: {EMessage}", nameof(AssessAttendanceImpact), e.Message);
            return null;
        }

    }

    private List<DetailedSubjectPerformance>? CalculateSubjectPerformanceDetails(Student student)
    {
        try
        {
            var detailedPerformanceList = new List<DetailedSubjectPerformance>();

            var marksGroupedBySubject = student.Marks.GroupBy(m => m.Subject.Name);

            foreach (var subjectGroup in marksGroupedBySubject)
            {
                var marks = subjectGroup.ToList();
                var subjectDetail = new SubjectDetail
                {
                    MarksCount = marks.Count,
                    HighestMark = marks.Max(m => m.Value),
                    LowestMark = marks.Min(m => m.Value)
                };

                detailedPerformanceList.Add(new DetailedSubjectPerformance
                {
                    SubjectName = subjectGroup.Key,
                    Details = subjectDetail
                });
            }

            return detailedPerformanceList;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {CalculateSubjectPerformanceDetails}: {EMessage}", nameof(CalculateSubjectPerformanceDetails), e.Message);
            return null;
        }
    }


    private AttendanceRecord? CalculateAttendance(Student student)
    {
        try
        {
            var totalAbsences = student.Absences.Count;
            var unexcusedAbsences = student.Absences.Count(a => !a.Excused);
            var excusedAbsences = totalAbsences - unexcusedAbsences;

            return new AttendanceRecord
            {
                TotalAbsence = totalAbsences,
                UnExcusedAbsence = unexcusedAbsences,
                ExcusedAbsence = excusedAbsences
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {CalculateAttendance}: {EMessage}", nameof(CalculateAttendance), e.Message);
            return null;
        }

    }

    public async Task<StudentAnalysisResultViewModel?> GenerateAnalysisModel(int studentId)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetStudentWithDetailsAsync(studentId);
            if (student is null) return null;

            var overallAvg = student.Marks.Select(m => m.Value).Average();
            var successPerSubject = CalculateStudentPerformanceBySubject(student);
            if (successPerSubject == null) return null;

            var detailedSubjectPerformance = CalculateSubjectPerformanceDetails(student);
            if (detailedSubjectPerformance == null) return null;

            var attendanceRecord = CalculateAttendance(student);
            if (attendanceRecord == null) return null;

            // Calculate class averages
            var classAverages = await _unitOfWork.MarkRepository.CalculateClassAverages(student.GradeId);

            var model = new StudentAnalysisResultViewModel
            {
                StudentName = student.User.FirstName + ' ' + student.User.LastName,
                SubjectAvg = successPerSubject,
                DetailedSubjectPerformance = detailedSubjectPerformance,
                ClassAverageComparison = classAverages,
                OverallAverage = Math.Round(overallAvg, 2),
                Attendance = attendanceRecord,
                PerformanceSummary = null! // Placeholder, will be set next
            };

            var perfSummary = GeneratePerformanceSummary(model);
            if (perfSummary != null) model.PerformanceSummary = perfSummary;

            return model;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while trying to {GenerateAnalysisModel}: {EMessage}", nameof(GenerateAnalysisModel), e.Message);
            return null;
        }

    }

}