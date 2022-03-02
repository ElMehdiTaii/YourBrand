﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using OfficeOpenXml;

using Skynet.TimeReport.Application.Common.Interfaces;

namespace Skynet.TimeReport.Application.Reports.Queries;

public class CreateReportCommand : IRequest<Stream?>
{
    public CreateReportCommand(string[] projectIds, string? userId, DateTime startDate, DateTime endDate, bool includeUnlocked)
    {
        ProjectIds = projectIds;
        UserId = userId;
        StartDate = startDate;
        EndDate = endDate;
        IncludeUnlocked = includeUnlocked;
    }

    public string[] ProjectIds { get; }

    public string? UserId { get; }

    public DateTime StartDate { get; }

    public DateTime EndDate { get; }

    public bool IncludeUnlocked { get; }

    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Stream?>
    {
        private readonly ITimeReportContext _context;

        public CreateReportCommandHandler(ITimeReportContext context)
        {
            _context = context;
        }

        public async Task<Stream?> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            DateOnly startDate2 = DateOnly.FromDateTime(request.StartDate);
            DateOnly endDate2 = DateOnly.FromDateTime(request.EndDate);

            var query = _context.Entries
                .Include(p => p.TimeSheet)
                .ThenInclude(p => p.User)
                .Include(p => p.Project)
                .Include(p => p.Activity)
                .Where(p => request.ProjectIds.Any(x => x == p.Project.Id))
                .Where(p => p.Date >= startDate2 && p.Date <= endDate2)
                .AsSplitQuery();

            if(!request.IncludeUnlocked)
            {
                query = query.Where(x => x.Status == Domain.Entities.EntryStatus.Locked);
            }

            if (request.UserId is not null)
            {
                query = query.Where(x => x.TimeSheet.User.Id == request.UserId);
            }

            var entries = await query.ToListAsync(cancellationToken);

            int row = 1;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Projects");

                var projectGroups = entries.GroupBy(x => x.Project);

                foreach (var project in projectGroups)
                {
                    worksheet.Cells[row++, 1]
                          .LoadFromCollection(new[] { new { Project = project.Key.Name } });

                    int headerRow = row - 1;

                    var activityGroups = project.GroupBy(x => x.Activity);

                    foreach (var activityGroup in activityGroups)
                    {
                        var data = activityGroup
                            .OrderBy(e => e.Date)
                            .Select(e => new { e.Date, User = $" {e.TimeSheet.User.LastName}, {e.TimeSheet.User.FirstName}", Activity = e.Activity.Name, e.Hours, e.Description, Unlocked = e.Status == Domain.Entities.EntryStatus.Unlocked ? "*" : String.Empty });

                        worksheet.Cells[row, 1]
                            .LoadFromCollection(data);

                        row += data.Count();

                        worksheet.Cells[headerRow, 4]
                            .Value = data.Sum(e => e.Hours.GetValueOrDefault());
                    }

                    row++;
                }

                Stream stream = new MemoryStream(package.GetAsByteArray());

                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
    }
}