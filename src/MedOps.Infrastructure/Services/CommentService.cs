namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Domain.Exceptions;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Comment = MedOps.Domain.Entities.Comment;

public class CommentService : ICommentService
{
    private readonly MedOpsDbContext _context;
    public CommentService(MedOpsDbContext context) { _context = context; }

    public async Task<List<CommentDto>> GetCommentsAsync(string entityType, Guid entityId)
    {
        return await _context.Comments
            .Where(c => c.EntityType == entityType && c.EntityId == entityId && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id, EntityType = c.EntityType, EntityId = c.EntityId,
                UserId = c.UserId, UserName = c.UserName, Content = c.Content,
                CreatedAt = c.CreatedAt, UpdatedAt = c.UpdatedAt
            }).ToListAsync();
    }

    public async Task<CommentDto> AddCommentAsync(string entityType, Guid entityId, Guid userId, string userName, CreateCommentDto dto)
    {
        var comment = new Comment(entityType, entityId, userId, userName, dto.Content);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return new CommentDto
        {
            Id = comment.Id, EntityType = comment.EntityType, EntityId = comment.EntityId,
            UserId = comment.UserId, UserName = comment.UserName, Content = comment.Content,
            CreatedAt = comment.CreatedAt, UpdatedAt = comment.UpdatedAt
        };
    }

    public async Task<CommentDto> UpdateCommentAsync(Guid commentId, Guid userId, string content)
    {
        var comment = await _context.Comments.FindAsync(commentId) ?? throw new DomainException("Comment not found", "COMMENT_NOT_FOUND");
        if (comment.UserId != userId) throw new DomainException("Not authorized", "UNAUTHORIZED");
        comment.UpdateContent(content);
        await _context.SaveChangesAsync();
        return new CommentDto
        {
            Id = comment.Id, EntityType = comment.EntityType, EntityId = comment.EntityId,
            UserId = comment.UserId, UserName = comment.UserName, Content = comment.Content,
            CreatedAt = comment.CreatedAt, UpdatedAt = comment.UpdatedAt
        };
    }

    public async Task DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _context.Comments.FindAsync(commentId) ?? throw new DomainException("Comment not found", "COMMENT_NOT_FOUND");
        if (comment.UserId != userId) throw new DomainException("Not authorized", "UNAUTHORIZED");
        comment.SoftDelete();
        await _context.SaveChangesAsync();
    }
}
