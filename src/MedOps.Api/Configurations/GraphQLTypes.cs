namespace MedOps.Api.Configurations;

using HotChocolate;
using HotChocolate.Types;
using MedOps.Application.DTOs;

public class StudyType : ObjectType<StudyDto>
{
    protected override void Configure(IObjectTypeDescriptor<StudyDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<IdType>();
        descriptor.Field(f => f.Name).Type<StringType>();
        descriptor.Field(f => f.Status).Type<EnumType<MedOps.Domain.Enums.StudyStatus>>();
    }
}

public class SiteType : ObjectType<SiteDto>
{
    protected override void Configure(IObjectTypeDescriptor<SiteDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<IdType>();
        descriptor.Field(f => f.Status).Type<EnumType<MedOps.Domain.Enums.SiteStatus>>();
    }
}

public class TaskType : ObjectType<TaskDto>
{
    protected override void Configure(IObjectTypeDescriptor<TaskDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<IdType>();
        descriptor.Field(f => f.Status).Type<EnumType<MedOps.Domain.Enums.TaskStatus>>();
        descriptor.Field(f => f.Priority).Type<EnumType<MedOps.Domain.Enums.TaskPriority>>();
    }
}

public class RequestType : ObjectType<RequestDto>
{
    protected override void Configure(IObjectTypeDescriptor<RequestDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<IdType>();
        descriptor.Field(f => f.Status).Type<EnumType<MedOps.Domain.Enums.RequestStatus>>();
    }
}