
using YourBrand.TimeReport.Domain.Common;

namespace YourBrand.TimeReport.Domain.Events;

public record TimeSheetActivityAddedEvent : DomainEvent
{
    public TimeSheetActivityAddedEvent(string timeSheetId, string timeSheetActivityId, string activityId)
    {
        TimeSheetId = timeSheetId;
        TimeSheetActivityId = timeSheetActivityId;
        ActivityId = activityId;
    }

    public string TimeSheetId { get; }

    public string TimeSheetActivityId { get; }

    public string ActivityId { get; }
}