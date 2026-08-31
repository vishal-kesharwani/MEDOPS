namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface ICommentService
{
    Task<List<CommentDto>> GetCommentsAsync(string entityType, Guid entityId);
    Task<CommentDto> AddCommentAsync(string entityType, Guid entityId, Guid userId, string userName, CreateCommentDto dto);
    Task<CommentDto> UpdateCommentAsync(Guid commentId, Guid userId, string content);
    Task DeleteCommentAsync(Guid commentId, Guid userId);
}
