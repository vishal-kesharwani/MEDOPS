namespace MedOps.Application.DTOs;

public class DashboardDto
{
    public DashboardStats Stats { get; set; } = new();
    public List<StatusBreakdown> StudiesByStatus { get; set; } = new();
    public List<StatusBreakdown> TasksByStatus { get; set; } = new();
    public List<StatusBreakdown> RequestsByStatus { get; set; } = new();
    public List<MonthlyActivity> MonthlyActivity { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<OverdueItemDto> OverdueTasks { get; set; } = new();
    public List<OverdueItemDto> PendingRequests { get; set; } = new();
}

public class DashboardStats
{
    public int TotalStudies { get; set; }
    public int ActiveStudies { get; set; }
    public int TotalSites { get; set; }
    public int ActiveSites { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int PendingRequests { get; set; }
    public int TotalDepartments { get; set; }
}

public class StatusBreakdown
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class MonthlyActivity
{
    public string Month { get; set; } = string.Empty;
    public int StudiesCreated { get; set; }
    public int TasksCompleted { get; set; }
    public int RequestsProcessed { get; set; }
}

public class RecentActivityDto
{
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string? EntityName { get; set; }
    public DateTime Timestamp { get; set; }
}

public class OverdueItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly? DueDate { get; set; }
    public int DaysOverdue { get; set; }
}
